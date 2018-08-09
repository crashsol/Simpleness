using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Utility.CommonDto
{
    /// <summary>
    /// 穿梭框数据项
    /// </summary>
    public class TransferItem<TKey>
    {

        /// <summary>
        /// 唯一标识
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// 显示字符串
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 是否禁止
        /// </summary>
        public bool Disabled { get; set; }
    }

    /// <summary>
    /// 穿梭框模型
    /// </summary>
    public class TransferDto<T>
    {
        /// <summary>
        /// 所有数据项
        /// </summary>
        public List<TransferItem<T>> Items { get; set; }

        /// <summary>
        /// 已经勾选的数据项 Key 集合
        /// </summary>
        public List<T> SelectItems { get; set; }
    }
}
