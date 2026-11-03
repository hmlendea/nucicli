# NuciConsoleColour API Reference

**Namespace**: `NuciCLI`
**Type**: `public sealed class`
**Assembly**: `NuciCLI.dll`
**Since**: v1.0.0

## Overview

`NuciConsoleColour` is an immutable value object representing a console colour. It wraps `ConsoleColor` with a "Default" sentinel value that means "use current console colour".

## Static Instances (17)

All instances are pre-created static properties. **No public constructor** — consumers use these instances directly.

| Property | Value | ConsoleColor | Name | Use Case |
|----------|-------|--------------|------|----------|
| `Default` | -1 | `null` | "Default" | No colour change |
| `Black` | 0 | `Black` | "Black" | |
| `DarkBlue` | 1 | `DarkBlue` | "DarkBlue" | |
| `DarkGreen` | 2 | `DarkGreen` | "DarkGreen" | |
| `DarkCyan` | 3 | `DarkCyan` | "DarkCyan" | |
| `DarkRed` | 4 | `DarkRed` | "DarkRed" | |
| `DarkMagenta` | 5 | `DarkMagenta` | "DarkMagenta" | |
| `DarkYellow` | 6 | `DarkYellow` | "DarkYellow" | |
| `Gray` | 7 | `Gray` | "Gray" | |
| `DarkGray` | 8 | `DarkGray` | "DarkGray" | |
| `Blue` | 9 | `Blue` | "Blue" | |
| `Green` | 10 | `Green` | "Green" | |
| `Cyan` | 11 | `Cyan` | "Cyan" | |
| `Red` | 12 | `Red` | "Red" | |
| `Magenta` | 13 | `Magenta` | "Magenta" | |
| `Yellow` | 14 | `Yellow` | "Yellow" | |
| `White` | 15 | `White` | "White" | |

## Properties

### Value

```csharp
public int Value { get; }
```

**Gets** the numeric colour code.

**Values**:
- `-1` = `Default` (no colour change)
- `0-15` = `ConsoleColor` enum value

**Since**: v1.0.0

---

### ConsoleColour

```csharp
public ConsoleColor? ConsoleColour { get; }
```

**Gets** the nullable `ConsoleColor` mapping.

**Values**:
- `null` = `Default` (use current colour)
- `ConsoleColor.Black` through `ConsoleColor.White` = explicit colour

**Since**: v1.0.0

---

### Name

```csharp
public string Name { get; }
```

**Gets** the constant name of the colour.

**Values**: `"Default"`, `"Black"`, `"DarkBlue"`, ..., `"White"`

**Use Cases**: Debugging, logging, serialization.

**Since**: v1.0.0

---

## Constructor

```csharp
private NuciConsoleColour(int value, string name)
```

**Private** — only static instances can be created.

**Parameters**:
- `value` — Numeric code (-1 for Default, 0-15 for colours)
- `name` — Constant name

**Implementation**:
```csharp
Value = value;
Name = name;
if (Value.Equals(-1))
    ConsoleColour = null;
else
    ConsoleColour = (ConsoleColor)value;
```

---

## Usage in NuciConsole

### As Foreground Colour

```csharp
NuciConsole.Write("Error!", NuciConsoleColour.Red);
NuciConsole.WriteLine("Warning", NuciConsoleColour.Yellow);
NuciConsole.ReadLine("Input: ", NuciConsoleColour.Cyan);
```

### As Background Colour

```csharp
NuciConsole.Write("Highlight", NuciConsoleColour.Black, NuciConsoleColour.Yellow);
NuciConsole.ReadPermission("Confirm?", NuciConsoleColour.White, NuciConsoleColour.DarkBlue);
```

### Default Colour

```csharp
// Explicit default (same as omitting colour)
NuciConsole.Write("Text", NuciConsoleColour.Default);

// In overloads with both colours:
NuciConsole.Write("Text", NuciConsoleColour.Red, NuciConsoleColour.Default); // FG only
NuciConsole.Write("Text", NuciConsoleColour.Default, NuciConsoleColour.Blue); // BG only
```

---

## Colour Mapping Table

| NuciConsoleColour | Value | ConsoleColor | ANSI 16-colour | RGB (approx) |
|-------------------|-------|--------------|----------------|--------------|
| Default | -1 | (current) | — | — |
| Black | 0 | Black | 0 | #000000 |
| DarkBlue | 1 | DarkBlue | 1 | #000080 |
| DarkGreen | 2 | DarkGreen | 2 | #008000 |
| DarkCyan | 3 | DarkCyan | 3 | #008080 |
| DarkRed | 4 | DarkRed | 4 | #800000 |
| DarkMagenta | 5 | DarkMagenta | 5 | #800080 |
| DarkYellow | 6 | DarkYellow | 6 | #808000 |
| Gray | 7 | Gray | 7 | #C0C0C0 |
| DarkGray | 8 | DarkGray | 8 | #808080 |
| Blue | 9 | Blue | 9 | #0000FF |
| Green | 10 | Green | 10 | #00FF00 |
| Cyan | 11 | Cyan | 11 | #00FFFF |
| Red | 12 | Red | 12 | #FF0000 |
| Magenta | 13 | Magenta | 13 | #FF00FF |
| Yellow | 14 | Yellow | 14 | #FFFF00 |
| White | 15 | White | 15 | #FFFFFF |

**Note**: Actual rendered colours depend on terminal theme/colour scheme.

---

## Equality & Comparison

### Reference Equality

Static instances are singletons — reference equality works:

```csharp
NuciConsoleColour.Red == NuciConsoleColour.Red  // true (same instance)
```

### Value Equality

No `Equals`/`GetHashCode` override — uses reference equality.

### Custom Colours

**Not supported** — cannot create custom `NuciConsoleColour` instances (private constructor).

**Workaround**: Use `ConsoleColor` directly with `Console.ForegroundColor` if needed.

---

## Serialization

### JSON (System.Text.Json)

```json
{
  "Value": 12,
  "ConsoleColour": "Red",
  "Name": "Red"
}
```

**Note**: `ConsoleColour` serializes as string (enum name) or null.

### Custom Serialization

For custom serialization, use `Value` or `Name`:

```csharp
// Serialize
int code = color.Value;

// Deserialize
NuciConsoleColour color = code switch {
    -1 => NuciConsoleColour.Default,
    0 => NuciConsoleColour.Black,
    // ... or use reflection to find by Value
};
```

---

## Thread Safety

**Immutable** — all properties `get`-only, set in constructor. Safe for concurrent access.

---

## Error Scenarios

| Scenario | Behaviour |
|----------|-----------|
| Invalid `Value` in constructor | Not possible (private, only 17 valid instances) |
| Null `Name` | Not possible (static instances have constants) |
| `ConsoleColour` null | Expected for `Default`; handled by `NuciConsole.Write` |

---

## Flow References

- [Startup and Rendering](../flows/startup-and-rendering.md#colour-rendering-details)

---

## Related Documentation

- [API Reference Index](./INDEX.md)
- [NuciConsole API](./nuci-console.md)
- [Presentation Component](../components/presentation.md#nuciConsoleColour)
- [Design Decisions](../design-decisions.md#2-nuciConsoleColour-as-value-objects-instead-of-consolecolor-directly)