# Build and Deployment

## Build Pipeline

### Local Build

```bash
# Restore dependencies
dotnet restore

# Build solution (Debug)
dotnet build --configuration Debug

# Build solution (Release)
dotnet build --configuration Release --no-restore

# Run tests
dotnet test --no-build --verbosity normal
```

### CI Pipeline (`.github/workflows/dotnet.yml`)

**Triggers**: Push to master, Pull requests to master

**Jobs**:
1. **Build**
   - Runner: `ubuntu-latest`
   - Steps:
     - Checkout (actions/checkout@v2)
     - Setup .NET 10.0.x (actions/setup-dotnet@v1)
     - Restore: `dotnet restore`
     - Build: `dotnet build --no-restore`
     - Test: `dotnet test --no-build --verbosity normal`

**No code coverage, linting, or static analysis steps currently configured.**

## Release Pipeline

### NuGet.org Release (`.github/workflows/nuget-release.yml`)

**Trigger**: GitHub Release published

**Permissions**: `contents: read`, `id-token: write` (for OIDC)

**Steps**:
1. Checkout with full history (`fetch-depth: 0`)
2. Setup .NET 10.0.x
3. Extract version from tag (strips `v` prefix)
4. Restore dependencies
5. Pack: `dotnet pack -c Release --output ./nupkg -p:Version=<version>`
6. NuGet OIDC login (NuGet/login@v1)
7. Push: `dotnet nuget push ... --source https://api.nuget.org/v3/index.json --skip-duplicate`

**Version Strategy**: Git tag → package version (e.g., tag `v3.0.2` → package `3.0.2`)

### GitHub Release (`.github/workflows/github-release.yml`)

**Trigger**: GitHub Release published

**Permissions**: `contents: write`

**Steps**:
1. Checkout with full history
2. Setup .NET 10.0.x
3. Extract version from tag
4. Restore dependencies
5. Pack: `dotnet pack -c Release --output ./nupkg -p:Version=<version>`
6. Calculate SHA256 of `.nupkg`
7. Upload package to GitHub Release (`gh release upload`)
8. Append SHA256 to release notes via GitHub API

## Package Configuration

### NuciCLI.csproj Package Metadata

```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <RootNamespace>NuciCLI</RootNamespace>
  <Version>3.0.2</Version>
  <Description>Lightweight library for building interactive command-line applications.</Description>
  <Authors>Horațiu Mlendea</Authors>
  <Copyright>Copyright 2026 © Horațiu Mlendea</Copyright>
  <RepositoryUrl>https://github.com/hmlendea/nucicli</RepositoryUrl>
  <PackageLicenseExpression>GPL-3.0-or-later</PackageLicenseExpression>
  <PackageTags>Console Terminal CommandLine CLI</PackageTags>
  <GeneratePackageOnBuild>false</GeneratePackageOnBuild>
</PropertyGroup>
```

### Dependencies

```xml
<ItemGroup>
  <PackageReference Include="NuciExtensions" Version="6.0.2" />
</ItemGroup>
```

## Environments

| Environment | Purpose | Configuration |
|-------------|---------|---------------|
| **Development** | Local coding | Debug, net10.0 |
| **CI** | Validation | Release, net10.0, ubuntu-latest |
| **NuGet.org** | Public distribution | Release, signed via OIDC |
| **GitHub Releases** | Artifact archive | Release, with SHA256 |

## Deployment Artifacts

| Artifact | Location | Contents |
|----------|----------|----------|
| NuGet Package | nuget.org | `NuciCLI.<version>.nupkg` |
| GitHub Release Asset | GitHub Releases | `NuciCLI.<version>.nupkg` + SHA256 in notes |
| Build Output | Local `bin/Release/net10.0/` | `NuciCLI.dll`, `NuciCLI.pdb`, `NuciCLI.xml` (if generated) |

## Versioning

- **Scheme**: Semantic Versioning (Major.Minor.Patch)
- **Source**: Git tag (`v<version>`)
- **Current**: 3.0.2
- **Assembly Version**: Derived from `<Version>` in csproj
- **File Version**: Same

## Rollback Procedure

1. **NuGet.org**: Cannot unpublish after 72 hours. Use `--skip-duplicate` prevents overwrite.
2. **GitHub Release**: Delete release + tag, recreate.
3. **Mitigation**: Test thoroughly before publishing release.

## Quality Gates

### Current (CI)
- ✅ Build succeeds
- ✅ Tests pass

### Recommended Additions
- [ ] Code coverage threshold
- [ ] Static analysis (Roslyn analyzers, SonarCloud)
- [ ] Dependency vulnerability scan
- [ ] License compliance check
- [ ] API compatibility check (vs previous version)

## Local Development Workflow

```bash
# 1. Create feature branch
git checkout -b feature/xyz

# 2. Make changes
# ...

# 3. Verify locally
dotnet build --no-restore
dotnet test --no-build --verbosity normal

# 4. Commit and push
git add .
git commit -m "feat: description"
git push origin feature/xyz

# 5. Open PR → CI runs

# 6. Merge to master → triggers CI

# 7. Create release (when ready)
git tag v3.0.3
git push origin v3.0.3
# → GitHub Actions publishes to NuGet.org and GitHub Releases
```

## Related Documentation

- [repository-structure.md](./repository-structure.md#cicd-pipeline-files)
- [configuration.md](./configuration.md)
- [dependencies.md](./dependencies.md)