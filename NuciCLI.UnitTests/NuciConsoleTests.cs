using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace NuciCLI.UnitTests
{
    [TestFixture]
    public sealed class NuciConsoleTests
    {
        private TextWriter originalOut = null!;
        private ConsoleColor originalForegroundColour;
        private ConsoleColor originalBackgroundColour;
        private bool originalTreatControlCAsInput;

        [SetUp]
        public void SetUp()
        {
            originalOut = Console.Out;
            originalForegroundColour = Console.ForegroundColor;
            originalBackgroundColour = Console.BackgroundColor;
            originalTreatControlCAsInput = Console.TreatControlCAsInput;
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(originalOut);
            RestoreColours();
            Console.TreatControlCAsInput = originalTreatControlCAsInput;
        }

        [Test]
        public static void GivenCursorXBounded_WhenAssigned_ThenTheAssignmentSucceeds()
        {
            NuciConsole.CursorX = NuciConsole.Width - 1;

            Assert.That(NuciConsole.CursorX, Is.GreaterThanOrEqualTo(0));
        }

        [TestCase(-1)]
        [TestCase(int.MaxValue)]
        public static void GivenCursorXOutOfBounds_WhenAssigned_ThenArgumentOutOfRangeExceptionIsThrown(int value)
            => Assert.That(
                () => NuciConsole.CursorX = value,
                Throws.TypeOf<ArgumentOutOfRangeException>());

        [Test]
        public static void GivenCursorYBounded_WhenAssigned_ThenTheAssignmentSucceeds()
        {
            NuciConsole.CursorX = 0;
            NuciConsole.CursorY = NuciConsole.Height - 1;

            Assert.That(NuciConsole.CursorY, Is.GreaterThanOrEqualTo(0));
        }

        [TestCase(-1)]
        [TestCase(int.MaxValue)]
        public static void GivenCursorYOutOfBounds_WhenAssigned_ThenArgumentOutOfRangeExceptionIsThrown(int value)
            => Assert.That(
                () => NuciConsole.CursorY = value,
                Throws.TypeOf<ArgumentOutOfRangeException>());

        [Test]
        public static void GivenCursorSizeRequested_WhenRead_ThenItMatchesTheConsole()
            => Assert.That(
                NuciConsole.CursorSize,
                Is.EqualTo(Console.CursorSize));

        [Test]
        public static void GivenBufferDimensionsRequested_WhenRead_ThenTheyMatchTheConsole()
        {
            Assert.That(NuciConsole.Width, Is.EqualTo(Console.BufferWidth));
            Assert.That(NuciConsole.Height, Is.EqualTo(Console.BufferHeight));
        }

        [Test]
        public static void GivenBufferDimensions_WhenAreaIsRead_ThenItIsTheProductOfWidthAndHeight()
            => Assert.That(
                NuciConsole.Area,
                Is.EqualTo(Console.BufferWidth * Console.BufferHeight));

        [Test]
        public static void GivenNoPrompt_WhenReadingAKey_ThenTheFailureIsPropagatedWithoutOutput()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            Assert.That(
                () => NuciConsole.ReadKey(),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.Empty);
        }

        [Test]
        public static void GivenAPrompt_WhenReadingAKey_ThenThePromptIsWrittenBeforeTheFailure()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            Assert.That(
                () => NuciConsole.ReadKey("Press any key: "),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Press any key: "));
        }

        [Test]
        public static void GivenAColouredPrompt_WhenReadingAKey_ThenThePromptUsesTheRequestedForegroundColour()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            ConsoleColor defaultBackgroundColour = ResetColours();

            Assert.That(
                () => NuciConsole.ReadKey("Blue: ", NuciConsoleColour.Blue),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Blue: "));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Blue));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(defaultBackgroundColour));
        }

        [Test]
        public static void GivenAFullyColouredPrompt_WhenReadingAKey_ThenThePromptUsesBothRequestedColours()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            Assert.That(
                () => NuciConsole.ReadKey("Cyan on red: ", NuciConsoleColour.Cyan, NuciConsoleColour.Red),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Cyan on red: "));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Cyan));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(ConsoleColor.Red));
        }

        [Test]
        public static void GivenAKeyReadFailure_WhenItOccurs_ThenControlCAsInputIsRestored()
        {
            Console.TreatControlCAsInput = false;

            Assert.That(
                () => NuciConsole.ReadKey("Failing: "),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(Console.TreatControlCAsInput, Is.False);
        }

        [Test]
        public static void GivenNoPrompt_WhenReadingALine_ThenTheFailureIsPropagatedWithoutOutput()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            Assert.That(
                () => NuciConsole.ReadLine(),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.Empty);
        }

        [Test]
        public static void GivenAPrompt_WhenReadingALine_ThenThePromptIsWrittenBeforeTheFailure()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            Assert.That(
                () => NuciConsole.ReadLine("Username: "),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Username: "));
        }

        [Test]
        public static void GivenAColouredPrompt_WhenReadingALine_ThenThePromptUsesTheRequestedForegroundColour()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            Assert.That(
                () => NuciConsole.ReadLine("Green: ", NuciConsoleColour.Green),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Green: "));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Green));
        }

        [Test]
        public static void GivenAFullyColouredPrompt_WhenReadingALine_ThenThePromptUsesBothRequestedColours()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            Assert.That(
                () => NuciConsole.ReadLine("Yellow on blue: ", NuciConsoleColour.Yellow, NuciConsoleColour.Blue),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Yellow on blue: "));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Yellow));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(ConsoleColor.Blue));
        }

        [Test]
        public static void GivenAPrompt_WhenReadingAPermission_ThenTheDenyDefaultOptionsAreShown()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            Assert.That(
                () => NuciConsole.ReadPermission("Proceed?"),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Proceed? (y/N) "));
        }

        [Test]
        public static void GivenATrueDefault_WhenReadingAPermission_ThenTheAllowDefaultOptionsAreShown()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            Assert.That(
                () => NuciConsole.ReadPermission("Proceed?", true),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Proceed? (Y/n) "));
        }

        [Test]
        public static void GivenAColouredPrompt_WhenReadingAPermission_ThenThePromptUsesTheRequestedForegroundColour()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            Assert.That(
                () => NuciConsole.ReadPermission("Proceed?", NuciConsoleColour.Magenta),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Proceed? (y/N) "));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Magenta));
        }

        [Test]
        public static void GivenATrueDefaultAndAColour_WhenReadingAPermission_ThenBothAreApplied()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            Assert.That(
                () => NuciConsole.ReadPermission("Proceed?", true, NuciConsoleColour.DarkCyan),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Proceed? (Y/n) "));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.DarkCyan));
        }

        [Test]
        public static void GivenForegroundAndBackgroundColours_WhenReadingAPermission_ThenBothAreApplied()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            Assert.That(
                () => NuciConsole.ReadPermission("Proceed?", NuciConsoleColour.White, NuciConsoleColour.DarkGreen),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Proceed? (y/N) "));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.White));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(ConsoleColor.DarkGreen));
        }

        [Test]
        public static void GivenADefaultAndBothColours_WhenReadingAPermission_ThenAllAreApplied()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            Assert.That(
                () => NuciConsole.ReadPermission("Proceed?", true, NuciConsoleColour.DarkYellow, NuciConsoleColour.DarkMagenta),
                Throws.TypeOf<InvalidOperationException>());
            Assert.That(writer.Content, Is.EqualTo("Proceed? (Y/n) "));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.DarkYellow));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(ConsoleColor.DarkMagenta));
        }

        [Test]
        public static void GivenMultipleLines_WhenWritingThem_ThenEachLineIsWrittenOnItsOwnLine()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            NuciConsole.WriteLines(BuildLines());

            Assert.That(
                writer.Content,
                Is.EqualTo(string.Concat("Tux the Penguin", Environment.NewLine, "Solaire of Astora", Environment.NewLine)));
        }

        [Test]
        public static void GivenMultipleLines_WhenWritingThemWithAForegroundColour_ThenEveryLineUsesThatColour()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            ConsoleColor defaultBackgroundColour = ResetColours();

            NuciConsole.WriteLines(BuildLines(), NuciConsoleColour.Cyan);

            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Cyan));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(defaultBackgroundColour));
        }

        [Test]
        public static void GivenMultipleLines_WhenWritingThemWithBothColours_ThenEveryLineUsesBothColours()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            NuciConsole.WriteLines(BuildLines(), NuciConsoleColour.Yellow, NuciConsoleColour.DarkBlue);

            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Yellow));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(ConsoleColor.DarkBlue));
        }

        [Test]
        public static void GivenNoLines_WhenWritingThem_ThenNothingIsWritten()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            NuciConsole.WriteLines([]);
            NuciConsole.WriteLines([], NuciConsoleColour.Red);
            NuciConsole.WriteLines([], NuciConsoleColour.Red, NuciConsoleColour.Black);

            Assert.That(writer.Content, Is.Empty);
        }

        [Test]
        public static void GivenNullLines_WhenWritingThem_ThenArgumentNullExceptionIsThrown()
            => Assert.That(
                () => NuciConsole.WriteLines(null!),
                Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("lines"));

        [Test]
        public static void GivenNullLinesAndAForegroundColour_WhenWritingThem_ThenArgumentNullExceptionIsThrown()
            => Assert.That(
                () => NuciConsole.WriteLines(null!, NuciConsoleColour.Red),
                Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("lines"));

        [Test]
        public static void GivenNullLinesAndBothColours_WhenWritingThem_ThenArgumentNullExceptionIsThrown()
            => Assert.That(
                () => NuciConsole.WriteLines(null!, NuciConsoleColour.Red, NuciConsoleColour.Black),
                Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("lines"));

        [Test]
        public static void GivenNoText_WhenWritingALine_ThenOnlyTheNewLineIsWritten()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            NuciConsole.WriteLine();

            Assert.That(writer.Content, Is.EqualTo(Environment.NewLine));
        }

        [Test]
        public static void GivenText_WhenWritingALine_ThenTheTextAndNewLineAreWritten()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            NuciConsole.WriteLine("John Wick");

            Assert.That(writer.Content, Is.EqualTo(string.Concat("John Wick", Environment.NewLine)));
        }

        [Test]
        public static void GivenTextAndAForegroundColour_WhenWritingALine_ThenTheColourIsApplied()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            ConsoleColor defaultBackgroundColour = ResetColours();

            NuciConsole.WriteLine("Grumpy Cat", NuciConsoleColour.Red);

            Assert.That(writer.Content, Is.EqualTo(string.Concat("Grumpy Cat", Environment.NewLine)));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Red));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(defaultBackgroundColour));
        }

        [Test]
        public static void GivenTextAndBothColours_WhenWritingALine_ThenBothColoursAreApplied()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            NuciConsole.WriteLine("Yes Man", NuciConsoleColour.Green, NuciConsoleColour.Black);

            Assert.That(writer.Content, Is.EqualTo(string.Concat("Yes Man", Environment.NewLine)));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Green));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(ConsoleColor.Black));
        }

        [Test]
        public static void GivenText_WhenWriting_ThenTheTextIsWrittenWithoutColourChanges()
        {
            RecordingTextWriter writer = UseRecordingWriter();

            NuciConsole.Write("Monty Python");

            Assert.That(writer.Content, Is.EqualTo("Monty Python"));
        }

        [Test]
        public static void GivenTextAndAForegroundColour_WhenWriting_ThenTheColourIsApplied()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            ConsoleColor defaultBackgroundColour = ResetColours();

            NuciConsole.Write("John Rambo", NuciConsoleColour.DarkGray);

            Assert.That(writer.Content, Is.EqualTo("John Rambo"));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.DarkGray));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(defaultBackgroundColour));
        }

        [Test]
        public static void GivenTextAndBothColours_WhenWriting_ThenBothColoursAreApplied()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ResetColor();

            NuciConsole.Write("Robbie Rotten", NuciConsoleColour.Blue, NuciConsoleColour.DarkYellow);

            Assert.That(writer.Content, Is.EqualTo("Robbie Rotten"));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(ConsoleColor.Blue));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(ConsoleColor.DarkYellow));
        }

        [Test]
        public static void GivenDefaultColours_WhenWriting_ThenTheConsoleDefaultsAreUsed()
        {
            RecordingTextWriter writer = UseRecordingWriter();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            ConsoleColor expectedForegroundColour = ResetColours();
            ConsoleColor expectedBackgroundColour = Console.BackgroundColor;

            NuciConsole.Write("Gică Contra", NuciConsoleColour.Default, NuciConsoleColour.Default);

            Assert.That(writer.Content, Is.EqualTo("Gică Contra"));
            Assert.That(writer.LastForegroundColour, Is.EqualTo(expectedForegroundColour));
            Assert.That(writer.LastBackgroundColour, Is.EqualTo(expectedBackgroundColour));
        }

        [Test]
        public static void GivenAColouredWrite_WhenTheWriteSucceeds_ThenThePreviousColoursAreRestored()
        {
            UseRecordingWriter();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.Gray;

            NuciConsole.Write("Ilarion", NuciConsoleColour.Cyan, NuciConsoleColour.DarkBlue);

            Assert.That(Console.ForegroundColor, Is.EqualTo(ConsoleColor.DarkRed));
            Assert.That(Console.BackgroundColor, Is.EqualTo(ConsoleColor.Gray));
        }

        [Test]
        public static void GivenAColouredWrite_WhenTheWriteFails_ThenThePreviousColoursAreStillRestored()
        {
            Console.SetOut(new ThrowingTextWriter());
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;

            Assert.That(
                () => NuciConsole.Write("Gunnar Gunnarsson", NuciConsoleColour.White, NuciConsoleColour.Black),
                Throws.TypeOf<IOException>());
            Assert.That(Console.ForegroundColor, Is.EqualTo(ConsoleColor.DarkGreen));
            Assert.That(Console.BackgroundColor, Is.EqualTo(ConsoleColor.DarkMagenta));
        }

        [Test]
        public static void GivenAColouredLineWrite_WhenTheWriteFails_ThenThePreviousColoursAreStillRestored()
        {
            Console.SetOut(new ThrowingTextWriter());
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.DarkYellow;

            Assert.That(
                () => NuciConsole.WriteLine("Willy Wonka", NuciConsoleColour.Red),
                Throws.TypeOf<IOException>());
            Assert.That(Console.ForegroundColor, Is.EqualTo(ConsoleColor.DarkBlue));
            Assert.That(Console.BackgroundColor, Is.EqualTo(ConsoleColor.DarkYellow));
        }

        private static ConsoleColor ResetColours()
        {
            Console.ResetColor();

            return Console.ForegroundColor;
        }

        private static List<string> BuildLines()
            => new() { "Tux the Penguin", "Solaire of Astora" };

        private static RecordingTextWriter UseRecordingWriter()
        {
            RecordingTextWriter writer = new();
            Console.SetOut(writer);

            return writer;
        }

        private void RestoreColours()
        {
            Console.ResetColor();

            if (IsRealColour(originalForegroundColour))
            {
                Console.ForegroundColor = originalForegroundColour;
            }

            if (IsRealColour(originalBackgroundColour))
            {
                Console.BackgroundColor = originalBackgroundColour;
            }
        }

        private static bool IsRealColour(ConsoleColor colour)
            => Enum.IsDefined(colour);
    }
}