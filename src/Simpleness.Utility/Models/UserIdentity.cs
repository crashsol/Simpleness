﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Utility.Models
{
    /// <summary>
    /// 当前登录用户信息
    /// </summary>
    public class UserIdentity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
    }
}
