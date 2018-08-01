using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.Domain.SeedWork
{
    /// <summary>
    /// 聚合跟实体基类
    /// </summary>
    public abstract class AggregatesEntity
    {
        /// <summary>
        /// 领域事件
        /// </summary>
        private List<INotification> _domainEvents;

        /// <summary>
        /// 领域事件 (只读列表)
        /// </summary>
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// 添加一个领域事件
        /// </summary>
        /// <param name="eventItem"></param>
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        /// <summary>
        /// 移除一个领域事件
        /// </summary>
        /// <param name="eventItem"></param>
        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        /// <summary>
        /// 清空领域事件
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
