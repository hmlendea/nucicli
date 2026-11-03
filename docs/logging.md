# Logging

## Current State

**NuciCLI has no logging.**

- No logging framework referenced (Serilog, NLog, Microsoft.Extensions.Logging, etc.)
- No `ILogger` usage
- No log calls in source code
- No log configuration

## Rationale

1. **Minimal dependency**: Single assembly, no transitive logging deps
2. **Library, not application**: Libraries should not log; consumers decide
3. **Console is the output**: Writing to console *is* the primary operation
4. **Exceptions propagate**: Errors surface as exceptions, not log entries

## Consumer Responsibility

Consumers **must** implement logging if needed:

```csharp
try
{
    NuciConsole.WriteLine("Operation complete", NuciConsoleColour.Green);
    logger.LogInformation("Console output written");
}
catch (IOException ex)
{
    logger.LogError(ex, "Console unavailable");
}
```

## What Could Be Logged (If Added)

| Event | Level | Context |
|-------|-------|---------|
| Colour change | Debug | fg, bg, text length |
| Cursor move | Debug | x, y |
| Input read | Debug | prompt, result length |
| Escape pressed | Info | operation cancelled |
| IOException | Warning | operation, redirected? |
| ArgumentOutOfRange | Warning | property, value, bounds |

## No Structured Logging

No `LogInfoKey`, no structured properties, no correlation IDs.

## No Telemetry

No metrics, no tracing, no OpenTelemetry.

## Future: Optional Logging

If logging added, should be:
- **Opt-in**: `NuciConsole.EnableLogging(ILogger)` or similar
- **Abstraction**: `Microsoft.Extensions.Logging.Abstractions` only
- **Zero-cost when disabled**: `if (logger.IsEnabled(LogLevel.Debug))`

## Related Documentation

- [architecture.md](./architecture.md#no-logging)
- [components/presentation.md](../components/presentation.md#logging)
- [integrations.md](./integrations.md)
- [ambiguities-and-open-questions.md](../ambiguities-and-open-questions.md)