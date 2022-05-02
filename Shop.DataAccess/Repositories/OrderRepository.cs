using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domin.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repositories
{
    public class OrderRepository : BaseRepository<Order>,IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context):base(context)
        {
                
        }

    }
}
