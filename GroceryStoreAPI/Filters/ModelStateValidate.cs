using System.Linq;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GroceryStoreAPI.Filters
{
    public class ModelStateValidate : ActionFilterAttribute
    {
        private readonly IHostEnvironment _hostingEnv;
        private readonly ILogger<ModelStateValidate> _logger;

        public ModelStateValidate(IHostEnvironment hostingEnv, ILogger<ModelStateValidate> logger)
        {
            _hostingEnv = hostingEnv;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;

            var errors = context.ModelState
                .Where(a => a.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors).ToList();

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;

            var actionDescriptor = ((ControllerBase) context.Controller).ControllerContext.ActionDescriptor;
            var controllerName = actionDescriptor.ControllerName;
            var actionName = actionDescriptor.ActionName;

            var stackTrace = $"Error happened in {controllerName} controller while executing {actionName} action";

            //stop information leak.
            if (!_hostingEnv.IsDevelopment())
                stackTrace = "";

            var result = new
            {
                error = errors,
                stackTrace
            };
            context.Result = new JsonResult(result);

            _logger.LogError(JsonSerializer.Serialize(result));
        }
    }
}