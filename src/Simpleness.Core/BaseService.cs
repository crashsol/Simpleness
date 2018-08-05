using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Simpleness.DataEntityFramework;
using Simpleness.Infrastructure.AspNetCore.Extensions;
using Simpleness.Infrastructure.AspNetCore.Models;

namespace Simpleness.Core
{
    /// <summary>
    /// 服务基类
    /// </summary>
   public class BaseService
    {
        public readonly SimplenessDbContext _dbContent;
        public readonly ILogger _logger;
        public UserIdentity UserIdentity => _httpContextAccessor.HttpContext.User?.UserIdentity();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseService(SimplenessDbContext dbContext,ILogger<BaseService> logger,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _dbContent = dbContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
       
        
    }
}
