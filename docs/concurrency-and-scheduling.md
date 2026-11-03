# Concurrency and Scheduling

## Threading Model

**Single-threaded by design**. NuciCLI:
- Creates no threads
- Uses no `Task`/`async`/`await`
- Has no timers, schedulers, or background workers
- Delegates all blocking to `System.Console` (synchronous)

## Thread Safety

### Not Thread-Safe

| Component | Thread-Safe? | Reason |
|-----------|--------------|--------|
| `NuciConsole` static members | ❌ | Wraps `System.Console` (not thread-safe) |
| `NuciConsoleColour` instances | ✅ | Immutable, read-only |
| `InputCancellationException` | ✅ | Standard exception, no shared state |
| `PerformKeyRead` | ❌ | Modifies global `Console.TreatControlCAsInput` |
| Cursor properties | ❌ | Non-atomic X/Y set, shared `Console` state |
| Colour properties | ❌ | Global `Console.ForegroundColor`/`BackgroundColor` |

### Race Conditions

```csharp
// Thread A                    // Thread B
NuciConsole.CursorX = 10;      NuciConsole.CursorY = 5;
// Result: cursor at (10, 5) or (oldX, 5) — unpredictable

// Thread A                    // Thread B
NuciConsole.Write("A", Red);   NuciConsole.Write("B", Blue);
// Result: colours interleaved, restore order unpredictable
```

## Synchronization Responsibility

**Consumer must synchronize** if calling from multiple threads:

```csharp
private static readonly object ConsoleLock = new();

public static string SafeReadLine(string prompt)
{
    lock (ConsoleLock)
    {
        return NuciConsole.ReadLine(prompt);
    }
}
```

## Async Integration

### No Native Async

```csharp
// Not available
// public static Task<string> ReadLineAsync(...)
```

### Consumer Wrapper

```csharp
public static Task<string> ReadLineAsync(string prompt)
    => Task.Run(() => NuciConsole.ReadLine(prompt));
```

**Caveat**: `Task.Run` uses thread pool; console access still serialized by consumer lock.

## Scheduling

**No scheduling**. All operations execute immediately on calling thread.

## Blocking Behaviour

| Method | Blocks Until |
|--------|--------------|
| `ReadKey` | Key pressed |
| `ReadLine` | Enter or Escape |
| `ReadPermission` | Y, N, or Enter |
| `Write*` | Write completes (usually immediate) |
| Properties | Immediate (BCL property access) |

## Cancellation

### No CancellationToken Support

```csharp
// Not available
// public static string ReadLine(string prompt, CancellationToken ct)
```

### Escape Cancellation

Only `ReadLine` supports cancellation via Escape key → `InputCancellationException`.

### Cooperative Cancellation Pattern

```csharp
public static string ReadLineWithCancellation(string prompt, CancellationToken ct)
{
    ct.ThrowIfCancellationRequested();

    // Poll during input (not possible with blocking ReadLine)
    // Would require custom input loop
    return NuciConsole.ReadLine(prompt);
}
```

**Limitation**: Cannot interrupt blocking `Console.ReadKey`.

## Parallelism

**No internal parallelism**. Single-threaded execution.

## Resource Contention

### Console Resource

`System.Console` is a **process-wide singleton**. Contention sources:
- Multiple threads writing
- Multiple threads reading
- External processes writing to same console
- Terminal emulator buffering

### Mitigation

1. Single-threaded console access (recommended)
2. Consumer-level locking
3. Dedicated console thread with message queue

## Related Documentation

- [components/presentation.md](../components/presentation.md)
- [flows/detail-mutation.md](../flows/detail-mutation.md#state-mutation-treatcontrolcasinput)
- [error-handling.md](../error-handling.md)
- [design-decisions.md](../design-decisions.md#5-no-async-api)