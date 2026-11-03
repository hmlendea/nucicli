# Inspect, Edit, Delete Behaviour

NuciCLI provides line input editing, cancellation, and confirmation behaviours.

## Line Input Editing (ReadLine)

### Editing Capabilities

| Key | Action | Visual Effect |
|-----|--------|---------------|
| Printable chars | Append to input | Character echoed at cursor |
| Backspace | Delete previous char | Cursor back, space overwrite, cursor back |
| Enter | Confirm input | Newline, return input string |
| Escape | Cancel input | Clear line, throw `InputCancellationException` |
| Other keys | Ignored | No effect |

### Editing Flow

```
Prompt: "Name: "
User types: "John"
Buffer: "John" | Cursor after 'n'

User presses Backspace:
  Buffer: "Joh" | Cursor after 'h'
  Console: "Name: Joh " (space overwrote 'n')

User types "n":
  Buffer: "John" | Cursor after 'n'
  Console: "Name: John"

User presses Enter:
  Returns "John"
  Console: "Name: John\n"
```

### Input Buffer Management

**Internal**: `string inputValue = string.Empty;` — immutable string, recreated on each append.

**Performance**: O(n²) allocations for n characters (string concat per char). Acceptable for typical CLI input (<100 chars).

**Optimization Opportunity**: Use `StringBuilder` for longer inputs.

### Visual Feedback

- **Prompt**: Rendered once via `Write(prompt, fg, bg)` before loop
- **Characters**: Echoed via `Console.Write(key.KeyChar)` — respects current console colours
- **Backspace**: Manual cursor manipulation + space overwrite
- **Escape Cancel**: Line cleared via `WriteLine(padRight)`

### Colour During Editing

- Prompt colours applied once at start
- Input characters use **current console colours** (not prompt colours)
- No way to colour input differently from prompt

## Cancellation (Escape)

### Trigger

`ConsoleKey.Escape` during `ReadLine` loop.

### Sequence

```csharp
if (key.Key.Equals(ConsoleKey.Escape))
{
    CursorX -= inputValue.Length;           // Move to prompt end
    WriteLine(string.Empty.PadRight(inputValue.Length)); // Overwrite with spaces
    throw new InputCancellationException(); // Propagate
}
```

### Visual Result

```
Before:  Name: John▌
After:   Name:       ▌  (cursor at prompt end, line cleared)
Exception thrown
```

### Handling Pattern

```csharp
try
{
    string name = NuciConsole.ReadLine("Name: ", NuciConsoleColour.Yellow);
    // Use name...
}
catch (InputCancellationException)
{
    NuciConsole.WriteLine("Input cancelled.", NuciConsoleColour.DarkYellow);
    // Handle cancellation (return, retry, exit)
}
```

### No Partial Recovery

- No "undo cancellation"
- No "save draft" on cancel
- Input buffer discarded on exception

## Confirmation Editing (ReadPermission)

### Editing Model

**Not traditional editing** — single-key decision with visual feedback.

### Key Handling

| Key | Action | Returns |
|-----|--------|---------|
| Y | Confirm | `true` |
| N | Deny | `false` |
| Enter | Accept default | `defaultValue` |
| Other | Ignored (cursor back) | Loop continues |

### Visual Feedback

```
Prompt: "Continue? (Y/n) "
User presses 'X':
  CursorX -= 1  (backs up over 'X')
  Loop repeats
  Prompt re-rendered: "Continue? (Y/n) "
```

**Effect**: Invalid key briefly visible, then overwritten by next `ReadKey` prompt render.

### Default Value Indication

| Default | Prompt Suffix | Visual Hint |
|---------|---------------|-------------|
| `true` | ` (Y/n)` | Capital Y = default |
| `false` | ` (y/N)` | Capital N = default |

### No Cancellation

- Escape key treated as "Other" → cursor back, loop continues
- No way to cancel confirmation without choosing Y/N/Enter
- Infinite loop until valid input

## Delete Behaviour

### No Explicit Delete

NuciCLI has no "delete" operation distinct from:
- **Backspace** in `ReadLine` (delete previous char)
- **Escape** in `ReadLine` (cancel entire input)
- **Overwrite** via cursor positioning + `Write`

### Building Delete with Primitives

```csharp
// Delete line at cursor
NuciConsole.CursorX = 0;
NuciConsole.WriteLine(new string(' ', NuciConsole.Width));

// Delete from cursor to end of line
int remaining = NuciConsole.Width - NuciConsole.CursorX;
NuciConsole.Write(new string(' ', remaining));
NuciConsole.CursorX -= remaining;
```

## Inspect Behaviour

### Runtime Inspection

| Property | Inspects |
|----------|----------|
| `CursorX` | Current column |
| `CursorY` | Current row |
| `Width` | Buffer width |
| `Height` | Buffer height |
| `Area` | Total cells |
| `CursorSize` | Cursor height % |

### Input Inspection

```csharp
// Peek at key without consuming (not directly supported)
// Workaround: ReadKey, then handle or re-inject (not possible)
// Must consume to inspect
ConsoleKeyInfo key = NuciConsole.ReadKey();
if (key.Key == ConsoleKey.F1) ShowHelp();
else ProcessKey(key);
```

### No Buffer Inspection

Cannot read:
- Existing buffer contents
- Character at position
- Line content
- Scrollback history

## Limitations Summary

| Capability | Supported | Notes |
|------------|-----------|-------|
| Character insert | ✅ | At cursor end only |
| Character delete (backspace) | ✅ | Previous char only |
| Word delete (Ctrl+Backspace) | ❌ | Not implemented |
| Line delete (Ctrl+U) | ❌ | Escape cancels entire line |
| Kill to end (Ctrl+K) | ❌ | Not implemented |
| Undo | ❌ | Not implemented |
| History/recall | ❌ | Not implemented |
| Copy/paste | ❌ | Terminal-dependent |
| Selection | ❌ | Not supported |
| Cancel confirmation | ❌ | Must choose Y/N/Enter |

## Related Documentation

- [flows/detail-mutation.md](../flows/detail-mutation.md)
- [components/presentation.md](../components/presentation.md#line-input-readline-overloads)
- [error-handling.md](../error-handling.md#inputcancellationexception)