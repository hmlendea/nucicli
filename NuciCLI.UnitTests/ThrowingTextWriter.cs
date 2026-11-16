using System;
using System.IO;
using System.Text;

namespace NuciCLI.UnitTests
{
    internal sealed class ThrowingTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
            => throw new IOException("Simulated console failure.");

        public override void Write(string? value)
            => throw new IOException("Simulated console failure.");
    }
}