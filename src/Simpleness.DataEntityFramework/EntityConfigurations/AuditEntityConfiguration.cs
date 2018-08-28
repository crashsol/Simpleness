using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpleness.DataEntityFramework.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.DataEntityFramework.EntityConfigurations
{
    public class AuditEntityConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("Audit");
        }
    }
}
