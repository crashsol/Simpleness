using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.DataEntityFramework.Entity
{
    public class AppRole:IdentityRole<Guid>
    {        

        public AppRole(string name) : base(name) { }
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }
    }
}
