using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.Common.Backend.Auth
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException()
        {
        }

        public NotAuthenticatedException(string message) : base(message)
        {
        }

        public NotAuthenticatedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
