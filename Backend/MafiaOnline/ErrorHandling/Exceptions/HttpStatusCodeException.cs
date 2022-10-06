using System;
using System.Net;

namespace MafiaOnline.ErrorHandling.Exceptions
{
    public class HttpStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpStatusCodeException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, string message, Exception innerException = null) 
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public virtual ErrorDetails ToErrorDetails()
        {
            return new ErrorDetails(StatusCode, Message);
        }
    }
}
