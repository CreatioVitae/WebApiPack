using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApiPack.Controllers.ApiResults;

namespace WebApiPack.Controllers {
    public static class ControllerBaseExtensions {
        public static ObjectResult MethodNotAllowed(this ControllerBase controllerBase) =>
            controllerBase.StatusCode(StatusCodes.Status405MethodNotAllowed, new ErrorResult($"リソース[{controllerBase.HttpContext.GetPathPlusQueryStrings()}]では、メソッド[{controllerBase.HttpContext.GetHttpMethod()}]は許可されていません。"));

        public static ObjectResult PreconditionFailed(this ControllerBase controllerBase, object? errorResult) =>
            controllerBase.StatusCode(StatusCodes.Status412PreconditionFailed, errorResult);

        public static ActionResult CreateHttpGetResult(this ControllerBase controllerBase, object? apiResult) =>
            apiResult is IEnumerable<object> e
                ? e.Any()
                    ? (ObjectResult)controllerBase.Ok(apiResult)
                    : controllerBase.NotFound(new ErrorResult($"リソース[{controllerBase.HttpContext.GetPathPlusQueryStrings()}]が見つかりません"))
                : (apiResult != default)
                    ? (ObjectResult)controllerBase.Ok(apiResult)
                    : controllerBase.NotFound(new ErrorResult($"リソース[{controllerBase.HttpContext.GetPathPlusQueryStrings()}]が見つかりません"));
    }
}
