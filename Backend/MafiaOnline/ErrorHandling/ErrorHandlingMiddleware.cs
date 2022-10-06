using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MafiaOnline.ErrorHandling.Exceptions;

namespace MafiaOnline.ErrorHandling
{
    public class ErrorDetails
    {
        public ErrorDetails(HttpStatusCode statusCode, params string[] messages)
        {
            StatusCode = statusCode;
            Messages = messages.ToList();
        }

        public List<string> Messages { get; } = new List<string>();
        public HttpStatusCode StatusCode { get; }
    }

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                await WriteExceptionAsync(context, ex.ToErrorDetails());
            }
            catch (Exception ex)
            {
                await WriteExceptionAsync(context, ex.ToErrorDetails());
            }
        }

        private Task WriteExceptionAsync(HttpContext context, ErrorDetails errorDetails)
        {
            string errorDetailsJson = JsonConvert.SerializeObject(errorDetails, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            context.Response.StatusCode = (int)errorDetails.StatusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(errorDetailsJson);
        }
    }
}
