using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.DataContext
{
    public class SeedIdentityUsersAndRoles
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManger = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManger = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var cartRep = serviceProvider.GetRequiredService<ICartRepository>();
            context.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
                await roleManger.CreateAsync(new IdentityRole
                {
                    Name = "Super-Admin"
                });

                await roleManger.CreateAsync(new IdentityRole
                {
                    Name = "Admin"

                });
            }

            if (!context.Users.Any())
            {
                ApplicationUser user = new()
                {
                    UserName = "mohamed",
                    Email = "Mohamed@gmail.com",
                    CreatedAt = DateTime.Now,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),

                };

                await userManger.CreateAsync(user, "1234");
                await userManger.AddToRoleAsync(user, "Super-Admin");
                await userManger.AddToRoleAsync(user, "Admin");
                var cart = new Cart();
                cart.UserId = user.Id;
                await context.Carts.AddAsync(cart);
                await context.SaveChangesAsync();
            }

        }

    }
}
