
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Authorize
{
    /// <summary>
    /// 自定义授权（基于Policy实现）
    /// </summary>
    public class PermissionAttribute: AuthorizeAttribute
    {

        /// <summary>
        /// 策略前缀
        /// </summary>
        private const string POLICY_PREFIX = "Permission";
      
        /// <summary>
        /// 授权标记
        /// </summary>
        /// <param name="name">权限验证字符串</param>
        /// <param name="description">权限描述（用于前台显示)</param>
        public PermissionAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }


        /// <summary>
        /// 权限前台显示名称
        /// </summary>
        public string Description { get; set; } 

        public string Name
        {
            get
            {
                return Policy.Substring(POLICY_PREFIX.Length);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
