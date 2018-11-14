using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpleness.App.Filters;
using Simpleness.Infrastructure.AspNetCore.Extensions;
using Simpleness.Infrastructure.AspNetCore.Models;
using Simpleness.Infrastructure.AspNetCore.Mvc;

namespace Simpleness.App.Controllers
{
    [Audit]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        protected UserIdentity UserIdentity => User.UserIdentity();    
      

    }
}