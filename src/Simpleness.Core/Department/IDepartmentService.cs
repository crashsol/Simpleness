using Simpleness.Utility.CommonDto;
using Simpleness.Core.Department.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpleness.Core.Department
{
    /// <summary>
    /// 部门管理服务
    /// </summary>
    public interface IDepartmentService
    {

        /// <summary>
        /// 获取全部部门结构,菜单树结构
        /// </summary>
        /// <returns></returns>
        Task<TreeItem<Guid>> GetDepartmentTreeAsync();

        /// <summary>
        /// 获取所有部门列表
        /// </summary>
        /// <returns></returns>
        Task<List<DepartmentRDto>> DepartmentListAsync();
      

        /// <summary>
        /// 创建一个部门
        /// </summary>
        /// <param name="dto"><see cref="DepartmentCDto"/>创建模型</param>
        /// <returns><see cref="Guid"/>部门ID</returns>
        Task<Guid> CreateAsync(DepartmentCDto dto);


        /// <summary>
        /// 根据ID获取部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        Task<DepartmentUDto> GetDepartmentByIdAsync(Guid id);

        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateAsync(DepartmentUDto dto);

        /// <summary>
        /// 删除一个部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);


        /// <summary>
        /// 获取一个部门所有用户成员
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns></returns>
        Task<TransferDto<Guid>> GetDepartmentUsersAsync(Guid  id);

        /// <summary>
        /// 更新部门成员
        /// </summary>
        /// <param name="dto"><see cref="DepartmentUsersDto"/></param>
        /// <returns></returns>
        Task UpdateDepartmentUsersAsync(DepartmentUsersDto dto);
    }

}
