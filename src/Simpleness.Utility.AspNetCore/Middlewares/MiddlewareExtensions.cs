using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Middlewares
{

    /// <summary>
    /// 中间件扩展类
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// 启用CSRF 中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAntiforgery(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AntiforgeryMiddlerware>();
        }

        /// <summary>
        /// 使用自定义错误的中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppExceptionHandMiddleware>();
        }
    }
}
