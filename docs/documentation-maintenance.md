# Documentation Maintenance

## Ownership

- **Primary**: Horațiu Mlendea (repository owner)
- **Contributors**: Update docs with code changes (enforced by PR guidelines)

## Maintenance Principles

1. **Docs with Code**: Documentation updates in same PR as code changes
2. **Single Source of Truth**: Code is authoritative; docs reflect code
3. **No Drift**: CI should validate docs match implementation (future)

## Update Triggers

| Trigger | Docs to Update |
|---------|----------------|
| New public API | API reference, INDEX, usage examples |
| Behaviour change | Flows, behaviour, component docs |
| New dependency | Dependencies, integration models |
| Breaking change | Change guide, architecture, design decisions |
| Bug fix | Error handling, troubleshooting, FAQ |
| Refactor | Architecture, component docs, design decisions |

## Documentation Structure

```
docs/
├── INDEX.md                      # Master index (update last)
├── architecture.md               # High-level architecture
├── design-decisions.md           # Rationale for choices
├── repository-overview.md        # Purpose, scope, entry points
├── repository-structure.md       # Source tree, modules
├── components/                   # Per-component deep dives
├── flows/                        # End-to-end execution flows
├── behaviour/                    # User-facing behaviours
├── api-reference/                # API reference (per type)
├── api-usage-examples.md         # Practical examples
├── ambiguities-and-open-questions.md  # Known gaps
├── build-and-deployment.md       # CI/CD, release process
├── change-guide.md               # How to modify safely
├── configuration.md              # Configuration (none)
├── concurrency-and-scheduling.md # Threading, async
├── data-model.md                 # Domain entities
├── dependencies.md               # External deps
├── error-handling.md             # Error taxonomy
├── faq.md                        # Common questions
├── invariants.md                 # System invariants
├── integrations.md               # External integrations
├── logging.md                    # Logging (none)
├── quick-start.md                # Newcomer guide
├── security.md                   # Security model
├── state-and-persistence.md      # State management
├── testing.md                    # Test strategy
└── troubleshooting.md            # Common issues
```

## Review Checklist

Per PR, verify:

- [ ] API reference matches implementation (signatures, exceptions, behaviour)
- [ ] Flow diagrams match code paths
- [ ] Examples compile and run
- [ ] Cross-references valid (relative links from docs/)
- [ ] No duplicate content with root docs (ARCHITECTURE.md, SECURITY.md, etc.)
- [ ] Root docs referenced, not duplicated

## Link Validation

All cross-references use relative paths from `docs/`:
- `[Architecture](./architecture.md)` ✅
- `[API Reference](./api-reference/INDEX.md)` ✅
- `[Root Architecture](../ARCHITECTURE.md)` ✅

## Root Document Integration

| Root Doc | Docs Integration |
|----------|------------------|
| ARCHITECTURE.md | Referenced from INDEX.md, architecture.md elaborates |
| SECURITY.md | Referenced from INDEX.md, security.md elaborates |
| PRIVACY.md | Referenced from INDEX.md |
| ROADMAP.md | Referenced from INDEX.md (if exists) |
| LICENSE | Referenced from INDEX.md |
| CONTRIBUTING.md | Referenced from INDEX.md |
| CODE_OF_CONDUCT.md | Referenced from INDEX.md |

**Rule**: Never duplicate root doc content under `docs/`. Complement only.

## Versioning

- Docs versioned with code (same repo, same commit)
- No separate versioning
- `INDEX.md` has "Last reviewed" date

## Automation Opportunities

Future improvements:
- [ ] DocFX or similar for API reference generation
- [ ] Link checker in CI
- [ ] Example code compilation test
- [ ] Coverage report for documented vs undocumented APIs

## Related Documentation

- [CONTRIBUTING.md](../CONTRIBUTING.md#documentation)
- [repository-structure.md](./repository-structure.md#documentation-files-docs)
- [change-guide.md](./change-guide.md#documentation-sync)