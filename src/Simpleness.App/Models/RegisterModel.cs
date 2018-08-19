using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simpleness.App.Models
{
    /// <summary>
    /// 注册模型
    /// </summary>
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "密码最短6位,最长100位", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密    码")]
        public string Password { get; set; }    
    }
}
