using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using Shop.Domin.Models.Claims;
using Shop.UI.Paginated;
using Shop.UI.ViewModel.Address;
using Shop.UI.ViewModel.Claims;
using Shop.UI.ViewModel.Roles;
using Shop.UI.ViewModel.Users;
using System.Security.Claims;

namespace Shop.UI.Controllers.Security
{
  [Authorize(Policy = "Admin-Panel")]
    public class AdministrationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly INotyfService _notyf;
        private readonly IAddressRepository _addressRepo;
        private readonly ICartRepository _cartRepository;
        private readonly ApplicationDbContext _context;
        public AdministrationController(UserManager<ApplicationUser> userManger,INotyfService notyf,RoleManager<IdentityRole> roleManager,IAddressRepository addressRepo,ICartRepository cartRepository,ApplicationDbContext context)
        {
            _userManger = userManger;
            _roleManger = roleManager;
            _notyf = notyf;
            _addressRepo = addressRepo;
            _cartRepository = cartRepository;
            _context = context; 
        }
        [HttpGet]
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
            var users=_userManger.Users;

            var model = new List<UsersViewModel>();
            foreach(var user in users)
            {
                model.Add(new UsersViewModel {
                UserName = user.UserName,
                Email = user.Email,
                CraetedAt=user.CreatedAt.ToString(),
                Id = user.Id,
                Roles= await _userManger.GetRolesAsync(user),
                Claims= (List<System.Security.Claims.Claim>)await _userManger.GetClaimsAsync(user)
                });
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.UserName.Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.UserName).ToList();
                    break;
                case "Date":
                    model = model.OrderBy(s => s.CraetedAt).ToList();
                    break;
                case "date_desc":
                    model = model.OrderByDescending(s => s.CraetedAt).ToList();
                    break;
                default:
                    model = model.OrderBy(s => s.UserName).ToList();
                    break;
            }
            int pageSize = 7;
            return View(PaginatedList<UsersViewModel>.CreateAsync(model, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult CreateNewUser()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewUser(UsersViewModel model)
        {
            if(ModelState.IsValid)
            {

                var user = new ApplicationUser
                {

                    UserName = model.UserName,
                    CreatedAt = DateTime.Now,
                    Email = model.Email,
                    EmailConfirmed = true

                };
              var result=  await _userManger.CreateAsync(user
             ,model.Password);
                if (result.Succeeded)
                {
                    var cart = new Cart();
                    cart.UserId = user.Id;
                    await _cartRepository.AddAsync(cart);
                    _notyf.Success($"{model.UserName} user Created");
                    return RedirectToAction("Index", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    
                }
                _notyf.Error("Error");
                return View(model);
            }
            else
            {
                _notyf.Error("Validation Error");
                return View(model);
            }


        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {

            var roles = _roleManger.Roles;
            var model = new List<RoleViewModel>();

            foreach (var role in roles)
            {

                model.Add(new RoleViewModel
                {
                    RoleName = role.Name,

                    Id = role.Id,

                });

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateNewRole()
        {
            return View();
           
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewRole(RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
         var result= await   _roleManger.CreateAsync(new IdentityRole
                {
                    Name=model.RoleName,
                    
                });

                if(result.Succeeded)
                {
                    _notyf.Success($"{model.RoleName} is Created");
                    return RedirectToAction("ListRoles");

                }
                  foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    
                }
                _notyf.Error("Error");
                return View(model);

            }
            _notyf.Error("Error");
            return View(model);

        }
        [AcceptVerbs("Post", "Get")]
        [AllowAnonymous]
        public async Task<IActionResult> IsRoleInUser(string Name)
        {
            var role = await _roleManger.FindByNameAsync(Name);
            if (role == null)
            {
                return Json(true);
            }
            else
            {
                return Json($" {role} is allready in use");
            }

        }


        [HttpPost]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManger.FindByIdAsync(Id);
                if(role == null)
                {
                _notyf.Warning("role Cant be Found");
                return RedirectToAction("ListRoles");
               }
              else{
                if(role.Name=="Admin"||role.Name=="SuperAdmin"||role.Name=="Users")
                {
                    _notyf.Error($"you cant delete  {role.Name} role");
                    return RedirectToAction("ListRoles");
                }
              await  _roleManger.DeleteAsync(role);
                _notyf.Warning("Role is deleted");
                return RedirectToAction("ListRoles");
 
              }
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await _roleManger.FindByIdAsync(Id);
            var model = new RoleViewModel
            {
                RoleName = role.Name,
               
            };
            if(model.RoleName == "Admin" || model.RoleName == "Users" || model.RoleName == "SuperAdmin")
            {
                _notyf.Warning("Warning",5);
                return RedirectToAction("ListRoles");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
              if(model.RoleName=="Admin"||model.RoleName=="Users"||model.RoleName=="SuperAdmin")
                {
                    _notyf.Error($"you can't Edit {model.RoleName} role");
                    return View(model);
                }
                else
                {
                    var role = await _roleManger.FindByIdAsync(model.Id);
                    role.Name = model.RoleName;
                   var result= await _roleManger.UpdateAsync(role);

                    if(result.Succeeded)
                    {
                        _notyf.Success("Role is Updated");
                        return RedirectToAction("ListRoles");
                    }
                    foreach( var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        
                    }
                    return View(model);
                }

            }
            _notyf.Error("Error");
            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user= await _userManger.FindByIdAsync(Id);
            if(user==null)
            {
                _notyf.Error("User Can't be Found");
                return RedirectToAction("index");
            }
            else
            {
                if(user.UserName=="Mohamed"||user.UserName=="Admin"||await _userManger.IsInRoleAsync(user,"Admin")||await _userManger.IsInRoleAsync(user,"Super-Admin"))
                {
                    _notyf.Warning($"you cant delete {user.UserName} user");
                    return RedirectToAction("index");

                }
                else
                {
                   await _userManger.DeleteAsync(user);
                    _notyf.Warning("deleted");
                    return RedirectToAction("index");

                }
            }

        }
        [HttpGet]
        public async  Task<IActionResult> EditUser(string userId)
        {
            var user=await _userManger.FindByIdAsync(userId);
            if(user==null)
            {
                _notyf.Error("Error");
                return RedirectToAction("Index");
            }
            else
            {
                var model = new EditUserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Id = user.Id,
                    Roles = await _userManger.GetRolesAsync(user),
                    Claims = (List<System.Security.Claims.Claim>)await _userManger.GetClaimsAsync(user),
                    IsEmailConfirmed=user.EmailConfirmed,
                    

                };
                return View(model);
            }
          
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManger.FindByIdAsync(model.Id);
            
            if(user==null)
            {
                _notyf.Error("Error");
                return View(model);
            }
            else
            {
                if(ModelState.IsValid)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNummber;
                  
                    if ((bool)model.IsEmailConfirmed)
                    {
                        user.EmailConfirmed = true;
                    }
                    else
                    {
                        user.EmailConfirmed = false;
                    }
                   if (model.Password!=null)
                    {
                        user.PasswordHash = _userManger.PasswordHasher.HashPassword(user, model.Password);
                    }

                   
                    var result=   await _userManger.UpdateAsync(user);
                  if(result.Succeeded)
                    {
                        _notyf.Success("User Is Updated");
                        return RedirectToAction("Index");
                    }
                  foreach( var error in result.Errors )
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                    _notyf.Error("Error");
                  return View(model);
                }
                else
                {
                    _notyf.Error("Error");
                    return View(model);
                }

            }

        }

        [HttpGet]
        public async Task<IActionResult> EditRolesInUser(string userId)
        {
            ViewBag.UserId = userId;
            var user= await _userManger.FindByIdAsync(userId);
            if(user==null)
            {
                _notyf.Error("Error");
                return RedirectToAction("Index");
            }
            var model = new List<EditRolesInUserViewModel>() { };
            foreach (var role in _roleManger.Roles)
            {
                var userRolesViewModel = new EditRolesInUserViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                if (await _userManger.IsInRoleAsync(user, role.Name))
                { userRolesViewModel.IsSelected = true; }
                else { userRolesViewModel.IsSelected = false; }
                model.Add(userRolesViewModel);

            }
            return View(model);
        }


        public async Task<IActionResult> EditRolesInUser(string userId,List<EditRolesInUserViewModel> model)
        {
            var user = await _userManger.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await _userManger.GetRolesAsync(user);
            var result = await _userManger.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManger.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            _notyf.Success("Done");

            return RedirectToAction("EditUser", new { userId = userId });



        }

        public async Task<IActionResult> EditUserClaims(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManger.FindByIdAsync(userId); ;
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id {userId} cannt be found";
                return View("NotFound");
            }
            else
            {
                var existingUserClaims = await _userManger.GetClaimsAsync(user);
                var model = new UserClaimsViewModel
                {
                    userId = userId,
                };

                foreach (var claim in ClaimsStore.AllClaims)
                {
                    var userclaim = new UserClaim
                    {
                        ClaimType = claim.Type,
                    };
                    if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                    {
                        userclaim.IsSelected = true;
                    }
                    model.Claims.Add(userclaim);
                }
                return View(model);

            }
               
        }
        [HttpPost]
        public async Task<IActionResult> EditUserClaims(UserClaimsViewModel model)
        {

            var user = await _userManger.FindByIdAsync(model.userId);

            if (user == null)
            {
                _notyf.Error("Error");
                return View(model);
            }

            // Get all the user existing claims and delete them
            var claims = await _userManger.GetClaimsAsync(user);
            var result = await _userManger.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }
            IEnumerable<Claim> cl = model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false"));
            // Add all the claims that are selected on the UI
            result = await _userManger.AddClaimsAsync(user, cl);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                _notyf.Error("Error");
                return View(model);
            }
            _notyf.Success("Done");

            return RedirectToAction("editUser", new { userId = model.userId });


        }

        [HttpGet]
        public async Task<IActionResult> DetailsUser(string userId)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if(user == null)
            {
                _notyf.Error("Error User Cannt be Found");
                return RedirectToAction("Index");
            }

            var add= await _addressRepo.GetAddressForUser(user);
            var address = new AddressViewModel();
            if (add != null)
            {
               address = new AddressViewModel
                {
                    UserId = userId,
                    City =add.City,
                    Country = add.Country,
                    PostalCode = add.PostalCode,
                    Street = add.Street,
                    
                };
            }
            else
            {
                address = null;
            }



            int cartId = 0;
            var usercart =  _context.Users.Include(u => u.Cart).Where(s => s.Id == userId).Select(u=>u.Cart).SingleOrDefault();
            if(usercart != null)
            {
                cartId = usercart.Id;
            }
           
            var model = new DetailsUserViewModel()
            {
                Id = user.Id,
                address = address,
                Claims = (List<Claim>)await _userManger.GetClaimsAsync(user),
                Roles = await _userManger.GetRolesAsync(user),
                Email = user.Email,
                UserName = user.UserName,
                IsEmailConfirmed = user.EmailConfirmed,
                CraetedAt = user.CreatedAt.ToString(),
                CartId = cartId 
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditAddressForUser(string userId)
        {
            ViewBag.userId = userId;
           var user = await _userManger.FindByIdAsync(userId);
            var address=await _addressRepo.GetAddressForUser(user);
            var model = new AddressViewModel();
            if (address == null)
            {
                model = new AddressViewModel()
                {
                    UserId=userId,
                };
            }
            else
            {
                model = new AddressViewModel()
                {
                    AddressId = user.Address.AddressId,
                    City = user.Address.City,
                    Country = user.Address.Country,
                    PostalCode = user.Address.PostalCode,
                    Street = user.Address.Street,
                    UserId = user.Id,
                };
            }
        


            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditAddressForUser(AddressViewModel model,string userId )
        {
            var user = await _userManger.FindByIdAsync(userId);
            var address=await _addressRepo.GetAddressForUser(user);
            var Address = new Address()
            {
                City = model.City,
                Country = model.Country,
                PostalCode = model.PostalCode,
                Street = model.Street,
                 User = user,
            };
            if (address == null)
            {
                
               
               var a= await _addressRepo.AddAsync(Address);
                _notyf.Success("Added");
                return RedirectToAction("EditUser", new { userId=userId});
            }
            else
            {
               var b=await _addressRepo.UpdateAsync(Address);
                _notyf.Success("Updated");
                return RedirectToAction("EditUser", new { userId = userId });
              
            }
          
           
        }
    }
}
