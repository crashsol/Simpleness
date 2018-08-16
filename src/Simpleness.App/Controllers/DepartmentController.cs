using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simpleness.App.Models;
using Simpleness.Utility.CommonDto;
using Simpleness.Core.Department;
using Simpleness.Core.Department.Dto;
using Simpleness.Infrastructure.AspNetCore.Authorize;

namespace Simpleness.App.Controllers
{

    [Permission(nameof(PermissionSettings.Departments), PermissionSettings.Departments)]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// 获取全部部门信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        [ProducesResponseType(typeof(List<DepartmentRDto>), 200)]
        public async Task<IActionResult> DepartmentsAsync()
        {
            return Ok(await _departmentService.DepartmentListAsync());
        }


        /// <summary>
        /// 获取全部部门信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("tree")]
        [ProducesResponseType(typeof(DepartmentTreeItem), 200)]
        public async Task<IActionResult> DepartmentTreeAsync()
        {
        
            return Ok(await _departmentService.GetDepartmentTreeAsync());
        }

        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="dto"><see cref="DepartmentCDto"/>创建部门Dto</param>
        /// <returns></returns>       
        [HttpPost("create")]
        [ProducesResponseType(typeof(Guid), 200)]
        [Permission(nameof(PermissionSettings.Departments_Create), PermissionSettings.Departments_Create)]
        public async Task<IActionResult> CreateAsync([FromBody]DepartmentCDto dto)
        {
            return Ok(await _departmentService.CreateAsync(dto));
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="dto"><see cref="DepartmentUDto"/>部门更新模型</param>
        /// <returns></returns>
        [HttpPost("update")]
        [ProducesResponseType(typeof(string), 200)]
        [Permission(nameof(PermissionSettings.Departments_Edit), PermissionSettings.Departments_Edit)]
        public async Task<IActionResult> UpdateAsync([FromBody]DepartmentUDto dto)
        {
            await _departmentService.UpdateAsync(dto);
            return Ok("更新成功");
        }



        /// <summary>
        /// 删除部门及所有子部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        [HttpPost("delete/{id}")]
        [ProducesResponseType(200)]
        [Permission(nameof(PermissionSettings.Departments_Delete), PermissionSettings.Departments_Delete)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _departmentService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// 获取指定部门成员
        /// </summary>
        /// <param name="id">部门编号</param>
        /// <returns></returns>
        [HttpGet("users/{id}")]
        [ProducesResponseType(typeof(TransferDto<Guid>), 200)]
        [Permission(nameof(PermissionSettings.Deaprtments_Member), PermissionSettings.Deaprtments_Member)]
        public async Task<IActionResult> GetDepartmentUsers(Guid id) => Ok(await _departmentService.GetDepartmentUsersAsync(id));

        /// <summary>
        /// 更新部门成员
        /// </summary>
        /// <param name="dto"><see cref="DepartmentUsersDto"/><paramref name="dto"/></param>
        /// <returns></returns>
        [HttpPost("users")]
        [ProducesResponseType(typeof(string), 200)]
        [Permission(nameof(PermissionSettings.Deaprtments_Member), PermissionSettings.Deaprtments_Member)]
        public async Task<IActionResult> UpdateDepartUsersAsync([FromBody]DepartmentUsersDto dto)
        {
            await _departmentService.UpdateDepartmentUsersAsync(dto);
            return Ok("操作成功");
        }
    }
}