namespace MediationAPIGateWay.미들웨어
{
    public class SqlInjectionProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _sqlInjectionPatterns = { "SELECT", "INSERT", "DELETE", "DROP", "--", "OR", "AND" };

        public SqlInjectionProtectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var queryString = context.Request.QueryString.ToString();

            // SQL 인젝션 패턴 탐지
            foreach (var pattern in _sqlInjectionPatterns)
            {
                if (queryString.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Bad Request: Possible SQL Injection detected.");
                    return;
                }
            }

            await _next(context);
        }
    }

}
