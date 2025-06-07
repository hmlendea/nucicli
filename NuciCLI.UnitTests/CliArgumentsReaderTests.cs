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
        public void HasOption_CalledWithEmptyArgs_ReturnsFalse()
            => Assert.That(!CliArgumentsReader.HasOption([], "--test"));

        [Test]
        public void HasOption_CalledWithMissingOption_ReturnsFalse()
            => Assert.That(!CliArgumentsReader.HasOption(["--test", "--bla", "--bla2"], "--hori"));

        [Test]
        public void HasOption_CalledWithMissingOptionButWithSimilarPrefix_ReturnsFalse()
            => Assert.That(!CliArgumentsReader.HasOption(["--test", "--testing"], "--tes"));

        [Test]
        public void HasOption_CalledWithNullOption_ThrowsException()
            => Assert.Throws<ArgumentNullException>(() => CliArgumentsReader.HasOption(["--test"], null));

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
        public void GetOptionValue_OptionAtEndWithoutValue_ThrowsArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => CliArgumentsReader.GetOptionValue(["--foo", "--option"], "--option"));

        [Test]
        public void GetOptionValue_TwoFormsOfSameOption_ReturnsFirstMatchValue()
            => Assert.That(CliArgumentsReader.GetOptionValue(["-o", "first", "--option", "second"], "-o", "--option"), Is.EqualTo("first"));

        [Test]
        public void GetOptionValue_OptionEqualsEmptyString_ReturnsEmptyString()
            => Assert.That(CliArgumentsReader.GetOptionValue(["--option="], "--option"), Is.EqualTo(""));

        [Test]
        public void GetOptionValue_NullArgs_ThrowsArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => CliArgumentsReader.GetOptionValue(null, "--option"));

        [Test]
        public void GetOptionValue_EmptyOptionString_ThrowsArgumentException()
            => Assert.Throws<ArgumentException>(() => CliArgumentsReader.GetOptionValue(["--test", "value"], ""));

        [Test]
        public void GetOptionValue_OnlyDashesAsOption_ReturnsNull()
            => Assert.That(CliArgumentsReader.GetOptionValue(["--", "something"], "--"), Is.EqualTo("something"));

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
        public void TryGetOptionValue_OptionExistsWithoutValue_ReturnsNull()
        {
            string option = "--option";
            string[] args = ["--test", option, "--bla2"];

            Assert.That(CliArgumentsReader.TryGetOptionValue(args, option), Is.Null);
        }

        [Test]
        public void TryGetOptionValue_EmptyOptionString_ThrowsArgumentException()
            => Assert.Throws<ArgumentException>(() => CliArgumentsReader.GetOptionValue(["--test", "value"], ""));

        [Test]
        public void TryGetOptionValue_OptionDoesNotExist_ReturnsNull()
            => Assert.That(CliArgumentsReader.TryGetOptionValue(["--test", "--bla", "--bla2"], "--hori"), Is.Null);

        [Test]
        public void TryGetOptionValue_OnlyDashesAsOption_ReturnsNull()
            => Assert.That(CliArgumentsReader.GetOptionValue(["--", "something"], "--"), Is.EqualTo("something"));

    }
}
