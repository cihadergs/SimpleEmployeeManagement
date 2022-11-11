using AutoMapper;
using ExampleProject.DataAccess.Enums;
using ExampleProject.Models;
using ExampleProject.Web.Models;
using ExampleProject.Web.Repositories;
using ExampleProject.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.VisualBasic;

namespace ExampleProject.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ICompanyRepository _companyRepository;

        public AccountController(ICompanyRepository companyRepository,IMapper mapper, SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _companyRepository = companyRepository;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var company = new Company
                {
                    Name = model.CompanyName,
                    CompanyType = model.CompanyType,
                    IsActive = true
                };
                _companyRepository.Create(company);

                var user = new AppUser
                {

                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CompanyId = company.Id
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Manager");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel signIn)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(signIn.Email, signIn.Password, signIn.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(signIn);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        public IActionResult Edit(UserViewModel model, int id)
        {
            var currUser = _userManager.GetUserAsync(HttpContext.User).Result;
            //HttpContext.Session.SetString("_CompanyName", company.Name);
            HttpContext.Session.SetString("_UserName", currUser.FirstName);

            model.FirstName = currUser.FirstName;
            model.LastName = currUser.LastName;
            model.Email = currUser.Email;
            model.UserRole = UserRole.Manager;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var result2 = await _userManager.UpdateAsync(user);
                if (result2.Succeeded)
                {
                    await _userManager.RemovePasswordAsync(user);
                    //     Add a user password only if one does not already exist
                    await _userManager.AddPasswordAsync(user, model.Password);
                    return RedirectToAction("Login", "Account");
                }
            }
            
            return View(model);
        }

        #region CREATE ADMIN AND ROLES
        //public async Task CreateRolesandUsers()
        //{
        //    bool x = await _roleManager.RoleExistsAsync("Admin");
        //    if (!x)
        //    {
        //        // first we create Admin rool    
        //        var role = new IdentityRole<int>();
        //        role.Name = "Admin";
        //        await _roleManager.CreateAsync(role);

        //        //Here we create a Admin super user who will maintain the website                   

        //        var user = new AppUser();

        //        user.Email = "cihaderagus98@gmail.com";
        //        user.UserName = "cihaderagus98@gmail.com";
        //        user.FirstName = "Cihad";
        //        user.LastName = "Eragus";

        //        string userPWD = "Cihad123*";

        //        IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

        //        //Add default User to Role Admin    
        //        if (chkUser.Succeeded)
        //        {
        //            var result1 = await _userManager.AddToRoleAsync(user, "Admin");
        //        }
        //    }

        //    // creating Creating Manager role     
        //    x = await _roleManager.RoleExistsAsync("Manager");
        //    if (!x)
        //    {
        //        var role = new IdentityRole<int>();
        //        role.Name = "Manager";
        //        await _roleManager.CreateAsync(role);
        //    }

        //    // creating Creating Employee role     
        //    x = await _roleManager.RoleExistsAsync("Supervisor");
        //    if (!x)
        //    {
        //        var role = new IdentityRole<int>();
        //        role.Name = "Supervisor";
        //        await _roleManager.CreateAsync(role);
        //    }
        //    x = await _roleManager.RoleExistsAsync("Chef");
        //    if (!x)
        //    {
        //        var role = new IdentityRole<int>();
        //        role.Name = "Chef";
        //        await _roleManager.CreateAsync(role);
        //    }
        //    x = await _roleManager.RoleExistsAsync("Operator");
        //    if (!x)
        //    {
        //        var role = new IdentityRole<int>();
        //        role.Name = "Operator";
        //        await _roleManager.CreateAsync(role);
        //    }
        //}
        #endregion
    }
}
