using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Simpleness.DataEntityFramework;
using Simpleness.DataEntityFramework.Entity;
using Microsoft.EntityFrameworkCore;

namespace Simpleness.Core.Audit
{
    public class AuditService : BaseService,IAuditService
    {

        public AuditService(SimplenessDbContext dbContext, ILogger<AuditService> logger,
           IMapper mapper) : base(dbContext, logger, mapper){           

        }
        public async Task CreateAsync(DataEntityFramework.Entity.Audit enity)
        {
            await _dbContent.Audits.AddAsync(enity);
            await _dbContent.SaveChangesAsync();
        }

        public async Task<IEnumerable<DataEntityFramework.Entity.Audit>> GetAuditLogAsync()
        {
           return await _dbContent.Audits.AsNoTracking().ToListAsync();
        }
    }
}
