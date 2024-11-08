using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoffeShop.Models
{
    public partial class CoffeShopContext : DbContext
    {
        public static CoffeShopContext Ins = new CoffeShopContext();
		public CoffeShopContext()
        {
			if (Ins == null) Ins = this;
		}

        public CoffeShopContext(DbContextOptions<CoffeShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Inventory> Inventories { get; set; } = null!;
        public virtual DbSet<LoyaltyPoint> LoyaltyPoints { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<ProductIngredient> ProductIngredients { get; set; } = null!;
        public virtual DbSet<ProductPointRedemption> ProductPointRedemptions { get; set; } = null!;
        public virtual DbSet<Table> Tables { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			optionsBuilder.UseSqlServer(config.GetConnectionString("MyCnn"));
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.AddedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("added_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MenuId).HasColumnName("menu_id");

                entity.Property(e => e.PriceAtAdd)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price_at_add");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cart__menu_id__71D1E811");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cart__user_id__70DDC3D8");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("PK__Inventor__52020FDD26167768");

                entity.ToTable("Inventory");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.LastRestocked)
                    .HasColumnType("datetime")
                    .HasColumnName("last_restocked");

                entity.Property(e => e.MinimumQuantity).HasColumnName("minimum_quantity");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("quantity");

                entity.Property(e => e.Unit)
                    .HasMaxLength(50)
                    .HasColumnName("unit");
            });

            modelBuilder.Entity<LoyaltyPoint>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__LoyaltyP__B9BE370F3C58D3EE");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.Points)
                    .HasColumnName("points")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.LoyaltyPoint)
                    .HasForeignKey<LoyaltyPoint>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LoyaltyPo__user___412EB0B6");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.MenuId).HasColumnName("menu_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("image_url");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Menu__category_i__3D5E1FD2");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.TableId).HasColumnName("table_id");

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("total_price");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.TableId)
                    .HasConstraintName("FK__Orders__table_id__49C3F6B7");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Orders__user_id__48CFD27E");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.OrderDetailId).HasColumnName("order_detail_id");

                entity.Property(e => e.MenuId).HasColumnName("menu_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK__OrderDeta__menu___4D94879B");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__order__4CA06362");
            });

            modelBuilder.Entity<ProductIngredient>(entity =>
            {
                entity.HasKey(e => e.IngredientId)
                    .HasName("PK__ProductI__B0E453CFE4C1F545");

                entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.MenuId).HasColumnName("menu_id");

                entity.Property(e => e.QuantityPerProduct).HasColumnName("quantity_per_product");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ProductIngredients)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK__ProductIn__item___534D60F1");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.ProductIngredients)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK__ProductIn__menu___52593CB8");
            });

            modelBuilder.Entity<ProductPointRedemption>(entity =>
            {
                entity.HasKey(e => e.MenuId)
                    .HasName("PK__ProductP__4CA0FADC1ACD5F6A");

                entity.ToTable("ProductPointRedemption");

                entity.Property(e => e.MenuId)
                    .ValueGeneratedNever()
                    .HasColumnName("menu_id");

                entity.Property(e => e.RequiredPoints).HasColumnName("required_points");

                entity.HasOne(d => d.Menu)
                    .WithOne(p => p.ProductPointRedemption)
                    .HasForeignKey<ProductPointRedemption>(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductPo__menu___440B1D61");
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.Property(e => e.TableId).HasColumnName("table_id");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.Location)
                    .HasMaxLength(100)
                    .HasColumnName("location");

                entity.Property(e => e.ReservationTime)
                    .HasColumnType("datetime")
                    .HasColumnName("reservation_time");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164CAC9761F")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "UQ__Users__F3DBC57217704C53")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .HasColumnName("full_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .HasColumnName("phone");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
