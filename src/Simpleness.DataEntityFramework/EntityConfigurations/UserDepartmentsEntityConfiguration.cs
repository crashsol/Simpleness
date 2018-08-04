using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpleness.DataEntityFramework.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.DataEntityFramework.EntityConfigurations
{
    class UserDepartmentsEntityConfiguration : IEntityTypeConfiguration<UserDepartments>
    {
        public void Configure(EntityTypeBuilder<UserDepartments> builder)
        {
            builder.ToTable("UserDepartment");         
            builder.HasKey(b => new { b.AppUserId, b.DepartmentId });
        }
    }
}
