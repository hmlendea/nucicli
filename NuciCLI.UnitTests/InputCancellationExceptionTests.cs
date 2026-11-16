using System;
using System.IO;
using NUnit.Framework;

namespace NuciCLI.UnitTests
{
    [TestFixture]
    public sealed class InputCancellationExceptionTests
    {
        [Test]
        public static void GivenNoArguments_WhenConstructed_ThenTheMessageIsEmptyAndThereIsNoInnerException()
        {
            InputCancellationException exception = new();

            Assert.That(exception.Message, Is.EqualTo($"Exception of type '{typeof(InputCancellationException)}' was thrown."));
            Assert.That(exception.InnerException, Is.Null);
        }

        [Test]
        public static void GivenAMessage_WhenConstructed_ThenTheMessageIsExposed()
        {
            InputCancellationException exception = new("Input cancelled by Testy McTestface.");

            Assert.That(exception.Message, Is.EqualTo("Input cancelled by Testy McTestface."));
            Assert.That(exception.InnerException, Is.Null);
        }

        [Test]
        public static void GivenAMessageAndAnInnerException_WhenConstructed_ThenBothAreExposed()
        {
            IOException innerException = new("Simulated failure.");

            InputCancellationException exception = new("Input cancelled.", innerException);

            Assert.That(exception.Message, Is.EqualTo("Input cancelled."));
            Assert.That(exception.InnerException, Is.SameAs(innerException));
        }

        [Test]
        public static void GivenAnInputCancellationException_WhenInspected_ThenItDerivesFromException()
            => Assert.That(
                new InputCancellationException(),
                Is.InstanceOf<Exception>());
    }
}