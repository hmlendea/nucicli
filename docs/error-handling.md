# Error Handling

## Error Taxonomy

| Category | Types | Source |
|----------|-------|--------|
| **Input Cancellation** | `InputCancellationException` | NuciCLI (Escape in ReadLine) |
| **I/O Errors** | `IOException` | BCL (console redirected, unavailable) |
| **Argument Errors** | `ArgumentOutOfRangeException`, `ArgumentNullException` | BCL/NuciCLI validation |
| **Platform Errors** | `PlatformNotSupportedException` | BCL (non-console host) |

## NuciCLI Exceptions

### InputCancellationException

**Thrown by**: `ReadLine()` overloads only

**Condition**: User presses Escape during line input

**Constructors**:
```csharp
InputCancellationException()
InputCancellationException(string message)
InputCancellationException(string message, Exception innerException)
```

**Properties**: Standard `Exception` properties (Message, StackTrace, InnerException)

**No custom properties** (e.g., Prompt, PartialInput) — see [ambiguities](../ambiguities-and-open-questions.md#7-escape-key-in-readpermission)

**Handling Pattern**:
```csharp
try
{
    string input = NuciConsole.ReadLine("Input: ");
}
catch (InputCancellationException)
{
    // User cancelled
}
```

## BCL Exceptions (Propagated)

### IOException

**Thrown by**: All I/O methods (`ReadKey`, `ReadLine`, `ReadPermission`, `Write*`, property access)

**Conditions**:
- Console output redirected (`Console.IsOutputRedirected`)
- No console available (service, background process)
- Stdout/stderr closed

**Handling**:
```csharp
if (Console.IsOutputRedirected)
{
    // Non-interactive mode, skip console ops
}
else
{
    try { NuciConsole.WriteLine("Hello"); }
    catch (IOException) { /* fallback */ }
}
```

### ArgumentOutOfRangeException

**Thrown by**: `CursorX`/`CursorY` setters

**Conditions**:
- `CursorX` < 0 or ≥ `Console.BufferWidth`
- `CursorY` < 0 or ≥ `Console.BufferHeight`

**Handling**:
```csharp
try
{
    NuciConsole.CursorX = desiredX;
}
catch (ArgumentOutOfRangeException)
{
    // Clamp to valid range
    NuciConsole.CursorX = Math.Clamp(desiredX, 0, NuciConsole.Width - 1);
}
```

### ArgumentNullException

**Thrown by**: `WriteLines(IEnumerable<string> lines, ...)`

**Condition**: `lines` parameter is null

**Handling**: Validate before call.

### PlatformNotSupportedException

**Thrown by**: BCL console methods on unsupported platforms

**Conditions**: Non-Windows without terminal, certain container environments

## Error Handling Patterns

### Pattern 1: Try/Catch for Cancellation

```csharp
string ReadWithCancel(string prompt)
{
    while (true)
    {
        try
        {
            return NuciConsole.ReadLine(prompt);
        }
        catch (InputCancellationException)
        {
            NuciConsole.WriteLine("Cancelled. Retry? (Y/n)");
            if (!NuciConsole.ReadPermission("", true))
                return null; // User chose not to retry
        }
    }
}
```

### Pattern 2: Check Console Availability

```csharp
void SafeWrite(string text, NuciConsoleColour fg)
{
    if (Console.IsOutputRedirected)
        return; // Silently skip in non-interactive

    try
    {
        NuciConsole.WriteLine(text, fg);
    }
    catch (IOException)
    {
        // Console became unavailable
    }
}
```

### Pattern 3: Validate Cursor Bounds

```csharp
void SafeSetCursor(int x, int y)
{
    x = Math.Clamp(x, 0, NuciConsole.Width - 1);
    y = Math.Clamp(y, 0, NuciConsole.Height - 1);
    NuciConsole.CursorX = x;
    NuciConsole.CursorY = y;
}
```

## Recovery Strategies

| Error | Recovery |
|-------|----------|
| `InputCancellationException` | Retry, return default, abort operation |
| `IOException` | Fallback to non-coloured output, log, skip |
| `ArgumentOutOfRangeException` | Clamp values, recalculate layout |
| `PlatformNotSupportedException` | Disable interactive features, use basic I/O |

## No Retry Logic

NuciCLI provides **no built-in retry**. Consumers implement retry if needed.

## No Circuit Breaker

Not applicable — no external dependencies that fail intermittently.

## Error Logging

**No logging in NuciCLI**. Exceptions propagate to consumer. Consumer decides logging.

## Related Documentation

- [components/presentation.md](../components/presentation.md#error-handling)
- [flows/detail-mutation.md](../flows/detail-mutation.md#cancellation-propagation)
- [ambiguities-and-open-questions.md](../ambiguities-and-open-questions.md)
- [troubleshooting.md](../troubleshooting.md)