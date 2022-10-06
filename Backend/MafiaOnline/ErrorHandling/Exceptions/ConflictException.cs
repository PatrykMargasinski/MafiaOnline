using System;
using System.Net;

namespace MafiaOnline.ErrorHandling.Exceptions
{
    public class ConflictException : HttpStatusCodeException
    {
        public ConflictException(string message, Exception innerException = null) 
            : base(HttpStatusCode.Conflict, message, innerException)
        {
        }
    }
}
