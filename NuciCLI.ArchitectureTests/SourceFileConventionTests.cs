using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NuciCLI;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace NuciCLI.ArchitectureTests
{
    [TestFixture]
    public sealed class SourceFileConventionTests
    {
        private static readonly string[] sourceFiles = FindProductionSourceFiles();

        [Test]
        public void GivenProductionSources_WhenDiscovered_ThenEligibleFilesExist()
            => Assert.That(
                sourceFiles,
                Is.Not.Empty,
                "No eligible handwritten NuciCLI source files were found.");

        [Test]
        public void GivenProductionSourceFiles_WhenParsed_ThenEachFileDeclaresExactlyOneType()
        {
            foreach (string sourceFile in sourceFiles)
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(ReadSource(sourceFile));
                TypeDeclarationSyntax[] declarations = GetTypeDeclarations(syntaxTree).ToArray();

                Assert.That(
                    declarations,
                    Has.Length.EqualTo(1),
                    $"'{sourceFile}' must declare exactly one type.");
            }
        }

        [Test]
        public void GivenProductionSourceFiles_WhenParsed_ThenFilenameMatchesDeclaredType()
        {
            foreach (string sourceFile in sourceFiles)
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(ReadSource(sourceFile));
                TypeDeclarationSyntax declaration = GetTypeDeclarations(syntaxTree).Single();
                string expectedName = Path.GetFileNameWithoutExtension(sourceFile);

                Assert.That(
                    declaration.Identifier.ValueText,
                    Is.EqualTo(expectedName),
                    $"'{sourceFile}' must use the declared type name as its filename.");
            }
        }

        [Test]
        public void GivenProductionSourceFiles_WhenParsed_ThenExplicitBlockNamespacesAreUsed()
        {
            foreach (string sourceFile in sourceFiles)
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(ReadSource(sourceFile));

                Assert.That(
                    syntaxTree.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>(),
                    Is.Empty,
                    $"'{sourceFile}' must use a block namespace.");
            }
        }

        [Test]
        public void GivenProductionSourceFiles_WhenParsed_ThenAllTypesHaveExplicitAccessibility()
        {
            foreach (string sourceFile in sourceFiles)
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(ReadSource(sourceFile));
                TypeDeclarationSyntax declaration = GetTypeDeclarations(syntaxTree).Single();

                Assert.That(
                    declaration.Modifiers.Any(SyntaxKind.PublicKeyword)
                    || declaration.Modifiers.Any(SyntaxKind.InternalKeyword),
                    Is.True,
                    $"'{sourceFile}' must declare explicit type accessibility.");
            }
        }

        [Test]
        public void GivenProductionSourceFiles_WhenParsed_ThenNoGeneratedOrBuildFilesAreIncluded()
        {
            Assert.That(sourceFiles, Has.All.Matches<string>(path =>
                !path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .Any(part => part is "bin" or "obj")));
        }

        private static IEnumerable<TypeDeclarationSyntax> GetTypeDeclarations(SyntaxTree syntaxTree)
            => syntaxTree.GetRoot()
                .DescendantNodes()
                .OfType<BaseTypeDeclarationSyntax>()
                .Where(type => type.Parent is not BaseTypeDeclarationSyntax)
                .Cast<TypeDeclarationSyntax>();

        private static string[] FindProductionSourceFiles()
        {
            Assembly assembly = typeof(SourceFileConventionTests).Assembly;
            string[] resourceNames = assembly.GetManifestResourceNames();

            return resourceNames
                .Where(name => name.StartsWith("NuciCLI.Source.", StringComparison.Ordinal))
                .Select(name => name["NuciCLI.Source.".Length..])
                .Where(name => name.EndsWith(".cs", StringComparison.Ordinal))
                .OrderBy(name => name, StringComparer.Ordinal)
                .Select(name => WriteResourceToTemporaryFile(assembly, name))
                .ToArray();
        }

        private static string WriteResourceToTemporaryFile(Assembly assembly, string resourceName)
        {
            return resourceName;
        }

        private static string ReadSource(string resourceName)
        {
            using Stream resourceStream = typeof(SourceFileConventionTests).Assembly
                .GetManifestResourceStream($"NuciCLI.Source.{resourceName}")
                ?? throw new InvalidOperationException($"Embedded source '{resourceName}' is unavailable.");
            using StreamReader reader = new(resourceStream);

            return reader.ReadToEnd();
        }
    }
}
