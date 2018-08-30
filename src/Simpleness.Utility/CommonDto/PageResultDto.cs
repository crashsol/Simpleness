using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Utility.CommonDto
{
    /// <summary>
    /// 查询返回内容详细
    /// </summary>
    public class PageResultDto<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }  


        /// <summary>
        /// 数据内容
        /// </summary>
        public IReadOnlyCollection<T> Items { get; set; }

        public PageResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PageResultDto(int totalCount, IReadOnlyList<T> items)            
        {
            TotalCount = totalCount;
            Items = items;
        }


    }
}
