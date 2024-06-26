﻿namespace ReservationManagement.Infrastructure.Configuration;
    
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationManagement.Domain.Entities.Customers;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.Reservations)
            .WithOne(r => r.Customer)
            .HasForeignKey(r => r.CustomerId);
    }
}
