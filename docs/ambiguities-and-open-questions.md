# Ambiguities and Open Questions

## Known Gaps

### 1. Test Coverage Visibility
**Status**: Unknown
**Issue**: `NuciCLI.UnitTests` project exists but no test files visible in workspace.
**Questions**:
- Are tests in a separate location?
- Are tests generated at build time?
- What is actual coverage?

### 2. NuciExtensions Usage
**Status**: Unverified
**Issue**: `NuciExtensions` 6.0.2 referenced but usage not visible in source.
**Questions**:
- Which extension methods are used?
- Is dependency truly required or transitive?

### 3. Console Resize Handling
**Status**: Not implemented
**Issue**: No handling of console buffer/window resize events.
**Questions**:
- Should NuciCLI expose resize events?
- Should `Width`/`Height` be cached or always read from BCL?

### 4. Colour Restoration Exception Safety
**Status**: Gap identified
**Issue**: `Write(text, fg, bg)` restores colours sequentially, not in `finally`.
**Risk**: If `Console.Write` throws, colours left modified.
**Fix**: Wrap in `try/finally`.

### 5. Thread Safety Documentation
**Status**: Documented as not thread-safe
**Issue**: No synchronization primitives provided.
**Questions**:
- Should NuciCLI provide a `Lock` object for consumers?
- Should docs emphasize single-threaded usage more strongly?

### 6. Input Validation Extensibility
**Status**: Fixed validation
**Issue**: `ReadLine` only accepts letter/digit/symbol/punctuation/whitespace.
**Questions**:
- Should validation be customizable?
- Should control characters (tab, etc.) be configurable?

### 7. Escape Key in ReadPermission
**Status**: Not handled
**Issue**: Escape treated as invalid key in `ReadPermission`, loops infinitely.
**Questions**:
- Should Escape cancel confirmation?
- Should it return a third state (cancelled vs yes/no)?

### 8. NuciConsoleColour Extensibility
**Status**: Sealed, private constructor
**Issue**: Consumers cannot define custom colours.
**Questions**:
- Is this intentional (simplicity) or oversight?
- Should a factory method be added?

### 9. Target Framework Currency
**Status**: net10.0
**Issue**: .NET 10 is very recent (Nov 2026).
**Questions**:
- Multi-targeting for older .NET (net8.0, net9.0)?
- CI tests on multiple frameworks?

### 10. GPL License Implications
**Status**: GPL-3.0-or-later
**Issue**: Copyleft may limit adoption.
**Questions**:
- Is dual-licensing considered?
- Are there downstream consumers affected?

## Technical Debt

### 1. String Concatenation in ReadLine Loop
```csharp
inputValue += key.KeyChar;  // Allocates new string per char
```
**Impact**: O(n²) allocations for n-character input.
**Fix**: Use `StringBuilder` or `List<char>`.

### 2. No XML Documentation on Private Members
**Impact**: Internal maintenance harder.
**Fix**: Add `/// <summary>` to `PerformKeyRead`.

### 3. Magic Numbers in Colour Mapping
```csharp
if (Value.Equals(-1))  // Default sentinel
```
**Impact**: -1 meaning not self-documenting.
**Fix**: `const int DefaultValue = -1;`

### 4. Parameter Name Typo
```csharp
/// <param name="proxmpt">  // Should be "prompt"
```
**File**: `NuciConsole.cs`, `ReadLine(string, NuciConsoleColour, NuciConsoleColour)`
**Fix**: Correct XML comment.

## Future Considerations

### 1. Async API
**Pros**: Integrate with async pipelines, non-blocking.
**Cons**: Console I/O inherently blocking; `Task.Run` wrapper trivial.
**Decision**: Defer unless strong demand.

### 2. ANSI Escape Sequence Support
**Pros**: Rich formatting (bold, underline, 256-colour, truecolor).
**Cons**: Terminal-dependent; `System.Console` doesn't abstract.
**Decision**: Out of scope for minimal library.

### 3. Input History/Completion
**Pros**: Better UX for REPL-style tools.
**Cons**: Adds state, complexity, memory.
**Decision**: Consumer responsibility.

### 4. Cross-Platform Raw Mode
**Pros**: Key-by-key input without Enter, mouse support.
**Cons**: Platform-specific (Windows vs Unix), complex.
**Decision**: Out of scope.

### 5. Structured Output Helpers
**Pros**: Tables, progress bars, trees.
**Cons**: Scope creep; separate library better.
**Decision**: Separate package if needed.

## Unresolved Design Questions

| Question | Current State | Recommendation |
|----------|---------------|----------------|
| Multi-target frameworks? | net10.0 only | Add net8.0, net9.0 |
| Test visibility? | Unknown | Verify test project contents |
| Colour restoration fix? | Not in `finally` | Apply `try/finally` |
| Escape in ReadPermission? | Ignored | Add cancellation support |
| NuciExtensions necessity? | Unverified | Audit usage or remove |
| Thread safety aids? | None | Document clearly, no primitives |
| Custom colour support? | Sealed | Keep sealed, document rationale |

## Tracking

Review this document quarterly or when:
- New version released
- Consumer feedback received
- Dependency updates
- Breaking changes considered