using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domin.Models.Carts
{
    public class Cart : BaseClass <int>
    {
        public string? UserId { get; set; }
       public   ApplicationUser User { get; set; }

        //Navigation Property
        public List<CartItem>? CartItems { get; set; }
    }
}
