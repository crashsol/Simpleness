using Simpleness.Infrastructure.AspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Extensions 
{
    /// <summary>
    /// 获取当前登录用户信息
    /// </summary>
    public static class UserAccessorExtend
    {
        public static UserIdentity UserIdentity(this ClaimsPrincipal User)
        {
            var userIdentity = new UserIdentity();
            userIdentity.Id = Guid.Parse(User.Claims.FirstOrDefault(b => b.Type =="sub").Value);
            userIdentity.UserName = User.Claims.FirstOrDefault(b => b.Type == "name").Value;
            return userIdentity;
        }
    }
}
