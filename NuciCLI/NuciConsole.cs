using System;

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
            set
            {
                Console.SetCursorPosition(value, CursorY);
            }
        }

        public static int CursorY
        {
            get => Console.CursorTop;
            set
            {
                Console.SetCursorPosition(CursorX, value);
            }
        }

        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
        
        public static string ReadLine() => NuciConsole.ReadLine(string.Empty);
        public static string ReadLine(string prompt) => ReadLine(prompt, Colour.Default);
        public static string ReadLine(string prompt, Colour foregroundColour) => ReadLine(prompt, foregroundColour, Colour.Default);
        public static string ReadLine(string prompt, Colour foregroundColour, Colour backgroundColour)
        {
            NuciConsole.Write(prompt);

            string inputValue = Console.ReadLine();

            return inputValue;
        }

        /// <summary>
        /// Writes an empty line to the standard output.
        /// </summary>
        public static void WriteLine() => WriteLine(string.Empty);
        
        /// <summary>
        /// Writes the line to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        public static void WriteLine(string text) => Console.WriteLine(text);
        
        /// <summary>
        /// Writes the line to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">Foreground colour.</param>
        public static void WriteLine(string text, Colour foregroundColour) => WriteLine(text, foregroundColour, Colour.Default);

        /// <summary>
        /// Writes the line to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">Foreground colour.</param>
        /// <param name="backgroundColour">Background colour.</param>
        public static void WriteLine(string text, Colour foregroundColour, Colour backgroundColour) => Write(text + Environment.NewLine, foregroundColour, backgroundColour);
        
        /// <summary>
        /// Writes the text to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        public static void Write(string text) => Console.Write(text);
        
        /// <summary>
        /// Writes the text to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">Foreground colour.</param>
        public static void Write(string text, Colour foregroundColour) => Write(text, foregroundColour, Colour.Default);
        
        /// <summary>
        /// Writes the text to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">Foreground colour.</param>
        /// <param name="backgroundColour">Background colour.</param>
        public static void Write(string text, Colour foregroundColour, Colour backgroundColour)
        {
            ConsoleColor oldForegroundColour = Console.ForegroundColor;
            ConsoleColor oldBackgroundColour = Console.BackgroundColor;

            Console.ResetColor();

            if (!(foregroundColour.ConsoleColour is null))
            {
                Console.ForegroundColor = (ConsoleColor)foregroundColour.ConsoleColour;
            }

            if (!(backgroundColour.ConsoleColour is null))
            {
                Console.BackgroundColor = (ConsoleColor)backgroundColour.ConsoleColour;
            }

            Console.Write(text, foregroundColour, backgroundColour);
            
            Console.ForegroundColor = oldForegroundColour;
            Console.BackgroundColor = oldBackgroundColour;
        }
    }
}
