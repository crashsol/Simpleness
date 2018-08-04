using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.DataEntityFramework.Entity
{
    /// <summary>
    /// 人员部门关系映射
    /// </summary>
    public class UserDepartments
    {

        public AppUser AppUser { get; set; }

        public Guid AppUserId { get; set; }

        public Department Department { get; set; }

        public Guid DepartmentId { get; set; }
    }
}
