using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Extensions
{
    /// <summary>
    /// Request扩展方法
    /// </summary>
   public static class HttpRequestExtend
    {
        //判断当前请求是不是Ajax请求
        public static bool IsAjax(this HttpRequest req)
        {
            bool result = false;
            var xreq = req.Headers.ContainsKey("x-requested-with");
            if(xreq)
            {
               result =  req.Headers["x-requested-with"] == "XMLHttpRequest";
            }
            return result;
        }
    }
}
