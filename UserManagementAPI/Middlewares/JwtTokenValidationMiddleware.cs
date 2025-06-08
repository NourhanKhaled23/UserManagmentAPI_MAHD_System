using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;

namespace UserManagementAPI.Middlewares
{
    public class JwtTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtTokenValidationMiddleware> _logger;

        public JwtTokenValidationMiddleware(RequestDelegate next, ILogger<JwtTokenValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip validation for /api/Auth endpoints
            if (context.Request.Path.StartsWithSegments("/api/Auth"))
            {
                await _next(context);
                return;
            }

            // Check if token is already validated by JwtBearer
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                _logger.LogWarning("No valid token provided or authentication failed.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("{\"message\": \"No valid token provided\"}");
                return;
            }

            // Add extra validation if needed (e.g., custom claims check)
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    // Get the secret key from environment variable
                    var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "X7kP9mQ2vL5jR8yT3wZ6nB4xC1uF8hJ9kLmP3qW4rT6yU8iO9pX2vC5mN7bV1j";
                    var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "http://localhost:5003";
                    var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "http://localhost:5003";

                    if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 32)
                    {
                        throw new InvalidOperationException("JWT SecretKey is not set or is too short in environment variables.");
                    }

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(secretKey);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out _);

                    _logger.LogInformation("Token validated successfully for request: {Path}", context.Request.Path);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Token validation failed for request: {Path}", context.Request.Path);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("{\"message\": \"Invalid or expired token\"}");
                    return;
                }
            }

            // Proceed to the next middleware or controller
            await _next(context);
        }
    }

    // Extension method to register the middleware
    public static class JwtTokenValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtTokenValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtTokenValidationMiddleware>();
        }
    }
}