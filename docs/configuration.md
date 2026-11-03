# Configuration

## Overview

NuciCLI has **no configuration system**. It does not use:
- `appsettings.json`
- `IConfiguration`
- Environment variables
- Command-line arguments
- Registry/settings files
- Dependency injection options pattern

## Behaviour Configuration

All behaviour is configured **per-call** via method parameters:

| Behaviour | Configuration Method |
|-----------|---------------------|
| Prompt text | Method parameter (`prompt`) |
| Foreground colour | Method parameter (`foregroundColour`) |
| Background colour | Method parameter (`backgroundColour`) |
| Default confirmation | Method parameter (`defaultValue`) |
| Cursor position | Property setters (`CursorX`, `CursorY`) |

## No Global Settings

| Setting | Available? | Alternative |
|---------|------------|-------------|
| Default prompt colour | ❌ | Pass per-call or wrap |
| Default confirmation default | ❌ | Pass per-call or wrap |
| Input validation rules | ❌ | Fixed in `ReadLine` |
| Escape cancellation | ❌ | Only in `ReadLine` |
| Buffer size | ❌ | Read-only via `Width`/`Height` |
| Cursor size | ❌ | Read-only via `CursorSize` |

## Consumer-Level Configuration

Consumers typically create wrapper/configuration classes:

```csharp
public sealed class ConsoleTheme
{
    public NuciConsoleColour Primary { get; init; } = NuciConsoleColour.Cyan;
    public NuciConsoleColour Secondary { get; init; } = NuciConsoleColour.DarkCyan;
    public NuciConsoleColour Success { get; init; } = NuciConsoleColour.Green;
    public NuciConsoleColour Warning { get; init; } = NuciConsoleColour.Yellow;
    public NuciConsoleColour Error { get; init; } = NuciConsoleColour.Red;
    public NuciConsoleColour Prompt { get; init; } = NuciConsoleColour.Yellow;
    public NuciConsoleColour Muted { get; init; } = NuciConsoleColour.DarkGray;
}

public static class ConsoleExtensions
{
    public static string ReadLine(this ConsoleTheme theme, string prompt)
        => NuciConsole.ReadLine(prompt, theme.Prompt);

    public static bool Confirm(this ConsoleTheme theme, string prompt, bool defaultValue = false)
        => NuciConsole.ReadPermission(prompt, defaultValue, theme.Primary);
}
```

## Build-Time Configuration

Only project-level settings in `NuciCLI.csproj`:

```xml
<TargetFramework>net10.0</TargetFramework>
<Version>3.0.2</Version>
<PackageLicenseExpression>GPL-3.0-or-later</PackageLicenseExpression>
```

## Runtime Configuration

**None**. The library has no runtime configuration surface.

## Precedence

Not applicable — no configuration sources.

## Related Documentation

- [design-decisions.md](./design-decisions.md#10-no-configuration-system)
- [components/host-and-composition.md](./components/host-and-composition.md)
- [api-usage-examples.md](../api-usage-examples.md#colour-theme-helper)