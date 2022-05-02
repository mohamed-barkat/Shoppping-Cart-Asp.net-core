using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using Shop.Domin.Models.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domin.Models.Products
{
    public class Product:BaseClass<int>
    {
        public Product()
        {
            CreatedAt = DateTime.Now;
            IsUpdated=false;
            if(IsUpdated==true)
            {
                LastUpdatedAt = DateTime.Now;
            }
        }
        public string Name { get; set; }

      
        public string Description { get; set; }

    
        public string ImageUrl { get; set; }

        public  DateTime CreatedAt { get; set; }  
        public DateTime ? LastUpdatedAt { get; set; }
        public bool IsUpdated { get; set; }
        public decimal? Price { get; set; }
        public   List<CartItem> CartItems;
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
