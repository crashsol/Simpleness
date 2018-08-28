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

namespace Simpleness.App.Filters
{
    public class AuditAttribute : ActionFilterAttribute 
    {

        private  SimplenessDbContext _dbContext;
        private Audit audit;
        public AuditAttribute()        {
            
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            audit = new Audit();
            if(context.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = context.HttpContext.User;
                audit.UserId = user.Claims.FirstOrDefault(b => b.Type == ClaimTypes.NameIdentifier).Value ?? "";
                audit.UserName = user.Claims.FirstOrDefault(b=>b.Type =="name").Value ??"";
            }
            audit.ServiceName = context.HttpContext.Request.Path;
            audit.MethodType = context.HttpContext.Request.Method;
            audit.Parameters = JsonConvert.SerializeObject(context.ActionArguments);
            audit.IPAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            audit.ComputerName =  System.Net.Dns.GetHostEntry(context.HttpContext.Connection.RemoteIpAddress).HostName;
            audit.ExcutionTime = DateTime.Now;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result =JsonConvert.SerializeObject(context.Result);
            var etime = DateTime.Now;
            audit.Duration = (DateTime.Now - audit.ExcutionTime).TotalMilliseconds;
            audit.Result = result;
            audit.StatusCode = context.HttpContext.Response.StatusCode;
            if(context.Exception !=null)
            {
                audit.Exception = context.Exception.StackTrace;
            }                
            _dbContext = context.HttpContext.RequestServices.GetRequiredService<SimplenessDbContext>();
            _dbContext.Audits.Add(audit);
            _dbContext.SaveChanges();           
            base.OnActionExecuted(context);
        }

    }
}
