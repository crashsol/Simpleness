using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Simpleness.Core.Department.Dto
{
    /// <summary>
    /// 创建部门Dto
    /// </summary>
    public class DepartmentCDto
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [Required(ErrorMessage ="必须输入部门名称")]
        [StringLength(40,ErrorMessage ="名称最大长度40")]
        public string Name { get; set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        [StringLength(255,ErrorMessage ="描述最大长度255")]
        public string Description { get; set; }

        /// <summary>
        /// 部门排序
        /// </summary>
        public float Order { get; set; }

        [Required(ErrorMessage ="必须选择上级部门")]
        /// <summary>
        /// 上级部门名称
        /// </summary>
        public Guid Pid { get; set; }

    }
}
