using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Simpleness.DataEntityFramework;
using Simpleness.DataEntityFramework.Entity;
using Simpleness.Infrastructure.AspNetCore.Extensions;
using Newtonsoft;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Diagnostics;

namespace Simpleness.App.Filters
{
    /// <summary>
    /// 日志记录标记
    /// </summary>
    public class AuditAttribute : ActionFilterAttribute
    {

        private SimplenessDbContext _dbContext;
        private Audit audit;
        private Stopwatch stopwatch;
        private bool auditSwitch;

      
        public AuditAttribute()
        {         
        }

        /// <summary>
        /// 执行任务开始前
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            audit = new Audit();           
            auditSwitch = context.Filters.Any(b => b.GetType() == typeof(DisableAuditAttribute));         
            if(!auditSwitch)
            {             
                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    var user = context.HttpContext.User.UserIdentity();
                    audit.UserId = user.Id.ToString();
                    audit.UserName = user.UserName;
                }
                audit.ServiceName = context.HttpContext.Request.Path;
                audit.MethodType = context.HttpContext.Request.Method;
                audit.Parameters = JsonConvert.SerializeObject(context.ActionArguments);
                audit.IPAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                audit.ComputerName = System.Net.Dns.GetHostEntry(context.HttpContext.Connection.RemoteIpAddress).HostName;
                audit.ExcutionTime = DateTime.Now;

            }         
         
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 执行任务完成时记录
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {           
            if(!auditSwitch)
            {
                if (context.Result is ObjectResult)
                {
                    var resultObj = (context.Result as ObjectResult);
                    audit.Result = JsonConvert.SerializeObject(resultObj.Value);
                }
                else
                {
                    var result = JsonConvert.SerializeObject(context.Result);
                    audit.Result = result;
                }
                audit.StatusCode = context.HttpContext.Response.StatusCode;
                if (context.Exception != null)
                {
                    audit.Exception = context.Exception.StackTrace;
                }
                stopwatch.Stop();
                audit.Duration = stopwatch.ElapsedMilliseconds;
                _dbContext = context.HttpContext.RequestServices.GetRequiredService<SimplenessDbContext>();
                _dbContext.Audits.Add(audit);
                _dbContext.SaveChanges();
            }
       
            base.OnActionExecuted(context);
        }

    }
}
