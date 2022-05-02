using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domin.Models.Orders
{
    public enum OrderState
    {
        Pending = 0,
        NotPaid = 1,
        Completed = 2,
        InCargo = 3
    }
}
