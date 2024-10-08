﻿using System.Security.Cryptography;
using System.Text;

namespace MediationAPIGateWay.미들웨어
{
    public class HMACVerificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _sharedSecretKey = "YourSharedSecretKey";

        public HMACVerificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string receivedHmac = context.Request.Headers["X-HMAC"];
            string requestBody;

            using (var reader = new StreamReader(context.Request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
            }

            string calculatedHmac = GenerateHmac(requestBody, _sharedSecretKey);

            if (calculatedHmac != receivedHmac)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("HMAC validation failed.");
                return;
            }

            await _next(context);
        }

        private string GenerateHmac(string data, string key)
        {
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hashmessage);
            }
        }
    }

    public static class VerificationMiddlewareExtensions
    {
        public static IApplicationBuilder UseHMACVerificationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HMACVerificationMiddleware>();
        }
    }
}
