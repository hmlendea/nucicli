using System;
using System.IO;
using System.Text;

namespace NuciCLI.UnitTests
{
    internal sealed class RecordingTextWriter : TextWriter
    {
        private readonly StringBuilder content = new();

        public override Encoding Encoding => Encoding.UTF8;

        public string Content => content.ToString();

        public ConsoleColor? LastForegroundColour { get; private set; }

        public ConsoleColor? LastBackgroundColour { get; private set; }

        public override void Write(char value)
            => Write(value.ToString());

        public override void Write(string? value)
        {
            LastForegroundColour = Console.ForegroundColor;
            LastBackgroundColour = Console.BackgroundColor;

            content.Append(value);
        }
    }
}