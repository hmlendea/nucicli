# Design Decisions

Key architectural and design choices with rationale.

## 1. Static Façade Over Instance-Based Design

**Decision**: All public API members are `static` on `NuciConsole` and `NuciConsoleColour`.

**Rationale**:
- Console is a process-wide singleton resource; no need for multiple instances
- Eliminates DI container overhead for a lightweight library
- Simplifies consumer usage: `NuciConsole.ReadLine()` vs `new ConsoleService().ReadLine()`
- Matches `System.Console` static API pattern consumers already know

**Trade-offs**:
- Not testable via mocking (no interface to mock)
- Global state (console colours, cursor) shared across callers
- Cannot support multiple simultaneous console sessions

**Mitigation**: Consumers wrap NuciCLI in their own abstractions for testing.

## 2. NuciConsoleColour as Value Objects Instead of ConsoleColor Directly

**Decision**: Custom `NuciConsoleColour` type with `Default` sentinel instead of using `ConsoleColor?` directly.

**Rationale**:
- `ConsoleColor` has no "default/unchanged" value — all 16 values are explicit colours
- `Default` (Value=-1, ConsoleColour=null) signals "use current console colour"
- Encapsulates mapping logic: `Value` → `ConsoleColor?` conversion in one place
- Provides `Name` property for debugging/logging
- Immutable (readonly properties, private constructor)

**Trade-offs**:
- Extra allocation per colour parameter (mitigated: static instances reused)
- Consumers must learn NuciCLI colour constants

## 3. InputCancellationException for Escape Key

**Decision**: `ReadLine()` throws `InputCancellationException` when user presses Escape.

**Rationale**:
- Escape is a standard "cancel input" convention in CLI tools
- Exception cleanly unwinds call stack without return-value ambiguity
- Distinguishes "user cancelled" from "empty input" (empty string valid)
- No `CancellationToken` plumbing needed for simple CLI scenarios

**Trade-offs**:
- Exception for control flow (but: exceptional = user explicitly cancelled)
- Callers must catch to handle gracefully
- No cancellation support in `ReadKey()` or `ReadPermission()`

## 4. TreatControlCAsInput Toggle in PerformKeyRead

**Decision**: Temporarily set `Console.TreatControlCAsInput = true` during key reads.

**Rationale**:
- Default `false` means Ctrl+C terminates process — hostile for interactive CLI
- Setting `true` returns Ctrl+C as `ConsoleKeyInfo` for caller handling
- Restored immediately after read to avoid side effects

**Trade-offs**:
- Not thread-safe (global console property)
- Brief window where Ctrl+C doesn't terminate

## 5. No Async API

**Decision**: All methods synchronous.

**Rationale**:
- `System.Console` APIs are synchronous
- CLI tools typically single-threaded, blocking input is expected
- Async would add `Task` overhead without benefit for console I/O
- Simpler mental model for consumers

**Trade-offs**:
- Blocks calling thread during input
- Cannot integrate with async pipelines without `Task.Run`

## 6. Single Assembly, No Internal Layering

**Decision**: One project, one assembly, no internal namespaces/layers.

**Rationale**:
- Library is small (~300 lines); layering adds complexity without benefit
- All types are public API or private implementation details
- Single NuGet package, simple versioning

**Trade-offs**:
- No internal/protected extensibility points
- All implementation details visible via reflection

## 7. NuciExtensions Dependency

**Decision**: Depend on `NuciExtensions` (v6.0.2) for `IsNullOrEmpty` checks.

**Rationale**:
- Avoids duplicating common string/collection helpers
- NuciExtensions is a stable, lightweight utility library by same author
- Reduces maintenance burden

**Trade-offs**:
- External dependency (though minimal and same-org)
- Version coupling

## 8. GPL-3.0-or-later License

**Decision**: Copyleft license.

**Rationale**:
- Ensures derivatives remain free/open source
- Aligns with author's preference for copyleft
- NuGet package includes license expression

**Trade-offs**:
- Limits commercial proprietary use
- May deter some corporate adopters

## 9. net10.0 Target Framework

**Decision**: Target .NET 10.0.

**Rationale**:
- Uses latest .NET features and performance improvements
- Console APIs stable across versions
- Author controls deployment environment

**Trade-offs**:
- Consumers on older .NET cannot use directly (but can multi-target)

## 10. No Configuration System

**Decision**: No configuration files, options classes, or settings.

**Rationale**:
- Library is stateless; nothing to configure
- Colours passed per-call; cursor manipulated directly
- Simplicity over flexibility

**Trade-offs**:
- No global defaults for colours/prompts
- Consumers build their own wrappers for consistent styling