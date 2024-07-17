using Microsoft.AspNetCore.Http;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static string GetStringForHeader(this HttpRequest request, string headerName, string defaultValue = "")
        {
            var value = defaultValue;
            Microsoft.Extensions.Primitives.StringValues values;
            if (request.Headers.TryGetValue(headerName, out values))
            {
                value = values.FirstOrDefault();
            }

            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        public static bool GetBooleanForHeader(this HttpRequest request, string headerName, bool defaultValue = false)
        {
            Microsoft.Extensions.Primitives.StringValues values;
            if (request.Headers.TryGetValue(headerName, out values))
            {
                var value = values.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(value))
                {
                    return defaultValue;
                }

                switch (value.Trim().ToLowerInvariant())
                {
                    case "1":
                    case "true":
                    case "yes":
                        return true;
                    default:
                        return false;
                }
            }

            return defaultValue;
        }
    }
}
