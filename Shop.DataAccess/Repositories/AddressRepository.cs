using Microsoft.EntityFrameworkCore;
using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repositories
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {

        public AddressRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<Address> GetAddressForUser(ApplicationUser user)
        {







            Address address = await _context.Addresses.SingleOrDefaultAsync(x => x.User == user);


            return address;
        }
    }
}
