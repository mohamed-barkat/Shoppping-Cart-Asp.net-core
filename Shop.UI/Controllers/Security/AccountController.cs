using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Contracts;
using Shop.Data.DataContext;
using Shop.Domain.Models;
using Shop.Domin.Models.Carts;
using Shop.UI.EmailServices;
using Shop.UI.SessionHelpers;
using Shop.UI.ViewModel;

namespace Shop.UI.Controllers.Security
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManger;
        private readonly IAddressRepository _addressRepository;
        private readonly IEmailService _emailservices;
        private readonly ICartRepository _cartRepository;

        public AccountController(UserManager<ApplicationUser> userManger
            , SignInManager<ApplicationUser> signInManger, IAddressRepository addressRepository,
            IEmailService emailservices, ICartRepository cartRepository)
        {
            _userManger = userManger;
            _signInManger = signInManger;
            _addressRepository = addressRepository;
            _emailservices = emailservices;
            _cartRepository = cartRepository;
        }
        [AcceptVerbs("post", "Get")]
        [AllowAnonymous]
        public async Task<IActionResult> IsUserInUse(string userName)
        {
            var user = await _userManger.FindByNameAsync(userName);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"userName {userName} is allready in use");
            }

        }
        [AcceptVerbs("Post", "Get")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string Email)
        {
            var user = await _userManger.FindByEmailAsync(Email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"userName {Email} is allready in use");
            }

        }

        //LogIn Actions
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await EmailOrName(model.UserName);
            if (user == null || !await _userManger.CheckPasswordAsync(user, model.Password))
            {
                ViewData["Message"] = "User Name Or Password is inCorrect";
                return View(model);
            }
            else if (!user.EmailConfirmed) { ModelState.AddModelError("", "Your Email Not Confirme yet"); return View(model); }
            else
            {
                var result = await _signInManger.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    Response.Cookies.Delete(".ShoppingCart.Session");
                    if (model.returnUrl == null) return RedirectToAction("Index", "Home");

                    return Redirect(model.returnUrl);

                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }

                ModelState.AddModelError("", "InValid Login attempt");
                return View(model);
            }


        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.Session.LoadAsync();
            //HttpContext.Session.Clear();
            HttpContext.Response.Cookies.Delete(".ShoppingCart.Session");
            await _signInManger.SignOutAsync();


            return RedirectToAction("index", "home");

        }
        //End----LogIn Actions


        //Register Actions
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {

                    UserName = model.UserName,
                    Email = model.Email,
                    //   Address=model.Address,
                };
                var result = await _userManger.CreateAsync(user, model.Password);

                var userId = user.Id;
                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("index", "Employee");
                    var token = await _userManger.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                          new { userId = user.Id, token = token }, Request.Scheme);
                    //logger.Log(LogLevel.Warning, confirmationLink);
                    var ToEmailAddress = new EmailAddress
                    {
                        Name = user.UserName,
                        Address = user.Email,
                    };
                    var FromeEmailAddress = new EmailAddress
                    {
                        Name = "Mohamed Barkat",
                        Address = "Mohamed@support.Zina",
                    };
                    List<EmailAddress> ToEmailAddresses = new();
                    ToEmailAddresses.Add(ToEmailAddress);
                    List<EmailAddress> FromeEmailAddresses = new();
                    FromeEmailAddresses.Add(FromeEmailAddress);

                    var emailmessage = new EmailMessage
                    {
                        Content = $"<a>{confirmationLink}</a>",
                        Subject = "Confirm Email Address in Zina shop",
                        ToAddresses = ToEmailAddresses,
                        FromAddresses = FromeEmailAddresses

                    };
                    await _emailservices.Send(emailmessage);



                    if (user.EmailConfirmed)
                    {
                        await _signInManger.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("index", "home");
                    }

                    ViewBag.ErrorTitle = "Registration successful";
                    ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                            "email, by clicking on the confirmation link we have emailed you";
                    return View("Error");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManger.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The UserID {userId} is invalid";
                return View("NotFound");
            }
            var result = await _userManger.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                var cart = new Cart();
                cart.UserId = userId;
                await _cartRepository.AddAsync(cart);

                await _signInManger.SignInAsync(user, false);

                return View();
            }
            ViewBag.ErrorTitle = "Email Cannot be confirmed";
            return View("Error");
        }





        public IActionResult ForgetPassword()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {


            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManger.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await _userManger.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await _userManger.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                    var ToEmailAddress = new EmailAddress
                    {
                        Name = user.UserName,
                        Address = user.Email,
                    };
                    var FromeEmailAddress = new EmailAddress
                    {
                        Name = "Mohamed Barkat",
                        Address = "Mohamed@support.Zina",
                    };
                    List<EmailAddress> ToEmailAddresses = new();
                    ToEmailAddresses.Add(ToEmailAddress);
                    List<EmailAddress> FromeEmailAddresses = new();
                    FromeEmailAddresses.Add(FromeEmailAddress);

                    var emailmessage = new EmailMessage
                    {
                        Content = $"<a>{passwordResetLink}</a>",
                        Subject = "Confirm Email Address in Zina shop",
                        ToAddresses = ToEmailAddresses,
                        FromAddresses = FromeEmailAddresses

                    };
                    await _emailservices.Send(emailmessage);



                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null)
            {
                ModelState.AddModelError(string.Empty, "Inavalid Password Reset Token");
            }

            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManger.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManger.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        if (await _userManger.IsLockedOutAsync(user))
                        {
                            await _userManger.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");

            }
            return View(model);

        }


        public async Task<ApplicationUser> EmailOrName(string user)
        {
            var userByName = await _userManger.FindByNameAsync(user);
            var userByEmail = await _userManger.FindByEmailAsync(user);
            if (userByName != null) { return userByName; }
            else if (userByEmail != null) { return userByEmail; }
            else { return null; }
        }

        public IActionResult AccessDenied()
        {

            return View();
        }

        public class response
        {
            public bool Success { get; set; }
            public string UserNameValid { get; set; }
        }
        [HttpPost]
        public async Task<response> isusernameinuse(string username)
        {

            var user = await _userManger.FindByNameAsync(username);
            response response = new response();
            var random = new Random();
            if (user == null)
            {
                response.Success = false;

            }
            else
            {
                response.Success = true;
                response.UserNameValid = username + random.Next().ToString();
                await isusernameinuse(response.UserNameValid);
            }
            return response;

        }

        [HttpGet]
        public IActionResult LoginBynummber()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginBynummber(string email)
        {

            var user = await _userManger.FindByEmailAsync(email);
            if (user != null)
            {
                Random random = new Random();
                var ra = random.Next();
                var ToEmailAddress = new EmailAddress
                {
                    Name = user.UserName,
                    Address = user.Email,
                };
                var FromeEmailAddress = new EmailAddress
                {
                    Name = "Mohamed Barkat",
                    Address = "Mohamed@support.Zina",
                };
                List<EmailAddress> ToEmailAddresses = new();
                ToEmailAddresses.Add(ToEmailAddress);
                List<EmailAddress> FromeEmailAddresses = new();
                FromeEmailAddresses.Add(FromeEmailAddress);

                var emailmessage = new EmailMessage
                {
                    Content = $"<a>{ra}</a>",
                    Subject = "",
                    ToAddresses = ToEmailAddresses,
                    FromAddresses = FromeEmailAddresses

                };
                await _emailservices.Send(emailmessage);

                SessionHelper.Set<string>(HttpContext.Session, "code", ra.ToString());
                SessionHelper.Set<string>(HttpContext.Session, "email", email);
                return RedirectToAction("CheckFromCode");

            }

            else
            {
                TempData["Email"] = email;
                return View();
            }


        }

        [HttpGet]
        public IActionResult CheckFromCode()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckFromCode(string code)
        {

            var codefromserver = SessionHelper.Get<string>(HttpContext.Session, "code");
            var email = SessionHelper.Get<string>(HttpContext.Session, "email");
            if (code == codefromserver)
            {
                var user = await _userManger.FindByEmailAsync(email);
                if (user != null)
                {
                    await _signInManger.SignInAsync(user, true);
                }

                HttpContext.Session.Remove("code");
                HttpContext.Session.Remove("email");
                return RedirectToAction("Index", "home");
            }

            else
            {
                HttpContext.Session.Remove("code");
                HttpContext.Session.Remove("email");
                return View();
            }


        }

    }

}
