using System;

namespace NuciCLI
{
    public sealed class Colour
    {
        public int Value { get; set; }

        public ConsoleColor? ConsoleColour { get; set; }

        public string Name { get; set; }

        public static Colour Default => new Colour(-1, null, nameof(Default));
        public static Colour Black => new Colour(0, ConsoleColor.Black, nameof(Black));
        public static Colour DarkBlue => new Colour(1, ConsoleColor.DarkBlue, nameof(DarkBlue));
        public static Colour DarkGreen => new Colour(2, ConsoleColor.DarkGreen, nameof(DarkGreen));
        public static Colour DarkCyan => new Colour(3, ConsoleColor.DarkCyan, nameof(DarkCyan));
        public static Colour DarkRed => new Colour(4, ConsoleColor.DarkRed, nameof(DarkRed));
        public static Colour DarkMagenta => new Colour(5, ConsoleColor.DarkMagenta, nameof(DarkMagenta));
        public static Colour DarkYellow => new Colour(6, ConsoleColor.DarkYellow, nameof(DarkYellow));
        public static Colour Gray => new Colour(7, ConsoleColor.Gray, nameof(Gray));
        public static Colour DarkGray => new Colour(8, ConsoleColor.DarkGray, nameof(DarkGray));
        public static Colour Blue => new Colour(9, ConsoleColor.Blue, nameof(Blue));
        public static Colour Green => new Colour(10, ConsoleColor.Green, nameof(Green));
        public static Colour Cyan => new Colour(11, ConsoleColor.Cyan, nameof(Cyan));
        public static Colour Red => new Colour(12, ConsoleColor.Red, nameof(Red));
        public static Colour Magenta => new Colour(13, ConsoleColor.Magenta, nameof(Magenta));
        public static Colour Yellow => new Colour(14, ConsoleColor.Yellow, nameof(Yellow));
        public static Colour White => new Colour(15, ConsoleColor.White, nameof(White));

        Colour(int value, ConsoleColor? colour, string name)
        {
            Value = value;
            ConsoleColour = colour;
            Name = name;
        }
    }
}
