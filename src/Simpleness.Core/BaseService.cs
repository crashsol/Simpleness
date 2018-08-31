using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
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
        public readonly IMapper _mapper;   
        public BaseService(SimplenessDbContext dbContext,ILogger<BaseService> logger,
            IMapper mapper)
        {
            _dbContent = dbContext;
            _logger = logger;
            _mapper = mapper;
         
        }      

      
    }
}
