namespace MediationAPIGateWay.Middleware
{
    public class XssProtectionMiddleware
    {
        private readonly RequestDelegate _next;

        public XssProtectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // XSS 및 XXE 방지
            if (context.Request.Method == "POST" && context.Request.ContentType != null && context.Request.ContentType.Contains("application/x-www-form-urlencoded"))
            {
                // 저장형 XSS 방지
                var body = await ReadRequestBody(context);

                // 악성 스크립트 태그 탐지
                if (ContainsXss(body))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden: XSS detected.");
                    return;
                }

                // Request body를 다시 읽을 수 있도록 스트림을 초기화
                var memoryStream = new MemoryStream();
                var writer = new StreamWriter(memoryStream);
                await writer.WriteAsync(body);
                await writer.FlushAsync();
                memoryStream.Position = 0;
                context.Request.Body = memoryStream;
            }

            await _next(context);
        }

        private async Task<string> ReadRequestBody(HttpContext context)
        {
            using (var reader = new StreamReader(context.Request.Body))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private bool ContainsXss(string body)
        {
            return body.Contains("<script>", StringComparison.OrdinalIgnoreCase) ||
                   body.Contains("</script>", StringComparison.OrdinalIgnoreCase) ||
                   body.Contains("javascript:", StringComparison.OrdinalIgnoreCase);
        }
    }
}
