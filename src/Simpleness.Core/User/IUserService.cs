using Simpleness.Core.User.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simpleness.Core.User
{
    /// <summary>
    /// 用户信息管理
    /// </summary>
   public interface IUserService
    {

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreateUserAsync(UserCDto dto);


        /// <summary>
        /// 获取系统所有用户列表
        /// </summary>
        /// <returns></returns>
        Task<List<UserListDto>> GetAllUsersAsync();


        
    }
}
