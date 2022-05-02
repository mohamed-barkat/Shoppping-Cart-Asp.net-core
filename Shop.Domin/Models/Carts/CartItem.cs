
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domin.Models.Products;

namespace Shop.Domin.Models.Carts
{
    public class CartItem:BaseClass<int>
    {
     
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        //Navigation Property
     
        public int? CartId  { get; set; }
        public Cart? Cart { get; set; }
        public int? Quantity { get; set; }
    }
}
