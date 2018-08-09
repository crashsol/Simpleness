using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Simpleness.Core.Role.Dtos
{
    /// <summary>
    /// 更新角色成员dto
    /// </summary>
    public class RoleUsersDto
    {
        [Required(ErrorMessage ="必须设定要更新的角色ID")]
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户IDS
        /// </summary>
        public List<Guid> UserIds { get; set; } = new List<Guid>();
    }
}
