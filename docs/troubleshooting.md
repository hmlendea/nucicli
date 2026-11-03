# Troubleshooting

## Common Issues

### 1. Colours Not Appearing

**Symptoms**: `WriteLine` with colours shows no colour change.

**Causes**:
- Output redirected (`Console.IsOutputRedirected == true`)
- Terminal doesn't support ANSI colours
- Console host doesn't support colours (some IDE terminals)

**Solutions**:
```csharp
if (Console.IsOutputRedirected)
{
    // Don't use colours, or use plain text
    NuciConsole.WriteLine("Plain text");
}
else
{
    NuciConsole.WriteLine("Coloured", NuciConsoleColour.Red);
}
```

### 2. "Handle is invalid" / IOException

**Symptoms**: `IOException` on any console operation.

**Causes**:
- Running as Windows Service
- Running in background without console
- Stdout/stderr closed
- Docker without `-it` flag

**Solutions**:
```csharp
if (Console.IsOutputRedirected || Console.IsInputRedirected)
{
    // Non-interactive mode
    return;
}

try
{
    NuciConsole.WriteLine("Interactive");
}
catch (IOException)
{
    // Fallback
}
```

### 3. Cursor Position Wrong After Resize

**Symptoms**: `CursorX`/`CursorY` don't match visible position after terminal resize.

**Cause**: NuciCLI doesn't handle resize events. Buffer size may change.

**Solutions**:
```csharp
// Re-read dimensions after resize
int w = NuciConsole.Width;
int h = NuciConsole.Height;

// Clamp cursor
NuciConsole.CursorX = Math.Clamp(x, 0, w - 1);
NuciConsole.CursorY = Math.Clamp(y, 0, h - 1);
```

### 4. Escape Key Doesn't Cancel

**Symptoms**: Pressing Escape in `ReadPermission` or `ReadKey` doesn't cancel.

**Cause**: By design — only `ReadLine` throws `InputCancellationException`.

**Solutions**:
```csharp
// For ReadKey: check manually
var key = NuciConsole.ReadKey(true);
if (key.Key == ConsoleKey.Escape) { /* handle */ }

// For ReadPermission: not cancellable by Escape
// Use ReadLine with Y/N parsing instead
```

### 5. Colours Leak Between Writes

**Symptoms**: After coloured write, subsequent plain writes have wrong colour.

**Cause**: Exception during write prevented colour restoration (bug) or concurrent writes.

**Solutions**:
```csharp
// Ensure no exceptions during write
// Synchronize if multi-threaded
lock (consoleLock)
{
    NuciConsole.WriteLine("Red", NuciConsoleColour.Red);
    NuciConsole.WriteLine("Normal"); // Should be default colour
}
```

### 6. ArgumentOutOfRangeException on Cursor Set

**Symptoms**: Setting `CursorX` or `CursorY` throws.

**Cause**: Value outside buffer bounds.

**Solutions**:
```csharp
NuciConsole.CursorX = Math.Clamp(x, 0, NuciConsole.Width - 1);
NuciConsole.CursorY = Math.Clamp(y, 0, NuciConsole.Height - 1);
```

### 7. No Console in Unit Tests

**Symptoms**: Tests fail with `IOException` or no output.

**Cause**: Test runner may not provide interactive console.

**Solutions**:
- Run tests in interactive terminal
- Use `dotnet test` in terminal, not VS Test Explorer
- Mock console (requires abstraction — not currently available)

### 8. GPL-3.0 Licence Concerns

**Symptoms**: Legal review flags GPL-3.0.

**Cause**: NuciCLI is GPL-3.0-or-later (copyleft).

**Solutions**:
- Ensure your project is GPL-compatible
- Or don't use NuciCLI in proprietary code
- Consult legal counsel

## Debugging Tips

### Enable Verbose Output

```bash
dotnet run --verbosity detailed
```

### Check Console Capabilities

```csharp
Console.WriteLine($"Redirected: {Console.IsOutputRedirected}");
Console.WriteLine($"Buffer: {Console.BufferWidth}x{Console.BufferHeight}");
Console.WriteLine($"Window: {Console.WindowWidth}x{Console.WindowHeight}");
Console.WriteLine($"Cursor: {Console.CursorLeft},{Console.CursorTop}");
```

### Test Colour Support

```csharp
foreach (var c in Enum.GetValues<ConsoleColor>())
{
    Console.ForegroundColor = c;
    Console.WriteLine($"This is {c}");
}
Console.ResetColor();
```

## Known Limitations (Not Bugs)

| Limitation | Workaround |
|------------|------------|
| No async API | `Task.Run(() => NuciConsole.ReadLine(...))` |
| No password masking | Implement with `ReadKey(true)` |
| No tab completion | Build on `ReadKey` |
| No history | Build on `ReadLine` + list |
| No mouse support | Not available in `System.Console` |
| No raw/ANSI mode | Use `Console` directly |
| Single-threaded | Synchronize externally |

## Getting Help

1. Check [FAQ](../faq.md)
2. Check [Error Handling](../error-handling.md)
3. Search [GitHub Issues](https://github.com/hmlendea/nucicli/issues)
4. Open new issue with:
   - .NET version
   - OS/Terminal
   - Minimal reproduction
   - Expected vs actual behaviour

## Related Documentation

- [error-handling.md](../error-handling.md)
- [faq.md](../faq.md)
- [invariants.md](../invariants.md)
- [ambiguities-and-open-questions.md](../ambiguities-and-open-questions.md)