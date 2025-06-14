using System;
using System.Collections.Generic;

namespace NuciCLI
{
    /// <summary>
    /// Console extras.
    /// </summary>
    public static class NuciConsole
    {
        public static int CursorX
        {
            get => Console.CursorLeft;
            set => Console.SetCursorPosition(value, CursorY);
        }

        public static int CursorY
        {
            get => Console.CursorTop;
            set => Console.SetCursorPosition(CursorX, value);
        }

        public static int CursorSize => Console.CursorSize;

        public static int Width => Console.BufferWidth;

        public static int Height => Console.BufferHeight;

        public static int Area => Width * Height;

        public static ConsoleKeyInfo ReadKey()
            => ReadKey(string.Empty);

        public static ConsoleKeyInfo ReadKey(string prompt)
            => ReadKey(prompt, NuciConsoleColour.Default);

        public static ConsoleKeyInfo ReadKey(string prompt, NuciConsoleColour foregroundColour)
            => ReadKey(prompt, foregroundColour, NuciConsoleColour.Default);

        public static ConsoleKeyInfo ReadKey(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
        {
            Write(prompt);

            return PerformKeyRead();
        }

        public static string ReadLine()
            => ReadLine(string.Empty);

        public static string ReadLine(string prompt)
            => ReadLine(prompt, NuciConsoleColour.Default);

        public static string ReadLine(string prompt, NuciConsoleColour foregroundColour)
            => ReadLine(prompt, foregroundColour, NuciConsoleColour.Default);

        public static string ReadLine(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
        {
            Write(prompt, foregroundColour, backgroundColour);

            string inputValue = string.Empty;

            ConsoleKeyInfo key = PerformKeyRead(true);

            while (true)
            {
                if (key.Key.Equals(ConsoleKey.Escape))
                {
                    CursorX -= inputValue.Length;
                    WriteLine(string.Empty.PadRight(inputValue.Length));

                    throw new InputCancellationException();
                }

                if (key.Key.Equals(ConsoleKey.Enter))
                {
                    WriteLine();
                    break;
                }
                else if (key.Key.Equals(ConsoleKey.Backspace) && !string.IsNullOrEmpty(inputValue))
                {
                    inputValue = inputValue[..^1];
                    CursorX -= 1;
                    Write(" ");
                    CursorX -= 1;
                }
                else if (
                    char.IsLetterOrDigit(key.KeyChar) ||
                    char.IsSymbol(key.KeyChar) ||
                    char.IsPunctuation(key.KeyChar) ||
                    char.IsWhiteSpace(key.KeyChar))
                {
                    inputValue += key.KeyChar;
                    Console.Write(key.KeyChar);
                }

                key = PerformKeyRead(true);
            }

            return inputValue;
        }

        public static bool ReadPermission(string prompt)
            => ReadPermission(prompt, false);

        public static bool ReadPermission(string prompt, bool defaultValue)
            => ReadPermission(prompt, defaultValue, NuciConsoleColour.Default);

        public static bool ReadPermission(string prompt, NuciConsoleColour foregroundColour)
            => ReadPermission(prompt, false, foregroundColour);

        public static bool ReadPermission(string prompt, bool defaultValue, NuciConsoleColour foregroundColour)
            => ReadPermission(prompt, defaultValue, foregroundColour, NuciConsoleColour.Default);

        public static bool ReadPermission(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
            => ReadPermission(prompt, false, foregroundColour, backgroundColour);

        public static bool ReadPermission(
            string prompt,
            bool defaultValue,
            NuciConsoleColour foregroundColour,
            NuciConsoleColour backgroundColour)
        {
            while (true)
            {
                string yesNo = "y/N";

                if (defaultValue)
                {
                    yesNo = "Y/n";
                }

                ConsoleKeyInfo inputValue = ReadKey($"{prompt} ({yesNo}) ");

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
        public static void WriteLines(IEnumerable<string> lines)
        {
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
        public static void WriteLines(IEnumerable<string> lines, NuciConsoleColour foregroundColour)
        {
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
        public static void WriteLines(IEnumerable<string> lines, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
        {
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
        /// Writes a coloured line to the standard output.
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
        public static void WriteLine(string text, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
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
        public static void Write(string text, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
        {
            ConsoleColor oldForegroundColour = Console.ForegroundColor;
            ConsoleColor oldBackgroundColour = Console.BackgroundColor;

            Console.ResetColor();

            if (foregroundColour.ConsoleColour is not null)
            {
                Console.ForegroundColor = foregroundColour.ConsoleColour.Value;
            }

            if (backgroundColour.ConsoleColour is not null)
            {
                Console.BackgroundColor = backgroundColour.ConsoleColour.Value;
            }

            Console.Write(text, foregroundColour, backgroundColour);

            Console.ForegroundColor = oldForegroundColour;
            Console.BackgroundColor = oldBackgroundColour;
        }

        private static ConsoleKeyInfo PerformKeyRead()
            => PerformKeyRead(false);

        private static ConsoleKeyInfo PerformKeyRead(bool intercept)
        {
            bool previousCtrlCBehaviour = Console.TreatControlCAsInput;
            Console.TreatControlCAsInput = true;

            ConsoleKeyInfo inputValue = Console.ReadKey(intercept);

            Console.TreatControlCAsInput = previousCtrlCBehaviour;

            return inputValue;
        }
    }
}
