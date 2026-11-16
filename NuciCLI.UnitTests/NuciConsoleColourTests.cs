using System;
using NUnit.Framework;

namespace NuciCLI.UnitTests
{
    [TestFixture]
    public sealed class NuciConsoleColourTests
    {
        [TestCase(nameof(NuciConsoleColour.Black), 0, ConsoleColor.Black)]
        [TestCase(nameof(NuciConsoleColour.DarkBlue), 1, ConsoleColor.DarkBlue)]
        [TestCase(nameof(NuciConsoleColour.DarkGreen), 2, ConsoleColor.DarkGreen)]
        [TestCase(nameof(NuciConsoleColour.DarkCyan), 3, ConsoleColor.DarkCyan)]
        [TestCase(nameof(NuciConsoleColour.DarkRed), 4, ConsoleColor.DarkRed)]
        [TestCase(nameof(NuciConsoleColour.DarkMagenta), 5, ConsoleColor.DarkMagenta)]
        [TestCase(nameof(NuciConsoleColour.DarkYellow), 6, ConsoleColor.DarkYellow)]
        [TestCase(nameof(NuciConsoleColour.Gray), 7, ConsoleColor.Gray)]
        [TestCase(nameof(NuciConsoleColour.DarkGray), 8, ConsoleColor.DarkGray)]
        [TestCase(nameof(NuciConsoleColour.Blue), 9, ConsoleColor.Blue)]
        [TestCase(nameof(NuciConsoleColour.Green), 10, ConsoleColor.Green)]
        [TestCase(nameof(NuciConsoleColour.Cyan), 11, ConsoleColor.Cyan)]
        [TestCase(nameof(NuciConsoleColour.Red), 12, ConsoleColor.Red)]
        [TestCase(nameof(NuciConsoleColour.Magenta), 13, ConsoleColor.Magenta)]
        [TestCase(nameof(NuciConsoleColour.Yellow), 14, ConsoleColor.Yellow)]
        [TestCase(nameof(NuciConsoleColour.White), 15, ConsoleColor.White)]
        public static void GivenColourValue_WhenCreated_ThenItMapsToConsoleColour(
            string name,
            int value,
            ConsoleColor consoleColour)
        {
            NuciConsoleColour colour = GetColour(name);

            Assert.That(colour.Value, Is.EqualTo(value));
            Assert.That(colour.Name, Is.EqualTo(name));
            Assert.That(colour.ConsoleColour, Is.EqualTo(consoleColour));
        }

        [Test]
        public static void GivenDefaultColour_WhenCreated_ThenItHasNoConsoleColour()
        {
            NuciConsoleColour colour = NuciConsoleColour.Default;

            Assert.That(colour.Value, Is.EqualTo(-1));
            Assert.That(colour.Name, Is.EqualTo(nameof(NuciConsoleColour.Default)));
            Assert.That(colour.ConsoleColour, Is.Null);
        }

        [Test]
        public static void GivenColourProperty_WhenChanged_ThenTheNewValueIsExposed()
        {
            NuciConsoleColour colour = NuciConsoleColour.Red;
            colour.Value = 42;
            colour.Name = "Testy McTestface";
            colour.ConsoleColour = ConsoleColor.Blue;

            Assert.That(colour.Value, Is.EqualTo(42));
            Assert.That(colour.Name, Is.EqualTo("Testy McTestface"));
            Assert.That(colour.ConsoleColour, Is.EqualTo(ConsoleColor.Blue));
        }

        private static NuciConsoleColour GetColour(string name)
            => name switch
            {
                nameof(NuciConsoleColour.Black) => NuciConsoleColour.Black,
                nameof(NuciConsoleColour.DarkBlue) => NuciConsoleColour.DarkBlue,
                nameof(NuciConsoleColour.DarkGreen) => NuciConsoleColour.DarkGreen,
                nameof(NuciConsoleColour.DarkCyan) => NuciConsoleColour.DarkCyan,
                nameof(NuciConsoleColour.DarkRed) => NuciConsoleColour.DarkRed,
                nameof(NuciConsoleColour.DarkMagenta) => NuciConsoleColour.DarkMagenta,
                nameof(NuciConsoleColour.DarkYellow) => NuciConsoleColour.DarkYellow,
                nameof(NuciConsoleColour.Gray) => NuciConsoleColour.Gray,
                nameof(NuciConsoleColour.DarkGray) => NuciConsoleColour.DarkGray,
                nameof(NuciConsoleColour.Blue) => NuciConsoleColour.Blue,
                nameof(NuciConsoleColour.Green) => NuciConsoleColour.Green,
                nameof(NuciConsoleColour.Cyan) => NuciConsoleColour.Cyan,
                nameof(NuciConsoleColour.Red) => NuciConsoleColour.Red,
                nameof(NuciConsoleColour.Magenta) => NuciConsoleColour.Magenta,
                nameof(NuciConsoleColour.Yellow) => NuciConsoleColour.Yellow,
                nameof(NuciConsoleColour.White) => NuciConsoleColour.White,
                _ => throw new ArgumentOutOfRangeException(nameof(name))
            };
    }
}
