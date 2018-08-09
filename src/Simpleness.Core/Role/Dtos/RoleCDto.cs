using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Simpleness.Core.Role.Dtos
{
    /// <summary>
    /// 创建角色Dto
    /// </summary>
   public class RoleCDto
    {
        [MaxLength(80,ErrorMessage ="名称长度不能大于80")]
        [Required(ErrorMessage ="必须输入角色名")]
        public string Name { get; set; }

        [MaxLength(255,ErrorMessage ="角色描述长度不能大于255")]
        public string Description { get; set; }
        
    }
}
