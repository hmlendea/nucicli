# Quick Start

## Install

```bash
dotnet add package NuciCLI
```

## Hello World

```csharp
using NuciCLI;

NuciConsole.WriteLine("Hello, NuciCLI!", NuciConsoleColour.Green);
```

## Read Input

```csharp
string name = NuciConsole.ReadLine("Your name: ", NuciConsoleColour.Cyan);
NuciConsole.WriteLine($"Hello, {name}!", NuciConsoleColour.Yellow);
```

## Handle Cancellation

```csharp
try
{
    string input = NuciConsole.ReadLine("Type something (Esc to cancel): ");
    NuciConsole.WriteLine($"You typed: {input}");
}
catch (InputCancellationException)
{
    NuciConsole.WriteLine("Cancelled.", NuciConsoleColour.Red);
}
```

## Ask Confirmation

```csharp
if (NuciConsole.ReadPermission("Delete file?", defaultValue: false))
{
    NuciConsole.WriteLine("Deleted.", NuciConsoleColour.Red);
}
else
{
    NuciConsole.WriteLine("Kept.", NuciConsoleColour.Green);
}
```

## Colours

```csharp
// Foreground only
NuciConsole.Write("Red text", NuciConsoleColour.Red);

// Foreground + background
NuciConsole.WriteLine("White on Blue", NuciConsoleColour.White, NuciConsoleColour.Blue);

// Default (no change)
NuciConsole.Write("Normal ", NuciConsoleColour.Default);
NuciConsole.WriteLine("Highlighted", NuciConsoleColour.Yellow);
```

## Cursor Control

```csharp
NuciConsole.CursorX = 10;
NuciConsole.CursorY = 5;
NuciConsole.Write("Positioned");

// Get buffer size
int w = NuciConsole.Width;
int h = NuciConsole.Height;
```

## Available Colours

| Colour | Value |
|--------|-------|
| Default | -1 (no change) |
| Black | 0 |
| DarkBlue | 1 |
| DarkGreen | 2 |
| DarkCyan | 3 |
| DarkRed | 4 |
| DarkMagenta | 5 |
| DarkYellow | 6 |
| Gray | 7 |
| DarkGray | 8 |
| Blue | 9 |
| Green | 10 |
| Cyan | 11 |
| Red | 12 |
| Magenta | 13 |
| Yellow | 14 |
| White | 15 |

## Key Methods

| Method | Purpose |
|--------|---------|
| `Write(text, fg?, bg?)` | Write without newline |
| `WriteLine(text, fg?, bg?)` | Write with newline |
| `WriteLines(lines, fg?, bg?)` | Write multiple lines |
| `ReadLine(prompt, fg?)` | Read line, throws on Escape |
| `ReadKey(intercept?)` | Read single key |
| `ReadPermission(prompt, default)` | Y/N confirmation |

## Run the Examples

```bash
# Create console app
dotnet new console -n MyCli
cd MyCli
dotnet add package NuciCLI

# Paste examples into Program.cs
dotnet run
```

## Next Steps

- [API Usage Examples](../api-usage-examples.md) — more patterns
- [API Reference](../api-reference/INDEX.md) — full signatures
- [Behaviour Docs](../behaviour/INDEX.md) — detailed behaviours
- [Troubleshooting](../troubleshooting.md) — common issues

## Requirements

- .NET 10.0 SDK or runtime
- Terminal with ANSI colour support (most modern terminals)
- Interactive console (not redirected)

## Licence

GPL-3.0-or-later — see [LICENSE](../LICENSE)