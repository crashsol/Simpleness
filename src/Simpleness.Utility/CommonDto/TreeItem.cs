using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Utility.CommonDto
{

    /// <summary>
    /// 树形结构显示模型
    /// </summary>
    public class TreeDto<TKey>
    {
        /// <summary>
        /// 树根节点（嵌套包含子节点）
        /// </summary>
        public TreeItem<TKey> Tree { get; set; }

        /// <summary>
        /// 已经勾选中的节点id集合
        /// </summary>
        public List<TKey> SelectKeys { get; set; }
        

    }
   
    /// <summary>
    /// 树形结构模型
    /// </summary>
   public class TreeItem<T>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public T Id { get; set; }
        /// <summary>
        /// 树形节点显示名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 节点是否被禁用
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeItem<T>> Children { get; set; }

    }
}
