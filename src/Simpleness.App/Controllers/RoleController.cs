using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simpleness.App.Models;
using Simpleness.Utility.CommonDto;
using Simpleness.Core.Role;
using Simpleness.Core.Role.Dtos;
using Simpleness.Infrastructure.AspNetCore.Authorize;

namespace Simpleness.App.Controllers
{
    [Permission(nameof(PermissionSettings.Roles), PermissionSettings.Roles)]
    public class RoleController : BaseController
    {

        public readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(List<RoleRDto>), 200)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _roleService.RoleListsAsync());
        }


        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="dto">创建模型</param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(string), 200)]
        [Permission(nameof(PermissionSettings.Roles_Create), PermissionSettings.Roles_Create)]
        public async Task<IActionResult> CreateAsync([FromBody]RoleCDto dto)
        {
            return Ok(await _roleService.CreateAsync(dto));
        }


        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="dto">更新模型</param>
        /// <returns></returns>
        [HttpPost("update")]
        [ProducesResponseType(200)]
        [Permission(nameof(PermissionSettings.Roles_Edit), PermissionSettings.Roles_Edit)]
        public async Task<IActionResult> UpdateAsync([FromBody]RoleUDto dto)
        {
            await _roleService.UpdateAsync(dto);
            return Ok();
        }



        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>

        [HttpPost("delete/{id}")]
        [ProducesResponseType(200)]
        [Permission(nameof(PermissionSettings.Roles_Delete), PermissionSettings.Roles_Delete)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _roleService.DeleteAsync(id);
            return Ok();
        }



        /// <summary>
        /// 获取角色所有成员
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns> 
        [HttpGet("users/{id}")]
        [ProducesResponseType(typeof(TransferDto<Guid>), 200)]
        [Permission(nameof(PermissionSettings.Roles_Memeber), PermissionSettings.Roles_Memeber)]
        public async Task<IActionResult> GetRoleUsersAsync(Guid id)
        {
            return Ok(await _roleService.GetRoleUsersByRoleIdAsync(id));
        }


        /// <summary>
        /// 更新角色成员
        /// </summary>
        /// <param name="dto"><see cref="RoleUsersDto"/>角色成员更新模型</param>
        /// <returns></returns>     
        [HttpPost("users")]
        [ProducesResponseType(200)]
        [Permission(nameof(PermissionSettings.Roles_Memeber), PermissionSettings.Roles_Memeber)]
        public async Task<IActionResult> UpdateRoleUsersAsync([FromBody]RoleUsersDto dto)
        {
            await _roleService.UpdateRoleUsersAsync(dto);
            return Ok();
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpGet("permission/{id}")]
        [ProducesResponseType(typeof(TreeDto<string>), 200)]
        [Permission(nameof(PermissionSettings.Roles_Permission), PermissionSettings.Roles_Permission)]
        public async Task<IActionResult> GetRolePermissionByIdAsync(Guid id)
        {
            return Ok(await _roleService.GetRolePermissionAsync(id));
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="dto">更新权限Dto</param>
        /// <returns></returns>
        [HttpPost("permission")]
        [ProducesResponseType(typeof(string), 200)]
        [Permission(nameof(PermissionSettings.Roles_Permission), PermissionSettings.Roles_Permission)]
        public async Task<IActionResult> UpdateRolePersmissionAsync([FromBody]RolePermissionDto dto)
        {
            await _roleService.UpdateRolePermissionAsync(dto);
            return Ok("更新角色权限成功");
        }


    }
}