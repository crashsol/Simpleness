using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.DataEntityFramework.Entity
{
    /// <summary>
    /// 实体数据基类
    /// </summary>
    public abstract class BaseEntity<TKey>
    {
        /// <summary>
        /// 组件
        /// </summary>
        public TKey Id { get; set; }       
    }
}
