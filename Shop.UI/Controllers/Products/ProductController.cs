using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shop.Application.Contracts;
using Shop.Application.Contracts.Notifications;
using Shop.Domain.Models;
using Shop.Domin.Models.Categories;
using Shop.Domin.Models.Entities;
using Shop.Domin.Models.Notifications;
using Shop.Domin.Models.Products;
using Shop.Domin.Models.Relationships;
using Shop.UI.Hups;
using Shop.UI.Models;
using Shop.UI.Paginated;
using Shop.UI.ViewModel.Categories;
using Shop.UI.ViewModel.Products;
using System.Text.Json;
using System.Text.Json.Serialization;
using Notification = Shop.Domin.Models.Notifications.Notification;

namespace Shop.UI.Controllers.Products
{
    [Authorize(Policy = "Admin-Panel")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly INotyfService _notyf;

        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly SignInManager<ApplicationUser> _signInManger;
        private readonly INotification _notificationManger;
        private readonly IHubContext<NotificationsHup> _hubContext;
        public ProductController(IHubContext<NotificationsHup> hubContext, INotification notificationManger,
        RoleManager<IdentityRole> roleManger,
            UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signInManager, IProductRepository productRepo, ICategoryRepository categoryRepo, INotyfService notyf, IWebHostEnvironment hosting)
        {
            _hubContext = hubContext;
            _roleManger = roleManger;
            _signInManger = signInManager;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _notyf = notyf;
            _hosting = hosting;
            _userManger = userManger;

            _notificationManger = notificationManger;
        }

        public async Task<IActionResult> Index(string sortOrder,
        string currentFilter,
        string searchString,
         int? pageNumber,
         int selectCategory)
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

            var products = await _productRepo.GetAllAsync();
            var model = new List<ProductsViewModel>();
            foreach (var product in products)
            {
                model.Add(new ProductsViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    IsEdited = product.IsUpdated ? false : true,
                    Price = product.Price,
                    CreatedAt = product.CreatedAt.ToString(),
                    CategoriesNames = await _categoryRepo.GetCategoriesNamesforProduct(product),
                });
            }


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(m => m.Name.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Date":
                    model = model.OrderBy(s => s.CreatedAt).ToList();
                    break;
                case "date_desc":
                    model = model.OrderByDescending(s => s.CreatedAt).ToList();
                    break;
                default:
                    model = model.OrderBy(s => s.Name).ToList();
                    break;
            }



            int pageSize = 5;
            return View(PaginatedList<ProductsViewModel>.CreateAsync(model, pageNumber ?? 1, pageSize));
        }



        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model, List<int> selectCategory)
        {
            if (!ModelState.IsValid)
            {
                _notyf.Error("Error");
                return View(model);
            }
            string uniqname = UploadeFiles(model.Files, string.Empty);
            var listProductCategories = new List<ProductCategory>();
            if (selectCategory.Count > 0)
            {
                for (int i = 0; i < selectCategory.Count; i++)
                {
                    listProductCategories.Add(new ProductCategory { CategoryId = selectCategory[i] });
                }
            }
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CreatedAt = DateTime.Now,
                IsUpdated = false,
                ImageUrl = uniqname,
                ProductCategories = listProductCategories
            };

            var Product = await _productRepo.AddAsync(product);
            var userss = await _userManger.GetUsersInRoleAsync("Admin");


            if (userss != null)
            {

                foreach (var user in userss)
                {
                    var not = new Notification
                    {
                        ActorId = await GetCurrentUserId(),
                        EntityTypeId = EntityType.Product,
                        EntityId = Product.Id.ToString(),
                        IsReaded = false,
                        ReadTime = null,
                        UserNotifications = new List<UserNotifications>
                        {
                            new UserNotifications
                            {
                                NotifierId = user.Id,
                            }
                        },
                        RedierectUrl = "ffdsf",
                        Status = 1,
                        Body = $" {User.Identity.Name}  Create New Product",
                        Title = "Creat Product",




                    };

                    await _notificationManger.AddAsync(not);
                    await _hubContext.Clients.User(user.Id).SendAsync("ReceiveNotification", "notfi");
                }
            }



            _notyf.Success("Done");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<CategoriyListPagViewModel> GetCategoriesPag(int? pageNumber, string term)
        {
            var categories = await _categoryRepo.GetAllAsync();



            var model = new List<CategoryListViewModel>();
            foreach (var category in categories)
            {
                model.Add(new CategoryListViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                });
            }
            var pageSize = 4;

            if (!String.IsNullOrEmpty(term))
            {
                model = model.Where(s => s.Name.Contains(term)).ToList();
            }

            PaginatedList<CategoryListViewModel> vm = PaginatedList<CategoryListViewModel>.CreateAsync(model, pageNumber ?? 1, pageSize);

            var catlipagvm = new CategoriyListPagViewModel()
            {
                categories = vm,
                HasNextPage = vm.HasNextPage,
                HasPreviousPage = vm.HasPreviousPage,
                PageIndex = vm.PageIndex,


            };

            return catlipagvm;
        }


        public async Task<int> GetCategoriesCount()
        {

            var categories = await _categoryRepo.GetAllAsync();

            return categories.Count;
        }

        private string UploadeFiles(List<IFormFile> files, string Photopath)
        {
            var uniqName = string.Empty;
            if (Photopath != string.Empty && files == null)
            {

                return Photopath;
            }
            else
            {
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        string uploads = Path.Combine(_hosting.WebRootPath, "ProductsImages");
                        string filename = file.FileName;
                        uniqName = Guid.NewGuid().ToString() + "_" + filename;
                        string fullpath = Path.Combine(uploads, uniqName);
                        using var filestream = new FileStream(fullpath, FileMode.Create);
                        file.CopyTo(filestream);


                    }
                    return uniqName;

                }
                else
                    return "No-Image.jpg";
            }


        }


        [HttpPost]

        public async Task<IActionResult> Delete(int id, int pageIndex)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
            {
                _notyf.Error("NOT FOUND");
                return NotFound();
            }
            await _productRepo.DeleteAsync(product);
            _notyf.Warning("Done");
            return RedirectToAction("Index", new { pageNumber = pageIndex });

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int pageIndex)
        {

            var Product = await _productRepo.GetByIdAsync(id);
            ViewBag.PageIndex = pageIndex;
            var model = new EditProductViewModel()
            {
                Id = id,
                Name = Product.Name,
                Price = Product.Price,
                ImageUrl = Product.ImageUrl,
                Description = Product.Description,


            };




            return View(model);
        }


        public async Task<IActionResult> Edit(EditProductViewModel model, List<int> selectCategory, int pageIndex)
        {
            if (!ModelState.IsValid)
            {
                _notyf.Error("Error");
                return View(model);
            }

            var product = await _productRepo.GetByIdAsync(model.Id);

            if (product == null)
            {
                _notyf.Error("Error");
                return NotFound();
            }
            var uniqname = UploadeFiles(model.Files, model.ImageUrl);


            if (model.ImageUrl != null && model.ImageUrl != "No-Image.jpg" && model.Files != null)
            {
                var oldpath = Path.Combine(_hosting.WebRootPath, "ProductsImages", model.ImageUrl);
                System.IO.File.Delete(oldpath);
            }


            product.Description = model.Description;
            product.ImageUrl = model.ImageUrl;
            product.Name = model.Name;
            product.Price = model.Price;
            product.IsUpdated = true;
            product.ImageUrl = uniqname;

            var categories = new List<Category>();

            if (selectCategory.Count > 0 && selectCategory != null)
            {

                categories = await _categoryRepo.GetCategoriesForSelect(selectCategory);

                await _productRepo.UpdateAsyncProductWithCategory(product, categories);

                _notyf.Success("Updated");
                return RedirectToAction("Index", new { pageNumber = pageIndex });
            }

            await _productRepo.UpdateAsyncProductWithCategory(product, categories);

            _notyf.Success("updated");
            return RedirectToAction("Index");

        }



        public async Task<EditCategoriesInProduct> GetAllCategoriesForEdit(int id, int? pageNumber, string term)
        {
            var product = await _productRepo.GetByIdAsync(id);
            var allCategories = await _categoryRepo.GetAllAsync();

            var vm = new List<CategoryListViewModel>();
            var categoriesforProduct = await _categoryRepo.GetCategoriesForProduct(product);

            for (int i = 0; i < allCategories.Count; i++)
            {
                vm.Add(new CategoryListViewModel
                {
                    Name = allCategories[i].Name,
                    Id = allCategories[i].Id,
                });

                for (int j = 0; j < categoriesforProduct.Count; j++)
                {
                    if (vm[i].Id == categoriesforProduct[j].Id)
                    {
                        vm[i].IsSelected = true;
                    }
                }

            }

            int pageSize = 3;
            if (!String.IsNullOrEmpty(term))
            {
                vm = vm.Where(s => s.Name.Contains(term)).ToList();
            }

            var model = PaginatedList<CategoryListViewModel>.CreateAsync(vm, pageNumber ?? 1, pageSize);
            var EditCatInProduct = new EditCategoriesInProduct()
            {

                categories = model,
                HasNextPage = model.HasNextPage,
                HasPreviousPage = model.HasPreviousPage,
                PageIndex = model.PageIndex,
            };

            return EditCatInProduct;

        }

        public IActionResult MCreate()
        {



            return View();
        }

        public async Task<string> GetCurrentUserId()
        {
            var user = await _userManger.GetUserAsync(User);
            if (user == null)
            {
                return string.Empty;
            }
            return user.Id;

        }


    }


}
