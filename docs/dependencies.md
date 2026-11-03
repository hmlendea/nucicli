# Dependencies

## Direct Dependencies

### NuciExtensions

| Property | Value |
|----------|-------|
| Package | `NuciExtensions` |
| Version | `6.0.2` |
| Type | NuGet package |
| Author | Horațiu Mlendea (same org) |
| Purpose | Extension methods (e.g., `IsNullOrEmpty`) |
| Transitive | No |

**Usage in NuciCLI**: Referenced in `NuciCLI.csproj`. Actual usage not visible in current source files (may be in non-visible code or transitive).

### System.Console (BCL)

| Property | Value |
|----------|-------|
| Framework | .NET 10.0 |
| Type | Built-in framework library |
| Purpose | Low-level console I/O, cursor, colours |
| Version | Part of .NET runtime |

**Usage**: Direct calls to `Console.*` static members throughout `NuciConsole.cs`.

## Transitive Dependencies

### NuciExtensions → (unknown)

Not analyzed. Would require inspecting `NuciExtensions` package.

### .NET 10.0 Runtime

Includes: `System.Runtime`, `System.Console`, `System.Threading`, etc.

## Dependency Graph

```
NuciCLI (net10.0)
├── NuciExtensions (6.0.2) ──► [transitive deps]
└── System.Console (BCL) ──► .NET 10.0 Runtime
```

## Version Policy

| Dependency | Policy | Current |
|------------|--------|---------|
| NuciExtensions | Pinned exact version | 6.0.2 |
| .NET Runtime | Target framework | net10.0 |

**No floating versions** (`*`, ranges). All pinned.

## Vulnerability Management

- **NuGet.org**: Scans packages on publish
- **GitHub Dependabot**: Not configured (no dependabot.yml)
- **Manual**: Author updates NuciExtensions when needed

## License Compatibility

| Dependency | License | Compatible with GPL-3.0? |
|------------|---------|-------------------------|
| NuciExtensions | Unknown (same author) | Likely yes |
| .NET Runtime | MIT | Yes |

## Update Procedure

### NuciExtensions

1. Check `NuciExtensions` changelog/releases
2. Update `<PackageReference Version="x.y.z" />` in `NuciCLI.csproj`
3. Run `dotnet restore`
4. Run `dotnet build`
5. Run `dotnet test`
6. Verify no breaking changes

### .NET Version

1. Install target .NET SDK
2. Update `<TargetFramework>` in `NuciCLI.csproj`
3. Update CI workflow (`.github/workflows/dotnet.yml`)
4. Test on new runtime
5. Consider multi-targeting

## No Other Dependencies

NuciCLI does **not** depend on:
- Logging frameworks (Serilog, NLog, etc.)
- DI containers (Microsoft.Extensions.DependencyInjection, etc.)
- Configuration (Microsoft.Extensions.Configuration, etc.)
- JSON serialization (System.Text.Json, Newtonsoft.Json)
- HTTP clients (HttpClient, etc.)
- Testing frameworks (in main project)
- UI frameworks (Spectre.Console, Terminal.Gui, etc.)

## Development Dependencies (Test Project Only)

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.NET.Test.Sdk | 18.11.0 | Test runner |
| NUnit | 5.0.0 | Test framework |
| NUnit3TestAdapter | 6.3.1 | VS Test Explorer integration |

## Related Documentation

- [build-and-deployment.md](./build-and-deployment.md)
- [repository-structure.md](./repository-structure.md#dependency-graph)
- [components/integration-models.md](../components/integration-models.md)