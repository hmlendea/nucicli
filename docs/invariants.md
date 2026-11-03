# Invariants

## System-Wide Invariants

### I1: Statelessness
**All public API is stateless.** No instance state, no static mutable state (except `NuciConsoleColour` instances which are immutable). Each call operates on global `Console` state only.

**Implication**: No initialization required. No disposal. No configuration.

### I2: Colour Restoration
**Every `Write*` call that changes colour restores original colours on exit.**

```csharp
var origFg = Console.ForegroundColor;
var origBg = Console.BackgroundColor;
try { /* write with new colours */ }
finally { Console.ForegroundColor = origFg; Console.BackgroundColor = origBg; }
```

**Exception**: If `IOException` occurs during write, restoration still executes (finally block).

**Gap**: Current implementation uses `try/finally` but not verified in all overloads — see [ambiguities](../ambiguities-and-open-questions.md#1-colour-restoration-not-exception-safe).

### I3: Ctrl+C Handling
**`ReadKey` and `ReadLine` intercept Ctrl+C and return it as a key, not throwing `ConsoleCancelEventArgs`.**

```csharp
var key = NuciConsole.ReadKey(); // Returns ConsoleKey.C with Control modifier
```

**Not applicable**: `ReadPermission` does not handle Ctrl+C.

### I4: Escape Cancellation
**Only `ReadLine` throws `InputCancellationException` on Escape.**

- `ReadKey`: Returns Escape key info
- `ReadPermission`: Ignores Escape (loops until Y/N/Enter)
- `ReadLine`: Throws `InputCancellationException`

### I5: Buffer-Bound Cursor
**`CursorX` ∈ [0, BufferWidth-1], `CursorY` ∈ [0, BufferHeight-1].**

Setting outside throws `ArgumentOutOfRangeException` (BCL).

### I6: Default Colour Sentinel
**`NuciConsoleColour.Default.Value == -1` and `ConsoleColor == null`.**

Passing `Default` to `Write*` means "don't change this colour".

### I7: No Allocation in Hot Paths
**`Write*` methods avoid allocations** (no string formatting, no LINQ). `WriteLines` enumerates directly.

### I8: Synchronous Only
**No async methods.** All blocking. No `CancellationToken` support.

### I9: Single Assembly
**Single DLL: `NuciCLI.dll`.** No satellite assemblies, no native deps.

### I10: GPL-3.0-or-later
**Licence is viral.** Any derivative work must be GPL-compatible.

## Component Invariants

### NuciConsole
| Invariant | Description |
|-----------|-------------|
| Static only | No instance members |
| No DI | No constructor, no services |
| Wraps Console | All ops delegate to `System.Console` |
| Colour save/restore | Every colour change paired with restore |

### NuciConsoleColour
| Invariant | Description |
|-----------|-------------|
| 17 instances | 16 colours + Default |
| Immutable | `readonly` static fields |
| Value mapping | -1=Default, 0-15=ConsoleColor |
| Name property | Human-readable name |

### InputCancellationException
| Invariant | Description |
|-----------|-------------|
| Standard exception | No custom properties |
| Thrown only by ReadLine | Not by ReadKey/ReadPermission |
| Serializable | Standard constructors |

## Violation Consequences

| Invariant | If Violated |
|-----------|-------------|
| I1 | Breaking change, thread-safety issues |
| I2 | Colours leak, terminal corruption |
| I3 | Ctrl+C kills process unexpectedly |
| I4 | Inconsistent cancellation UX |
| I5 | Crashes on cursor set |
| I6 | Default colour misbehaves |
| I7 | GC pressure in render loops |
| I8 | Blocks async callers |
| I9 | Deployment complexity |
| I10 | Legal non-compliance |

## Enforcement

- **Code review**: Check invariants in PRs
- **Tests**: Unit tests for each invariant (future)
- **Static analysis**: None currently
- **Documentation**: This file as reference

## Related Documentation

- [architecture.md](./architecture.md#invariants)
- [components/presentation.md](../components/presentation.md#invariants)
- [ambiguities-and-open-questions.md](../ambiguities-and-open-questions.md)
- [change-guide.md](./change-guide.md#invariant-preservation)