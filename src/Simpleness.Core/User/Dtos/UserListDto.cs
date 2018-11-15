using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Core.User.Dtos
{
    /// <summary>
    /// 用户列表
    /// </summary>
    public class UserListDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }
      

        public string Phone { get; set; }

        /// <summary>
        /// 锁定到期日期
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
