using ExampleProject.Models;
using ExampleProject.Web.Models;
using ExampleProject.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExampleProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICompanyRepository _companyRepository;

        public HomeController(ILogger<HomeController> logger,UserManager<AppUser> userManager,
            ICompanyRepository companyRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _companyRepository = companyRepository;
        }

        public async Task<IActionResult> Index(PanelViewModel model)
        {         
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var company =  _companyRepository.Find(user.CompanyId ?? 0);

            HttpContext.Session.SetString("_UserId", user.Id.ToString());
            HttpContext.Session.SetString("_UserName", user.FirstName);

            if (company == null)
            {
                ModelState.AddModelError(string.Empty, "Company Not Exist");
                return RedirectToAction("Login", "Account");
            }
            model.UserFirstName = user.FirstName;
            model.CompanyName = company.Name;

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Unauthorize()
        {
            return View();
        }
    }
}