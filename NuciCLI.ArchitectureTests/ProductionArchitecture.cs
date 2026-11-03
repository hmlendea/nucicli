using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNet;
using ArchUnitNet.Files.FluentApi;
using NUnit.Framework;

namespace NuciCLI.ArchitectureTests
{
    internal static class ProductionArchitecture
    {
        public static string ProjectFile
        {
            get
            {
                string absolutePath = System.IO.Path.GetFullPath(
                    System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../NuciCLI/NuciCLI.csproj"));
                return "/" + absolutePath.Replace(System.IO.Path.DirectorySeparatorChar, '/');
            }
        }

        public static IEnumerable<Type> ProductionTypes
            => typeof(NuciConsole).Assembly
                .GetTypes()
                .Where(type => type.Namespace == "NuciCLI")
                .OrderBy(type => type.FullName, StringComparer.Ordinal);

        public static IEnumerable<Type> PublicProductionTypes
            => ProductionTypes.Where(type => type.IsPublic);

        public static void AssertSelectorIsNotEmpty(IEnumerable<Type> types, string selector)
            => Assert.That(
                types,
                Is.Not.Empty,
                $"Architecture selector '{selector}' matched no production types.");

        public static void AssertAssemblyHasNoReferenceTo(string assemblyName)
            => Assert.That(
                typeof(NuciConsole).Assembly.GetReferencedAssemblies().Select(assembly => assembly.Name),
                Does.Not.Contain(assemblyName),
                $"NuciCLI must not reference '{assemblyName}'.");
    }

    internal static class PathResolver
    {
        public static string ResolveProjectFile()
        {
            string projectFile = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../NuciCLI/NuciCLI.csproj"));

            if (!System.IO.File.Exists(projectFile))
            {
                projectFile = System.IO.Path.GetFullPath(
                    System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../../nucicli/NuciCLI/NuciCLI.csproj"));
            }

            if (!System.IO.File.Exists(projectFile))
            {
                throw new InvalidOperationException($"Required project metadata is unavailable: '{projectFile}'.");
            }

            return projectFile;
        }
    }
}
