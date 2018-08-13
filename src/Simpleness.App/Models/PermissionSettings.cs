using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simpleness.App.Models
{
    /// <summary>
    /// 系统权限定义
    /// </summary>
    public class PermissionSettings
    {

        public const string Users = "用户管理";
        public const string Users_Create = "创建用户";
        public const string Users_Locked = "锁定/解锁";
        public const string Users_Delete = "删除用户";


        public const string Roles = "角色管理";
        public const string Roles_Create = "创建角色";
        public const string Roles_Edit = "修改角色";
        public const string Roles_Delete = "删除角色";
        public const string Roles_Memeber = "设定成员";
        public const string Roles_Permission = "设定权限";


        public const string Departments = "部门管理";
        public const string Departments_Create = "创建部门";
        public const string Departments_Edit = "编辑部门";
        public const string Departments_Delete = "删除部门";
        public const string Deaprtments_Member = "成员设定";
    }
}
