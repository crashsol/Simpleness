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

            Guid.TryParse(User.Claims.FirstOrDefault(b => b.Type == "sub")?.Value, out Guid userId);
            userIdentity.Id = userId;
            userIdentity.UserName = User.Claims.FirstOrDefault(b => b.Type == "name")?.Value ?? "";
            return userIdentity;
        }
    }
}
