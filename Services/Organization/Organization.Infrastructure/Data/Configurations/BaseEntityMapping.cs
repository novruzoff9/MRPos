using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.Domain.Common;
using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Data.Configurations;

public class BaseEntityMapping<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
    }
}

public class BranchMapping : BaseEntityMapping<Branch>
{
    public override void Configure(EntityTypeBuilder<Branch> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Id)
            .HasMaxLength(6);

        builder.Property(e => e.ServiceFee)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(120);

        builder.Property(e => e.LastModifiedBy)
            .HasMaxLength(120);

        builder.HasOne(e => e.Company)
            .WithMany(b => b.Branches)
            .HasForeignKey(b => b.CompanyId);

    }
}