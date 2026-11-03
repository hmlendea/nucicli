# NuciConsole API Reference

**Namespace**: `NuciCLI`
**Type**: `public static class`
**Assembly**: `NuciCLI.dll`
**Since**: v1.0.0

## Cursor & Buffer Properties

### CursorX

```csharp
public static int CursorX { get; set; }
```

**Gets/Sets** the horizontal cursor position (column, 0-based).

**Getter**: Returns `Console.CursorLeft`.

**Setter**: Calls `Console.SetCursorPosition(value, CursorY)`.

**Exceptions**:
- `ArgumentOutOfRangeException` — `value` < 0 or ≥ `Console.BufferWidth`
- `IOException` — No console available

**Example**:
```csharp
int col = NuciConsole.CursorX;  // Read
NuciConsole.CursorX = 10;       // Write
```

### CursorY

```csharp
public static int CursorY { get; set; }
```

**Gets/Sets** the vertical cursor position (row, 0-based).

**Getter**: Returns `Console.CursorTop`.

**Setter**: Calls `Console.SetCursorPosition(CursorX, value)`.

**Exceptions**:
- `ArgumentOutOfRangeException` — `value` < 0 or ≥ `Console.BufferHeight`
- `IOException` — No console available

### CursorSize

```csharp
public static int CursorSize { get; }
```

**Gets** the cursor height as percentage of cell height (1-100).

**Returns**: `Console.CursorSize`.

**Exceptions**: `IOException` — No console available.

### Width

```csharp
public static int Width { get; }
```

**Gets** the console buffer width in columns.

**Returns**: `Console.BufferWidth`.

**Exceptions**: `IOException` — No console available.

### Height

```csharp
public static int Height { get; }
```

**Gets** the console buffer height in rows.

**Returns**: `Console.BufferHeight`.

**Exceptions**: `IOException` — No console available.

### Area

```csharp
public static int Area { get; }
```

**Gets** the total buffer cell count (`Width * Height`).

**Returns**: `Width * Height`.

**Exceptions**: `IOException` — No console available.

---

## Key Input Methods

### ReadKey()

```csharp
public static ConsoleKeyInfo ReadKey()
```

Reads a key without prompt.

**Returns**: `ConsoleKeyInfo` of pressed key.

**Exceptions**: `IOException` — No console available.

**Flow**: `PerformKeyRead(false)` — key echoed by terminal.

---

### ReadKey(string prompt)

```csharp
public static ConsoleKeyInfo ReadKey(string prompt)
```

Reads a key with prompt (default colours).

**Parameters**:
- `prompt` — Text to display before reading.

**Returns**: `ConsoleKeyInfo` of pressed key.

**Exceptions**: `IOException` — No console available.

**Flow**: `Write(prompt)` → `PerformKeyRead(false)`.

---

### ReadKey(string prompt, NuciConsoleColour foregroundColour)

```csharp
public static ConsoleKeyInfo ReadKey(string prompt, NuciConsoleColour foregroundColour)
```

Reads a key with prompt and foreground colour.

**Parameters**:
- `prompt` — Text to display.
- `foregroundColour` — Prompt text colour.

**Returns**: `ConsoleKeyInfo` of pressed key.

**Exceptions**: `IOException` — No console available.

**Flow**: `Write(prompt, foregroundColour)` → `PerformKeyRead(false)`.

---

### ReadKey(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)

```csharp
public static ConsoleKeyInfo ReadKey(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
```

Reads a key with prompt, foreground and background colours.

**Parameters**:
- `prompt` — Text to display.
- `foregroundColour` — Prompt text colour.
- `backgroundColour` — Prompt background colour.

**Returns**: `ConsoleKeyInfo` of pressed key.

**Exceptions**: `IOException` — No console available.

**Flow**: `Write(prompt, foregroundColour, backgroundColour)` → `PerformKeyRead(false)`.

---

## Line Input Methods

### ReadLine()

```csharp
public static string ReadLine()
```

Reads a line without prompt.

**Returns**: Input string (empty if Enter pressed immediately).

**Exceptions**:
- `InputCancellationException` — User pressed Escape
- `IOException` — No console available

**Flow**: `PerformKeyRead(true)` loop.

---

### ReadLine(string prompt)

```csharp
public static string ReadLine(string prompt)
```

Reads a line with prompt (default colours).

**Parameters**:
- `prompt` — Text to display before reading.

**Returns**: Input string.

**Exceptions**:
- `InputCancellationException` — User pressed Escape
- `IOException` — No console available

---

### ReadLine(string prompt, NuciConsoleColour foregroundColour)

```csharp
public static string ReadLine(string prompt, NuciConsoleColour foregroundColour)
```

Reads a line with prompt and foreground colour.

**Parameters**:
- `prompt` — Text to display.
- `foregroundColour` — Prompt text colour.

**Returns**: Input string.

**Exceptions**:
- `InputCancellationException` — User pressed Escape
- `IOException` — No console available

---

### ReadLine(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)

```csharp
public static string ReadLine(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
```

Reads a line with prompt, foreground and background colours.

**Parameters**:
- `prompt` — Text to display.
- `foregroundColour` — Prompt text colour.
- `backgroundColour` — Prompt background colour.

**Returns**: Input string.

**Exceptions**:
- `InputCancellationException` — User pressed Escape
- `IOException` — No console available

**Behaviour Details**:
1. Writes prompt with colours
2. Loops reading keys via `PerformKeyRead(true)` (intercepted)
3. **Escape**: Clears line, throws `InputCancellationException`
4. **Enter**: Returns accumulated input
5. **Backspace**: Removes last char, updates cursor
6. **Valid char** (letter, digit, symbol, punctuation, whitespace): Appends, echoes
7. **Other keys**: Ignored

---

## Confirmation Input Methods

### ReadPermission(string prompt)

```csharp
public static bool ReadPermission(string prompt)
```

Reads confirmation with prompt, default `false`.

**Parameters**:
- `prompt` — Question text.

**Returns**: `true` for Y, `false` for N/Enter.

**Exceptions**: `IOException` — No console available.

---

### ReadPermission(string prompt, bool defaultValue)

```csharp
public static bool ReadPermission(string prompt, bool defaultValue)
```

Reads confirmation with prompt and default value.

**Parameters**:
- `prompt` — Question text.
- `defaultValue` — Returned on Enter.

**Returns**: `true` for Y, `false` for N, `defaultValue` for Enter.

**Exceptions**: `IOException` — No console available.

---

### ReadPermission(string prompt, NuciConsoleColour foregroundColour)

```csharp
public static bool ReadPermission(string prompt, NuciConsoleColour foregroundColour)
```

Reads confirmation with prompt and foreground colour (default `false`).

**Parameters**:
- `prompt` — Question text.
- `foregroundColour` — Prompt text colour.

**Returns**: `true` for Y, `false` for N/Enter.

**Exceptions**: `IOException` — No console available.

---

### ReadPermission(string prompt, bool defaultValue, NuciConsoleColour foregroundColour)

```csharp
public static bool ReadPermission(string prompt, bool defaultValue, NuciConsoleColour foregroundColour)
```

Reads confirmation with prompt, default, and foreground colour.

**Parameters**:
- `prompt` — Question text.
- `defaultValue` — Returned on Enter.
- `foregroundColour` — Prompt text colour.

**Returns**: `true` for Y, `false` for N, `defaultValue` for Enter.

**Exceptions**: `IOException` — No console available.

---

### ReadPermission(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)

```csharp
public static bool ReadPermission(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
```

Reads confirmation with prompt, foreground and background colours (default `false`).

**Parameters**:
- `prompt` — Question text.
- `foregroundColour` — Prompt text colour.
- `backgroundColour` — Prompt background colour.

**Returns**: `true` for Y, `false` for N/Enter.

**Exceptions**: `IOException` — No console available.

---

### ReadPermission(string prompt, bool defaultValue, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)

```csharp
public static bool ReadPermission(string prompt, bool defaultValue, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
```

Reads confirmation with all options.

**Parameters**:
- `prompt` — Question text.
- `defaultValue` — Returned on Enter.
- `foregroundColour` — Prompt text colour.
- `backgroundColour` — Prompt background colour.

**Returns**: `true` for Y, `false` for N, `defaultValue` for Enter.

**Exceptions**: `IOException` — No console available.

**Behaviour Details**:
1. Constructs suffix: `" (Y/n)"` if default=false, `" (y/N)"` if default=true
2. Loops: `ReadKey(fullPrompt, fg, bg)`
3. **Y**: `WriteLine()`, return `true`
4. **N**: `WriteLine()`, return `false`
5. **Enter**: `WriteLine()`, return `defaultValue`
6. **Other**: `CursorX -= 1`, continue loop

---

## Output Methods

### Write(string text)

```csharp
public static void Write(string text)
```

Writes text with current console colours.

**Parameters**:
- `text` — Text to write.

**Exceptions**: `IOException` — No console available.

---

### Write(string text, NuciConsoleColour foregroundColour)

```csharp
public static void Write(string text, NuciConsoleColour foregroundColour)
```

Writes text with foreground colour.

**Parameters**:
- `text` — Text to write.
- `foregroundColour` — Text colour.

**Exceptions**: `IOException` — No console available.

---

### Write(string text, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)

```csharp
public static void Write(string text, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
```

Writes text with foreground and background colours.

**Parameters**:
- `text` — Text to write.
- `foregroundColour` — Text colour.
- `backgroundColour` — Background colour.

**Exceptions**: `IOException` — No console available.

**Implementation**:
```csharp
ConsoleColor oldFg = Console.ForegroundColor;
ConsoleColor oldBg = Console.BackgroundColor;
Console.ResetColor();
if (foregroundColour.ConsoleColour != null) Console.ForegroundColor = foregroundColour.ConsoleColour.Value;
if (backgroundColour.ConsoleColour != null) Console.BackgroundColor = backgroundColour.ConsoleColour.Value;
Console.Write(text);
Console.ForegroundColor = oldFg;
Console.BackgroundColor = oldBg;
```

**Note**: Colours restored sequentially (not in `finally`). If `Console.Write` throws, colours may not restore.

---

### WriteLine()

```csharp
public static void WriteLine()
```

Writes empty line (`Environment.NewLine`).

**Exceptions**: `IOException` — No console available.

---

### WriteLine(string text)

```csharp
public static void WriteLine(string text)
```

Writes text + newline with current colours.

**Parameters**:
- `text` — Text to write.

**Exceptions**: `IOException` — No console available.

**Implementation**: `Console.WriteLine(text)`.

---

### WriteLine(string text, NuciConsoleColour foregroundColour)

```csharp
public static void WriteLine(string text, NuciConsoleColour foregroundColour)
```

Writes text + newline with foreground colour.

**Parameters**:
- `text` — Text to write.
- `foregroundColour` — Text colour.

**Exceptions**: `IOException` — No console available.

**Implementation**: `WriteLine(text, foregroundColour, Default)`.

---

### WriteLine(string text, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)

```csharp
public static void WriteLine(string text, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
```

Writes text + newline with both colours.

**Parameters**:
- `text` — Text to write.
- `foregroundColour` — Text colour.
- `backgroundColour` — Background colour.

**Exceptions**: `IOException` — No console available.

**Implementation**: `Write(text + Environment.NewLine, foregroundColour, backgroundColour)`.

---

### WriteLines(IEnumerable<string> lines)

```csharp
public static void WriteLines(IEnumerable<string> lines)
```

Writes multiple lines with current colours.

**Parameters**:
- `lines` — Lines to write.

**Exceptions**:
- `IOException` — No console available
- `ArgumentNullException` — `lines` is null

**Implementation**: `foreach (var line in lines) WriteLine(line);`

---

### WriteLines(IEnumerable<string> lines, NuciConsoleColour foregroundColour)

```csharp
public static void WriteLines(IEnumerable<string> lines, NuciConsoleColour foregroundColour)
```

Writes multiple lines with foreground colour.

**Parameters**:
- `lines` — Lines to write.
- `foregroundColour` — Text colour for all lines.

**Exceptions**:
- `IOException` — No console available
- `ArgumentNullException` — `lines` is null

---

### WriteLines(IEnumerable<string> lines, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)

```csharp
public static void WriteLines(IEnumerable<string> lines, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
```

Writes multiple lines with both colours.

**Parameters**:
- `lines` — Lines to write.
- `foregroundColour` — Text colour for all lines.
- `backgroundColour` — Background colour for all lines.

**Exceptions**:
- `IOException` — No console available
- `ArgumentNullException` — `lines` is null

---

## Private Methods

### PerformKeyRead()

```csharp
private static ConsoleKeyInfo PerformKeyRead()
    => PerformKeyRead(false);
```

### PerformKeyRead(bool intercept)

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

**Parameters**:
- `intercept` — `true` = key not displayed, `false` = key displayed

**Returns**: `ConsoleKeyInfo` of pressed key.

**Purpose**: Ensures Ctrl+C returns as key instead of terminating process.

**Thread Safety**: NOT thread-safe (global `TreatControlCAsInput`).

---

## Error Scenarios

| Scenario | Method | Exception | Handling |
|----------|--------|-----------|----------|
| User presses Escape | `ReadLine` | `InputCancellationException` | Catch and handle |
| Console not available | Any I/O | `IOException` | Check `Console.IsOutputRedirected` |
| Cursor out of bounds | `CursorX`/`CursorY` set | `ArgumentOutOfRangeException` | Validate before set |
| Null lines | `WriteLines` | `ArgumentNullException` | Check before call |
| Ctrl+C during read | `ReadKey`/`ReadLine` | Returns as key | Handle in caller |

---

## Flow References

- [Startup and Rendering](../flows/startup-and-rendering.md)
- [Detail Mutation](../flows/detail-mutation.md)

---

## Related Documentation

- [API Reference Index](./INDEX.md)
- [NuciConsoleColour API](./nuci-console-colour.md)
- [Presentation Component](../components/presentation.md)
- [Error Handling](../error-handling.md)