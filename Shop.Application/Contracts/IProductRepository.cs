using Shop.Domin.Models.Categories;
using Shop.Domin.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contracts
{
    public interface IProductRepository:IAsyncRepository<Product>
    {

       Task<List<Product>> GetProductsCategory(Category category);

       Task<Product> UpdateAsyncProductWithCategory(Product product,List<Category> categories);

       
    }
}
