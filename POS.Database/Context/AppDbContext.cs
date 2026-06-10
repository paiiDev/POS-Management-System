using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using POS.Database.Entities;

namespace POS.Database.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleItem> SaleItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VoidLog> VoidLogs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>().HasQueryFilter(c => !c.IsDeleted);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.CostPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.SellingPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Categories_Id__Products_CategoryId");
        });

        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted && !p.Category.IsDeleted );

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.Property(e => e.InvoiceNo).HasMaxLength(50);
            entity.Property(e => e.SaleDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Paid");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserId).HasDefaultValue(1);

            entity.HasOne(d => d.User).WithMany(p => p.Sales)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Id_Sales_UserId");
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Id_SaleItems_ProductId");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Id_SaleItems_SaleId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasDefaultValue("Name");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted)
            ;
        modelBuilder.Entity<VoidLog>(entity =>
        {
            entity.ToTable("VoidLog");

            entity.Property(e => e.CashierName).HasMaxLength(50);
            entity.Property(e => e.InvoiceNo).HasMaxLength(50);
            entity.Property(e => e.VoidedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VoidedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Sale).WithMany(p => p.VoidLogs)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Id_VoidLog_SaleId");
        });
        modelBuilder.HasSequence<int>("OrderNumbers");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
