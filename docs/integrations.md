# Integrations

## External Integrations

### 1. System.Console (BCL)

| Aspect | Details |
|--------|---------|
| Type | Built-in framework library |
| Version | .NET 10.0 runtime |
| Coupling | Tight — direct static calls |
| Surface | `Console.*` properties and methods |

**Used By**: All `NuciConsole` methods

**Key APIs Consumed**:
- `Console.ReadKey(bool)`
- `Console.ReadLine()`
- `Console.Write(string)`
- `Console.WriteLine(string)`
- `Console.ForegroundColor` / `BackgroundColor`
- `Console.CursorLeft` / `CursorTop`
- `Console.BufferWidth` / `BufferHeight`
- `Console.WindowWidth` / `WindowHeight`
- `Console.IsOutputRedirected`
- `Console.CursorVisible`
- `Console.CancelKeyPress` (not used directly)

**No Abstraction Layer**: Direct calls. No interface, no wrapper, no mockability.

### 2. NuciExtensions

| Aspect | Details |
|--------|---------|
| Type | NuGet package (same author) |
| Version | 6.0.2 (pinned) |
| Coupling | Unknown — referenced but usage not visible in source |
| Purpose | Extension methods (e.g., `IsNullOrEmpty`) |

**Status**: Referenced in `NuciCLI.csproj`. Actual usage requires inspecting compiled IL or non-visible source.

## Internal Integration Points

### NuciConsole ↔ NuciConsoleColour

| Direction | Mechanism |
|-----------|-----------|
| NuciConsole → NuciConsoleColour | Reads `Value`, `ConsoleColor`, `Name` properties |
| NuciConsoleColour → NuciConsole | None (data only) |

**Contract**: `Value == -1` means "no change", `0-15` maps to `ConsoleColor` enum.

### NuciConsole ↔ InputCancellationException

| Direction | Mechanism |
|-----------|-----------|
| NuciConsole → Exception | `throw new InputCancellationException()` in `ReadLine` |
| Exception → NuciConsole | None |

## No Other Integrations

NuciCLI does **not** integrate with:
- Logging frameworks
- Configuration systems
- DI containers
- Serialization (JSON, XML, etc.)
- HTTP clients
- Database/ORM
- Message queues
- Cloud services
- UI frameworks (Spectre.Console, Terminal.Gui, etc.)
- Testing frameworks (in main assembly)
- Telemetry/APM

## Integration Patterns

### Pattern: Colour Save/Restore

```csharp
// Used in every Write* overload
var origFg = Console.ForegroundColor;
var origBg = Console.BackgroundColor;
try
{
    if (fg != NuciConsoleColour.Default) Console.ForegroundColor = fg.ConsoleColor.Value;
    if (bg != NuciConsoleColour.Default) Console.BackgroundColor = bg.ConsoleColor.Value;
    Console.Write(text);
}
finally
{
    Console.ForegroundColor = origFg;
    Console.BackgroundColor = origBg;
}
```

### Pattern: Cursor Bounds Delegation

```csharp
// Property setters delegate directly to BCL
public static int CursorX
{
    get => Console.CursorLeft;
    set => Console.CursorLeft = value; // BCL validates bounds
}
```

### Pattern: Input Loop

```csharp
// ReadLine: manual loop over ReadKey
while (true)
{
    var key = Console.ReadKey(true);
    if (key.Key == ConsoleKey.Escape) throw new InputCancellationException();
    if (key.Key == ConsoleKey.Enter) break;
    // ... handle backspace, chars
}
```

## Testing Integration

### Test Project Dependencies

| Package | Purpose |
|---------|---------|
| NUnit | Test framework |
| NUnit3TestAdapter | VS Test Explorer |
| Microsoft.NET.Test.Sdk | Runner |

**No mocking framework** (Moq, NSubstitute) — tests likely integration-style against real console.

## Future Integration Points

| Area | Candidate | Rationale |
|------|-----------|-----------|
| Async | `ValueTask`/`Task` APIs | Non-blocking for GUI apps |
| Theming | `IConsoleTheme` interface | Global colour schemes |
| Extensibility | `IConsoleWriter` | Custom output targets |
| Testing | `IConsole` abstraction | Mockable for unit tests |

## Related Documentation

- [dependencies.md](./dependencies.md)
- [components/integration-models.md](../components/integration-models.md)
- [architecture.md](./architecture.md#integration-model)
- [ambiguities-and-open-questions.md](../ambiguities-and-open-questions.md#2-nuci-extensions-usage)