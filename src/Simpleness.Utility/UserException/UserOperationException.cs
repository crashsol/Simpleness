using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Utility.UserException
{
    /// <summary>
    /// 用户自定义异常
    /// </summary>
    public class UserOperationException:Exception
    {
        public UserOperationException() : base() { }
        public UserOperationException(string message) : base(message) { }
        public UserOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
