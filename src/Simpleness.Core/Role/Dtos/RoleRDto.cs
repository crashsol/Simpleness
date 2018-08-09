using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Core.Role.Dtos
{
    /// <summary>
    /// 角色列表Dto
    /// </summary>
    public class RoleRDto 
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }


    }
}
