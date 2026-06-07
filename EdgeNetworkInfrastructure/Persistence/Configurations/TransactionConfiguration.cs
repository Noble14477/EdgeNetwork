using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdgeNetworkInfrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

                builder.Property(m => m.Amount).HasColumnType("numeric(18,2)").IsRequired();
                builder.Property(m => m.Currency).HasMaxLength(3).IsRequired();
           

            builder.Property(t => t.Type)
                .HasConversion<string>();

            builder.Property(t => t.Status)
                .HasConversion<string>();

            builder.Property(t => t.Reference)
                .HasMaxLength(50);
        }
    }
}
