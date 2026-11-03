# Repository Structure

## Source Tree Layout

```
nucicli/
├── .github/
│   └── workflows/
│       ├── dotnet.yml           # CI: build, test on push/PR
│       ├── nuget-release.yml    # CD: publish to NuGet.org on release
│       └── github-release.yml   # CD: upload package to GitHub Release
├── NuciCLI/                     # Main library project
│   ├── NuciCLI.csproj           # Project config, deps, package metadata
│   ├── NuciConsole.cs           # Static façade: all console operations
│   ├── NuciConsoleColour.cs     # Immutable colour value objects
│   ├── InputCancellationException.cs  # Escape-key cancellation exception
│   ├── bin/                     # Build output (gitignored)
│   └── obj/                     # Build intermediates (gitignored)
├── NuciCLI.UnitTests/           # Unit test project
│   ├── NuciCLI.UnitTests.csproj # Test project config (NUnit)
│   ├── bin/                     # Build output (gitignored)
│   └── obj/                     # Build intermediates (gitignored)
├── NuciCLI.sln                  # Solution file (2 projects)
├── ARCHITECTURE.md              # Root architecture document
├── README.md                    # Project overview, quick start
├── SECURITY.md                  # Security policy
├── PRIVACY.md                   # Privacy/data handling
├── LICENSE                      # GPL-3.0-or-later
├── CONTRIBUTING.md              # Contribution guidelines
├── CODE_OF_CONDUCT.md           # Community conduct
└── docs/                        # Generated documentation (this folder)
    ├── INDEX.md
    ├── architecture.md
    ├── design-decisions.md
    ├── repository-overview.md
    ├── repository-structure.md
    ├── components/
    ├── flows/
    ├── behaviour/
    ├── api-reference/
    └── ... (other docs)
```

## Module Organisation

### NuciCLI (Library)

**Single project, single assembly, no internal namespaces.**

| File | Lines | Responsibility |
|------|-------|----------------|
| `NuciConsole.cs` | ~300 | All public console operations, private `PerformKeyRead` |
| `NuciConsoleColour.cs` | ~60 | 17 static colour instances, value object |
| `InputCancellationException.cs` | ~30 | Exception type with 3 constructors |
| `NuciCLI.csproj` | ~25 | net10.0, NuciExtensions 6.0.2, package metadata |

**No subdirectories.** All types in `NuciCLI` namespace.

### NuciCLI.UnitTests (Tests)

**Single test project, NUnit 5.0.0.**

| File | Purpose |
|------|---------|
| `NuciCLI.UnitTests.csproj` | References NuciCLI, Microsoft.NET.Test.Sdk, NUnit, NUnit3TestAdapter |

**No test files visible in workspace** — test implementation not present in source tree (may be generated or in separate location).

### Solution

`NuciCLI.sln` — Two projects:
- `NuciCLI` (class library)
- `NuciCLI.UnitTests` (test project)

Configurations: Debug|Any CPU, Release|Any CPU

## Build Artefacts (Gitignored)

```
NuciCLI/bin/Debug/net10.0/
NuciCLI/bin/Release/net10.0/
NuciCLI/obj/
NuciCLI.UnitTests/bin/Debug/net10.0/
NuciCLI.UnitTests/bin/Release/net10.0/
NuciCLI.UnitTests/obj/
```

## CI/CD Pipeline Files

| File | Trigger | Actions |
|------|---------|---------|
| `.github/workflows/dotnet.yml` | push/PR to master | checkout, setup .NET 10, restore, build, test |
| `.github/workflows/nuget-release.yml` | release published | checkout, setup .NET 10, extract version, restore, pack, NuGet OIDC login, push to nuget.org |
| `.github/workflows/github-release.yml` | release published | checkout, setup .NET 10, extract version, restore, pack, calculate SHA256, upload to GitHub Release, append checksum to release notes |

## Documentation Files (Root)

| File | Purpose |
|------|---------|
| `ARCHITECTURE.md` | High-level architecture, system context, components |
| `README.md` | Project description, installation, quick start, API overview |
| `SECURITY.md` | Supported versions, vulnerability reporting, disclosure |
| `PRIVACY.md` | Data handling (none), external integrations (none) |
| `LICENSE` | GPL-3.0-or-later full text |
| `CONTRIBUTING.md` | Issue reporting, PR process, code style, testing |
| `CODE_OF_CONDUCT.md` | Expected/unacceptable conduct, moderation, reporting |

## Documentation Files (docs/)

Generated under `docs/` per the documentation structure specification:

```
docs/
├── INDEX.md                      # Master index
├── architecture.md               # Detailed architecture
├── design-decisions.md           # Design choices with rationale
├── repository-overview.md        # This file's content
├── repository-structure.md       # This file
├── components/
│   ├── presentation.md
│   ├── host-and-composition.md
│   ├── application-services.md
│   └── integration-models.md
├── flows/
│   ├── startup-and-rendering.md
│   ├── calendar-search.md
│   └── detail-mutation.md
├── behaviour/
│   ├── browse-and-search.md
│   └── inspect-edit-delete.md
├── api-reference/
│   ├── INDEX.md
│   ├── nuci-console.md
│   └── nuci-console-colour.md
├── api-usage-examples.md
├── ambiguities-and-open-questions.md
├── build-and-deployment.md
├── change-guide.md
├── configuration.md
├── concurrency-and-scheduling.md
├── data-model.md
├── dependencies.md
├── documentation-maintenance.md
├── error-handling.md
├── faq.md
├── invariants.md
├── integrations.md
├── logging.md
├── quick-start.md
├── security.md
├── state-and-persistence.md
├── testing.md
└── troubleshooting.md
```

## Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Projects | PascalCase | `NuciCLI`, `NuciCLI.UnitTests` |
| Namespaces | PascalCase | `NuciCLI` |
| Classes/Types | PascalCase | `NuciConsole`, `NuciConsoleColour` |
| Methods | PascalCase | `ReadLine`, `WriteLine` |
| Properties | PascalCase | `CursorX`, `Width` |
| Fields | camelCase (private) | `previousCtrlCBehaviour` |
| Parameters | camelCase | `prompt`, `foregroundColour` |
| Constants | PascalCase | `Default`, `Black` |
| Files | PascalCase.cs | `NuciConsole.cs` |
| Solution | PascalCase.sln | `NuciCLI.sln` |
| Workflow files | kebab-case.yml | `dotnet.yml`, `nuget-release.yml` |
| Markdown docs | kebab-case.md | `architecture.md`, `design-decisions.md` |

## Dependency Graph

```
NuciCLI.UnitTests
       │
       ▼ (ProjectReference)
NuciCLI
       │
       ├──► NuciExtensions (6.0.2) — PackageReference
       └──► System.Console (BCL) — FrameworkReference
```

No other dependencies. No circular references.