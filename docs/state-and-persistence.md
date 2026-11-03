# State and Persistence

## Current State

**NuciCLI has no persistent state.**

- No configuration files
- No user preferences storage
- No session persistence
- No database
- No file I/O
- No registry access

## Runtime State

### Global Console State (BCL)

| State | Owner | Mutated By |
|-------|-------|------------|
| `Console.ForegroundColor` | Process | `Write*`, consumer |
| `Console.BackgroundColor` | Process | `Write*`, consumer |
| `Console.CursorLeft` | Process | `CursorX`, consumer |
| `Console.CursorTop` | Process | `CursorY`, consumer |
| `Console.CursorVisible` | Process | Consumer |
| `Console.BufferWidth/Height` | Process | Consumer (resize) |
| `Console.WindowWidth/Height` | Process | OS/Terminal |

**NuciCLI does not own this state** — it reads/writes the global `Console` state.

### NuciCLI Static State

| State | Type | Mutability |
|-------|------|------------|
| `NuciConsoleColour.*` | 17 static readonly fields | Immutable |
| `InputCancellationException` | Type definition | N/A |

**No mutable static fields** in NuciCLI.

## No Configuration

- No `appsettings.json`
- No environment variables read
- No command-line parsing
- No feature flags

## No Serialization

- No JSON, XML, binary serialization
- No settings persistence
- No state export/import

## No Caching

- No in-memory caches
- No computed value memoization

## Thread Safety

**Not thread-safe.** Global `Console` state is process-wide.

```csharp
// Unsafe: concurrent calls interleave colours
Parallel.Invoke(
    () => NuciConsole.WriteLine("A", NuciConsoleColour.Red),
    () => NuciConsole.WriteLine("B", NuciConsoleColour.Blue)
);
// Result: colours may leak between writes
```

**Consumer must synchronize** if multi-threaded.

## Process Lifetime

| Phase | State |
|-------|-------|
| Start | Console state = OS defaults |
| Run | Modified by NuciCLI/consumer |
| Exit | Console state = last values (OS restores on close) |

## No Recovery

- No checkpointing
- No transaction log
- No crash recovery

## Testing Implications

- Tests share global console state
- Run sequentially or reset state between tests
- No isolated test context

## Future: Optional State

If state added, consider:
- `IConsoleState` abstraction for testability
- Per-thread or per-scope state
- Explicit save/restore API

## Related Documentation

- [architecture.md](./architecture.md#stateless-design)
- [invariants.md](./invariants.md#i1-statelessness)
- [components/presentation.md](../components/presentation.md#state)
- [concurrency-and-scheduling.md](./concurrency-and-scheduling.md)