using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Core.Department.Dto
{
    /// <summary>
    /// 更新部门成员Dto
    /// </summary>
    public class DepartmentUsersDto
    {
        /// <summary>
        /// 待更新的部门ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户IDS
        /// </summary>
        public List<Guid> UserIds { get; set; }

    }
}
