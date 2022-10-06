using System;
using System.Net;

namespace MafiaOnline.ErrorHandling.Exceptions
{
    public class UnauthorizedException : HttpStatusCodeException
    {
        public UnauthorizedException(string message, Exception innerException = null) 
            : base(HttpStatusCode.Unauthorized, message, innerException)
        {

        }
    }
}
