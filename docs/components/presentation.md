# Presentation Layer: NuciConsole & NuciConsoleColour

## NuciConsole

**Location**: `NuciCLI/NuciConsole.cs`
**Namespace**: `NuciCLI`
**Type**: `public static class`

### Purpose

Primary entry point for all console interactions. Static façade over `System.Console` providing:
- Coloured output with automatic colour restoration
- Prompted input (line, key, confirmation)
- Cursor and buffer dimension access
- Escape-key cancellation for line input

### Public API Surface

#### Cursor & Buffer Properties

| Property | Type | Get/Set | Implementation |
|----------|------|---------|----------------|
| `CursorX` | `int` | Get/Set | `Console.CursorLeft` / `Console.SetCursorPosition` |
| `CursorY` | `int` | Get/Set | `Console.CursorTop` / `Console.SetCursorPosition` |
| `CursorSize` | `int` | Get | `Console.CursorSize` |
| `Width` | `int` | Get | `Console.BufferWidth` |
| `Height` | `int` | Get | `Console.BufferHeight` |
| `Area` | `int` | Get | `Width * Height` |

#### Key Input: `ReadKey` Overloads

```csharp
ReadKey()                                    // no prompt
ReadKey(string prompt)                       // prompt, default colours
ReadKey(string prompt, NuciConsoleColour fg) // prompt + foreground
ReadKey(string prompt, NuciConsoleColour fg, NuciConsoleColour bg) // prompt + both colours
```

**Behaviour**:
1. Writes prompt via `Write(prompt, fg, bg)`
2. Calls `PerformKeyRead(false)` — non-intercepted read
3. Returns `ConsoleKeyInfo` directly

**No cancellation support** — Escape returns as normal key.

#### Line Input: `ReadLine` Overloads

```csharp
ReadLine()                                    // no prompt
ReadLine(string prompt)                       // prompt, default colours
ReadLine(string prompt, NuciConsoleColour fg) // prompt + foreground
ReadLine(string prompt, NuciConsoleColour fg, NuciConsoleColour bg) // prompt + both
```

**Behaviour**:
1. Writes prompt via `Write(prompt, fg, bg)`
2. Calls `PerformKeyRead(true)` — intercepted read (key not displayed)
3. Loop until Enter or Escape:
   - **Escape**: Restore cursor, clear line, throw `InputCancellationException`
   - **Enter**: Write newline, break, return accumulated input
   - **Backspace**: If input not empty, remove last char, move cursor back, overwrite with space, move cursor back
   - **Valid char** (letter, digit, symbol, punctuation, whitespace): Append to input, `Console.Write(char)`
4. Returns accumulated string

**Validation**: Only accepts `char.IsLetterOrDigit`, `IsSymbol`, `IsPunctuation`, `IsWhiteSpace`. Control keys (except Enter/Backspace/Escape) ignored.

#### Confirmation Input: `ReadPermission` Overloads

```csharp
ReadPermission(string prompt)
ReadPermission(string prompt, bool defaultValue)
ReadPermission(string prompt, NuciConsoleColour fg)
ReadPermission(string prompt, bool defaultValue, NuciConsoleColour fg)
ReadPermission(string prompt, NuciConsoleColour fg, NuciConsoleColour bg)
ReadPermission(string prompt, bool defaultValue, NuciConsoleColour fg, NuciConsoleColour bg)
```

**Behaviour**:
1. Constructs prompt suffix: `" (Y/n)"` if default=false, `" (y/N)"` if default=true
2. Calls `ReadKey(fullPrompt, fg, bg)` in loop
3. Switch on key:
   - **Y**: `WriteLine()`, return `true`
   - **N**: `WriteLine()`, return `false`
   - **Enter**: `WriteLine()`, return `defaultValue`
   - **Other**: `CursorX -= 1` (back up over invalid key), continue loop
4. Infinite loop until valid input

#### Output: `Write`, `WriteLine`, `WriteLines` Overloads

```csharp
// Write
Write(string text)
Write(string text, NuciConsoleColour fg)
Write(string text, NuciConsoleColour fg, NuciConsoleColour bg)

// WriteLine
WriteLine()
WriteLine(string text)
WriteLine(string text, NuciConsoleColour fg)
WriteLine(string text, NuciConsoleColour fg, NuciConsoleColour bg)

// WriteLines
WriteLines(IEnumerable<string> lines)
WriteLines(IEnumerable<string> lines, NuciConsoleColour fg)
WriteLines(IEnumerable<string> lines, NuciConsoleColour fg, NuciConsoleColour bg)
```

**Core Implementation** (`Write(text, fg, bg)`):
```csharp
ConsoleColor oldFg = Console.ForegroundColor;
ConsoleColor oldBg = Console.BackgroundColor;
Console.ResetColor();
if (fg.ConsoleColour != null) Console.ForegroundColor = fg.ConsoleColour.Value;
if (bg.ConsoleColour != null) Console.BackgroundColor = bg.ConsoleColour.Value;
Console.Write(text);
Console.ForegroundColor = oldFg;
Console.BackgroundColor = oldBg;
```

**Key Properties**:
- Always saves/restores console colours (even on exception)
- `Default` colour (Value=-1, ConsoleColour=null) → no change
- `WriteLine` = `Write(text + Environment.NewLine, ...)`
- `WriteLines` = foreach `WriteLine(line, ...)`

### Private Implementation

#### `PerformKeyRead(bool intercept)`

```csharp
private static ConsoleKeyInfo PerformKeyRead(bool intercept)
{
    bool previousCtrlCBehaviour = Console.TreatControlCAsInput;
    Console.TreatControlCAsInput = true;
    ConsoleKeyInfo inputValue = Console.ReadKey(intercept);
    Console.TreatControlCAsInput = previousCtrlCBehaviour;
    return inputValue;
}
```

**Purpose**: Read key with `TreatControlCAsInput = true` so Ctrl+C returns as key instead of terminating process.

**Parameters**:
- `intercept`: `true` = key not displayed (used by `ReadLine`), `false` = key displayed (used by `ReadKey`)

**Thread Safety**: NOT thread-safe. `TreatControlCAsInput` is process-global.

### Colour Handling

All colour parameters accept `NuciConsoleColour`:
- `NuciConsoleColour.Default` → `ConsoleColour = null` → no colour change
- `NuciConsoleColour.Red` etc. → `ConsoleColour = ConsoleColor.Red` → sets colour
- Background colour independent of foreground

### Error Handling

| Method | Exceptions Thrown |
|--------|-------------------|
| `ReadLine` | `InputCancellationException` (Escape), BCL exceptions |
| `ReadKey` | BCL exceptions only |
| `ReadPermission` | BCL exceptions only |
| `Write*` | BCL exceptions only |
| Properties | BCL exceptions only |

No custom exceptions except `InputCancellationException`.

### Thread Safety

**Not thread-safe**. All operations delegate to `System.Console` which is not thread-safe. Concurrent calls from multiple threads will interleave output and corrupt cursor state.

**Consumer Responsibility**: Synchronize access if multi-threaded.

### Performance Characteristics

- **Allocations**: Minimal. `NuciConsoleColour` static instances reused. String concatenation in `ReadLine` loop (one per char).
- **Syscalls**: Each `Console.Write`/`ReadKey` crosses managed/unmanaged boundary.
- **Colour Save/Restore**: 4 property accesses per `Write` call.

## NuciConsoleColour

**Location**: `NuciCLI/NuciConsoleColour.cs`
**Namespace**: `NuciCLI`
**Type**: `public sealed class`

### Purpose

Immutable value object representing a console colour with:
- Numeric value (-1 for Default, 0-15 for ConsoleColor)
- Nullable `ConsoleColor` mapping
- Human-readable name

### Public API

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Value` | `int` | -1 = Default, 0-15 = ConsoleColor enum value |
| `ConsoleColour` | `ConsoleColor?` | Null for Default, else mapped enum |
| `Name` | `string` | Constant name (e.g., "Red", "Default") |

#### Static Instances (17)

```csharp
Default  // Value=-1, ConsoleColour=null
Black    // 0
DarkBlue // 1
DarkGreen // 2
DarkCyan // 3
DarkRed  // 4
DarkMagenta // 5
DarkYellow // 6
Gray     // 7
DarkGray // 8
Blue     // 9
Green    // 10
Cyan     // 11
Red      // 12
Magenta  // 13
Yellow   // 14
White    // 15
```

#### Constructor

```csharp
private NuciConsoleColour(int value, string name)
```
**Private** — only static instances creatable.

### Implementation Details

```csharp
NuciConsoleColour(int value, string name)
{
    Value = value;
    Name = name;
    if (Value.Equals(-1))
    {
        ConsoleColour = null;
    }
    else
    {
        ConsoleColour = (ConsoleColor)value;
    }
}
```

- Immutable (no setters, readonly fields effectively)
- `Value` and `Name` set once in constructor
- `ConsoleColour` computed once

### Usage in NuciConsole

```csharp
// In Write(text, fg, bg):
if (foregroundColour.ConsoleColour is not null)
    Console.ForegroundColor = foregroundColour.ConsoleColour.Value;
// Default (null) → skip, leave current colour unchanged
```

### Design Rationale

See [design-decisions.md](./design-decisions.md#2-nuciConsoleColour-as-value-objects-instead-of-consolecolor-directly).

### Extensibility

**Not extensible** — sealed class, private constructor, fixed 17 instances. Consumers cannot create custom colours (would require `ConsoleColor` values outside 0-15).

## InputCancellationException

**Location**: `NuciCLI/InputCancellationException.cs`
**Namespace**: `NuciCLI`
**Type**: `public class` (inherits `Exception`)

### Purpose

Signals user cancelled line input by pressing Escape.

### Constructors

```csharp
InputCancellationException()
InputCancellationException(string message)
InputCancellationException(string message, Exception innerException)
```

### Usage

Thrown **only** by `ReadLine()` overloads when `ConsoleKey.Escape` detected.

### Handling Pattern

```csharp
try
{
    string input = NuciConsole.ReadLine("Input: ");
}
catch (InputCancellationException)
{
    // User pressed Escape
    NuciConsole.WriteLine("Cancelled.", NuciConsoleColour.DarkYellow);
}
```

### Design Rationale

See [design-decisions.md](./design-decisions.md#3-inputcancellationexception-for-escape-key).