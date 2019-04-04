using System;

namespace NuciCLI
{
    /// <summary>
    /// Duplicate entity exception.
    /// </summary>
    public class InputCancellationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputCancellationException"/> exception.
        /// </summary>
        public InputCancellationException()
            : base()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputCancellationException"/> exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public InputCancellationException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputCancellationException"/> exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public InputCancellationException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
