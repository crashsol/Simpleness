using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpleness.DataEntityFramework.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.DataEntityFramework.EntityConfigurations
{
    public class DepartmentEntityConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");
            builder.Property(b => b.Description).HasMaxLength(PropertyDefinition.Large);
            builder.Property(b => b.Name).HasMaxLength(PropertyDefinition.Small).IsRequired();
            builder.Property(b => b.FullPath).HasMaxLength(PropertyDefinition.Large).IsRequired();
            builder.HasAlternateKey(b => b.FullPath);          
           
        }
    }
}
