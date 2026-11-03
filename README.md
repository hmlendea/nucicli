[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucicli)](https://github.com/hmlendea/nucicli/releases/latest)
[![Build Status](https://github.com/hmlendea/nucicli/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucicli/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/NuciCLI)](https://nuget.org/packages/NuciCLI)
[![License](https://img.shields.io/github/license/hmlendea/nucicli)](https://github.com/hmlendea/nucicli/blob/master/LICENSE)

# NuciCLI

NuciCLI is a lightweight .NET library for building interactive command-line applications.

## 📑 Table of Contents

- [Capabilities](#capabilities)
- [Usage](#usage)
- [Compatibility](#compatibility)
- [Installation](#installation)
  - [Package Manager Installation](#package-manager-installation)
- [Development](#development)
  - [Requirements](#requirements)
  - [Setup](#setup)
  - [Build](#build)
  - [Test](#test)
  - [Continuous Integration](#continuous-integration)
  - [Dependencies](#dependencies)
- [GitHub Actions](#github-actions)
- [Project Structure](#project-structure)
  - [Projects and Packages](#projects-and-packages)
  - [Directories](#directories)
- [Architecture](#architecture)
- [Documentation](#documentation)
- [API Reference](#api-reference)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [Code of Conduct](#code-of-conduct)
- [Related Projects](#related-projects)
- [Privacy and Data](#privacy-and-data)
- [Security](#security)
- [Project Engagement](#project-engagement)
- [License](#license)

## ✨ Capabilities

- Read prompted text and single-key input from the console.
- Ask yes or no questions with configurable defaults.
- Write plain and coloured console output.
- Read or set cursor position and console dimensions.
- Handle cancelled text input through `InputCancellationException`.

## 🚀 Usage

```csharp
using NuciCLI;

string name = NuciConsole.ReadLine("Name: ", NuciConsoleColour.Yellow);
bool proceed = NuciConsole.ReadPermission($"Continue as {name}?", defaultValue: true);

if (proceed)
{
\tNuciConsole.WriteLine("Starting...", NuciConsoleColour.Green);
}
```

## 🧩 Compatibility

| Component | Supported Versions | Notes |
|-----------|--------------------|-------|
| .NET | 10 | The package targets `net10.0`. |

## 📦 Installation

[![Obtain it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciCLI)

### Package Manager Installation

```bash
dotnet add package NuciCLI
```

Or, via the `Package Manager Console`:
```powershell
Install-Package NuciCLI
```

## 🛠️ Development

### Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Git

### Setup

```bash
git clone https://github.com/hmlendea/nucicli.git
cd nucicli
dotnet restore
```

### Build

```bash
dotnet build --no-restore
```

### Test

```bash
dotnet test --no-build --verbosity normal
```

### Continuous Integration

The `.NET` workflow restores dependencies, builds the solution, and runs tests on pushes and pull requests targeting `master`.

### Dependencies

| Package | Version | Scope | Purpose |
|---------|---------|-------|---------|
| `NuciExtensions` | `6.0.2` | Runtime | Provides the library's direct .NET dependency. |

## ⚙️ GitHub Actions

| Workflow | Purpose | What it does |
|----------|---------|--------------|
| [.NET](https://github.com/hmlendea/nucicli/blob/master/.github/workflows/dotnet.yml) | Continuous validation | Restores dependencies, builds the solution, and runs tests. |
| [GitHub Release](https://github.com/hmlendea/nucicli/blob/master/.github/workflows/github-release.yml) | GitHub package release | Packs the project, calculates a SHA256 checksum, and uploads the package to the GitHub Release. |
| [NuGet.org Release](https://github.com/hmlendea/nucicli/blob/master/.github/workflows/nuget-release.yml) | NuGet publication | Packs the project and publishes it to NuGet.org using OIDC authentication. |

## 🗂️ Project Structure

The repository contains the library, unit tests, architecture tests, and supporting documentation.

### Projects and Packages

| Project | Type | Purpose |
|---------|------|---------|
| `NuciCLI/` | .NET library | Provides the public console interaction API. |
| `NuciCLI.UnitTests/` | Test project | Tests library behaviour. |
| `NuciCLI.ArchitectureTests/` | Test project | Verifies architectural and source conventions. |

### Directories

| Directory | Purpose |
|-----------|---------|
| `docs/` | Detailed architecture, behaviour, API, testing, and maintenance documentation. |
| `.github/workflows/` | Continuous integration and release workflows. |

## 🏗️ Architecture

NuciCLI is a lightweight .NET library for building interactive command-line applications.

See the [architecture documentation](./ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## 📚 Documentation

The `docs/` directory contains the repository documentation:

| Resource | Description |
|----------|-------------|
| [Documentation Index](./docs/INDEX.md) | Master index with navigation. |
| [Quick Start](./docs/quick-start.md) | Getting started guide. |
| [API Usage Examples](./docs/api-usage-examples.md) | Practical usage patterns. |
| [Behaviour](./docs/behaviour/) | User-facing behaviour documentation. |
| [Components](./docs/components/) | Component deep dives. |
| [Flows](./docs/flows/) | End-to-end execution flows. |
| [Testing](./docs/testing.md) | Test strategy and organisation. |
| [Troubleshooting](./docs/troubleshooting.md) | Common issues and resolutions. |
| [FAQ](./docs/faq.md) | Frequently asked questions. |

## 🌐 API Reference

The [API Reference](./docs/api-reference/INDEX.md) documents the public `NuciConsole` and `NuciConsoleColour` APIs.

## 🩺 Troubleshooting

| Symptom | Probable Cause | Resolution |
|---------|----------------|------------|
| Input is cancelled unexpectedly | `Escape` was pressed during `ReadLine(...)`. | Catch `InputCancellationException` when cancellation should be handled by the calling application. |

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Preserve the existing public contract unless a breaking change is intentional
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes
- Properly test all modifications, including edge cases and error conditions
- Include tests for additional or modified functionality
- Raise a new [issue](https://github.com/hmlendea/nucicli/issues) for problems or suggestions

See the [contributing guidelines](./CONTRIBUTING.md) for details on how to report issues, suggest enhancements, and submit changes.

## 📜 Code of Conduct

See the [Code of Conduct](./CODE_OF_CONDUCT.md) for the standards of conduct expected in all project spaces.

## 🔗 Related Projects

- [NuciCLI.Arguments](https://github.com/hmlendea/nucicli.arguments): Command-line argument handling.
- [NuciCLI.Menus](https://github.com/hmlendea/nucicli.menus): Interactive terminal menus.

## 🛡️ Privacy and Data

NuciCLI does not collect, process, transmit, or persist any personal data.
For the detailed description of how the application handles privacy and personal data, see [PRIVACY.md](./PRIVACY.md).

NuciCLI processes only the console input and output explicitly provided by the calling application. It does not process personal data for any purpose.

## 🔒 Security

NuciCLI follows the security practices documented in [SECURITY.md](./SECURITY.md).

For information on reporting security vulnerabilities, see [SECURITY.md](./SECURITY.md).

## 💝 Project Engagement

Discovered a problem or have a suggestion? [Open an issue](https://github.com/hmlendea/nucicli/issues)!

If you find this project useful, consider starring ⭐️ it on GitHub!

## 📄 License

This project is being distributed under the `GNU General Public License v3.0` or later.
See [LICENSE](./LICENSE) for additional information.
