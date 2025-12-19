using Microsoft.AspNetCore.Mvc;
using QMS.Core.Enums;
using QMS.Core.Models;
using QMS.Core.Repositories.UsersRepository;

namespace QMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        public AccountController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var loginUser = await _usersRepository.Login(user);
                if (loginUser != null)
                {
                    HttpContext.Session.SetInt32("UserId", loginUser.Id);
                    HttpContext.Session.SetString("FullName", loginUser.Name);
                    HttpContext.Session.SetInt32("UserRole", (int)loginUser.RoleId);
                    HttpContext.Session.SetInt32("Vendor_Id", (int)loginUser.Vendor_Id);
                    HttpContext.Session.SetString("User_Type", loginUser.User_Type);
                    if (DateTime.Now.Month > 3)
                    {
                        HttpContext.Session.SetString("FYear", (DateTime.Now.Year.ToString().Substring(2) + (DateTime.Now.Year + 1).ToString().Substring(2)));
                    }
                    else
                    {
                        HttpContext.Session.SetString("FYear", ((DateTime.Now.Year - 1).ToString().Substring(2) + (DateTime.Now.Year).ToString().Substring(2)));
                    }
                    if (loginUser.RoleId == (int)UserRoles.Admin || loginUser.RoleId == (int)UserRoles.Manager || loginUser.RoleId == (int)UserRoles.Staff)
                    {
                        return RedirectToAction("OptionSelection", "Home");
                    }
                    else
                    {
                        return RedirectToAction("VendorOptionSelection", "Home");
                    }
                }
                ModelState.AddModelError("WrongCredentials", "Incorrect email address or password.");
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
