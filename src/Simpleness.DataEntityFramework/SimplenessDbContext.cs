using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simpleness.DataEntityFramework.Entity;
using Simpleness.Utility.Extensions;

namespace Simpleness.DataEntityFramework
{
   public  class SimplenessDbContext: IdentityDbContext<AppUser,AppRole,Guid>
    {

        public SimplenessDbContext(DbContextOptions<SimplenessDbContext> options) 
          : base(options)
        {

        }

        public DbSet<UserDepartments> UserDepartments { get; set; }

        public DbSet<Department> Departments { get; set; }


        /// <summary>
        /// 审计日志记录
        /// </summary>
        public DbSet<Audit> Audits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.AddEntityConfigurationFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

    }
}
