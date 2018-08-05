using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Simpleness.Core.User.Dtos
{
    /// <summary>
    /// 创建账号模型
    /// </summary>
   public class UserCDto
    {    


        public string NickName { get; set; }


        [Required(ErrorMessage ="必须输入邮箱地址")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage ="必须输入密码")]
        public string Password { get; set; }

        /// <summary>
        /// 角色选择
        /// </summary>
        public IList<string> Roles { get; set; }
    }
}
