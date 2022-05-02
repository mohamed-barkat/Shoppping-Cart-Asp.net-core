using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Shop.DataAccess.DataContext;
using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using Shop.Domin.Models.Categories;
using Shop.Domin.Models.Notifications;
using Shop.Domin.Models.Orders;
using Shop.Domin.Models.Products;
using Shop.Domin.Models.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Data.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Address).WithOne(a => a.User)
                .HasForeignKey<ApplicationUser>(u => u.AddressId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ApplicationUser>()
            .HasMany(a => a.Orders)
              .WithOne(o => o.User);
            modelBuilder.Entity<CartItem>()
            .HasOne(cr => cr.Product)
            .WithMany(p => p.CartItems)
           .HasForeignKey(cr => cr.ProductId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ct => ct.Cart)
                .WithMany(t => t.CartItems)
                .HasForeignKey(cr => cr.CartId);


            modelBuilder.Entity<Notification>().HasOne(x => x.Actor).WithMany(x => x.ActorNotifictions)
              .HasForeignKey(x => x.ActorId);


            modelBuilder.Entity<Notification>()
                .HasMany(x => x.UserNotifications)
                .WithOne(x => x.Notification)
                .HasForeignKey(x => x.NotificationId);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.UserNotifications)
                .WithOne(x => x.Notifier)
                .HasForeignKey(x => x.NotifierId);
            modelBuilder.Entity<Product>().HasMany(x => x.ProductCategories).WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);
            modelBuilder.Entity<Category>().HasMany(x => x.ProductCategories).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId);



            modelBuilder.Seed();

        }

        public DbSet<Address> Addresses { get; set; }

        internal object Entry<T>()
        {
            throw new NotImplementedException();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<UserNotifications> UserNotifications { get; set; }


    }
}
