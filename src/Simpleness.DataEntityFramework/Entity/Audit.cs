using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.DataEntityFramework.Entity
{
    public class Audit : BaseEntity<int>
    {
        /// <summary>
        /// 请求用户ID
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// 请求用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 调用方法名称
        /// </summary>
        public string ServiceName { get; set; }


        /// <summary>
        /// 调用方法的类型
        /// </summary>
        public string MethodType { get; set; }

        /// <summary>
        /// 调用参数
        /// </summary>
        public string Parameters { get; set; } 


        /// <summary>
        /// 调用结果
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExcutionTime { get; set; }


        /// <summary>
        /// 执行时间 毫秒
        /// </summary>
        public double Duration { get; set; }


        /// <summary>
        /// 客户端地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 客户电脑名称
        /// </summary>
        public string ComputerName { get; set; }


        /// <summary>
        /// 异常【如果方法抛出异常】）等信息。
        /// </summary>

        public string Exception { get; set; }

    }
}
