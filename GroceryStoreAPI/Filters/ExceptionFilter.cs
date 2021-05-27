using System;
using System.Net;
using System.Text.Json;
using GroceryStoreAPI.Operation.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GroceryStoreAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostEnvironment _hostingEnv;
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(IHostEnvironment hostingEnv, ILogger<ExceptionFilter> logger)
        {
            _hostingEnv = hostingEnv;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var statusCode = context.Exception switch
            {
                EntityNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int) statusCode;

            var stackTrace = context.Exception.StackTrace;

            //stop information leak.
            if (!_hostingEnv.IsDevelopment())
                stackTrace = "";

            var result = new
            {
                error = new[] {context.Exception.Message},
                stackTrace
            };
            context.Result = new JsonResult(result);

            _logger.LogError(JsonSerializer.Serialize(result));
        }
    }
}