using System;

namespace NuciCLI
{
    public sealed class NuciConsoleColour
    {
        public int Value { get; set; }

        public ConsoleColor? ConsoleColour { get; set; }

        public string Name { get; set; }

        public static NuciConsoleColour Default => new(-1, nameof(Default));
        public static NuciConsoleColour Black => new(0, nameof(Black));
        public static NuciConsoleColour DarkBlue => new(1, nameof(DarkBlue));
        public static NuciConsoleColour DarkGreen => new(2, nameof(DarkGreen));
        public static NuciConsoleColour DarkCyan => new(3, nameof(DarkCyan));
        public static NuciConsoleColour DarkRed => new(4, nameof(DarkRed));
        public static NuciConsoleColour DarkMagenta => new(5, nameof(DarkMagenta));
        public static NuciConsoleColour DarkYellow => new(6, nameof(DarkYellow));
        public static NuciConsoleColour Gray => new(7, nameof(Gray));
        public static NuciConsoleColour DarkGray => new(8, nameof(DarkGray));
        public static NuciConsoleColour Blue => new(9, nameof(Blue));
        public static NuciConsoleColour Green => new(10, nameof(Green));
        public static NuciConsoleColour Cyan => new(11, nameof(Cyan));
        public static NuciConsoleColour Red => new(12, nameof(Red));
        public static NuciConsoleColour Magenta => new(13, nameof(Magenta));
        public static NuciConsoleColour Yellow => new(14, nameof(Yellow));
        public static NuciConsoleColour White => new(15, nameof(White));

        NuciConsoleColour(int value, string name)
        {
            Value = value;
            Name = name;

            if (Value.Equals(-1))
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
