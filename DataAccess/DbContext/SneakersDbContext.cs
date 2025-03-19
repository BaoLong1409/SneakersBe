using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DataAccess.SeedData;

namespace DataAccess.DbContext
{
    public class SneakersDbContext : IdentityDbContext<User, Role, Guid>
    {
        public SneakersDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Cart> Cart { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistory { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Interaction> Interaction { get; set; }
        public DbSet<FeatureProducts> FeatureProducts { get; set; }
        public DbSet<ProductTranslation> ProductTranslation { get; set; }
        public DbSet<ProductCart> ProductCart { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductQuantity> ProductQuantity { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<ShippingInfor> ShippingInfor { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()) {
                    if (property.ClrType == typeof(Guid) && property.IsPrimaryKey())
                    {
                        property.SetDefaultValueSql("NewID()");
                    } 
                }
            }

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.OrderDetail)
                .WithMany()
                .HasForeignKey(c => c.OrderDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Rating>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Rating>()
                .HasOne(c => c.OrderDetail)
                .WithMany()
                .HasForeignKey(c => c.OrderDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Role");
            });

            builder.Entity<Payment>().HasData(
                new Payment { Id = SeedData.SeedData.CODPaymentId, Name = "COD" },
                new Payment { Id = SeedData.SeedData.VNPayPaymentId, Name = "VNPay" }
            );

            builder.Entity<Shipping>().HasData(
                new Shipping { Id = SeedData.SeedData.StandardShippingId, Name = "Standard", Price = 0.61m, MinimumDeliveredTime = 5, MaximumDeliveredTime = 7 },
                new Shipping { Id = SeedData.SeedData.ExpressShippingId, Name = "Express", Price = 0.90m, MinimumDeliveredTime = 3, MaximumDeliveredTime = 5 },
                new Shipping { Id = SeedData.SeedData.UltraFastShippingId, Name = "Ultra-Fast Delivery", Price = 1.63m, MinimumDeliveredTime = -12, MaximumDeliveredTime = -24 }
            );


            base.OnModelCreating(builder);
        }

    }
}
