using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Infrastructure.Persistence;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Transactions> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transactions>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("Transactions_pkey");

            entity.Property(e => e.IdCargo).UseIdentityAlwaysColumn();
            entity.Property(e => e.AuthorizationCode).HasMaxLength(30);
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.Cvv).HasMaxLength(4);
            entity.Property(e => e.Expiry).HasMaxLength(5);
            entity.Property(e => e.MerchantId).HasMaxLength(50);
            entity.Property(e => e.Pan).HasMaxLength(19);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
