using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdgeNetworkInfrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.OwnsOne(u => u.FullName, fn =>
            {
                fn.Property(f => f.FirstName).HasColumnName("FirstName").HasMaxLength(50).IsRequired();
                fn.Property(f => f.LastName).HasColumnName("LastName").HasMaxLength(50).IsRequired();
            });

            builder.OwnsOne(u => u.Email, e =>
            {
                e.Property(em => em.Value).HasColumnName("Email").HasMaxLength(100).IsRequired();
            });

            builder.OwnsOne(u => u.PhoneNumber, p =>
            {
                p.Property(ph => ph.Value).HasColumnName("PhoneNumber").HasMaxLength(11).IsRequired();
            });

            builder.Property(u => u.Status).HasConversion<string>();
            builder.HasMany(u => u.Wallets).WithOne().HasForeignKey(w => w.UserId);
        }
    }
}
