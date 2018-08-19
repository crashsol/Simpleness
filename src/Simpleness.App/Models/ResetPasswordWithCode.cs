using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simpleness.App.Models
{
    /// <summary>
    /// 重置密码Code
    /// </summary>
    public class ResetPasswordWithCode
    {

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 重置密码CODE
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string Password { get; set; }
    }
}
