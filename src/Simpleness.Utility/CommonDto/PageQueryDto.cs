using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Utility.CommonDto
{
    /// <summary>
    /// 查询基类
    /// </summary>
    public class PageQueryDto 
    {
        /// <summary>
        /// 每页多少条数据
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前的页码
        /// </summary>
        public int CurrentPage { get; set; }
    }
}
