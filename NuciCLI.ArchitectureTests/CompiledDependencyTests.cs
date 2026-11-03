using System;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNet;
using NUnit.Framework;

namespace NuciCLI.ArchitectureTests
{
    [TestFixture]
    public sealed class CompiledDependencyTests
    {
        [Test]
        public void GivenProductionAssembly_WhenLoaded_ThenExpectedPublicTypesArePresent()
        {
            Type[] types = ProductionArchitecture.PublicProductionTypes.ToArray();

            ProductionArchitecture.AssertSelectorIsNotEmpty(types, "public NuciCLI types");
            Assert.That(types.Select(type => type.Name), Does.Contain(nameof(NuciConsole)));
            Assert.That(types.Select(type => type.Name), Does.Contain(nameof(NuciConsoleColour)));
            Assert.That(types.Select(type => type.Name), Does.Contain(nameof(InputCancellationException)));
        }

        [Test]
        public void GivenProductionTypes_WhenChecked_ThenTheyBelongToTheNuciCliNamespace()
        {
            Type[] types = ProductionArchitecture.ProductionTypes.ToArray();

            ProductionArchitecture.AssertSelectorIsNotEmpty(types, "NuciCLI namespace types");
            Assert.That(types, Has.All.Matches<Type>(type => type.Namespace == "NuciCLI"));
        }

        [Test]
        public void GivenProductionTypes_WhenChecked_ThenNoTypeDependsOnTestAssemblies()
        {
            ProductionArchitecture.AssertAssemblyHasNoReferenceTo("NuciCLI.UnitTests");
            ProductionArchitecture.AssertAssemblyHasNoReferenceTo("NuciCLI.ArchitectureTests");
        }

        [Test]
        public void GivenProductionAssembly_WhenChecked_ThenItDoesNotDependOnUiFrameworks()
        {
            ProductionArchitecture.AssertAssemblyHasNoReferenceTo("Microsoft.AspNetCore");
            ProductionArchitecture.AssertAssemblyHasNoReferenceTo("EntityFramework");
            ProductionArchitecture.AssertAssemblyHasNoReferenceTo("MediatR");
        }

        [Test]
        public async Task GivenProductionProject_WhenChecked_ThenItHasNoTestProjectFiles()
        {
            var violations = await ArchUnit.ProjectFiles(ProductionArchitecture.ProjectFile)
                .InPath("*.cs")
                .ShouldNot()
                .DependOnFiles()
                .InFolder("unit-tests")
                .CheckAsync();

            Assert.That(violations, Is.Empty, "Production source must not depend on test project files.");
        }

        [Test]
        public async Task GivenProductionProject_WhenChecked_ThenItHasNoCycles()
        {
            var violations = await ArchUnit.ProjectFiles(ProductionArchitecture.ProjectFile)
                .InPath("*.cs")
                .Should()
                .HaveNoCycles()
                .CheckAsync();

            Assert.That(violations, Is.Empty, "Production source must not contain dependency cycles.");
        }

        [Test]
        public void GivenPublicProductionTypes_WhenChecked_ThenTheyAreNotNested()
        {
            Type[] types = ProductionArchitecture.PublicProductionTypes.ToArray();

            Assert.That(types, Has.All.Matches<Type>(type => type.DeclaringType is null));
        }
    }
}
