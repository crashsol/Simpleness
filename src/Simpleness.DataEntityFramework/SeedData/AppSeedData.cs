using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Simpleness.DataEntityFramework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Simpleness.DataEntityFramework.SeedData
{

   public class AppSeedData
    {


        public static async System.Threading.Tasks.Task InitAsync(IServiceProvider serviceProvider)
        {
            var _dbcontext = serviceProvider.GetRequiredService<SimplenessDbContext>();
            var _logger = serviceProvider.GetRequiredService<ILogger<AppSeedData>>();
            var _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
          
            _logger.LogInformation("初始化数据库数据");
            //更新数据库
            await _dbcontext.Database.MigrateAsync();

            //创建系统管理和系统管理用户
            if (!_dbcontext.Users.Any())
            {
                var user = new AppUser
                {
                    UserName = "47147551@qq.com",
                    Email = "47147551@qq.com",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "123qwe!@#");
                if (result.Succeeded)
                {
                    _logger.LogInformation("初始化数据，创建超级管理员成功");
                }
                else
                {
                    _logger.LogError($"初始化 系统人员 出错:{string.Join(',', result.Errors)}");
                }

                var role = new AppRole("超级管理员")
                {
                    Description = "具有系统所有权限"
                };
                //添加系统角色
                result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("初始化数据，创建超级管理员成功");
                    //添加角色权限
                    var claims = new List<Claim>();
                    claims.Add(new Claim("permission", "Users"));
                    claims.Add(new Claim("permission", "Users_Create"));
                    claims.Add(new Claim("permission", "Users_Delete"));
                    claims.Add(new Claim("permission", "Users_Locked"));
                    claims.Add(new Claim("permission", "Roles"));
                    claims.Add(new Claim("permission", "Roles_Create"));
                    claims.Add(new Claim("permission", "Roles_Delete"));
                    claims.Add(new Claim("permission", "Roles_Edit"));
                    claims.Add(new Claim("permission", "Roles_Memeber"));
                    claims.Add(new Claim("permission", "Roles_Permission"));
                    _dbcontext.RoleClaims.AddRange(claims.Select(b => new IdentityRoleClaim<Guid> { RoleId = role.Id, ClaimType = b.Type, ClaimValue = b.Value }));
                    _dbcontext.SaveChanges();
                }
                else
                {
                    _logger.LogError($"初始化系统角色出错:{string.Join(',', result.Errors)}");
                }
                _dbcontext.UserRoles.Add(new IdentityUserRole<Guid> { UserId = user.Id, RoleId = role.Id });
                _dbcontext.SaveChanges();

            }         
            if(!_dbcontext.Departments.Any())
            {
                _dbcontext.Departments.Add(new Department {

                     Name="组织架构",
                     Description = "部门组织结构",
                     Order = 99,
                     FullPath= "组织架构"
                });
                _dbcontext.SaveChanges();

            }
        }
    }
}
