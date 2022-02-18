using System;
using System.Security.Claims;

namespace ChatApp.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static String GetUserId(this ClaimsPrincipal @this)
        {
            return @this.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}