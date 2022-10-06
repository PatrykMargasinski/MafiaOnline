using System;

namespace MafiaOnline.ErrorHandling
{
    internal static class ErrorHandlingExtensions
    {
        internal static ErrorDetails ToErrorDetails(this Exception ex)
        {
            return new ErrorDetails(System.Net.HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
