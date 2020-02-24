using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WebApi.Utilities
{
    public static class AuthorizationExtensions
    {
        /// <summary>
        /// Get the HttpContext's current user ID.
        /// </summary>
        /// <param name="context">The HttpContext to get user information from.</param>
        /// <returns>The context's current user ID or null if there is no authenticated user.</returns>
        public static string? GetCurrentUserId(this HttpContext context) =>
            context.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
