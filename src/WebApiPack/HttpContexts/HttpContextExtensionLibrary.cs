namespace Microsoft.AspNetCore.Http {
    public static class HttpContextExtensionLibrary {
        public static string GetScheme(this HttpContext httpContext) {
            return $"{httpContext.Request.Scheme}://";
        }

        public static string GetHostString(this HttpContext httpContext) {
            return httpContext.Request.Host.Value;
        }

        public static string GetPathPlusQueryStrings(this HttpContext httpContext) {
            return $"{httpContext.Request.Path.Value}{httpContext.Request.QueryString.Value}";
        }

        public static string GetUrlString(this HttpContext httpContext) {
            return $"{httpContext.GetScheme()}{httpContext.GetHostString()}{httpContext.GetPathPlusQueryStrings()}";
        }

        public static string GetHttpMethod(this HttpContext httpContext) {
            return httpContext.Request.Method;
        }
    }
}
