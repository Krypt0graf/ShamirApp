using Microsoft.AspNetCore.Mvc;
using ShamirApp.Models;
using System.Diagnostics;
using ShamirApp.Services;

namespace ShamirApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            (bool isAuth, bool isAdmin) = HttpContext.TokenAuth();
            if (isAuth)
                if (isAdmin)
                    return Redirect("~/Admin");
                else
                    return Redirect("~/Account");
            else
                return View();
        }

        [HttpPost]
        public IActionResult In(string login, string password)
        {
            (bool isAuth, bool isAdmin) = HttpContext.LoginAuth(login, password);
            if (isAuth)
                if (isAdmin)
                    return Redirect("~/Admin");
                else
                    return Redirect("~/Account");
            else
                return Redirect("~/Login");
        }
    }
}