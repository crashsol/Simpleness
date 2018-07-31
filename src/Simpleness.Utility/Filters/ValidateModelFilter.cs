﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Simpleness.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simpleness.Utility.Filters
{
    /// <summary>
    /// 模型验证过滤器
    /// </summary>
    public class ValidateModelFilter: ActionFilterAttribute
    {
        private readonly ILogger _logger;
        public ValidateModelFilter(ILogger<ValidateModelFilter> logger)
        {
            _logger = logger;
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {

                if (context.HttpContext.Request.IsAjax())
                {
                    //如果是ajax请求，返回json数据
                    var errors = context.ModelState.Values.SelectMany(v => v.Errors);
                    context.Result = new JsonResult(errors.Select(a => a.ErrorMessage).Aggregate((i, next) => $"{i},{next}"));
                }
            }

        }
    }
}
