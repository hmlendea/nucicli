# Startup and Rendering Flow

## Library Initialization

NuciCLI requires **no explicit initialization**. The library is ready on first use.

### Assembly Load Sequence

```
Consumer calls NuciConsole.WriteLine("Hello")
        │
        ▼
.NET Runtime loads NuciCLI.dll
        │
        ▼
NuciConsole static type initializer (none)
        │
        ▼
NuciConsoleColour static type initializer
        │
        ├─► Creates 17 static instances (Default, Black...White)
        │
        ▼
First method call executes
```

### Static Constructor Details

**NuciConsole**: No static constructor (no static fields).

**NuciConsoleColour**: Implicit static initialization of 17 `static readonly` fields:
```csharp
public static NuciConsoleColour Default => new(-1, nameof(Default));
public static NuciConsoleColour Black => new(0, nameof(Black));
// ... 15 more
```
Each `new` calls private constructor, sets `Value`, `Name`, computes `ConsoleColour`.

**Cost**: ~17 small allocations on first access. Negligible.

## Rendering Flow: Write Operations

### Write(text, fg, bg) — Core Path

```
NuciConsole.Write("Hello", Red, Blue)
        │
        ▼
Save Console.ForegroundColor → oldFg
Save Console.BackgroundColor → oldBg
        │
        ▼
Console.ResetColor()
        │
        ▼
fg.ConsoleColour != null? ──Yes──► Console.ForegroundColor = fg.ConsoleColour.Value
        │
        No
        ▼
bg.ConsoleColour != null? ──Yes──► Console.BackgroundColor = bg.ConsoleColour.Value
        │
        No
        ▼
Console.Write("Hello")
        │
        ▼
Console.ForegroundColor = oldFg
Console.BackgroundColor = oldBg
        │
        ▼
Return
```

### Exception Safety

If `Console.Write` throws (e.g., `IOException`):
- `oldFg`/`oldBg` still restored (no `finally` but sequential)
- **Risk**: If `Console.Write` throws before restore, colours left changed
- **Actual code**: Restore after `Write` — not in `finally`
- **Fix needed**: Wrap in `try/finally` for true exception safety

### WriteLine(text, fg, bg)

```
WriteLine("Hello", Red, Blue)
        │
        ▼
Write("Hello" + Environment.NewLine, Red, Blue)
        │
        ▼
(See Write flow above)
```

### WriteLines(lines, fg, bg)

```
WriteLines(["A", "B"], Red, Blue)
        │
        ▼
foreach line in lines:
    WriteLine(line, Red, Blue)
        │
        ▼
(See WriteLine flow above)
```

## Rendering Flow: Prompt + Input

### ReadKey(prompt, fg, bg)

```
ReadKey("Press any key", Yellow, DarkBlue)
        │
        ▼
Write("Press any key", Yellow, DarkBlue)
        │
        ▼
PerformKeyRead(false)
        │
        ├─► Save Console.TreatControlCAsInput
        ├─► Console.TreatControlCAsInput = true
        ├─► Console.ReadKey(false)  // key echoed by terminal
        ├─► Restore TreatControlCAsInput
        │
        ▼
Return ConsoleKeyInfo
```

### ReadLine(prompt, fg, bg) — Full Flow

```
ReadLine("Name: ", Yellow, DarkBlue)
        │
        ▼
Write("Name: ", Yellow, DarkBlue)
        │
        ▼
PerformKeyRead(true)  // intercept = true (key not displayed)
        │
        ▼
Loop:
    │
    ├─► key = PerformKeyRead(true)
    │
    ├─► key.Key == Escape?
    │       │
    │       Yes ──► CursorX -= input.Length
    │       │       WriteLine("".PadRight(input.Length))  // clear line
    │       │       throw InputCancellationException()
    │       │
    │       No
    │       ▼
    ├─► key.Key == Enter?
    │       │
    │       Yes ──► WriteLine()  // newline
    │       │       break loop
    │       │       return input
    │       │
    │       No
    │       ▼
    ├─► key.Key == Backspace && input not empty?
    │       │
    │       Yes ──► input = input[..^1]
    │       │       CursorX -= 1
    │       │       Write(" ")  // overwrite with space
    │       │       CursorX -= 1
    │       │       continue
    │       │
    │       No
    │       ▼
    ├─► IsValidChar(key.KeyChar)?
    │       │
    │       Yes ──► input += key.KeyChar
    │       │       Console.Write(key.KeyChar)  // echo char
    │       │       continue
    │       │
    │       No ──► continue (ignore key)
    │
    ▼ (next iteration)
```

### ReadPermission(prompt, default, fg, bg) — Full Flow

```
ReadPermission("Continue?", true, Green, Black)
        │
        ▼
Loop:
    │
    ├─► suffix = default ? " (Y/n)" : " (y/N)"
    │
    ├─► ReadKey("Continue? (Y/n) ", Green, Black)
    │       │
    │       ▼ (See ReadKey flow)
    │
    ├─► Switch key.Key:
    │       │
    │       ├─► Y ──► WriteLine(); return true
    │       ├─► N ──► WriteLine(); return false
    │       ├─► Enter ──► WriteLine(); return default
    │       └─► Other ──► CursorX -= 1; continue
    │
    ▼ (infinite loop until return)
```

## Colour Rendering Details

### NuciConsoleColour Mapping

| NuciConsoleColour | Value | ConsoleColour | Effect |
|-------------------|-------|---------------|--------|
| `Default` | -1 | `null` | No change (use current) |
| `Black` | 0 | `ConsoleColor.Black` | Set to black |
| `DarkBlue` | 1 | `ConsoleColor.DarkBlue` | Set to dark blue |
| ... | ... | ... | ... |
| `White` | 15 | `ConsoleColor.White` | Set to white |

### Colour Restoration Guarantee

**Current Implementation**: Sequential restore after `Console.Write`.

```csharp
ConsoleColor oldFg = Console.ForegroundColor;
ConsoleColor oldBg = Console.BackgroundColor;
Console.ResetColor();
// ... set new colours ...
Console.Write(text);
Console.ForegroundColor = oldFg;  // ← Not in finally!
Console.BackgroundColor = oldBg;
```

**Gap**: If `Console.Write` throws, colours not restored.

**Recommended Fix**:
```csharp
try {
    ConsoleColor oldFg = Console.ForegroundColor;
    ConsoleColor oldBg = Console.BackgroundColor;
    Console.ResetColor();
    if (fg.ConsoleColour != null) Console.ForegroundColor = fg.ConsoleColour.Value;
    if (bg.ConsoleColour != null) Console.BackgroundColor = bg.ConsoleColour.Value;
    Console.Write(text);
}
finally {
    Console.ForegroundColor = oldFg;
    Console.BackgroundColor = oldBg;
}
```

## Cursor Rendering

### CursorX/CursorY Setters

```csharp
CursorX = 10  →  Console.SetCursorPosition(10, CursorY)
CursorY = 5   →  Console.SetCursorPosition(CursorX, 5)
```

**Race Condition**: Non-atomic X+Y set. If thread sets X then Y, another thread may read intermediate state.

### Cursor Movement in ReadLine

| Operation | Cursor Effect |
|-----------|---------------|
| Valid char | `Console.Write(char)` advances cursor |
| Backspace | `CursorX -= 1`, `Write(" ")`, `CursorX -= 1` |
| Escape cancel | `CursorX -= input.Length`, `WriteLine(pad)`, cursor at line start |

## Buffer Dimension Rendering

### Width/Height/Area

```csharp
Width  → Console.BufferWidth   // columns
Height → Console.BufferHeight  // rows
Area   → Width * Height        // computed
```

**Note**: Buffer ≠ Window. `BufferWidth` may exceed visible window width.

## Performance Profile

| Operation | Allocations | Syscalls | Notes |
|-----------|-------------|----------|-------|
| `Write` | ~2 (colour structs) | 1 `Write` + 4 colour props | Fast |
| `WriteLine` | +1 (newline concat) | Same + newline | Fast |
| `ReadKey` | 1 `ConsoleKeyInfo` | 1 `ReadKey` | Blocks |
| `ReadLine` | O(n) strings (concat) | O(n) `ReadKey` + `Write` | Allocates per char |
| `ReadPermission` | Few | 1 `ReadKey` per loop | Low |

## Related Documentation

- [components/presentation.md](../components/presentation.md)
- [flows/detail-mutation.md](./detail-mutation.md)
- [architecture.md](../architecture.md#runtime-flow)