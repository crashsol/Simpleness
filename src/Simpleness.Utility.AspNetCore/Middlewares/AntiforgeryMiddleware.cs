using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Simpleness.Infrastructure.AspNetCore.Middlewares
{
    /// <summary>
    /// CSRF中间件,生成Token
    /// </summary>
    public class AntiforgeryMiddlerware:IMiddleware
    {

        private readonly IAntiforgery _antiforgery;  

        public AntiforgeryMiddlerware(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;           
        }
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) || string.Equals(context.Request.Path.Value, "/index.html", StringComparison.OrdinalIgnoreCase))
            {
                // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
                var tokens = _antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                    new CookieOptions() { HttpOnly = false });
            }

            return next(context);
        }
    }
}
