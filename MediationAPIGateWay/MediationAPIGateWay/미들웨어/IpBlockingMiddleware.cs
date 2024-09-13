namespace MediationAPIGateWay.미들웨어
{
    public class IpBlockingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HashSet<string> _blockedIps;

        public IpBlockingMiddleware(RequestDelegate next)
        {
            _next = next;
            // 차단할 IP 목록
            _blockedIps = new HashSet<string> { "192.168.0.1", "10.0.0.1" };
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

            if (_blockedIps.Contains(remoteIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Forbidden: Your IP is blocked.");
                return;
            }

            await _next(context);
        }
    }

}
