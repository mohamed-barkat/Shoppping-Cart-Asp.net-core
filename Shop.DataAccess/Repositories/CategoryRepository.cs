using Microsoft.EntityFrameworkCore;
using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domin.Models.Categories;
using Shop.Domin.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<List<Category>> GetCategoriesForProduct(Product product)
        {

            var productId = product.Id;
            var categories = await _context.Categories.Where(a => a.ProductCategories.Any(a => a.ProductId == productId)).ToListAsync();
            return categories;
        }

        public int GetProductsCountInCatergory(Category category)
        {
            int count = _context.Products.Where(a => a.ProductCategories.Where(c => c.CategoryId == category.Id).Count() > 0).Count();
            return count;
        }
        public async Task<List<string>> GetCategoriesNamesforProduct(Product product)
        {
            List<string> categories = await _context.Categories.Where(c => c.ProductCategories.Any(p => p.ProductId == product.Id)).Select(c => c.Name).ToListAsync();
            return categories;
        }

        public async Task<List<string>> GetAllCategoriesNames()
        {
            List<string> categories = await _context.Categories.Select(c => c.Name).ToListAsync();
            return categories;
        }

        public async Task<List<Category>> GetCategoriesForSelect(List<int> selectcategories)
        {
            var categories = new List<Category>();
            for (int i = 0; i < selectcategories.Count; i++)
            {
                var category = await GetByIdAsync(selectcategories[i]);

                categories.Add(category);

            }
            return categories;
        }
    }
}
