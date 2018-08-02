using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Models
{
    /// <summary>
    /// JWT配置类
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// 接受者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }


        /// <summary>
        /// 秘钥
        /// </summary>
        public string SecretKey { get; set; }
    }
}
