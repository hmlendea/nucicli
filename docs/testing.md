# Testing

## Test Project

| Property | Value |
|----------|-------|
| Project | `NuciCLI.UnitTests` |
| Framework | net10.0 |
| Runner | NUnit 5.0.0 |
| Adapter | NUnit3TestAdapter 6.3.1 |
| SDK | Microsoft.NET.Test.Sdk 18.11.0 |

## Test Organization

```
NuciCLI.UnitTests/
├── NuciCLI.UnitTests.csproj
└── [test files not visible in source tree]
```

**Note**: Test source files not visible in current workspace. Structure inferred from project file.

## Test Categories

### Unit Tests (Target)

| Area | Coverage Target |
|------|-----------------|
| `NuciConsole.Write*` | Colour save/restore, null handling, empty strings |
| `NuciConsole.ReadLine` | Normal input, Escape cancellation, Backspace, Enter |
| `NuciConsole.ReadKey` | Key interception, modifiers, special keys |
| `NuciConsole.ReadPermission` | Y/N/Enter, default values, case insensitivity |
| `NuciConsoleColour` | All 17 instances, Value mapping, Default sentinel |
| `InputCancellationException` | Constructors, inheritance, serialization |

### Integration Tests (Target)

| Scenario | Description |
|----------|-------------|
| Redirected output | `Console.IsOutputRedirected == true` |
| No console | Background service, container |
| Buffer bounds | Cursor at edges, resize simulation |
| Colour combinations | All fg/bg pairs |

## Running Tests

```bash
# All tests
dotnet test --no-build --verbosity normal

# Filter by category (if traits used)
dotnet test --filter "Category=Unit"

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

## CI Integration

Defined in `.github/workflows/dotnet.yml`:
- Runs on push/PR to main
- `dotnet test --no-build --verbosity normal`
- Fails build on test failure

## Test Design Principles

1. **Test behaviour, not implementation** — verify observable outcomes
2. **Isolate global state** — reset Console between tests (difficult)
3. **Mock time/console** — not currently possible (no abstraction)
4. **Deterministic** — no flaky timing-dependent tests

## Current Gaps

| Gap | Impact |
|-----|--------|
| No visible test files | Cannot verify coverage |
| No Console abstraction | Hard to unit test in isolation |
| No async tests | Not applicable (sync only) |
| No property-based tests | Could validate colour restoration |

## Recommended Test Patterns

### Pattern: Colour Restoration

```csharp
[Test]
public void Write_RestoresColours_OnException()
{
    var origFg = Console.ForegroundColor;
    var origBg = Console.BackgroundColor;

    // Simulate IOException by redirecting output
    using var sw = new StringWriter();
    Console.SetOut(sw);
    Console.SetError(sw);

    Assert.Throws<IOException>(() => NuciConsole.WriteLine("test", NuciConsoleColour.Red));

    Assert.AreEqual(origFg, Console.ForegroundColor);
    Assert.AreEqual(origBg, Console.BackgroundColor);
}
```

### Pattern: Escape Cancellation

```csharp
[Test]
public void ReadLine_Throws_OnEscape()
{
    // Requires input simulation or Console abstraction
    Assert.Throws<InputCancellationException>(() =>
        NuciConsole.ReadLine("Prompt: "));
}
```

## Future: Testability Improvements

| Improvement | Effort | Benefit |
|-------------|--------|---------|
| `IConsole` abstraction | Medium | Mockable, testable |
| `TextWriter` injection | Low | Capture output |
| Virtual methods | Low | Override in tests |
| Test-only constructor | Low | Inject dependencies |

## Related Documentation

- [build-and-deployment.md](./build-and-deployment.md#test-execution)
- [components/presentation.md](../components/presentation.md#testing)
- [error-handling.md](./error-handling.md#test-scenarios)
- [ambiguities-and-open-questions.md](../ambiguities-and-open-questions.md#3-test-coverage-unknown)