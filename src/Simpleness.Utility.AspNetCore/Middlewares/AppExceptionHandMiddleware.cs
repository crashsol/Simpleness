using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Simpleness.Infrastructure.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simpleness.Infrastructure.AspNetCore.Middlewares
{
    /// <summary>
    /// 全局自定义异常处理
    /// </summary>
    public class AppExceptionHandMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<AppExceptionHandMiddleware> _logger;

        public AppExceptionHandMiddleware(RequestDelegate requestDelegate ,ILogger<AppExceptionHandMiddleware> logger)
        {
            _next = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message, ex.GetType());
                context.Response.Clear();
                context.Response.StatusCode = 500;
                var result =  ApiResponse.InterError(ex.Message);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(result));              

            }

        }
    }
}
