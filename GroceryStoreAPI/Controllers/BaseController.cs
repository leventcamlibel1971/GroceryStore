using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using GroceryStoreAPI.Operation.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace GroceryStoreAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ObjectResult GenerateModelStateError(ModelStateDictionary modelState, ILogger logger,
            string actionName)
        {
            var detail = string.Join(',',
                modelState.Values
                    .SelectMany(x => x.Errors.Select(y => y.ErrorMessage)));

            var problem = Problem(
                statusCode: (int) HttpStatusCode.BadRequest,
                detail: detail,
                title: "Model Validation is failed");

            logger.LogError("Error happened in {actionName} action. Details: {details}",
                actionName, JsonSerializer.Serialize(problem));

            return problem;
        }

        protected ObjectResult GenerateInternalServerError(ILogger logger, Exception ex, string actionName)
        {
            var problem = Problem(statusCode: (int) HttpStatusCode.InternalServerError);

            logger.LogError(ex, "Error happened in {actionName} action. Details: {details}",
                actionName, JsonSerializer.Serialize(problem));
            return problem;
        }

        protected ObjectResult GenerateBadRequestError(ILogger logger, BadRequestException badRequestException,
            string actionName, string title)
        {
            var problem = Problem(
                statusCode: (int) HttpStatusCode.BadRequest,
                detail: badRequestException.Message,
                title: title);

            logger.LogError(badRequestException, "Error happened in {actionName} action. Details: {details}",
                actionName, JsonSerializer.Serialize(problem));

            return problem;
        }
    }
}