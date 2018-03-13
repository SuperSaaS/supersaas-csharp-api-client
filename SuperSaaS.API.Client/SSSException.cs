using System;
namespace SuperSaaS.API
{
    public class SSSException : Exception
    {
        public SSSException()
        {
        }

        public SSSException(string message)
        : base(message)
        {
        }

        public SSSException(string message, Exception inner)
        : base(message, inner)
        {
        }

    }
}
