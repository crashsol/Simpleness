﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpleness.Infrastructure.AspNetCore.Extensions;
using Simpleness.Infrastructure.AspNetCore.Models;

namespace Simpleness.App.Controllers
{
    [Authorize]
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