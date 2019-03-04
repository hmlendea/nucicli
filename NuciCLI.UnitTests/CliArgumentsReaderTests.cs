using System;

using NUnit.Framework;

using NuciCLI;

namespace NuciCLI.UnitTests
{
    public class CliArgumentsReaderTests
    {
        [Test]
        public void HasOption_CalledWithOneVariant_OptionExists_ReturnsTrue()
        {
            string[] args = { "--test", "--bla", "--bla2" };

            Assert.IsTrue(CliArgumentsReader.HasOption(args, "--test"));
        }
        
        [Test]
        public void HasOption_CalledWithMultipleVariants_OptionExists_ReturnsTrue()
        {
            string[] args = { "--test", "--bla", "--bla2" };

            Assert.IsTrue(CliArgumentsReader.HasOption(args, "-t", "--test"));
        }

        [Test]
        public void HasOption_OptionDoesNotExist_ReturnsFalse()
        {
            string[] args = { "--test", "--bla", "--bla2" };

            Assert.IsFalse(CliArgumentsReader.HasOption(args, "--hori"));
        }

        [Test]
        public void GetOptionValue_OptionExistsWithValueAsDifferentArgument_ReturnsTheValue()
        {
            string option = "--option";
            string value = "value";
            string[] args = { "--test", option, value, "--bla2" };

            Assert.AreEqual(value, CliArgumentsReader.GetOptionValue(args, option));
        }

        [Test]
        public void GetOptionValue_OptionExistsWithValueAsSameArgument_ReturnsTheValue()
        {
            string option = "--option";
            string value = "value";
            string[] args = { "--test", $"{option}={value}", "--bla2" };

            Assert.AreEqual(value, CliArgumentsReader.GetOptionValue(args, option));
        }

        [Test]
        public void GetOptionValue_OptionExistsWithoutValue_ReturnsTheValue()
        {
            string option = "--option";
            string[] args = { "--test", option, "--bla2" };

            Assert.Throws<ArgumentNullException>(() => CliArgumentsReader.GetOptionValue(args, option));
        }

        [Test]
        public void GetOptionValue_OptionDoesNotExist_ReturnsTheValue()
        {
            string[] args = { "--test", "--bla", "--bla2" };

            Assert.Throws<ArgumentException>(() => CliArgumentsReader.GetOptionValue(args, "--hori"));
        }
    }
}
