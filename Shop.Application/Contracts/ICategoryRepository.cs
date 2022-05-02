using Shop.Domin.Models.Categories;
using Shop.Domin.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contracts
{
    public interface ICategoryRepository:IAsyncRepository<Category>
    {

        public Task<List<Category>> GetCategoriesForProduct(Product product);
        public int GetProductsCountInCatergory(Category category);
        Task<List<string>> GetCategoriesNamesforProduct(Product product);
        Task<List<string>> GetAllCategoriesNames();
        Task<List<Category>> GetCategoriesForSelect(List<int> selectcategories);

        
    }
}
