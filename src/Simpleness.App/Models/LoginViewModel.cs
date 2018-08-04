using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simpleness.App.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "必须输入用户名或者邮箱地址")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "必须输入密码")]
        public string Password { get; set; }
    }
}
