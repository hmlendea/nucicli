# Detail Mutation Flow

This document covers input reading, cancellation, and state mutation flows.

## Input Mutation: ReadLine

### State Machine

```
┌─────────────────────────────────────────────────────────────────┐
│                        ReadLine Loop                             │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌─────────────────┐
                    │ PerformKeyRead  │
                    │   (intercept)   │
                    └────────┬────────┘
                             │
              ┌──────────────┼──────────────┐
              ▼              ▼              ▼
       ┌────────────┐ ┌───────────┐ ┌──────────────┐
       │   Escape   │ │   Enter   │ │  Backspace   │
       └─────┬──────┘ └─────┬─────┘ └──────┬───────┘
             │              │              │
             ▼              ▼              ▼
    ┌────────────────┐ ┌──────────┐ ┌────────────────┐
    │ CursorX -= len │ │WriteLine()│ │ input = input  │
    │ WriteLine(pad) │ │  break   │ │ [..^1]         │
    │ throw          │ │ return   │ │ CursorX -= 1   │
    │ InputCancelExc │ │  input   │ │ Write(" ")     │
    └────────────────┘ └──────────┘ │ CursorX -= 1   │
                                    └────────────────┘
             │              │              │
             ▼              ▼              ▼
       (exception)    (return)      (continue loop)
                              │
                              ▼
                     ┌─────────────────┐
                     │ Valid Char?     │
                     │ (IsLetterOrDigit│
                     │  IsSymbol       │
                     │  IsPunctuation  │
                     │  IsWhiteSpace)  │
                     └────────┬────────┘
                              │
                 ┌────────────┴────────────┐
                 ▼                         ▼
            ┌─────────┐              ┌─────────┐
            │  Yes    │              │   No    │
            └────┬────┘              └────┬────┘
                 │                        │
                 ▼                        ▼
        ┌────────────────┐         (continue loop)
        │ input += char  │
        │ Console.Write  │
        │   (char)       │
        └────────────────┘
                 │
                 ▼
         (continue loop)
```

### Input Buffer Mutation

| Operation | Input String | Cursor Position | Console Output |
|-----------|--------------|-----------------|----------------|
| Start | `""` | After prompt | Prompt visible |
| Type 'a' | `"a"` | +1 | `a` |
| Type 'b' | `"ab"` | +1 | `b` |
| Backspace | `"a"` | -1 | Space overwrites `b`, cursor back |
| Type 'c' | `"ac"` | +1 | `c` |
| Enter | `"ac"` (returned) | New line | `\n` |
| Escape | (exception) | Line start | Line cleared |

### Character Validation

```csharp
bool isValid = char.IsLetterOrDigit(key.KeyChar) ||
               char.IsSymbol(key.KeyChar) ||
               char.IsPunctuation(key.KeyChar) ||
               char.IsWhiteSpace(key.KeyChar);
```

**Accepted**: Letters, digits, symbols (math, currency), punctuation, whitespace (space, tab)
**Rejected**: Control chars (except Enter/Backspace/Escape), surrogate pairs, non-char keys (F1, arrows, etc.)

### Escape Cancellation

**Trigger**: `ConsoleKey.Escape` during `ReadLine` loop

**Sequence**:
1. `CursorX -= inputValue.Length` — move to prompt end
2. `WriteLine(string.Empty.PadRight(inputValue.Length))` — overwrite with spaces
3. `throw new InputCancellationException()`

**Result**: Console line visually cleared, exception propagates.

**No Cleanup**: No resource disposal needed (no resources held).

## Input Mutation: ReadPermission

### State Machine

```
┌─────────────────────────────────────────────────────────────────┐
│                      ReadPermission Loop                         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌─────────────────┐
                    │ Construct Prompt │
                    │ suffix: Y/n or y/N│
                    └────────┬────────┘
                             │
                              ▼
                    ┌─────────────────┐
                    │  ReadKey(prompt) │
                    └────────┬────────┘
                             │
              ┌──────────────┼──────────────┐
              ▼              ▼              ▼
       ┌──────────┐  ┌──────────┐  ┌──────────────┐
       │    Y     │  │    N     │  │    Enter     │
       └────┬─────┘  └────┬─────┘  └──────┬───────┘
            │             │               │
            ▼             ▼               ▼
       ┌──────────┐  ┌──────────┐  ┌──────────────┐
       │WriteLine()│  │WriteLine()│  │WriteLine()   │
       │return true│  │return false│ │return default│
       └──────────┘  └──────────┘  └──────────────┘
            │             │               │
            └─────────────┴───────────────┘
                              │
                              ▼
                       (return to caller)
              ┌─────────────────────────────┐
              │         Other Key           │
              └──────────────┬──────────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ CursorX -= 1    │
                    │ (back up over   │
                    │  invalid key)   │
                    └────────┬────────┘
                             │
                             ▼
                      (continue loop)
```

### Default Value Behaviour

| Default | Prompt Suffix | Enter Returns |
|---------|---------------|---------------|
| `false` | ` (y/N)` | `false` |
| `true` | ` (Y/n)` | `true` |

### Invalid Key Handling

Any key other than Y, N, Enter:
1. `CursorX -= 1` — move cursor back over the typed character
2. Loop continues — prompt re-displayed (via `ReadKey`)

**Visual Effect**: Invalid key briefly appears, then overwritten by next prompt render.

## State Mutation: Cursor Properties

### CursorX Setter

```csharp
set => Console.SetCursorPosition(value, CursorY);
```

**Mutation**: Calls BCL to move cursor horizontally. Vertical position unchanged.

**Validation**: BCL validates bounds (0 to BufferWidth-1). Throws `ArgumentOutOfRangeException` if invalid.

### CursorY Setter

```csharp
set => Console.SetCursorPosition(CursorX, value);
```

**Mutation**: Calls BCL to move cursor vertically. Horizontal position unchanged.

**Validation**: BCL validates bounds (0 to BufferHeight-1). Throws `ArgumentOutOfRangeException` if invalid.

### Race Condition

```csharp
// Thread A                    // Thread B
CursorX = 10;                  CursorY = 5;
                               // Cursor now at (10, 5) or (oldX, 5)?
```

**Not Atomic**: X and Y set separately. No combined `SetCursorPosition(x, y)` exposed.

## State Mutation: Console Colours

### Write Mutation

```csharp
Write(text, fg, bg):
    oldFg = Console.ForegroundColor
    oldBg = Console.BackgroundColor
    Console.ResetColor()
    if (fg != Default) Console.ForegroundColor = fg.ConsoleColour
    if (bg != Default) Console.BackgroundColor = bg.ConsoleColour
    Console.Write(text)
    Console.ForegroundColor = oldFg
    Console.BackgroundColor = oldBg
```

**Mutation Sequence**:
1. Save current colours
2. Reset to defaults
3. Apply requested colours (if not Default)
4. Write text
5. Restore saved colours

**Global Effect**: Affects all subsequent `Console.Write` calls until changed again.

**Exception Gap**: If `Console.Write` throws between step 3-5, colours left modified.

## State Mutation: TreatControlCAsInput

### PerformKeyRead Mutation

```csharp
PerformKeyRead(intercept):
    prev = Console.TreatControlCAsInput
    Console.TreatControlCAsInput = true
    key = Console.ReadKey(intercept)
    Console.TreatControlCAsInput = prev
    return key
```

**Mutation**: Temporarily flips global process setting.

**Duration**: Only during `Console.ReadKey` call (blocking).

**Thread Safety**: NOT thread-safe. Concurrent calls race on `TreatControlCAsInput`.

**Restoration**: Always restored (sequential, no exception between set/restore in current code).

## Cancellation Propagation

### InputCancellationException Flow

```
ReadLine("Name: ")
    │
    ├─► User presses Escape
    │
    ├─► Throw InputCancellationException()
    │
    ▼
Caller's try/catch:
try {
    string name = NuciConsole.ReadLine("Name: ");
}
catch (InputCancellationException) {
    // Handle cancellation
    NuciConsole.WriteLine("Cancelled.");
}
```

**No CancellationToken**: No integration with `System.Threading.CancellationToken`.

**No Cleanup**: No resources to dispose on cancellation.

## Buffer Resize Handling

**Not Handled**: NuciCLI does not monitor or respond to console resize events.

**Consequences**:
- `Width`/`Height`/`Area` return stale values after resize
- `CursorX`/`CursorY` may be out of bounds
- `ReadLine` cursor manipulation may behave unexpectedly
- No `Console.CancelKeyPress` or resize event subscription

**Consumer Responsibility**: Handle resize externally if needed.

## Related Documentation

- [flows/startup-and-rendering.md](./startup-and-rendering.md)
- [components/presentation.md](../components/presentation.md)
- [error-handling.md](../error-handling.md)
- [concurrency-and-scheduling.md](../concurrency-and-scheduling.md)