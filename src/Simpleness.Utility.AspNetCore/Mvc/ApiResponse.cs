using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simpleness.Infrastructure.AspNetCore.Mvc
{
    /// <summary>
    /// 统一数据返回格式
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 响应状态码，与HttpStatusCode一致
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 消息提示
        /// </summary>
        public string Message { get; set; } = "操作成功";

        /// <summary>
        /// 数据内容
        /// </summary>
        public object Result { get; set; }

        public static ApiResponse Ok(string message)
        {

            return new ApiResponse
            {
                Message = message ?? "操作成功"
            };
        }


        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static ApiResponse Ok(object result)
        {
            return new ApiResponse { Result = result };
        }


        public static ApiResponse Ok(string message, object result)
        {
            return new ApiResponse { Message = message, Result = result };
        }

        /// <summary>
        /// 400错误类型
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static ApiResponse Error(string message = "")
        {
            return new ApiResponse
            {
                Code = 400,
                Message = message ?? "操作失败"
            };

        }


        /// <summary>
        /// 服务器内部错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ApiResponse InterError(string message)
        {
            return new ApiResponse
            {
                Code = 500,
                Message = message ?? "系统错误！"
            };
        }



    }
}
