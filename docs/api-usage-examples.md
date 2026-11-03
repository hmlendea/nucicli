# API Usage Examples

## Basic Output

```csharp
using NuciCLI;

// Simple coloured output
NuciConsole.WriteLine("Welcome to MyApp", NuciConsoleColour.Cyan);
NuciConsole.WriteLine("Version 1.0.0", NuciConsoleColour.Gray);

// Error styling
NuciConsole.WriteLine("ERROR: Connection failed", NuciConsoleColour.White, NuciConsoleColour.Red);

// Multiple lines
NuciConsole.WriteLines(new[]
{
    "Line 1",
    "Line 2",
    "Line 3"
}, NuciConsoleColour.Green);
```

## Prompted Input

```csharp
// Simple prompt
string name = NuciConsole.ReadLine("Enter your name: ");

// Coloured prompt
string email = NuciConsole.ReadLine("Email: ", NuciConsoleColour.Yellow);

// Coloured prompt with background
string password = NuciConsole.ReadLine("Password: ", NuciConsoleColour.White, NuciConsoleColour.DarkBlue);
```

## Confirmation Dialogs

```csharp
// Simple Y/n (default No)
bool proceed = NuciConsole.ReadPermission("Continue?");

// Y/n with default Yes
bool proceed = NuciConsole.ReadPermission("Continue?", defaultValue: true);

// Coloured confirmation
bool delete = NuciConsole.ReadPermission(
    "Delete all files?",
    defaultValue: false,
    NuciConsoleColour.Red,
    NuciConsoleColour.Black
);
```

## Key Input

```csharp
// Wait for any key
NuciConsole.WriteLine("Press any key to continue...");
NuciConsole.ReadKey();

// Prompted key
ConsoleKeyInfo key = NuciConsole.ReadKey("Choose (A/B/C): ", NuciConsoleColour.Cyan);

switch (key.Key)
{
    case ConsoleKey.A: DoA(); break;
    case ConsoleKey.B: DoB(); break;
    case ConsoleKey.C: DoC(); break;
}
```

## Cancellation Handling

```csharp
try
{
    string input = NuciConsole.ReadLine("Enter data (Esc to cancel): ");
    Process(input);
}
catch (InputCancellationException)
{
    NuciConsole.WriteLine("Operation cancelled.", NuciConsoleColour.DarkYellow);
}
```

## Cursor Positioning

```csharp
// Save position
int savedX = NuciConsole.CursorX;
int savedY = NuciConsole.CursorY;

// Move and write
NuciConsole.CursorX = 0;
NuciConsole.CursorY = 5;
NuciConsole.WriteLine("Status: OK", NuciConsoleColour.Green);

// Restore
NuciConsole.CursorX = savedX;
NuciConsole.CursorY = savedY;
```

## Buffer-Aware Layout

```csharp
// Center text
string title = "MY APPLICATION";
int centerX = (NuciConsole.Width - title.Length) / 2;
NuciConsole.CursorX = centerX;
NuciConsole.WriteLine(title, NuciConsoleColour.White, NuciConsoleColour.DarkBlue);

// Status bar at bottom
NuciConsole.CursorY = NuciConsole.Height - 1;
NuciConsole.CursorX = 0;
NuciConsole.WriteLine(" F1=Help  F2=Save  Esc=Exit ".PadRight(NuciConsole.Width),
    NuciConsoleColour.Black, NuciConsoleColour.Gray);
```

## Menu System Example

```csharp
string[] options = { "New Game", "Load Game", "Settings", "Exit" };
int selected = 0;

while (true)
{
    // Render menu
    NuciConsole.CursorX = 0;
    NuciConsole.CursorY = 5;

    for (int i = 0; i < options.Length; i++)
    {
        if (i == selected)
            NuciConsole.WriteLine($"► {options[i]}", NuciConsoleColour.Black, NuciConsoleColour.White);
        else
            NuciConsole.WriteLine($"  {options[i]}", NuciConsoleColour.White);
    }

    // Read navigation
    var key = NuciConsole.ReadKey();

    switch (key.Key)
    {
        case ConsoleKey.UpArrow:
            selected = (selected - 1 + options.Length) % options.Length;
            break;
        case ConsoleKey.DownArrow:
            selected = (selected + 1) % options.Length;
            break;
        case ConsoleKey.Enter:
            ExecuteOption(selected);
            break;
        case ConsoleKey.Escape:
            return;
    }
}
```

## Progress Indicator

```csharp
void ShowProgress(int current, int total, string label = "")
{
    int width = NuciConsole.Width - 10;
    int filled = (int)(width * current / (double)total);
    string bar = "[" + new string('#', filled) + new string('-', width - filled) + "]";
    int pct = current * 100 / total;

    NuciConsole.CursorX = 0;
    NuciConsole.Write($"{bar} {pct}% {label}");
}
```

## Error Handling Pattern

```csharp
string ReadRequired(string prompt, NuciConsoleColour? fg = null)
{
    while (true)
    {
        try
        {
            string input = fg.HasValue
                ? NuciConsole.ReadLine(prompt, fg.Value)
                : NuciConsole.ReadLine(prompt);

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            NuciConsole.WriteLine("Input cannot be empty.", NuciConsoleColour.Red);
        }
        catch (InputCancellationException)
        {
            NuciConsole.WriteLine("Input cancelled. Please try again.", NuciConsoleColour.DarkYellow);
        }
    }
}
```

## Colour Theme Helper

```csharp
static class Theme
{
    public static NuciConsoleColour Primary = NuciConsoleColour.Cyan;
    public static NuciConsoleColour Secondary = NuciConsoleColour.DarkCyan;
    public static NuciConsoleColour Success = NuciConsoleColour.Green;
    public static NuciConsoleColour Warning = NuciConsoleColour.Yellow;
    public static NuciConsoleColour Error = NuciConsoleColour.Red;
    public static NuciConsoleColour Prompt = NuciConsoleColour.Yellow;
    public static NuciConsoleColour Muted = NuciConsoleColour.DarkGray;
}

void LogInfo(string msg) => NuciConsole.WriteLine($"[INFO] {msg}", Theme.Primary);
void LogSuccess(string msg) => NuciConsole.WriteLine($"[OK] {msg}", Theme.Success);
void LogWarning(string msg) => NuciConsole.WriteLine($"[WARN] {msg}", Theme.Warning);
void LogError(string msg) => NuciConsole.WriteLine($"[ERROR] {msg}", Theme.Error);
```

## Complete Interactive Example

```csharp
using NuciCLI;

class Program
{
    static void Main()
    {
        NuciConsole.WriteLine("╔══════════════════════╗", NuciConsoleColour.Cyan);
        NuciConsole.WriteLine("║  User Registration   ║", NuciConsoleColour.Cyan);
        NuciConsole.WriteLine("╚══════════════════════╝", NuciConsoleColour.Cyan);
        NuciConsole.WriteLine();

        try
        {
            string username = ReadNonEmpty("Username: ", NuciConsoleColour.Yellow);
            string email = ReadNonEmpty("Email: ", NuciConsoleColour.Yellow);

            bool confirm = NuciConsole.ReadPermission(
                $"Register {username} ({email})?",
                defaultValue: true,
                NuciConsoleColour.Green
            );

            if (confirm)
            {
                NuciConsole.WriteLine("Registering...", NuciConsoleColour.Cyan);
                // Register user
                NuciConsole.WriteLine("Success!", NuciConsoleColour.Green);
            }
            else
            {
                NuciConsole.WriteLine("Cancelled.", NuciConsoleColour.Red);
            }
        }
        catch (InputCancellationException)
        {
            NuciConsole.WriteLine("\nRegistration cancelled.", NuciConsoleColour.DarkYellow);
        }
    }

    static string ReadNonEmpty(string prompt, NuciConsoleColour fg)
    {
        while (true)
        {
            try
            {
                string input = NuciConsole.ReadLine(prompt, fg);
                if (!string.IsNullOrWhiteSpace(input))
                    return input;
                NuciConsole.WriteLine("This field is required.", NuciConsoleColour.Red);
            }
            catch (InputCancellationException)
            {
                throw; // Re-throw to cancel entire operation
            }
        }
    }
}
```