# NuciCLI Documentation

## Repository Summary

NuciCLI is a lightweight .NET library for building interactive command-line applications. It wraps common console operations behind a small, focused API for prompted text input, single-key input, yes/no confirmation prompts, coloured output, and cursor/buffer dimension access. The library has no external runtime dependencies beyond the .NET base class library and NuciExtensions.

## Root Document Map

- [ARCHITECTURE.md](../ARCHITECTURE.md) — High-level architecture and design
- [SECURITY.md](../SECURITY.md) — Security policy and vulnerability reporting
- [ROADMAP.md](../ROADMAP.md) — Project roadmap (if present)
- [PRIVACY.md](../PRIVACY.md) — Privacy and data handling
- [LICENSE](../LICENSE) — GNU GPL v3 license
- [CONTRIBUTING.md](../CONTRIBUTING.md) — Contribution guidelines
- [CODE_OF_CONDUCT.md](../CODE_OF_CONDUCT.md) — Community conduct standards

## Documentation Catalogue

### Architecture & Design
- [architecture.md](./architecture.md) — Detailed architecture complementing root ARCHITECTURE.md
- [design-decisions.md](./design-decisions.md) — Key architectural choices and rationale
- [repository-overview.md](./repository-overview.md) — Repository purpose, scope, entry points
- [repository-structure.md](./repository-structure.md) — Source tree layout and module organisation

### Components
- [components/presentation.md](./components/presentation.md) — Console presentation layer (NuciConsole, NuciConsoleColour)
- [components/host-and-composition.md](./components/host-and-composition.md) — Static façade pattern, no DI/composition root
- [components/application-services.md](./components/application-services.md) — Core console interaction services
- [components/integration-models.md](./components/integration-models.md) — External integration models (BCL, NuciExtensions)

### Flows (End-to-End Execution)
- [flows/startup-and-rendering.md](./flows/startup-and-rendering.md) — Library initialization and output rendering
- [flows/calendar-search.md](./flows/calendar-search.md) — Not applicable (placeholder for pattern)
- [flows/detail-mutation.md](./flows/detail-mutation.md) — Input reading, cancellation, and mutation flows

### Behaviour (User-Facing)
- [behaviour/browse-and-search.md](./behaviour/browse-and-search.md) — Console navigation and discovery
- [behaviour/inspect-edit-delete.md](./behaviour/inspect-edit-delete.md) — Input editing, cancellation, confirmation

### API Reference
- [api-reference/INDEX.md](./api-reference/INDEX.md) — API reference index with endpoint summary
- [api-reference/nuci-console.md](./api-reference/nuci-console.md) — NuciConsole API documentation
- [api-reference/nuci-console-colour.md](./api-reference/nuci-console-colour.md) — NuciConsoleColour API documentation

### Cross-Cutting Concerns
- [configuration.md](./configuration.md) — Configuration schema, sources, precedence
- [dependencies.md](./dependencies.md) — External and internal dependencies
- [error-handling.md](./error-handling.md) — Error taxonomy, handling patterns, recovery
- [logging.md](./logging.md) — Logging framework, levels, structured fields, correlation, sinks
- [security.md](./security.md) — Security model, threats, mitigations (complements root SECURITY.md)
- [state-and-persistence.md](./state-and-persistence.md) — State management (stateless design)
- [concurrency-and-scheduling.md](./concurrency-and-scheduling.md) — Threading, async, schedulers, locks
- [data-model.md](./data-model.md) — Domain entities, relationships, persistence
- [invariants.md](./invariants.md) — System-wide invariants and contracts
- [testing.md](./testing.md) — Test strategy, organisation, coverage
- [build-and-deployment.md](./build-and-deployment.md) — Build pipeline, deployment, environments
- [integrations.md](./integrations.md) — External system integrations
- [change-guide.md](./change-guide.md) — How to modify common areas safely
- [documentation-maintenance.md](./documentation-maintenance.md) — How to keep docs current, ownership
- [troubleshooting.md](./troubleshooting.md) — Common issues and solutions
- [faq.md](./faq.md) — Frequently asked questions
- [ambiguities-and-open-questions.md](./ambiguities-and-open-questions.md) — Unresolved items, TODOs, known gaps
- [api-usage-examples.md](./api-usage-examples.md) — Example requests/responses for API usage
- [quick-start.md](./quick-start.md) — Getting started guide for newcomers

## Navigation Aids

- **Start here** (newcomers): [quick-start.md](./quick-start.md) → [repository-overview.md](./repository-overview.md) → [api-reference/INDEX.md](./api-reference/INDEX.md)
- **Deep dive** (component owners): [components/](./components/) → [architecture.md](./architecture.md) → [design-decisions.md](./design-decisions.md)
- **Flows** (debuggers): [flows/](./flows/) → [error-handling.md](./error-handling.md) → [troubleshooting.md](./troubleshooting.md)

## Maintenance Metadata

- **Last reviewed**: 2026-11-03
- **Owner**: Horațiu Mlendea
- **Coverage status**: Complete for current codebase (v3.0.2)