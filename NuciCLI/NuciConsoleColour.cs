using System;

namespace NuciCLI
{
    public sealed class NuciConsoleColour
    {
        public int Value { get; set; }

        public ConsoleColor? ConsoleColour { get; set; }

        public string Name { get; set; }

        public static NuciConsoleColour Default => new NuciConsoleColour(-1, nameof(Default));
        public static NuciConsoleColour Black => new NuciConsoleColour(0, nameof(Black));
        public static NuciConsoleColour DarkBlue => new NuciConsoleColour(1, nameof(DarkBlue));
        public static NuciConsoleColour DarkGreen => new NuciConsoleColour(2, nameof(DarkGreen));
        public static NuciConsoleColour DarkCyan => new NuciConsoleColour(3, nameof(DarkCyan));
        public static NuciConsoleColour DarkRed => new NuciConsoleColour(4, nameof(DarkRed));
        public static NuciConsoleColour DarkMagenta => new NuciConsoleColour(5, nameof(DarkMagenta));
        public static NuciConsoleColour DarkYellow => new NuciConsoleColour(6, nameof(DarkYellow));
        public static NuciConsoleColour Gray => new NuciConsoleColour(7, nameof(Gray));
        public static NuciConsoleColour DarkGray => new NuciConsoleColour(8, nameof(DarkGray));
        public static NuciConsoleColour Blue => new NuciConsoleColour(9, nameof(Blue));
        public static NuciConsoleColour Green => new NuciConsoleColour(10, nameof(Green));
        public static NuciConsoleColour Cyan => new NuciConsoleColour(11, nameof(Cyan));
        public static NuciConsoleColour Red => new NuciConsoleColour(12, nameof(Red));
        public static NuciConsoleColour Magenta => new NuciConsoleColour(13, nameof(Magenta));
        public static NuciConsoleColour Yellow => new NuciConsoleColour(14, nameof(Yellow));
        public static NuciConsoleColour White => new NuciConsoleColour(15, nameof(White));

        NuciConsoleColour(int value, string name)
        {
            Value = value;
            Name = name;

            if (Value == -1)
            {
                ConsoleColour = null;
            }
            else
            {
                ConsoleColour = (ConsoleColor)value;
            }
        }
    }
}
