namespace MediationAPIGateWay.미들웨어
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static Dictionary<string, DateTime> _requestTimes = new Dictionary<string, DateTime>();
        private static int _requestLimit = 5; // 허용하는 요청 횟수
        private static TimeSpan _timeLimit = TimeSpan.FromSeconds(10); // 시간 제한

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();
            var now = DateTime.UtcNow;

            if (_requestTimes.ContainsKey(remoteIp) && (now - _requestTimes[remoteIp]).TotalSeconds < _timeLimit.TotalSeconds)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                return;
            }

            // 허용된 시간이 지나면 다시 요청을 허용
            _requestTimes[remoteIp] = now;
            await _next(context);
        }
    }

}
