using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Simpleness.Utility.CommonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Authorize
{
    /// <summary>
    /// 授权策略注入
    /// </summary>
    public static class PermissionServiceInject
    {
        /// <summary>
        /// 通过反射获取所有的权限列表，并把权限信息注入，
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemeble">Controller所在程序集</param>
        /// <param name="controllerBaseType">BaseController类型</param>
        /// <returns></returns>
        public static IServiceCollection AddPermissions(this IServiceCollection services, Assembly assemeble, Type controllerBaseType)
        {
            #region 反射获取所有权限列表
            var permissionTree = new TreeItem<string>
            {
                Id = "System_",
                Label = "系统菜单",
                Children = new List<TreeItem<string>>()
            };
            //获取所有的控制器
            var controllers = Assembly.GetExecutingAssembly().GetTypes().Where(b => controllerBaseType.IsAssignableFrom(b));
            foreach (var controller in controllers)
            {
                var controllerAttrubites = controller.GetCustomAttributes().Where(b => b.GetType() == typeof(PermissionAttribute));
                foreach (var controllerAttribute in controllerAttrubites)
                {
                    PermissionAttribute permission = controllerAttribute as PermissionAttribute;
                    //Controller权限
                    var controllerPermission = new TreeItem<string>
                    {
                        Id = permission.Name,
                        Label = permission.Description,
                        Children = new List<TreeItem<string>>()
                    };
                    //获取Controller下所有 public IAction PermissionAttribute 标记
                    var methods = controller.GetMethods().Where(b => b.IsPublic && !b.IsDefined(typeof(NonActionAttribute)));
                    foreach (var method in methods)
                    {
                        var methodPermissions = method.GetCustomAttributes(true).Where(b => b.GetType() == typeof(PermissionAttribute));
                        foreach (var methodPermission in methodPermissions)
                        {
                            PermissionAttribute actionAttr = methodPermission as PermissionAttribute;
                            if (!controllerPermission.Children.Any(b => b.Id == actionAttr.Name))
                            {
                                controllerPermission.Children.Add(new TreeItem<string>
                                {
                                    Id = actionAttr.Name,
                                    Label = actionAttr.Description
                                });
                            }
                        }
                    }

                    permissionTree.Children.Add(controllerPermission);
                }

            }
            #endregion

            //将权限注入容器
            services.AddSingleton(permissionTree);
            //注册PolicyProvier
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            return services;

        }
    }
}
