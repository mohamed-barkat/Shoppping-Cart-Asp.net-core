using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contracts
{
    public interface IAddressRepository : IAsyncRepository<Address>
    {
        Task<Address> GetAddressForUser(ApplicationUser user);


    }
}
