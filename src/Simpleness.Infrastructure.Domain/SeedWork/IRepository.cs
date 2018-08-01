using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.Domain.SeedWork
{
    /// <summary>
    /// 聚合跟仓储对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
