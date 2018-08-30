using Simpleness.Utility.CommonDto;
using Simpleness.Core.Role.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simpleness.Core.Role
{
    /// <summary>
    /// 角色管理类
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 获取所有角色信息，用于列表显示
        /// </summary>
        /// <returns></returns>
        Task<PageResultDto<RoleRDto>> RoleListsAsync(PageQueryDto queryDto);

        /// <summary>
        /// 根据角色ID，获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RoleUDto> GetRoleByIdAsync(Guid id); 

        /// <summary>
        /// 创建一个角色,返回角色ID
        /// </summary>
        /// <param name="dto"><see cref="RoleCDto"/></param>
        /// <returns>返回角色ID</returns>
        Task<Guid> CreateAsync(RoleCDto dto);

        /// <summary>
        /// 更新一个角色
        /// </summary>
        /// <param name="dto"><see cref="RoleUDto"/></param>
        /// <returns></returns>
        Task UpdateAsync(RoleUDto dto); 


        /// <summary>
        /// 删除一个角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);


        /// <summary>
        /// 获取某个角色下的所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<TransferDto<Guid>> GetRoleUsersByRoleIdAsync(Guid roleId);


        /// <summary>
        ///  更新角色用户列表
        /// </summary>
        /// <param name="dto">用户角色更新模型</param>
        /// <returns></returns>
        Task UpdateRoleUsersAsync(RoleUsersDto dto);


        /// <summary>
        /// 获取指定角色所有权限
        /// </summary>
        /// <param name="roleid">角色Id</param>
        /// <returns>返回角色菜单树</returns>
        Task<TreeDto<string>> GetRolePermissionAsync(Guid roleid);

        /// <summary>
        /// 更新角色权限列表
        /// </summary>
        /// <param name="roleid">角色Id</param>
        /// <param name="permissions">权限集合</param>
        /// <returns></returns>
        Task UpdateRolePermissionAsync(RolePermissionDto dto);
    }
}
