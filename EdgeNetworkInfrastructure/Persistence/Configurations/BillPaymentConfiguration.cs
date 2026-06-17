using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdgeNetworkInfrastructure.Persistence.Configurations
{
    public class BillPaymentConfiguration : IEntityTypeConfiguration<BillPayment>
    {
        public void Configure(EntityTypeBuilder<BillPayment> builder)
        {
            builder.HasKey(b=>  b.Id);

            builder.Property(b => b.PhoneNumber).IsRequired().HasMaxLength(15);

            builder.Property(b => b.DataPlanId).HasMaxLength(50);

            builder.OwnsOne(b => b.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("Amount");
                money.Property(m => m.Currency).HasColumnName("Currency");
            });

            builder.HasOne(b=> b.Wallet).WithMany().HasForeignKey(b=> b.walletId).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
