[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Build Status](https://github.com/hmlendea/nucicli/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucicli/actions/workflows/dotnet.yml) [![Latest GitHub release](https://img.shields.io/github/v/release/hmlendea/nucicli)](https://github.com/hmlendea/nucicli/releases/latest)

# NuciCLI

## About

NuciCLI is a lightweight .NET library for building interactive command-line applications.
It wraps common console operations behind a small, focused API for:

- reading text input with prompts
- reading single-key input
- asking yes or no questions
- writing coloured console output
- working with console cursor and buffer dimensions

The package is intentionally small and works well as a base layer for richer CLI tooling.

## Features

- Prompted line input through `NuciConsole.ReadLine(...)`
- Prompted key input through `NuciConsole.ReadKey(...)`
- Confirmation prompts through `NuciConsole.ReadPermission(...)`
- Coloured output through `NuciConsole.Write(...)`, `WriteLine(...)`, and `WriteLines(...)`
- Access to cursor position and console dimensions through `CursorX`, `CursorY`, `Width`, and `Height`
- Explicit cancellation flow through `InputCancellationException` when text input is aborted with `Escape`

## Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciCLI)

### .NET CLI

```bash
dotnet add package NuciCLI
```

### Package Manager

```powershell
Install-Package NuciCLI
```

## Quick Start

```csharp
using NuciCLI;

NuciConsole.WriteLine("NuciCLI demo", NuciConsoleColour.Cyan);

try
{
	string name = NuciConsole.ReadLine("Name: ", NuciConsoleColour.Yellow);
	bool proceed = NuciConsole.ReadPermission($"Continue as {name}?", defaultValue: true);

	if (proceed)
	{
		NuciConsole.WriteLine("Starting...", NuciConsoleColour.Green);
	}
	else
	{
		NuciConsole.WriteLine("Cancelled.", NuciConsoleColour.Red);
	}
}
catch (InputCancellationException)
{
	NuciConsole.WriteLine("Input cancelled by user.", NuciConsoleColour.DarkYellow);
}
```

## API Overview

### `NuciConsole`

The main entry point for console interaction.

Common members include:

- `ReadLine(...)` for reading text input
- `ReadKey(...)` for reading a single key press
- `ReadPermission(...)` for yes or no confirmation prompts
- `Write(...)`, `WriteLine(...)`, and `WriteLines(...)` for output
- `CursorX` and `CursorY` for reading or setting cursor position
- `Width`, `Height`, `Area`, and `CursorSize` for console information

### `NuciConsoleColour`

Provides named colour values that map to `ConsoleColor`, including:

- `Default`
- `Black`, `DarkBlue`, `DarkGreen`, `DarkCyan`
- `DarkRed`, `DarkMagenta`, `DarkYellow`, `Gray`
- `DarkGray`, `Blue`, `Green`, `Cyan`
- `Red`, `Magenta`, `Yellow`, `White`

### `InputCancellationException`

Thrown by `ReadLine(...)` when the user cancels input with the `Escape` key.

## Behaviour Notes

- `ReadPermission(...)` accepts `Y`, `N`, or `Enter`.
- `ReadPermission(...)` uses the supplied default value when `Enter` is pressed without choosing an option.
- `ReadKey(...)` reads from the console without echoing the pressed key.
- `ReadLine(...)` supports interactive editing with backspace and allows the caller to handle cancellation explicitly.

## Related Packages

The NuciCLI ecosystem is split into focused packages:

- [NuciCLI](https://github.com/hmlendea/nucicli) for core console helpers
- [NuciCLI.Arguments](https://github.com/hmlendea/nucicli.arguments) for command-line argument handling
- [NuciCLI.Menus](https://github.com/hmlendea/nucicli.menus) for interactive terminal menus

## Target Framework

The current package targets `.NET 10`.

## License

This project is licensed under the `GNU General Public License v3.0` or later. See [LICENSE](./LICENSE) for details.
