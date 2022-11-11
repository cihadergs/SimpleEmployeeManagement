using AutoMapper;
using ExampleProject.Models;
using ExampleProject.Web.Models;
using ExampleProject.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExampleProject.Web.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class EmployeeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmloyeeRepository _employeeRepository;

        public EmployeeController(ICompanyRepository companyRepository, SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager, IEmloyeeRepository employeeRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
        }

        public IActionResult List()
        {
            var currUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var company = _companyRepository.Find(currUser.CompanyId ?? 0);
            //HttpContext.Session.SetString("_CompanyName", company.Name);
            HttpContext.Session.SetString("_UserName", currUser.FirstName);
            var companyEmployees = _employeeRepository.GetCompanyEmployees(company.Id);
            return View(companyEmployees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var currUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var company = _companyRepository.Find(currUser.CompanyId ?? 0);
            //HttpContext.Session.SetString("_CompanyName", company.Name);
            HttpContext.Session.SetString("_UserName", currUser.FirstName);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currUser = await _userManager.GetUserAsync(HttpContext.User);
                var company = _companyRepository.Find(currUser.CompanyId ?? 0);
                //HttpContext.Session.SetString("_CompanyName", company.Name);

                var user = new AppUser
                {

                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CompanyId = company.Id
                };

                var emlopyee = new Employee
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    EmployeeRole = model.UserRole,
                    CompanyId = company.Id,
                    IsActive = true
                };
                _employeeRepository.Create(emlopyee);

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.UserRole == DataAccess.Enums.UserRole.Operator)
                        await _userManager.AddToRoleAsync(user, "Operator");
                    if (model.UserRole == DataAccess.Enums.UserRole.Chef)
                        await _userManager.AddToRoleAsync(user, "Chef");
                    if (model.UserRole == DataAccess.Enums.UserRole.Supervisor)
                        await _userManager.AddToRoleAsync(user, "Supervisor");

                    return RedirectToAction("List", "Employee");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }

        public IActionResult Edit(UserViewModel model, int id)
        {
            var currUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var company = _companyRepository.Find(currUser.CompanyId ?? 0);
            //HttpContext.Session.SetString("_CompanyName", company.Name);
            HttpContext.Session.SetString("_UserName", currUser.FirstName);
            var employee = _employeeRepository.Find(id);

            model.EmployeeId = employee.Id;
            model.FirstName = employee.FirstName;
            model.LastName = employee.LastName;
            model.Email = employee.Email;
            model.UserRole = employee.EmployeeRole;
            model.IsActive = employee.IsActive;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel model)
        {
            var employee = _employeeRepository.Find(id);
            var user = await _userManager.FindByEmailAsync(employee.Email);
            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            var result = await _userManager.RemoveFromRoleAsync(user, role);

            if (result.Succeeded)
            {
                if (model.UserRole == DataAccess.Enums.UserRole.Operator)
                    await _userManager.AddToRoleAsync(user, "Operator");
                if (model.UserRole == DataAccess.Enums.UserRole.Chef)
                    await _userManager.AddToRoleAsync(user, "Chef");
                if (model.UserRole == DataAccess.Enums.UserRole.Supervisor)
                    await _userManager.AddToRoleAsync(user, "Supervisor");
            }
            user.Email = model.Email;
            user.UserName = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            employee.Email = model.Email;
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.EmployeeRole = model.UserRole;

            var result2 = await _userManager.UpdateAsync(user);
            if(result2.Succeeded)
                _employeeRepository.Update(employee);

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = _employeeRepository.Find(id);
            var user = await _userManager.FindByEmailAsync(employee.Email);
            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                _employeeRepository.Remove(id);
                await _userManager.DeleteAsync(user);
            }              
            return RedirectToAction("List");
        }
    }
}
