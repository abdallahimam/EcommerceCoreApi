using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Ecommerce.Errors;

namespace Ecommerce.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlerMiddleware> logger, IHostEnvironment environment)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context) {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _environment.IsDevelopment() 
                    ? new ApiExceptionError((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiExceptionError((int)HttpStatusCode.InternalServerError);

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

                await context.Response.WriteAsync(json);
            }
        }
    }
}