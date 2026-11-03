# Host and Composition

## Static Façade Pattern

NuciCLI uses a **static façade** pattern — all public API is exposed via `static` members on `NuciConsole` and `NuciConsoleColour`. There is no:

- Dependency injection container
- Composition root
- Instance-based services
- Factory patterns
- Plugin/extension points
- Configuration system

## Why No DI/Composition Root

| Reason | Explanation |
|--------|-------------|
| **Console is a singleton** | `System.Console` is process-global; no need for multiple instances |
| **Library size** | ~300 lines; DI adds complexity without benefit |
| **Consumer simplicity** | `NuciConsole.ReadLine()` vs `serviceProvider.GetRequiredService<IConsole>().ReadLine()` |
| **BCL precedent** | `System.Console` itself is static; consumers expect this pattern |
| **No runtime configuration** | Nothing to configure at startup |

## Initialization

**No initialization required.** The library is ready to use immediately after reference.

```csharp
// No setup needed
NuciConsole.WriteLine("Ready");
```

## Lifetime Management

| Aspect | Detail |
|--------|--------|
| **Assembly Load** | Automatic on first `NuciConsole` access |
| **Static Constructors** | None (no static state to initialize) |
| **Resource Acquisition** | None (delegates to `System.Console`) |
| **Cleanup** | None required |
| **Unload** | Not applicable |

## Composition for Consumers

Consumers wanting testability or abstraction typically wrap NuciCLI:

```csharp
public interface IConsole
{
    string ReadLine(string prompt, NuciConsoleColour fg, NuciConsoleColour bg);
    ConsoleKeyInfo ReadKey(string prompt, NuciConsoleColour fg, NuciConsoleColour bg);
    bool ReadPermission(string prompt, bool defaultValue, NuciConsoleColour fg, NuciConsoleColour bg);
    void Write(string text, NuciConsoleColour fg, NuciConsoleColour bg);
    // ... cursor/buffer properties
}

public class NuciConsoleAdapter : IConsole
{
    public string ReadLine(string prompt, NuciConsoleColour fg, NuciConsoleColour bg)
        => NuciConsole.ReadLine(prompt, fg, bg);
    // ... delegate all calls
}
```

Then register `IConsole` in their DI container.

## No Plugin System

NuciCLI does not support:
- Custom colour providers
- Custom input handlers
- Custom output sinks
- Middleware/interceptors
- Event hooks

All behaviour is fixed in `NuciConsole`.

## Versioning & Compatibility

- **Single assembly**: `NuciCLI.dll`
- **No internal APIs**: Everything public is the API surface
- **Breaking changes**: Major version bump (v3.x current)
- **Side-by-side**: Not applicable (single version per process)

## Comparison: Static Façade vs Alternatives

| Pattern | NuciCLI | Alternative |
|---------|---------|-------------|
| **Static Façade** | ✅ Used | — |
| **Instance Service** | ❌ | `IConsole` + DI |
| **Abstract Factory** | ❌ | `IConsoleFactory` |
| **Builder** | ❌ | `ConsoleBuilder` |
| **Module/Plugin** | ❌ | MEF, Autofac modules |

## Implications for Consumers

| Scenario | Approach |
|----------|----------|
| **Direct use** | `using NuciCLI; NuciConsole.ReadLine(...)` |
| **Unit testing** | Wrap in interface, mock interface |
| **Integration testing** | Use real `NuciConsole` (requires console) |
| **Multiple "consoles"** | Not supported (single process console) |
| **Custom behaviour** | Fork/wrap, not extend |

## Related Documentation

- [architecture.md](../architecture.md#architectural-style-static-façade)
- [design-decisions.md](../design-decisions.md#1-static-façade-over-instance-based-design)
- [components/presentation.md](./presentation.md)
- [components/application-services.md](./application-services.md)