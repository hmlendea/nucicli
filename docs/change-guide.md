# Change Guide

How to modify common areas safely.

## Adding a New Console Operation

### 1. Add to NuciConsole

```csharp
// In NuciConsole.cs
public static void NewOperation(string param, NuciConsoleColour fg = default, NuciConsoleColour bg = default)
{
    // Follow existing pattern:
    // 1. Save colours
    // 2. Apply colours
    // 3. Perform operation
    // 4. Restore colours (in finally!)
}
```

### 2. Add Overloads

Follow existing overload pattern:
```csharp
public static void NewOperation() => NewOperation(default, default, default);
public static void NewOperation(string param) => NewOperation(param, default, default);
public static void NewOperation(string param, NuciConsoleColour fg) => NewOperation(param, fg, default);
```

### 3. Update Documentation

- [api-reference/nuci-console.md](../api-reference/nuci-console.md)
- [api-reference/INDEX.md](../api-reference/INDEX.md)
- [api-usage-examples.md](../api-usage-examples.md)

### 4. Add Tests

- Unit test for each overload
- Integration test for colour behaviour
- Edge case tests (null, empty, escape, etc.)

## Modifying Colour Handling

### Current Pattern

```csharp
ConsoleColor oldFg = Console.ForegroundColor;
ConsoleColor oldBg = Console.BackgroundColor;
Console.ResetColor();
if (fg.ConsoleColour != null) Console.ForegroundColor = fg.ConsoleColour.Value;
if (bg.ConsoleColour != null) Console.BackgroundColor = bg.ConsoleColour.Value;
Console.Write(text);
Console.ForegroundColor = oldFg;
Console.BackgroundColor = oldBg;
```

### Required Fix: Exception Safety

```csharp
ConsoleColor oldFg = Console.ForegroundColor;
ConsoleColor oldBg = Console.BackgroundColor;
try
{
    Console.ResetColor();
    if (fg.ConsoleColour != null) Console.ForegroundColor = fg.ConsoleColour.Value;
    if (bg.ConsoleColour != null) Console.BackgroundColor = bg.ConsoleColour.Value;
    Console.Write(text);
}
finally
{
    Console.ForegroundColor = oldFg;
    Console.BackgroundColor = oldBg;
}
```

**Apply to ALL Write overloads** (Write, WriteLine, WriteLines).

## Adding a New Colour

### Not Currently Supported

`NuciConsoleColour` is `sealed` with private constructor. To add colours:

1. **Option A**: Modify NuciCLI source (add static property)
2. **Option B**: Consumer uses `Console.ForegroundColor` directly
3. **Option C**: Add factory method (requires API change)

### If Modifying Source

```csharp
// In NuciConsoleColour.cs
public static NuciConsoleColour Orange => new(16, nameof(Orange)); // But ConsoleColor only 0-15!
```

**Problem**: `ConsoleColor` enum only has 16 values (0-15). Cannot extend without breaking mapping.

**Alternative**: Support 256-colour/truecolor via ANSI sequences (out of scope).

## Modifying Input Validation

### Current Validation (ReadLine)

```csharp
if (char.IsLetterOrDigit(key.KeyChar) ||
    char.IsSymbol(key.KeyChar) ||
    char.IsPunctuation(key.KeyChar) ||
    char.IsWhiteSpace(key.KeyChar))
```

### To Customize

**Option 1**: Add `Func<char, bool>` parameter (breaking change)
**Option 2**: Add virtual/override point (requires instance model)
**Option 3**: Document as fixed, consumers wrap

**Recommended**: Keep fixed, document limitation.

## Adding Cancellation Support

### To ReadKey/ReadPermission

Currently only `ReadLine` supports Escape cancellation.

### Design Options

```csharp
// Option A: Return nullable
public static ConsoleKeyInfo? ReadKey(...) // null = cancelled

// Option B: Throw same exception
public static ConsoleKeyInfo ReadKey(...) // throws InputCancellationException

// Option C: Add CancellationToken
public static ConsoleKeyInfo ReadKey(..., CancellationToken ct)
```

**Recommendation**: Option B for consistency, but document behaviour change.

## Modifying Exception Types

### Current: InputCancellationException

```csharp
public class InputCancellationException : Exception
{
    public InputCancellationException() : base() { }
    public InputCancellationException(string message) : base(message) { }
    public InputCancellationException(string message, Exception inner) : base(message, inner) { }
}
```

### To Add Context

Add properties (non-breaking):
```csharp
public string Prompt { get; }
public string PartialInput { get; }
```

**Constructor**: `InputCancellationException(string prompt, string partialInput)`

## Updating Dependencies

### NuciExtensions

```xml
<PackageReference Include="NuciExtensions" Version="6.0.2" />
```

**Process**:
1. Check changelog for breaking changes
2. Update version in csproj
3. Run `dotnet restore`
4. Run full test suite
5. Verify no new warnings

### .NET Version

**Current**: net10.0

**To Multi-target**:
```xml
<TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>
```

**CI Update**: Matrix strategy in workflow.

## Breaking Change Checklist

Before releasing breaking changes:

- [ ] Major version bump (3.x → 4.0.0)
- [ ] Update `<Version>` in csproj
- [ ] Document migration in CHANGELOG/RELEASE_NOTES
- [ ] Update API reference docs
- [ ] Update usage examples
- [ ] Verify no internal consumers broken
- [ ] Consider obsoleting old API first (if possible)

## Safe Refactoring Patterns

### Extract Method

```csharp
// Before: inline in Write
// After: private static void ApplyColours(NuciConsoleColour fg, NuciConsoleColour bg)
```

### Introduce Parameter Object

```csharp
// Before: Write(text, fg, bg)
// After: Write(text, new ColourOptions(fg, bg))
```

### Replace Magic Numbers

```csharp
// Before: if (Value.Equals(-1))
// After: if (Value == DefaultColourValue)
private const int DefaultColourValue = -1;
```

## Testing Changes

### Required Test Coverage

| Change Type | Tests Needed |
|-------------|--------------|
| New method | Unit + integration |
| Colour logic | All colour combos (Default, FG, BG, both) |
| Input handling | Valid, invalid, escape, backspace, enter |
| Exception paths | Throw conditions, messages, inner exceptions |
| Cursor/buffer | Bounds, negative, max values |

### Test Commands

```bash
# All tests
dotnet test --no-build --verbosity normal

# Specific project
dotnet test NuciCLI.UnitTests/NuciCLI.UnitTests.csproj --no-build --verbosity normal

# With coverage (if configured)
dotnet test --collect:"XPlat Code Coverage"
```

## Documentation Sync

When changing code, update:

1. **API Reference**: `docs/api-reference/nuci-console.md` or `nuci-console-colour.md`
2. **API Index**: `docs/api-reference/INDEX.md` (summary table)
3. **Usage Examples**: `docs/api-usage-examples.md`
4. **Component Docs**: `docs/components/presentation.md`
5. **Flow Docs**: `docs/flows/startup-and-rendering.md` or `detail-mutation.md`
6. **Behaviour Docs**: `docs/behaviour/*.md`
7. **Architecture**: `docs/architecture.md` if structural
8. **Design Decisions**: `docs/design-decisions.md` if new pattern

## Related Documentation

- [repository-structure.md](./repository-structure.md)
- [api-reference/INDEX.md](../api-reference/INDEX.md)
- [components/presentation.md](../components/presentation.md)
- [ambiguities-and-open-questions.md](../ambiguities-and-open-questions.md)