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

        public static int Width
        {
            get => Console.BufferWidth;
            set
            {
                Console.SetBufferSize(value, Height);
            }
        }

        public static int Height
        {
            get => Console.BufferHeight;
            set
            {
                Console.SetBufferSize(Width, value);
            }
        }

        public static ConsoleKeyInfo ReadKey() => ReadKey(string.Empty);
        public static ConsoleKeyInfo ReadKey(string prompt) => ReadKey(prompt, Colour.Default);
        public static ConsoleKeyInfo ReadKey(string prompt, Colour foregroundColour) => ReadKey(prompt, foregroundColour, Colour.Default);
        public static ConsoleKeyInfo ReadKey(string prompt, Colour foregroundColour, Colour backgroundColour)
        {
            NuciConsole.Write(prompt);

            ConsoleKeyInfo inputValue = Console.ReadKey();

            return inputValue;
        }
        
        public static string ReadLine() => NuciConsole.ReadLine(string.Empty);
        public static string ReadLine(string prompt) => ReadLine(prompt, Colour.Default);
        public static string ReadLine(string prompt, Colour foregroundColour) => ReadLine(prompt, foregroundColour, Colour.Default);
        public static string ReadLine(string prompt, Colour foregroundColour, Colour backgroundColour)
        {
            NuciConsole.Write(prompt);

            string inputValue = Console.ReadLine();

            return inputValue;
        }

        public static bool ReadPermission(string prompt)
            => ReadPermission(prompt, false);

        public static bool ReadPermission(string prompt, bool defaultValue)
            => ReadPermission(prompt, defaultValue, Colour.Default);

        public static bool ReadPermission(string prompt, Colour foregroundColour)
            => ReadPermission(prompt, false, foregroundColour);

        public static bool ReadPermission(string prompt, bool defaultValue, Colour foregroundColour)
            => ReadPermission(prompt, defaultValue, foregroundColour, Colour.Default);

        public static bool ReadPermission(string prompt, Colour foregroundColour, Colour backgroundColour)
            => ReadPermission(prompt, false, foregroundColour, backgroundColour);

        public static bool ReadPermission(
            string prompt,
            bool defaultValue,
            Colour foregroundColour,
            Colour backgroundColour)
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
                        NuciConsole.WriteLine();
                        return true;

                    case ConsoleKey.N:
                        NuciConsole.WriteLine();
                        return false;
                    
                    case ConsoleKey.Enter:
                        NuciConsole.WriteLine();
                        return defaultValue;

                    default:
                        NuciConsole.CursorX -= 1;
                        break;
                }
            }
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
