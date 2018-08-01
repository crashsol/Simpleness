using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Simpleness.DataEntityFramework.Entity
{
    public class AppUser:IdentityUser<Guid> 
    {
        /// <summary>
        /// 职位
        /// </summary>
        public string Tilte { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avator { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}
