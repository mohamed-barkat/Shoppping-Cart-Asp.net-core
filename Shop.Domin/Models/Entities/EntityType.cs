using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domin.Models.Entities
{
    [Flags]
    public enum EntityType
    {
        User = 1,
        Cart = 2,
        CartItem = 3,
        Category = 4,
        Order = 5,
        OrderItem = 6,
        Product = 7,
        Address = 8,

    }
}
