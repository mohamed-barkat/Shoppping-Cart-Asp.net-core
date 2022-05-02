using Shop.Domain.Models;
using Shop.Domin.Models.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domin.Models.Categories
{
    public class Category:BaseClass<int>
    {
        public Category()
        {
            CreatedAt = DateTime.Now;
            IsUpdated=false;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsUpdated { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
