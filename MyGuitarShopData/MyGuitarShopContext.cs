using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

public partial class MyGuitarShopContext : DbContext
{
    public MyGuitarShopContext()
    {
    }

    public MyGuitarShopContext(DbContextOptions<MyGuitarShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderItemProduct> OrderItemProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost\\sqlexpress;Initial Catalog=MyGuitarShop;Integrated Security=True; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__091C2A1BB4062810");

            entity.Property(e => e.Line2).HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Addresses).HasConstraintName("FK__Addresses__Custo__4CA06362");
        });

        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Administ__719FE4E8EA9520AF");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B983EFD7A");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8565BD142");

            entity.Property(e => e.BillingAddressId).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.ShippingAddressId).HasDefaultValueSql("(NULL)");
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.ToView("CustomerAddresses");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF71507FE6");

            entity.Property(e => e.CardExpires).IsFixedLength();
            entity.Property(e => e.CardNumber).IsFixedLength();
            entity.Property(e => e.ShipDate).HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasConstraintName("FK__Orders__Customer__4F7CD00D");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__OrderIte__727E83EB3762A2EE");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("FK__OrderItem__Order__4D94879B");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems).HasConstraintName("FK__OrderItem__Produ__4E88ABD4");
        });

        modelBuilder.Entity<OrderItemProduct>(entity =>
        {
            entity.ToView("OrderItemProducts");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDA319FD45");

            entity.Property(e => e.DateAdded).HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK__Products__Catego__5070F446");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromoId).HasName("PK__Promotio__33D334D02402AA96");

            entity.Property(e => e.PromoId).ValueGeneratedNever();
        });
        modelBuilder.HasSequence("PROMO_SEQ")
            .StartsAt(10L)
            .IncrementsBy(5);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
