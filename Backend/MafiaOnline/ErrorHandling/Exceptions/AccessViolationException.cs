using System;
using System.Net;

namespace MafiaOnline.ErrorHandling.Exceptions
{
    public class AccessViolationException : HttpStatusCodeException
    {
        public AccessViolationException(string message, Exception innerException = null) 
            : base(HttpStatusCode.Unauthorized, message, innerException)
        {

        }
    }
}
