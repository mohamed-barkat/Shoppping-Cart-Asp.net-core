using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using Shop.Domin.Models.Categories;
using Shop.Domin.Models.Orders;
using Shop.Domin.Models.Products;
using Shop.Domin.Models.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.DataContext
{
    public static class ModelBuilderExt
    {
       
        public static void Seed(this ModelBuilder modelBuilder)
        {
        
          

        }
    }
}
