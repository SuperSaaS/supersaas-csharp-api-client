using System;

namespace SuperSaaS.API
{
    /// <summary>
    /// Thrown when an API request fails. For HTTP errors the message is the response body returned by SuperSaaS;
    /// for other failures (network, timeout) it is the message of the underlying exception, which is available as
    /// <see cref="Exception.InnerException"/>.
    /// </summary>
    public class SSSException : Exception
    {
        /// <summary>Creates an exception without a message.</summary>
        public SSSException()
        {
        }

        /// <summary>Creates an exception with a message.</summary>
        /// <param name="message">The error message.</param>
        public SSSException(string message)
            : base(message)
        {
        }

        /// <summary>Creates an exception with a message and the exception that caused it.</summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The exception that caused this one.</param>
        public SSSException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
