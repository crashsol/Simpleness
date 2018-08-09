using Microsoft.Extensions.DependencyInjection;
using Simpleness.Core.Department;
using Simpleness.Core.Role;
using Simpleness.Core.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Core
{
   public static class ServiceLayerInject
    {
        /// <summary>
        /// 应用层注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            return services;
        }
    }
}
