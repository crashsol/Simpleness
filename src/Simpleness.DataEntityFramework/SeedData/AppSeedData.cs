using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Simpleness.DataEntityFramework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (!_dbcontext.Users.Any())
            {
                var admin = new AppUser
                {
                    UserName = "admin@qq.com",
                    Email = "admin@qq.com"
                };

                var result = await _userManager.CreateAsync(admin, "123qwe!@#");
                if (result.Succeeded)
                {
                    _logger.LogInformation("初始化数据，创建超级管理员成功");
                }
                else
                {
                    _logger.LogError($"初始化 系统人员 出错:{string.Join(',', result.Errors)}");
                }
            }
            //创建系统角色
            if (!_dbcontext.Roles.Any())
            {
                var role = new AppRole("超级管理员");
                role.Description = "具有系统所有权限";

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("初始化数据，创建超级管理员成功");
                }
                else
                {
                    _logger.LogError($"初始化系统角色出错:{string.Join(',', result.Errors)}");
                }
            }
        }
    }
}
