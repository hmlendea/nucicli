# Data Model

## Domain Entities

NuciCLI has **no persistent domain entities**. It is a stateless façade over `System.Console`.

## In-Memory Types

### NuciConsoleColour

```csharp
public sealed class NuciConsoleColour
{
    public int Value { get; }           // -1 = Default, 0-15 = ConsoleColor
    public ConsoleColor? ConsoleColour { get; }  // null for Default
    public string Name { get; }         // "Default", "Red", etc.
}
```

**Instances**: 17 static readonly singletons (Default, Black...White)

**Relationships**: None. Value object, no references.

### InputCancellationException

```csharp
public class InputCancellationException : Exception
{
    // Standard Exception properties: Message, StackTrace, InnerException
}
```

**Relationships**: Inherits `Exception`. No custom properties.

### ConsoleKeyInfo (BCL)

Used as return type for `ReadKey`. Structure:
```csharp
public struct ConsoleKeyInfo
{
    public ConsoleKey Key { get; }
    public ConsoleModifiers Modifiers { get; }
    public char KeyChar { get; }
}
```

## No Persistence

| Aspect | Status |
|--------|--------|
| Database | ❌ |
| File storage | ❌ |
| Serialization | ❌ (except exception serialization) |
| Caching | ❌ |
| State recovery | ❌ |

## Console State (External)

All mutable state resides in `System.Console` (process-global):

| State | Type | Mutability |
|-------|------|------------|
| Cursor position | `int` (X, Y) | Read/Write |
| Foreground colour | `ConsoleColor` | Read/Write |
| Background colour | `ConsoleColor` | Read/Write |
| Buffer dimensions | `int` (Width, Height) | Read-only |
| Cursor size | `int` (1-100) | Read/Write |
| TreatControlCAsInput | `bool` | Read/Write (temporary) |
| Input buffer | OS-managed | Read via `ReadKey` |

## Data Flow

```
Input (keyboard)
    │
    ▼
System.Console (OS/terminal)
    │
    ▼
NuciConsole.ReadKey/ReadLine
    │
    ├─► ConsoleKeyInfo (key input)
    ├─► string (line input)
    └─► InputCancellationException (cancel)
    │
    ▼
Consumer Application
```

```
Consumer Application
    │
    ▼
NuciConsole.Write/WriteLine
    │
    ▼
System.Console (colours, cursor)
    │
    ▼
Output (stdout/terminal)
```

## Relationships

```
NuciConsole (static)
    │
    ├─► uses ──► NuciConsoleColour (17 instances)
    │
    ├─► throws ─► InputCancellationException
    │
    ├─► wraps ──► System.Console (BCL)
    │
    └─► uses ──► NuciExtensions.IsNullOrEmpty (extension)
```

## No Entity Relationships

- No parent/child
- No collections
- No foreign keys
- No navigation properties

## Schema Evolution

Not applicable — no persisted schema.

## Related Documentation

- [components/presentation.md](../components/presentation.md)
- [state-and-persistence.md](../state-and-persistence.md)
- [architecture.md](../architecture.md#public-api-surface)