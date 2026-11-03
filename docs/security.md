# Security

> Complements root [SECURITY.md](../SECURITY.md). This file covers implementation-level security details.

## Threat Model

### Trust Boundary

```
┌─────────────────────────────────────┐
│  Untrusted Input (Keyboard)         │
│  ↓                                  │
│  NuciConsole.ReadKey/ReadLine       │
│  ↓                                  │
│  Consumer Application               │
│  ↓                                  │
│  Trusted Operations                 │
└─────────────────────────────────────┘
```

**NuciCLI is a pass-through**: It does not validate, sanitize, or filter input. All security responsibility lies with the consumer.

## Input Handling

### No Input Validation

- `ReadLine` accepts any character (except Escape, Enter, Backspace)
- No length limit (except console buffer)
- No character filtering
- No encoding validation

**Consumer must validate all input.**

### No Password Masking

- `ReadLine` echoes characters to console
- No `ReadPassword` method
- **Consumer must implement masking** using `ReadKey(true)`

```csharp
// Consumer-implemented password masking
string ReadPassword(string prompt)
{
    NuciConsole.Write(prompt);
    var sb = new StringBuilder();
    while (true)
    {
        var key = NuciConsole.ReadKey(true);
        if (key.Key == ConsoleKey.Enter) break;
        if (key.Key == ConsoleKey.Backspace) { /* handle */ }
        else sb.Append(key.KeyChar);
    }
    return sb.ToString();
}
```

### No Input Length Limits

- `ReadLine` loops until Enter
- No max length parameter
- **Consumer must enforce limits**

## Output Handling

### No Output Encoding

- `Write*` methods write strings directly
- No HTML/ANSI escaping
- **Consumer must escape if needed**

### No Sensitive Data Redaction

- NuciCLI does not know what is sensitive
- **Consumer must not write secrets to console**

## Colour State

### Global State Risk

- `Console.ForegroundColor` is process-global
- Multi-threaded consumers may see race conditions
- **Consumer must synchronize**

### No Colour Injection

- Colours are enum values, not strings
- No ANSI escape sequence injection possible
- **Safe by design**

## Exception Information Leakage

### InputCancellationException

- Contains only standard exception data
- No sensitive information in Message
- **Safe to propagate**

### IOException

- May contain file paths or system info
- **Consumer should catch and sanitize**

## No Cryptography

- No encryption, hashing, or signing
- No key management
- No secure string handling

## No Authentication/Authorization

- No user identity
- No permission checks
- No role-based access

## No Network

- No HTTP, TCP, or socket usage
- No DNS resolution
- No certificate handling

## No File System Access

- No file reads/writes
- No path handling
- No temp files

## No Process Execution

- No `Process.Start`
- No shell commands
- No environment variable access

## Dependency Security

| Dependency | Risk | Notes |
|------------|------|-------|
| NuciExtensions 6.0.2 | Low | Same author, pinned version |
| System.Console (BCL) | Low | Part of .NET runtime |

**No transitive dependencies** beyond .NET runtime.

## Supply Chain

- **NuGet package**: Signed? Not verified
- **Source**: GitHub (public)
- **CI**: GitHub Actions (Microsoft-hosted runners)
- **No SBOM** generated

## Recommended Consumer Practices

1. **Validate all input** from `ReadLine`/`ReadKey`
2. **Implement password masking** if needed
3. **Catch `IOException`** for non-interactive environments
4. **Synchronize access** if multi-threaded
5. **Don't log sensitive data** to console
6. **Review GPL-3.0 compliance** for derivative works

## Known Security Considerations

| Issue | Severity | Status |
|-------|----------|--------|
| No password masking | Medium | By design (consumer responsibility) |
| No input length limits | Low | By design |
| Global colour state race | Low | Consumer must synchronize |
| GPL-3.0 viral licence | Legal | Documented |

## Related Documentation

- [SECURITY.md](../SECURITY.md) — root security policy
- [error-handling.md](./error-handling.md) — exception details
- [invariants.md](./invariants.md) — safety guarantees
- [integrations.md](./integrations.md) — external surface