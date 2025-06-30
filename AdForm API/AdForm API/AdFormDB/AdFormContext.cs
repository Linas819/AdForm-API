using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdForm_API.AdFormDB;

public partial class AdFormContext : DbContext
{
    public AdFormContext()
    {
    }

    public AdFormContext(DbContextOptions<AdFormContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;UserId=postgres;Password=admin;Database=AdForm;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("discounts_pk");

            entity.ToTable("discounts");

            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.MinQuantity)
                .HasDefaultValue(0)
                .HasColumnName("min_quantity");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("discounts_products_fk");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pk");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId)
                .HasColumnType("character varying")
                .HasColumnName("order_id");
            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .HasColumnName("product_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_products_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pk");

            entity.ToTable("products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
