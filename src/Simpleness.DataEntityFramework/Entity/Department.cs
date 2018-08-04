using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.DataEntityFramework.Entity
{
    /// <summary>
    /// 部门信息表
    /// </summary>
    public class Department:BaseEntity<Guid>
    {     

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 部门排序
        /// </summary>
        public float Order { get; set; }

        /// <summary>
        /// 上级部门名称
        /// </summary>
        public Guid Pid { get; set; }

        /// <summary>
        /// 部门全路径 根_人力资源部_XXXX
        /// </summary>
        public string FullPath { get; set; } 

        /// <summary>
        /// 部门成员（多对多关系映射)
        /// </summary>
        public List<UserDepartments> UserDepartments { get; set; }

    }

   
}
