using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MafiaOnline.ErrorHandling.Exceptions;
using Castle.Core.Logging;
using MafiaOnline.BusinessLogic.Services;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                await WriteExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await WriteExceptionAsync(context, ex);
            }
        }

        private Task WriteExceptionAsync(HttpContext context, Exception ex)
        {
            string errorDetailsJson = JsonConvert.SerializeObject(ex.ToErrorDetails(), new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            _logger.LogError(ex.ToString());

            context.Response.StatusCode = (int)ex.ToErrorDetails().StatusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(errorDetailsJson);
        }
    }
}
