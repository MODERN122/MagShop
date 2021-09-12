using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Exceptions
{
    public class InvalidOperationException : Exception
    {
        public InvalidOperationException(string message) : base(message) { }
    }
}
