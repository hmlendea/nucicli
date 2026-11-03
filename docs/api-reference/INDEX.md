# API Reference Index

## Overview

NuciCLI exposes a static façade API through two main types:
- `NuciConsole` — Console interaction operations
- `NuciConsoleColour` — Colour value objects

All members are `static`. No instantiation required.

## Endpoint Summary Table

| Method/Property | Type | Category | Parameters | Returns | Exceptions |
|-----------------|------|----------|------------|---------|------------|
| `CursorX` | Property | Cursor | get/set `int` | `int` | `ArgumentOutOfRangeException` |
| `CursorY` | Property | Cursor | get/set `int` | `int` | `ArgumentOutOfRangeException` |
| `CursorSize` | Property | Cursor | get | `int` | — |
| `Width` | Property | Buffer | get | `int` | — |
| `Height` | Property | Buffer | get | `int` | — |
| `Area` | Property | Buffer | get | `int` | — |
| `ReadKey()` | Method | Input | — | `ConsoleKeyInfo` | `IOException` |
| `ReadKey(prompt)` | Method | Input | `string` | `ConsoleKeyInfo` | `IOException` |
| `ReadKey(prompt, fg)` | Method | Input | `string`, `NuciConsoleColour` | `ConsoleKeyInfo` | `IOException` |
| `ReadKey(prompt, fg, bg)` | Method | Input | `string`, `NuciConsoleColour`, `NuciConsoleColour` | `ConsoleKeyInfo` | `IOException` |
| `ReadLine()` | Method | Input | — | `string` | `InputCancellationException`, `IOException` |
| `ReadLine(prompt)` | Method | Input | `string` | `string` | `InputCancellationException`, `IOException` |
| `ReadLine(prompt, fg)` | Method | Input | `string`, `NuciConsoleColour` | `string` | `InputCancellationException`, `IOException` |
| `ReadLine(prompt, fg, bg)` | Method | Input | `string`, `NuciConsoleColour`, `NuciConsoleColour` | `string` | `InputCancellationException`, `IOException` |
| `ReadPermission(prompt)` | Method | Input | `string` | `bool` | `IOException` |
| `ReadPermission(prompt, default)` | Method | Input | `string`, `bool` | `bool` | `IOException` |
| `ReadPermission(prompt, fg)` | Method | Input | `string`, `NuciConsoleColour` | `bool` | `IOException` |
| `ReadPermission(prompt, default, fg)` | Method | Input | `string`, `bool`, `NuciConsoleColour` | `bool` | `IOException` |
| `ReadPermission(prompt, fg, bg)` | Method | Input | `string`, `NuciConsoleColour`, `NuciConsoleColour` | `bool` | `IOException` |
| `ReadPermission(prompt, default, fg, bg)` | Method | Input | `string`, `bool`, `NuciConsoleColour`, `NuciConsoleColour` | `bool` | `IOException` |
| `Write(text)` | Method | Output | `string` | `void` | `IOException` |
| `Write(text, fg)` | Method | Output | `string`, `NuciConsoleColour` | `void` | `IOException` |
| `Write(text, fg, bg)` | Method | Output | `string`, `NuciConsoleColour`, `NuciConsoleColour` | `void` | `IOException` |
| `WriteLine()` | Method | Output | — | `void` | `IOException` |
| `WriteLine(text)` | Method | Output | `string` | `void` | `IOException` |
| `WriteLine(text, fg)` | Method | Output | `string`, `NuciConsoleColour` | `void` | `IOException` |
| `WriteLine(text, fg, bg)` | Method | Output | `string`, `NuciConsoleColour`, `NuciConsoleColour` | `void` | `IOException` |
| `WriteLines(lines)` | Method | Output | `IEnumerable<string>` | `void` | `IOException`, `ArgumentNullException` |
| `WriteLines(lines, fg)` | Method | Output | `IEnumerable<string>`, `NuciConsoleColour` | `void` | `IOException`, `ArgumentNullException` |
| `WriteLines(lines, fg, bg)` | Method | Output | `IEnumerable<string>`, `NuciConsoleColour`, `NuciConsoleColour` | `void` | `IOException`, `ArgumentNullException` |

## NuciConsoleColour Static Instances

| Instance | Value | ConsoleColor | Description |
|----------|-------|--------------|-------------|
| `Default` | -1 | `null` | Use current console colour |
| `Black` | 0 | `Black` | |
| `DarkBlue` | 1 | `DarkBlue` | |
| `DarkGreen` | 2 | `DarkGreen` | |
| `DarkCyan` | 3 | `DarkCyan` | |
| `DarkRed` | 4 | `DarkRed` | |
| `DarkMagenta` | 5 | `DarkMagenta` | |
| `DarkYellow` | 6 | `DarkYellow` | |
| `Gray` | 7 | `Gray` | |
| `DarkGray` | 8 | `DarkGray` | |
| `Blue` | 9 | `Blue` | |
| `Green` | 10 | `Green` | |
| `Cyan` | 11 | `Cyan` | |
| `Red` | 12 | `Red` | |
| `Magenta` | 13 | `Magenta` | |
| `Yellow` | 14 | `Yellow` | |
| `White` | 15 | `White` | |

## NuciConsoleColour Properties

| Property | Type | Description |
|----------|------|-------------|
| `Value` | `int` | Numeric code (-1 for Default, 0-15 for colours) |
| `ConsoleColour` | `ConsoleColor?` | Nullable mapped colour (null for Default) |
| `Name` | `string` | Constant name |

## Exception Types

| Exception | Thrown By | Condition |
|-----------|-----------|-----------|
| `InputCancellationException` | `ReadLine()` overloads | User presses Escape during line input |
| `IOException` | All I/O methods | Console redirected, no console available |
| `ArgumentOutOfRangeException` | `CursorX`/`CursorY` setters | Value outside buffer bounds |
| `ArgumentNullException` | `WriteLines` | `lines` parameter is null |

## Authentication/Authorisation

**Not applicable** — NuciCLI is a library, not a network service. No authentication.

## Versioning

- **Current**: 3.0.2
- **Scheme**: Semantic Versioning (Major.Minor.Patch)
- **Breaking Changes**: Major version increment
- **Package**: `NuciCLI` on NuGet.org

## Rate Limiting/Throttling

**Not applicable** — Local library, no rate limits.

## Links to Controller Files

- [NuciConsole API](./nuci-console.md) — Complete `NuciConsole` documentation
- [NuciConsoleColour API](./nuci-console-colour.md) — Complete `NuciConsoleColour` documentation

## Related Documentation

- [components/presentation.md](../components/presentation.md) — Implementation deep dive
- [flows/startup-and-rendering.md](../flows/startup-and-rendering.md) — Rendering flows
- [flows/detail-mutation.md](../flows/detail-mutation.md) — Input mutation flows
- [behaviour/browse-and-search.md](../behaviour/browse-and-search.md) — Navigation behaviour
- [behaviour/inspect-edit-delete.md](../behaviour/inspect-edit-delete.md) — Editing behaviour
- [error-handling.md](../error-handling.md) — Error taxonomy
- [api-usage-examples.md](../api-usage-examples.md) — Usage examples