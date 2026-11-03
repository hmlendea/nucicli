# Repository Overview

## Purpose

NuciCLI is a lightweight .NET library for building interactive command-line applications. It provides a minimal, focused API wrapping common console operations:

- Prompted text input (`ReadLine`)
- Single-key input (`ReadKey`)
- Yes/no confirmation prompts (`ReadPermission`)
- Coloured output (`Write`, `WriteLine`, `WriteLines`)
- Cursor position and console buffer dimension access (`CursorX`, `CursorY`, `Width`, `Height`, `Area`, `CursorSize`)

The library is intentionally small and works well as a base layer for richer CLI tooling.

## Scope

### In Scope
- Core console interaction primitives
- Colour abstraction over `ConsoleColor`
- Input cancellation via Escape key
- Single assembly with no runtime dependencies beyond BCL + NuciExtensions

### Out of Scope
- Argument parsing / command routing
- TUI layout / widget systems
- Async/await APIs
- Configuration management
- Logging/telemetry
- Cross-platform terminal capabilities (ANSI sequences, raw mode)

## Entry Points

### Primary: NuciConsole
```csharp
using NuciCLI;

NuciConsole.WriteLine("Hello", NuciConsoleColour.Cyan);
string name = NuciConsole.ReadLine("Name: ", NuciConsoleColour.Yellow);
bool ok = NuciConsole.ReadPermission("Continue?", defaultValue: true);
```

### Secondary: NuciConsoleColour
```csharp
var red = NuciConsoleColour.Red;
var custom = new NuciConsoleColour(12, "CustomRed"); // not public constructor
```

### Exception: InputCancellationException
```csharp
try {
    string input = NuciConsole.ReadLine("Input: ");
} catch (InputCancellationException) {
    // user pressed Escape
}
```

## Principal Capabilities

| Capability | Methods | Notes |
|------------|---------|-------|
| **Coloured Output** | `Write`, `WriteLine`, `WriteLines` | Foreground + background, per-call |
| **Line Input** | `ReadLine` (4 overloads) | Prompt + colours, Escape → exception |
| **Key Input** | `ReadKey` (4 overloads) | Prompt + colours, non-intercepted |
| **Confirmation** | `ReadPermission` (6 overloads) | Y/N/Enter, default value, colours |
| **Cursor Control** | `CursorX`, `CursorY` (get/set) | Direct `Console` passthrough |
| **Buffer Info** | `Width`, `Height`, `Area`, `CursorSize` | Read-only `Console` passthrough |

## Component Relationships

```
┌─────────────────────────────────────┐
│         Consumer Application        │
└──────────────────┬──────────────────┘
                   │ references
                   ▼
┌─────────────────────────────────────┐
│            NuciCLI                  │
│  ┌───────────────────────────────┐  │
│  │ NuciConsole (static façade)   │  │
│  │ NuciConsoleColour (values)    │  │
│  │ InputCancellationException    │  │
│  └───────────────────────────────┘  │
└──────────────────┬──────────────────┘
                   │ wraps/uses
         ┌─────────┴─────────┐
         ▼                   ▼
┌─────────────────┐  ┌─────────────────┐
│ System.Console  │  │ NuciExtensions  │
│ (BCL)           │  │ (IsNullOrEmpty) │
└─────────────────┘  └─────────────────┘
```

## Runtime Topology

- **Single process**, single thread (typically)
- **No background threads**, no timers, no async tasks
- **Global console state** shared with host process
- **No persistence**, no network, no file I/O

## Major State

| State | Location | Mutability |
|-------|----------|------------|
| Cursor position | `System.Console` | Mutable via `CursorX`/`CursorY` |
| Foreground/Background colour | `System.Console` | Mutable via `Write*` methods |
| Buffer dimensions | `System.Console` | Read-only (OS-controlled) |
| Input buffer | `System.Console` | Managed by OS/terminal |

NuciCLI itself holds **no instance state** — all state is in `System.Console`.

## Major Integrations

| Integration | Type | Purpose |
|-------------|------|---------|
| `System.Console` | BCL | Low-level I/O, cursor, colours |
| `NuciExtensions` | NuGet | `IsNullOrEmpty` extension methods |

## System-Wide Invariants

1. **Statelessness**: `NuciConsole` has no instance fields; all operations delegate to `System.Console`
2. **Colour Restoration**: `Write(text, fg, bg)` always restores previous console colours
3. **Ctrl+C Handling**: `PerformKeyRead` temporarily enables `TreatControlCAsInput`, always restores
4. **Escape Cancellation**: Only `ReadLine` throws `InputCancellationException` on Escape
5. **Default Colour**: `NuciConsoleColour.Default` (Value=-1) means "do not change current colour"

## Version & Distribution

- **Current Version**: 3.0.2
- **Target Framework**: net10.0
- **NuGet Package**: `NuciCLI`
- **License**: GPL-3.0-or-later
- **Repository**: https://github.com/hmlendea/nucicli
- **Author**: Horațiu Mlendea