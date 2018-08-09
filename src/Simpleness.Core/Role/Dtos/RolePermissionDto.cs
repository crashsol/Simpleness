using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Simpleness.Core.Role.Dtos
{
    /// <summary>
    /// 更新角色权限Dto
    /// </summary>
   public class RolePermissionDto
    {
        [Required(ErrorMessage ="必须填写要更新的角色")]
        public Guid Id { get; set; }

        public IEnumerable<string> Permissions { get; set; } = new List<string>();
    }
}
