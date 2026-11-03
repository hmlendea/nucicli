# Application Services

NuciCLI provides three core application services through `NuciConsole`:

## 1. Input Service

### Line Input (`ReadLine`)

**Purpose**: Read a line of text with editing support.

**Features**:
- Prompt with optional colours
- Character-by-character input
- Backspace support
- Escape cancellation → `InputCancellationException`
- Enter confirmation
- Character validation (alphanumeric, symbols, punctuation, whitespace)

**Limitations**:
- No history/recall
- No tab completion
- No password masking
- No input length limit
- No regex/pattern validation
- Single-line only

### Key Input (`ReadKey`)

**Purpose**: Read a single key press.

**Features**:
- Prompt with optional colours
- Non-intercepted (key echoed by terminal)
- Returns full `ConsoleKeyInfo` (key, modifiers, char)

**Limitations**:
- No cancellation
- No validation
- Key displayed by terminal (not suppressed)

### Confirmation Input (`ReadPermission`)

**Purpose**: Yes/No confirmation with default.

**Features**:
- Prompt with Y/n or y/N suffix based on default
- Coloured prompt
- Y/N/Enter handling
- Invalid keys ignored (cursor backed up)
- Returns `bool`

**Limitations**:
- Fixed Y/N keys (not localizable)
- No timeout
- No "cancel" option distinct from "no"

## 2. Output Service

### Coloured Write (`Write`, `WriteLine`, `WriteLines`)

**Purpose**: Write text with foreground/background colours.

**Features**:
- Per-call colour specification
- Automatic colour save/restore
- `Default` colour = no change
- `WriteLine` adds `Environment.NewLine`
- `WriteLines` iterates `IEnumerable<string>`

**Limitations**:
- No formatting/alignment helpers
- No progress bars/spinners
- No structured output (JSON, tables)
- No buffering/flushing control

## 3. Console Information Service

### Cursor & Buffer Properties

**Purpose**: Access console geometry and cursor position.

**Properties**:
| Property | Description |
|----------|-------------|
| `CursorX` | Left column (0-based), get/set |
| `CursorY` | Top row (0-based), get/set |
| `CursorSize` | Cursor height percentage (1-100), get |
| `Width` | Buffer width in columns, get |
| `Height` | Buffer height in rows, get |
| `Area` | `Width * Height`, get |

**Limitations**:
- Read-only for buffer dimensions
- Setting cursor may throw if out of bounds
- No window vs buffer distinction
- No resize events

## Service Interactions

```
Input Service          Output Service          Info Service
     │                      │                      │
     ├─ ReadLine ──────────►│ (prompt)             │
     │                      │                      │
     ├─ ReadKey ───────────►│ (prompt)             │
     │                      │                      │
     ├─ ReadPermission ────►│ (prompt)             │
     │                      │                      │
     │                      ├─ Write ─────────────►│ (colours)
     │                      │                      │
     │                      ├─ WriteLine ─────────►│ (colours)
     │                      │                      │
     │                      └─ WriteLines ────────►│ (colours)
     │                                             │
     └─ CursorX/Y (set) ──────────────────────────►│
```

## No Background Services

NuciCLI runs **no background threads, timers, or async operations**. All services are synchronous and blocking.

## No State Management

Services hold **no internal state**. All state resides in `System.Console`:
- Cursor position
- Foreground/background colours
- Buffer dimensions
- Input buffer

## Error Handling per Service

| Service | Errors |
|---------|--------|
| Input | `InputCancellationException` (Escape in ReadLine), `IOException` (console redirected), `ArgumentException` (invalid cursor set) |
| Output | `IOException` (console redirected), `ArgumentNullException` (null lines) |
| Info | `ArgumentException` (cursor out of bounds), `IOException` (console redirected) |

## Extensibility

**None built-in**. Consumers must wrap/adapt for:
- Custom validation
- Input history
- Output formatting
- Logging/interception
- Testing mocks

See [components/host-and-composition.md](./host-and-composition.md#composition-for-consumers).