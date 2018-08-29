using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simpleness.App.Filters
{
    /// <summary>
    /// 禁止记录日志
    /// </summary>
    public class DisableAuditAttribute:ActionFilterAttribute
    {
    }
}
