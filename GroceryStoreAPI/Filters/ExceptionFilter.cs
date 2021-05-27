using System;
using System.Net;
using GroceryStoreAPI.Operation.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace GroceryStoreAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostEnvironment _hostingEnv;

        public ExceptionFilter(IHostEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
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

            context.Result = new JsonResult(new
            {
                error = new[] {context.Exception.Message},
                stackTrace = stackTrace
            });
        }
    }
}