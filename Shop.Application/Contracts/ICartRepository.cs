using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contracts
{
    public interface ICartRepository:IAsyncRepository<Cart>
    {

        public Task<int> GetCartIdForUser(ApplicationUser user);
    }
}
