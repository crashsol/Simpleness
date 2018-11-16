using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Captcha
{
    /// <summary>
    /// 生成验证码描述类
    /// </summary>
    public class CaptchaResult
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string CaptchaCode { get; set; }

        /// <summary>
        /// 图片Byte[]
        /// </summary>
        public byte[] CaptchaByteData { get; set; }

        /// <summary>
        /// image Base64
        /// </summary>
        public string CaptchaBase64Data => Convert.ToBase64String(CaptchaByteData);

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
