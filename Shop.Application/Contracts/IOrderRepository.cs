using Shop.Domin.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contracts
{
    public interface IOrderRepository:IAsyncRepository<Order>
    {


    }
}
