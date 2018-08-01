using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simpleness.DataEntityFramework.Entity;

namespace Simpleness.DataEntityFramework
{
   public  class SimplenessDbContext: IdentityDbContext<AppUser,AppRole,Guid>
    {

        public SimplenessDbContext(DbContextOptions<SimplenessDbContext> options) 
          : base(options)
        {

        }
    }
}
