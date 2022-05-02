using Shop.Domain.Models;
using Shop.Domin.Models.Categories;
using Shop.Domin.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domin.Models.Relationships
{
    public class ProductCategory:BaseClass<int>
    {
       
        public int CategoryId { get; set; }

        public Category Category { get; set; }


        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
