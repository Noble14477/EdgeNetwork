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

            builder.OwnsOne(t => t.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)");
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
            });

            builder.Property(t => t.Type)
                .HasConversion<string>();

            builder.Property(t => t.Status)
                .HasConversion<string>();

            builder.Property(t => t.Reference)
                .HasMaxLength(50);
        }
    }
}
