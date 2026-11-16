using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NuciCLI
{
    /// <summary>
    /// Console extras.
    /// </summary>
    public static class NuciConsole
    {
        /// <summary>
        /// Gets or sets the horizontal cursor position in the console.
        /// </summary>
        public static int CursorX
        {
            get => Console.CursorLeft;
            set => Console.SetCursorPosition(value, CursorY);
        }

        /// <summary>
        /// Gets or sets the vertical cursor position in the console.
        /// </summary>
        public static int CursorY
        {
            get => Console.CursorTop;
            set => Console.SetCursorPosition(CursorX, value);
        }

        /// <summary>
        /// Gets the size of the cursor in the console.
        /// </summary>
        public static int CursorSize => Console.CursorSize;

        /// <summary>
        /// Gets the width of the console buffer.
        /// </summary>
        public static int Width => Console.BufferWidth;

        /// <summary>
        /// Gets the height of the console buffer.
        /// </summary>
        public static int Height => Console.BufferHeight;

        /// <summary>
        /// Gets the area of the console buffer (Width * Height).
        /// </summary>
        public static int Area => Width * Height;

        private static string NoDefaultOptions => "y/N";

        private static string YesDefaultOptions => "Y/n";

        /// <summary>
        /// Reads a key from the standard input without displaying it on the console.
        /// </summary>
        /// <returns>The key information of the pressed key.</returns>
        public static ConsoleKeyInfo ReadKey()
            => ReadKey(string.Empty);

        /// <summary>
        /// Reads a key from the standard input with a prompt, without displaying it on the console.
        /// </summary>
        /// <param name="prompt">The prompt to display before reading the key.</param>
        /// <returns>The key information of the pressed key.</returns>
        public static ConsoleKeyInfo ReadKey(string prompt)
            => ReadKey(prompt, NuciConsoleColour.Default);

        /// <summary>
        /// Reads a key from the standard input with a prompt and a specified foreground colour, without displaying it on the console.
        /// </summary>
        /// <param name="prompt">The prompt to display before reading the key.</param>
        public static ConsoleKeyInfo ReadKey(string prompt, NuciConsoleColour foregroundColour)
            => ReadKey(prompt, foregroundColour, NuciConsoleColour.Default);

        /// <summary>
        /// Reads a key from the standard input with a prompt, foreground colour, and background colour, without displaying it on the console.
        /// </summary>
        /// <param name="prompt">The prompt to display before reading the key.</param>
        /// <param name="foregroundColour">The text colour.</param>
        /// <param name="backgroundColour">The background colour.</param>
        /// <returns>The key information of the pressed key.</returns>
        public static ConsoleKeyInfo ReadKey(
            string prompt,
            NuciConsoleColour foregroundColour,
            NuciConsoleColour backgroundColour)
        {
            Write(prompt, foregroundColour, backgroundColour);

            return PerformKeyRead();
        }

        /// <summary>
        /// Reads a line of input from the standard input, displaying a prompt if provided.
        /// </summary>
        /// <returns>The input string entered by the user.</returns>
        public static string ReadLine()
            => ReadLine(string.Empty);

        /// <summary>
        /// Reads a line of input from the standard input with a prompt, displaying the prompt in the specified foreground colour.
        /// </summary>
        /// <param name="prompt">The prompt to display before reading the line.</param>
        /// <returns>The input string entered by the user.</returns>
        public static string ReadLine(string prompt)
            => ReadLine(prompt, NuciConsoleColour.Default);

        /// <summary>
        /// Reads a line of input from the standard input with a prompt, displaying the prompt in the specified foreground colour and background colour.
        /// </summary>
        /// <param name="prompt">The prompt to display before reading the line.</param>
        /// <param name="foregroundColour">The text colour for the prompt.</param>
        /// <returns>The input string entered by the user.</returns>
        public static string ReadLine(string prompt, NuciConsoleColour foregroundColour)
            => ReadLine(prompt, foregroundColour, NuciConsoleColour.Default);

        /// <summary>
        /// Reads a line of input from the standard input with a prompt, displaying the prompt in the specified foreground and background colours.
        /// </summary>
        /// <param name="prompt">The prompt to display before reading the line.</param>
        /// <param name="foregroundColour">The text colour for the prompt.</param>
        /// <param name="backgroundColour">The background colour for the prompt.</param>
        /// <returns>The input string entered by the user.</returns>
        /// <exception cref="InputCancellationException">Thrown when the user cancels input by pressing Escape.</exception>
        public static string ReadLine(
            string prompt,
            NuciConsoleColour foregroundColour,
            NuciConsoleColour backgroundColour)
        {
            Write(prompt, foregroundColour, backgroundColour);

            return ReadInputLine();
        }

        /// <summary>
        /// Reads a permission from the user with a prompt, allowing them to confirm or deny the action.
        /// </summary>
        /// <param name="prompt">The prompt to display before asking for permission.</param>
        /// <returns>True if the user confirms the action, false if they deny it.</returns>
        public static bool ReadPermission(string prompt)
            => ReadPermission(prompt, false);

        /// <summary>
        /// Reads a permission from the user with a prompt, allowing them to confirm or deny the action, with a default value.
        /// </summary>
        /// <param name="prompt">The prompt to display before asking for permission.</param>
        /// <param name="defaultValue">The default value to return if the user presses Enter without making a choice.</param>
        /// <returns>True if the user confirms the action, false if they deny it.</returns>
        public static bool ReadPermission(string prompt, bool defaultValue)
            => ReadPermission(prompt, defaultValue, NuciConsoleColour.Default);

        /// <summary>
        /// Reads a permission from the user with a prompt, allowing them to confirm or deny the action, with a specified foreground colour.
        /// </summary>
        /// <param name="prompt">The prompt to display before asking for permission.</param>
        /// <param name="foregroundColour">The text colour for the prompt.</param>
        /// <returns>True if the user confirms the action, false if they deny it.</returns>
        public static bool ReadPermission(string prompt, NuciConsoleColour foregroundColour)
            => ReadPermission(prompt, false, foregroundColour);

        /// <summary>
        /// Reads a permission from the user with a prompt, allowing them to confirm or deny the action, with a specified foreground colour and default value.
        /// </summary>
        /// <param name="prompt">The prompt to display before asking for permission.</param>
        /// <param name="defaultValue">The default value to return if the user presses Enter without making a choice.</param>
        /// <param name="foregroundColour">The text colour for the prompt.</param>
        /// <returns>True if the user confirms the action, false if they deny it.</returns>
        public static bool ReadPermission(
            string prompt,
            bool defaultValue,
            NuciConsoleColour foregroundColour)
            => ReadPermission(
                prompt,
                defaultValue,
                foregroundColour,
                NuciConsoleColour.Default);

        /// <summary>
        /// Reads a permission from the user with a prompt, allowing them to confirm or deny the action, with specified foreground and background colours.
        /// </summary>
        /// <param name="prompt">The prompt to display before asking for permission.</param>
        /// <param name="foregroundColour">The text colour for the prompt.</param>
        /// <param name="backgroundColour">The background colour for the prompt.</param>
        /// <returns>True if the user confirms the action, false if they deny it.</returns>
        public static bool ReadPermission(
            string prompt,
            NuciConsoleColour foregroundColour,
            NuciConsoleColour backgroundColour)
            => ReadPermission(prompt, false, foregroundColour, backgroundColour);

        /// <summary>
        /// Reads a permission from the user with a prompt, allowing them to confirm or deny the action, with a specified foreground colour, background colour, and default value.
        /// </summary>
        /// <param name="prompt">The prompt to display before asking for permission.</param>
        /// <param name="defaultValue">The default value to return if the user presses Enter without making a choice.</param>
        /// <param name="foregroundColour">The text colour for the prompt.</param>
        /// <param name="backgroundColour">The background colour for the prompt.</param>
        /// <returns>True if the user confirms the action, false if they deny it.</returns>
        public static bool ReadPermission(
            string prompt,
            bool defaultValue,
            NuciConsoleColour foregroundColour,
            NuciConsoleColour backgroundColour)
        {
            string options = NoDefaultOptions;

            if (defaultValue)
            {
                options = YesDefaultOptions;
            }

            while (true)
            {
                ConsoleKeyInfo inputValue = ReadKey(
                    $"{prompt} ({options}) ",
                    foregroundColour,
                    backgroundColour);

                switch (inputValue.Key)
                {
                    case ConsoleKey.Y:
                        WriteLine();
                        return true;

                    case ConsoleKey.N:
                        WriteLine();
                        return false;

                    case ConsoleKey.Enter:
                        WriteLine();
                        return defaultValue;

                    default:
                        CursorX -= 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Writes multiple lines to the standard output.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="lines"/> is null.</exception>
        public static void WriteLines(IEnumerable<string> lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            foreach (string line in lines)
            {
                WriteLine(line);
            }
        }

        /// <summary>
        /// Writes multiple coloured lines to the standard output.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="foregroundColour">The text colour for all lines.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="lines"/> is null.</exception>
        public static void WriteLines(
            IEnumerable<string> lines,
            NuciConsoleColour foregroundColour)
        {
            ArgumentNullException.ThrowIfNull(lines);

            foreach (string line in lines)
            {
                WriteLine(line, foregroundColour);
            }
        }

        /// <summary>
        /// Writes multiple coloured lines to the standard output.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="foregroundColour">The text colour for all lines.</param>
        /// <param name="backgroundColour">The background colour for all lines.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="lines"/> is null.</exception>
        public static void WriteLines(
            IEnumerable<string> lines,
            NuciConsoleColour foregroundColour,
            NuciConsoleColour backgroundColour)
        {
            ArgumentNullException.ThrowIfNull(lines);

            foreach (string line in lines)
            {
                WriteLine(line, foregroundColour, backgroundColour);
            }
        }

        /// <summary>
        /// Writes an empty line to the standard output.
        /// </summary>
        public static void WriteLine()
            => WriteLine(string.Empty);

        /// <summary>
        /// Writes a line to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        public static void WriteLine(string text)
            => Console.WriteLine(text);

        /// <summary>
        /// Writes a coloured line to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">The text colour.</param>
        public static void WriteLine(string text, NuciConsoleColour foregroundColour)
            => WriteLine(text, foregroundColour, NuciConsoleColour.Default);

        /// <summary>
        /// Writes a coloured line to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">The text colour.</param>
        /// <param name="backgroundColour">The background colour.</param>
        public static void WriteLine(
            string text,
            NuciConsoleColour foregroundColour,
            NuciConsoleColour backgroundColour)
            => Write(text + Environment.NewLine, foregroundColour, backgroundColour);

        /// <summary>
        /// Writes the text to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        public static void Write(string text)
            => Console.Write(text);

        /// <summary>
        /// Writes the text to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">The text colour.</param>
        public static void Write(string text, NuciConsoleColour foregroundColour)
            => Write(text, foregroundColour, NuciConsoleColour.Default);

        /// <summary>
        /// Writes the text to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">The text colour.</param>
        /// <param name="backgroundColour">The background colour.</param>
        public static void Write(
            string text,
            NuciConsoleColour foregroundColour,
            NuciConsoleColour backgroundColour)
        {
            ConsoleColor previousForegroundColour = Console.ForegroundColor;
            ConsoleColor previousBackgroundColour = Console.BackgroundColor;

            try
            {
                ApplyColours(foregroundColour, backgroundColour);
                Console.Write(text);
            }
            finally
            {
                Console.ForegroundColor = previousForegroundColour;
                Console.BackgroundColor = previousBackgroundColour;
            }
        }

        private static void ApplyColours(
            NuciConsoleColour foregroundColour,
            NuciConsoleColour backgroundColour)
        {
            Console.ResetColor();

            if (foregroundColour is not null &&
                foregroundColour.ConsoleColour is ConsoleColor foreground)
            {
                Console.ForegroundColor = foreground;
            }

            if (backgroundColour is not null &&
                backgroundColour.ConsoleColour is ConsoleColor background)
            {
                Console.BackgroundColor = background;
            }        }

        private static string ReadInputLine()
        {
            StringBuilder inputValue = new();

            while (true)
            {
                ConsoleKeyInfo key = PerformKeyRead(true);

                if (key.Key is ConsoleKey.Escape)
                {
                    CancelInput(inputValue.ToString());
                }

                if (key.Key is ConsoleKey.Enter)
                {
                    WriteLine();

                    return inputValue.ToString();
                }

                ProcessInputKey(inputValue, key);
            }
        }

        [DoesNotReturn]
        private static void CancelInput(string inputValue)
        {
            CursorX -= inputValue.Length;
            WriteLine(string.Empty.PadRight(inputValue.Length));

            throw new InputCancellationException();
        }

        private static void ProcessInputKey(StringBuilder inputValue, ConsoleKeyInfo key)
        {
            if (key.Key is ConsoleKey.Backspace && inputValue.Length > 0)
            {
                CursorX -= 1;
                Write(" ");
                CursorX -= 1;

                inputValue.Length -= 1;

                return;
            }

            if (IsInputCharacter(key.KeyChar))
            {
                Console.Write(key.KeyChar);

                inputValue.Append(key.KeyChar);
            }
        }

        private static bool IsInputCharacter(char character)
            => char.IsLetterOrDigit(character) ||
                char.IsSymbol(character) ||
                char.IsPunctuation(character) ||
                char.IsWhiteSpace(character);

        private static ConsoleKeyInfo PerformKeyRead()
            => PerformKeyRead(false);

        private static ConsoleKeyInfo PerformKeyRead(bool intercept)
        {
            bool wasControlCTreatedAsInput = Console.TreatControlCAsInput;
            Console.TreatControlCAsInput = true;

            try
            {
                return Console.ReadKey(intercept);
            }
            finally
            {
                Console.TreatControlCAsInput = wasControlCTreatedAsInput;
            }
        }
    }
}
