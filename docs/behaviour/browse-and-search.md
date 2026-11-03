# Browse and Search Behaviour

NuciCLI provides basic console navigation and discovery capabilities through cursor/buffer properties and key reading.

## Cursor Navigation

### Reading Cursor Position

```csharp
int x = NuciConsole.CursorX;  // Column (0-based)
int y = NuciConsole.CursorY;  // Row (0-based)
```

**Use Cases**:
- Save/restore cursor position for temporary output
- Build TUI layouts with precise positioning
- Implement scrolling regions

### Setting Cursor Position

```csharp
NuciConsole.CursorX = 10;  // Move to column 10
NuciConsole.CursorY = 5;   // Move to row 5
```

**Behaviour**:
- Immediate effect on subsequent output
- Validates bounds via `Console.SetCursorPosition`
- Throws `ArgumentOutOfRangeException` if out of buffer bounds

### Cursor Size

```csharp
int size = NuciConsole.CursorSize;  // 1-100 (percentage of cell height)
```

**Read-only**. Reflects `Console.CursorSize`.

## Buffer Discovery

### Buffer Dimensions

```csharp
int width  = NuciConsole.Width;   // Buffer columns
int height = NuciConsole.Height;  // Buffer rows
int area   = NuciConsole.Area;    // Width * Height
```

**Use Cases**:
- Layout calculations for full-buffer rendering
- Centering content: `CursorX = (Width - text.Length) / 2`
- Detecting buffer vs window size mismatch

### Buffer vs Window

| Property | NuciCLI Access | Description |
|----------|----------------|-------------|
| Buffer Width | `Width` | Total columns in buffer |
| Buffer Height | `Height` | Total rows in buffer |
| Window Width | Not exposed | Visible columns |
| Window Height | Not exposed | Visible rows |
| Window Left | Not exposed | Buffer column at window left |
| Window Top | Not exposed | Buffer row at window top |

**Limitation**: No access to `Console.WindowWidth`, `Console.WindowHeight`, `Console.WindowLeft`, `Console.WindowTop`.

## Key-Based Navigation

### Reading Single Keys

```csharp
ConsoleKeyInfo key = NuciConsole.ReadKey();           // No prompt
ConsoleKeyInfo key = NuciConsole.ReadKey("Navigate: "); // With prompt
```

**Returns**: Full `ConsoleKeyInfo` with:
- `Key` — `ConsoleKey` enum (ArrowUp, Enter, Escape, etc.)
- `Modifiers` — `ConsoleModifiers` (Shift, Alt, Control)
- `KeyChar` — Unicode char (if printable)

**Use Cases**:
- Arrow key navigation (Up/Down/Left/Right)
- Hotkey handling (F1-F12, Ctrl+X)
- Custom key bindings

### Key Filtering Example

```csharp
ConsoleKeyInfo key = NuciConsole.ReadKey("Select: ");
return key.Key switch
{
    ConsoleKey.UpArrow    => Direction.Up,
    ConsoleKey.DownArrow  => Direction.Down,
    ConsoleKey.LeftArrow  => Direction.Left,
    ConsoleKey.RightArrow => Direction.Right,
    ConsoleKey.Enter      => Direction.Select,
    ConsoleKey.Escape     => Direction.Cancel,
    _                     => Direction.None
};
```

## Search Behaviour

### No Built-in Search

NuciCLI does **not** provide:
- Incremental search/history search
- Text search in buffer
- Command palette / fuzzy finder
- Completion/tab completion

### Building Search with NuciCLI

Consumers can build search using primitives:

```csharp
string query = "";
while (true)
{
    NuciConsole.CursorX = 0;
    NuciConsole.Write($"Search: {query}", NuciConsoleColour.Cyan);
    NuciConsole.WriteLine(new string(' ', NuciConsole.Width - query.Length - 8));

    var key = NuciConsole.ReadKey();

    if (key.Key == ConsoleKey.Enter) break;
    if (key.Key == ConsoleKey.Escape) { query = ""; break; }
    if (key.Key == ConsoleKey.Backspace && query.Length > 0)
        query = query[..^1];
    else if (!char.IsControl(key.KeyChar))
        query += key.KeyChar;

    // Filter results with query...
}
```

## Colour-Assisted Navigation

### Highlighting Current Position

```csharp
void HighlightLine(int line, string text, bool selected)
{
    NuciConsole.CursorY = line;
    NuciConsole.CursorX = 0;
    if (selected)
        NuciConsole.WriteLine($"► {text}", NuciConsoleColour.Black, NuciConsoleColour.White);
    else
        NuciConsole.WriteLine($"  {text}", NuciConsoleColour.White, NuciConsoleColour.Default);
}
```

### Status Bar

```csharp
void ShowStatus(string message)
{
    int savedY = NuciConsole.CursorY;
    NuciConsole.CursorY = NuciConsole.Height - 1;
    NuciConsole.CursorX = 0;
    NuciConsole.WriteLine(message.PadRight(NuciConsole.Width), NuciConsoleColour.White, NuciConsoleColour.DarkBlue);
    NuciConsole.CursorY = savedY;
}
```

## Limitations

| Limitation | Workaround |
|------------|------------|
| No window size access | Assume buffer = window or use native interop |
| No resize events | Poll `Width`/`Height` periodically |
| No raw mode / ANSI control | Use `Console` directly for escape sequences |
| No mouse input | Not supported by `Console.ReadKey` |
| No key hold/repeat detection | Single `ReadKey` per press |

## Related Documentation

- [flows/startup-and-rendering.md](../flows/startup-and-rendering.md#cursor-rendering)
- [components/presentation.md](../components/presentation.md#cursor--buffer-properties)
- [behaviour/inspect-edit-delete.md](./inspect-edit-delete.md)