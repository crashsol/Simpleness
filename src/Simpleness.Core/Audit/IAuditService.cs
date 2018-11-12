using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simpleness.DataEntityFramework.Entity;


namespace Simpleness.Core.Audit
{
    public interface IAuditService
    {
        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        Task CreateAsync(DataEntityFramework.Entity.Audit enity);


        /// <summary>
        /// 获取审计日志
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DataEntityFramework.Entity.Audit>> GetAuditLogAsync();
    }
}
