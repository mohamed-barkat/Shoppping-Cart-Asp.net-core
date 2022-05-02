using Microsoft.EntityFrameworkCore;
using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domin.Models.Categories;
using Shop.Domin.Models.Products;
using Shop.Domin.Models.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repositories
{
    public class ProductRepository :BaseRepository<Product>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext context):base(context)
        {

        }

      
        public async Task<List<Product>> GetProductsCategory(Category category)
        {
            var products = await _context.Products.Where(p => p.ProductCategories.Any(pd=>pd.CategoryId==category.Id)).ToListAsync() ;
            return products ;
        }

        public async Task<Product> UpdateAsyncProductWithCategory(Product product,List<Category> categories)
        {
            product =_context.Products.Include(p => p.ProductCategories).Where(p=>p.Id==product.Id).Single();

            if (categories.Count()>0&&categories!=null)
            {
                product.ProductCategories.Clear();
                for (int i = 0; i < categories.Count; i++)
                {
                product.ProductCategories.Add(
                        new ProductCategory
                         {
                            CategoryId = categories[i].Id,
                            Product = product,
                            Category = categories[i],
                            ProductId = product.Id

                         }

                        );
                }
            }
            else
            {
                product.ProductCategories.Clear();
            }
            await UpdateAsync(product);
            return product ;
        }
    }
}
