# Integration Models

NuciCLI integrates with two external dependencies:

## 1. System.Console (BCL)

**Type**: Framework dependency (built-in)
**Namespace**: `System`
**Assembly**: `System.Console` (part of .NET runtime)

### Integration Points

| NuciCLI Member | BCL Member | Direction |
|----------------|------------|-----------|
| `CursorX` get/set | `Console.CursorLeft`, `Console.SetCursorPosition` | Read/Write |
| `CursorY` get/set | `Console.CursorTop`, `Console.SetCursorPosition` | Read/Write |
| `CursorSize` get | `Console.CursorSize` | Read |
| `Width` get | `Console.BufferWidth` | Read |
| `Height` get | `Console.BufferHeight` | Read |
| `ReadKey` | `Console.ReadKey(bool)` | Call |
| `ReadLine` loop | `Console.ReadKey(true)`, `Console.Write(char)` | Call |
| `Write` | `Console.Write(string)`, `Console.ForegroundColor`, `Console.BackgroundColor`, `Console.ResetColor()` | Call/Read/Write |
| `PerformKeyRead` | `Console.TreatControlCAsInput`, `Console.ReadKey(bool)` | Read/Write/Call |

### Contract Assumptions

| Assumption | Validation |
|------------|------------|
| `Console.CursorLeft/Top` reflect actual cursor | Not validated; trusted |
| `Console.BufferWidth/Height` stable during operation | Not monitored; resize not handled |
| `Console.ReadKey` blocks until key available | Expected behaviour |
| `Console.Write` outputs to stdout | Expected behaviour |
| `Console.ForegroundColor/BackgroundColor` affect subsequent writes | Expected behaviour |
| `Console.ResetColor()` resets to defaults | Expected behaviour |
| `Console.TreatControlCAsInput` toggles Ctrl+C handling | Expected behaviour |

### Failure Modes

| BCL Failure | NuciCLI Behaviour |
|-------------|-------------------|
| `IOException` (stdout redirected, no console) | Propagates to caller |
| `ArgumentException` (cursor out of bounds) | Propagates to caller |
| `PlatformNotSupported` (non-console host) | Propagates to caller |
| Console resize during operation | Undefined (cursor may be invalid) |

### Version Compatibility

- Target: `net10.0`
- `System.Console` API stable since .NET Core 1.0
- No version-specific code

## 2. NuciExtensions

**Type**: NuGet package dependency
**Package**: `NuciExtensions` v6.0.2
**Author**: Same org (hmlendea)
**Repository**: Likely https://github.com/hmlendea/nuciextensions

### Integration Points

| NuciCLI Usage | NuciExtensions Member | Purpose |
|---------------|----------------------|---------|
| `ReadLine` validation | `string.IsNullOrEmpty()` (extension) | Check input buffer |

**Note**: Actual usage not visible in current source (may be in non-visible code or transitive). Declared in `NuciCLI.csproj`.

### Contract Assumptions

| Assumption | Validation |
|------------|------------|
| `IsNullOrEmpty` behaves as `string.IsNullOrEmpty` | Standard BCL behaviour |
| Package available on NuGet | CI restores successfully |
| Version 6.0.2 compatible with net10.0 | CI builds successfully |

### Failure Modes

| Failure | NuciCLI Behaviour |
|---------|-------------------|
| Package not found | Build fails (restore) |
| Breaking change in NuciExtensions | Build/test fails |
| Version conflict | NuGet resolves or fails |

### Version Policy

- Pinned to `6.0.2` in `NuciCLI.csproj`
- No floating version (`*` or ranges)
- Updates require manual version bump + verification

## Integration Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      NuciCLI Assembly                        │
│  ┌─────────────────────────────────────────────────────┐    │
│  │ NuciConsole (static façade)                          │    │
│  │   ├─ Cursor/Buffer properties ──────────────────┐   │    │
│  │   ├─ ReadKey/ReadLine/ReadPermission ────────┐  │   │    │
│  │   └─ Write/WriteLine/WriteLines ────────────┐  │  │    │
│  └──────────────────────────────────────────────│──┘    │
│                                                 │         │
│                                                 ▼         │
│  ┌─────────────────────────────────────────────────────┐  │
│  │ Private: PerformKeyRead                              │  │
│  │   └─ Console.TreatControlCAsInput toggle            │  │
│  └─────────────────────────────────────────────────────┘  │
└────────────────────────────┬──────────────────────────────┘
                             │
              ┌──────────────┴──────────────┐
              ▼                             ▼
┌─────────────────────────┐     ┌─────────────────────────┐
│   System.Console        │     │   NuciExtensions        │
│   (BCL, built-in)       │     │   (NuGet, v6.0.2)       │
│                         │     │                         │
│ - CursorLeft/Top        │     │ - IsNullOrEmpty (ext)   │
│ - BufferWidth/Height    │     │                         │
│ - ReadKey(bool)         │     │                         │
│ - Write(string)         │     │                         │
│ - ForegroundColor       │     │                         │
│ - BackgroundColor       │     │                         │
│ - ResetColor()          │     │                         │
│ - TreatControlCAsInput  │     │                         │
└─────────────────────────┘     └─────────────────────────┘
```

## No Other Integrations

NuciCLI does **not** integrate with:
- Logging frameworks (Serilog, NLog, etc.)
- Configuration systems (IConfiguration, etc.)
- DI containers (Microsoft.Extensions.DependencyInjection, etc.)
- Telemetry (OpenTelemetry, Application Insights, etc.)
- File system (no file I/O)
- Network (no HTTP, sockets)
- OS-specific APIs (no P/Invoke, no ANSI sequences)
- Terminal libraries (Spectre.Console, Terminal.Gui, etc.)

## Testing Integration

Unit tests (in `NuciCLI.UnitTests`) would need to:
- Mock `System.Console` (difficult — static, sealed)
- Use real console (integration test)
- Wrap NuciCLI in testable abstraction

See [testing.md](../testing.md) for strategy.

## Related Documentation

- [architecture.md](../architecture.md#dependencies)
- [dependencies.md](../dependencies.md)
- [components/presentation.md](./presentation.md)
- [components/application-services.md](./application-services.md)