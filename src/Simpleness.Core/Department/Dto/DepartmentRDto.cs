using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Core.Department.Dto
{
    /// <summary>
    /// 部门列表显示
    /// </summary>
   public class DepartmentRDto
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string FullPath { get; set; }

        /// <summary>
        /// 部门排序
        /// </summary>
        public float Order { get; set; }

        /// <summary>
        /// 上级部门名称
        /// </summary>
        public Guid Pid { get; set; }

    }
}
