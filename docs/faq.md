# FAQ

## General

### What is NuciCLI?
A lightweight .NET library for building interactive command-line applications. It wraps common console operations (input, output, colours, cursor) behind a simple static API.

### Who is it for?
Developers building CLI tools who want a minimal dependency for console interaction without heavy TUI frameworks.

### What .NET versions are supported?
Currently **net10.0** only. The API uses only stable BCL console APIs, so multi-targeting older versions is feasible.

### Is it cross-platform?
Yes, runs anywhere .NET runs (Windows, Linux, macOS). Uses `System.Console` which abstracts platform differences.

### What licence?
GPL-3.0-or-later (copyleft).

## Usage

### How do I install?
```bash
dotnet add package NuciCLI
```

### How do I read a line with a coloured prompt?
```csharp
string name = NuciConsole.ReadLine("Name: ", NuciConsoleColour.Yellow);
```

### How do I handle Escape cancellation?
```csharp
try
{
    string input = NuciConsole.ReadLine("Input: ");
}
catch (InputCancellationException)
{
    // Handle cancel
}
```

### How do I write coloured text?
```csharp
NuciConsole.WriteLine("Error!", NuciConsoleColour.White, NuciConsoleColour.Red);
```

### How do I ask for confirmation?
```csharp
bool ok = NuciConsole.ReadPermission("Continue?", defaultValue: true);
```

### Can I set default colours globally?
No. Pass colours per-call or create a wrapper/theme class.

### Does it support async/await?
No. All methods synchronous. Wrap in `Task.Run` if needed.

### Does it support tab completion/history?
No. Build on top of `ReadKey`/`ReadLine` primitives.

### Can I use it in a background service?
Only if a console is attached. Check `Console.IsOutputRedirected` first.

## Troubleshooting

### Colours don't appear
- Terminal may not support colours
- Check `Console.IsOutputRedirected`
- Some terminals need explicit ANSI enable

### Cursor position wrong after resize
- NuciCLI doesn't handle resize events
- Re-read `Width`/`Height` after resize

### Escape doesn't cancel
- Only works in `ReadLine`, not `ReadKey` or `ReadPermission`

### Exception: "Handle is invalid" / IOException
- No console attached (service, redirected output)
- Check `Console.IsOutputRedirected` before calling

## Development

### How do I run tests?
```bash
dotnet test --no-build --verbosity normal
```

### How do I build locally?
```bash
dotnet build --configuration Release
```

### How do I contribute?
See [CONTRIBUTING.md](../CONTRIBUTING.md)

### Where is the source?
https://github.com/hmlendea/nucicli

## Limitations

### No mouse support
`Console.ReadKey` doesn't report mouse events.

### No raw mode / ANSI sequences
Use `Console` directly for escape codes.

### No window size (only buffer)
`Console.WindowWidth/Height` not exposed.

### Not thread-safe
Synchronize externally if multi-threaded.

### No password masking
`ReadLine` echoes characters. Build custom with `ReadKey`.

## Related Documentation

- [quick-start.md](./quick-start.md)
- [api-usage-examples.md](../api-usage-examples.md)
- [troubleshooting.md](../troubleshooting.md)
- [ambiguities-and-open-questions.md](../ambiguities-and-open-questions.md)