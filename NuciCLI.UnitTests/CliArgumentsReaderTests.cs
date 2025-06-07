using System;

using NUnit.Framework;

namespace NuciCLI.UnitTests
{
    public class CliArgumentsReaderTests
    {
        [Test]
        public void HasOption_CalledWithOneVariant_OptionExists_ReturnsTrue()
            => Assert.That(CliArgumentsReader.HasOption(["--test", "--bla", "--bla2"], "--test"));

        [Test]
        public void HasOption_CalledWithMultipleVariants_OptionExists_ReturnsTrue()
            => Assert.That(CliArgumentsReader.HasOption(["--test", "--bla", "--bla2"], "-t", "--test"));

        [Test]
        public void HasOption_OptionDoesNotExist_ReturnsFalse()
            => Assert.That(!CliArgumentsReader.HasOption(["--test", "--bla", "--bla2"], "--hori"));

        [Test]
        public void GetOptionValue_OptionExistsWithValueAsDifferentArgument_ReturnsTheValue()
        {
            string option = "--option";
            string value = "value";
            string[] args = ["--test", option, value, "--bla2"];

            Assert.That(CliArgumentsReader.GetOptionValue(args, option), Is.EqualTo(value));
        }

        [Test]
        public void GetOptionValue_OptionExistsWithValueAsSameArgument_ReturnsTheValue()
        {
            string option = "--option";
            string value = "value";
            string[] args = ["--test", $"{option}={value}", "--bla2"];

            Assert.That(CliArgumentsReader.GetOptionValue(args, option), Is.EqualTo(value));
        }

        [Test]
        public void GetOptionValue_OptionExistsWithoutValue_ThrowsArgumentNullException()
        {
            string option = "--option";
            string[] args = ["--test", option, "--bla2"];

            Assert.Throws<ArgumentNullException>(() => CliArgumentsReader.GetOptionValue(args, option));
        }

        [Test]
        public void GetOptionValue_OptionDoesNotExist_ThrowsArgumentException()
            => Assert.Throws<ArgumentException>(() => CliArgumentsReader.GetOptionValue(["--test", "--bla", "--bla2"], "--hori"));

        [Test]
        public void TryGetOptionValue_OptionExistsWithValueAsDifferentArgument_ReturnsTheValue()
        {
            string option = "--option";
            string value = "value";
            string[] args = ["--test", option, value, "--bla2"];

            Assert.That(CliArgumentsReader.TryGetOptionValue(args, option), Is.EqualTo(value));
        }

        [Test]
        public void TryGetOptionValue_OptionExistsWithValueAsSameArgument_ReturnsTheValue()
        {
            string option = "--option";
            string value = "value";
            string[] args = ["--test", $"{option}={value}", "--bla2"];

            Assert.That(CliArgumentsReader.TryGetOptionValue(args, option), Is.EqualTo(value));
        }

        [Test]
        public void TryGetOptionValue_OptionExistsWithoutValue_ThrowsArgumentNullException()
        {
            string option = "--option";
            string[] args = ["--test", option, "--bla2"];

            Assert.That(CliArgumentsReader.TryGetOptionValue(args, option), Is.Null);
        }

        [Test]
        public void TryGetOptionValue_OptionDoesNotExist_ThrowsArgumentException()
            => Assert.That(CliArgumentsReader.TryGetOptionValue(["--test", "--bla", "--bla2"], "--hori"), Is.Null);
    }
}
