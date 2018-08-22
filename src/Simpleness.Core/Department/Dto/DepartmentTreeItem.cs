using Simpleness.Utility.CommonDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Core.Department.Dto
{

    /// <summary>
    /// 部门树形结构
    /// </summary>
   public class DepartmentTreeItem:TreeItem<Guid>
    {
        /// <summary>
        /// 排序
        /// </summary>
        public float Order { get; set; }

        public string Description { get; set; }

        public Guid Pid { get; set; }

        new public List<DepartmentTreeItem> Children { get; set; } = new List<DepartmentTreeItem>();
    }
}
