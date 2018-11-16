using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Captcha
{
    public class CaptchaOption
    {
        /// <summary>
        /// 验证码图片的宽度
        /// </summary>
        public int Width { get; set; } = 100;

        /// <summary>
        /// 验证码图片的高度
        /// </summary>
        public int Height { get; set; } = 36;

        /// <summary>
        /// 验证码长度
        /// </summary>
        public int WordLenth { get; set; } = 4;
    }
}
