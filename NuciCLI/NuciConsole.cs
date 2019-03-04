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

        public static int CursorSize
        {
            get => Console.CursorSize;
            set => Console.CursorSize = value;
        }

        public static bool IsCursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        public static int Width
        {
            get => Console.BufferWidth;
            set => SetSize(value, Console.BufferHeight);
        }

        public static int Height
        {
            get => Console.BufferHeight;
            set => SetSize(Console.BufferWidth, value);
        }

        public static int Area => Width * Height;

        public static string Title
        {
            get => Console.Title;
            set => Console.Title = value;
        }

        public static ConsoleKeyInfo ReadKey()
            => ReadKey(string.Empty);

        public static ConsoleKeyInfo ReadKey(string prompt)
            => ReadKey(prompt, NuciConsoleColour.Default);

        public static ConsoleKeyInfo ReadKey(string prompt, NuciConsoleColour foregroundColour)
            => ReadKey(prompt, foregroundColour, NuciConsoleColour.Default);

        public static ConsoleKeyInfo ReadKey(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
        {
            NuciConsole.Write(prompt);

            ConsoleKeyInfo inputValue = Console.ReadKey();

            return inputValue;
        }
        
        public static string ReadLine()
            => NuciConsole.ReadLine(string.Empty);

        public static string ReadLine(string prompt)
            => ReadLine(prompt, NuciConsoleColour.Default);

        public static string ReadLine(string prompt, NuciConsoleColour foregroundColour)
            => ReadLine(prompt, foregroundColour, NuciConsoleColour.Default);

        public static string ReadLine(string prompt, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
        {
            NuciConsole.Write(prompt);

            string inputValue = Console.ReadLine();

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
        public static void WriteLine()
            => WriteLine(string.Empty);
        
        /// <summary>
        /// Writes the line to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        public static void WriteLine(string text)
            => Console.WriteLine(text);
        
        /// <summary>
        /// Writes the line to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">Foreground colour.</param>
        public static void WriteLine(string text, NuciConsoleColour foregroundColour)
            => WriteLine(text, foregroundColour, NuciConsoleColour.Default);

        /// <summary>
        /// Writes the line to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">Foreground colour.</param>
        /// <param name="backgroundColour">Background colour.</param>
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
        /// <param name="foregroundColour">Foreground colour.</param>
        public static void Write(string text, NuciConsoleColour foregroundColour)
            => Write(text, foregroundColour, NuciConsoleColour.Default);
        
        /// <summary>
        /// Writes the text to the standard output.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="foregroundColour">Foreground colour.</param>
        /// <param name="backgroundColour">Background colour.</param>
        public static void Write(string text, NuciConsoleColour foregroundColour, NuciConsoleColour backgroundColour)
        {
            ConsoleColor oldForegroundColour = Console.ForegroundColor;
            ConsoleColor oldBackgroundColour = Console.BackgroundColor;

            Console.ResetColor();

            if (!(foregroundColour.ConsoleColour is null))
            {
                Console.ForegroundColor = foregroundColour.ConsoleColour.Value;
            }

            if (!(backgroundColour.ConsoleColour is null))
            {
                Console.BackgroundColor = backgroundColour.ConsoleColour.Value;
            }

            Console.Write(text, foregroundColour, backgroundColour);
            
            Console.ForegroundColor = oldForegroundColour;
            Console.BackgroundColor = oldBackgroundColour;
        }

        public static void SetSize(int width, int height)
        {
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width, height);
        }
    }
}
