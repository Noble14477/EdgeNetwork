using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdgeNetworkInfrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(w => w.Id);

            builder.OwnsOne(w => w.Balance, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Balance").HasColumnType("decimal(18,2)");
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
            });

            builder.OwnsOne(w => w.AccountNumber, an =>
            {
                an.Property(a => a.Value).HasColumnName("AccountNumber").HasMaxLength(20);
            });

            builder.Property(w => w.Status)
                .HasConversion<string>();

            builder.HasMany(w => w.Transactions)
                .WithOne()
                .HasForeignKey(t => t.WalletId);
        }
    }
}
