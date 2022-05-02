using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Contracts;
using Shop.Domain.Models;
using Shop.Domin.Models.Categories;
using Shop.UI.Paginated;
using Shop.UI.ViewModel.Categories;

namespace Shop.UI.Controllers.Categories
{
    [Authorize(Policy = "Admin-Panel")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _category;
        private readonly INotyfService _notyf;
        public CategoryController(ICategoryRepository category,INotyfService notyf)
        {
           _notyf = notyf;  
            _category = category;
        }


        public async Task<IActionResult> Index(string sortOrder,
    string currentFilter,
    string searchString,
    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var categories = await _category.GetAllAsync();
            var model = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                model.Add(new CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ProductCount = _category.GetProductsCountInCatergory(category),
                    CraetedAt = category.CreatedAt.ToString(),
                });
            }


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s =>s.Name.Contains(searchString)   ).ToList();
                       }              
            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Date":
                    model = model.OrderBy(s => s.CraetedAt).ToList();
                    break;
                case "date_desc":
                    model = model.OrderByDescending(s => s.CraetedAt).ToList();
                    break;
                default:
                    model = model.OrderBy(s => s.Name).ToList();
                    break;
            }

           

            int pageSize = 8;
            return View( PaginatedList<CategoryViewModel>.CreateAsync(model, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if(!ModelState.IsValid)
            {
                _notyf.Error("Error");
                return View(model);
            }
            await _category.AddAsync(new Category
            {
                Name = model.Name,
                Description = model.Description,
                CreatedAt = DateTime.Now
            }); ;
            _notyf.Success($" {model.Name} Added "); 
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
         var model=  await _category.GetByIdAsync(Id);
            if(model == null) 
            {
                ViewBag.ErrorTitle = "Can't Found";
                ViewBag.ErrorMessage = $"{model.Name} Category Not be Found ";
                return View("Error");
            }
            else
            {
               await _category.DeleteAsync(model);
                _notyf.Error("Deleted");
           return RedirectToAction("index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var category=await _category.GetByIdAsync(Id);
            var model = new CategoryViewModel
            {
                Description = category.Description,
                Name = category.Name,
                Id = Id
            };
            if(model!=null)
            {
                return View(model);
            }
            else
            {
                ViewBag.ErrorTitle = "Can't Found";
                ViewBag.ErrorMessage = $"{model.Name} Category Not be Found ";
                return View("Error");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if(ModelState.IsValid)
            {
                var category = await _category.GetByIdAsync(model.Id);
                category.Description = model.Description;
                category.Name = model.Name;
                category.IsUpdated = true;
                category.UpdatedAt = DateTime.Now;
                await _category.UpdateAsync(category);
                _notyf.Information("Updated");
             return RedirectToAction("index");

            }
            else
            {
                _notyf.Error("Error");
                return View(model);

            }


        }

        [HttpGet]
        public async Task<IActionResult> Show(int Id)
        {
            var category= await _category.GetByIdAsync(Id);
            if(category!=null)
            {
                var model = new CategoryViewModel()
                {
                    Description = category.Description,
                    CraetedAt = category.CreatedAt.ToString(),
                    Name = category.Name,
                    IsUpdated = category.IsUpdated,
                    UpdatedAt = category.UpdatedAt.ToString(),
                    ProductCount = _category.GetProductsCountInCatergory(category),
                    Id = Id
                };
                return View(model);
            }
            else
            {
                ViewBag.ErrorTitle = "Can't Found";
                ViewBag.ErrorMessage = $"{category.Name} Category Not be Found ";
                return View("Error");
                
            }
          
    

        }
    }
}
