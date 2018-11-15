using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Simpleness.Infrastructure.AspNetCore.Mvc;
using Simpleness.Infrastructure.AspNetCore.UserException;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Filters
{
    /// <summary>
    ///全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter: IExceptionFilter
    {
        private readonly ILogger _logger;

        private readonly IHostingEnvironment _env;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IHostingEnvironment env)
        {
            _env = env;
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                context.Result = new BadRequestObjectResult(ApiResponse.Error(context.Exception.Message));
            }
            else
            {
                var Message = "发生了未知的内部错误";
                if (_env.IsDevelopment())
                {
                    //非生产环境就返回堆栈错误信息
                    Message = context.Exception.StackTrace;
                }
                context.Result = new BadRequestObjectResult(ApiResponse.Error(Message));
            }
            //记录错误信息
            _logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
}
