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
            var auth = HttpContext.TokenAuth();
            if (auth.Exist)
                if (auth.IsAdmin)
                    return Redirect("~/Admin");
                else
                    return Redirect("~/Account");
            else
                return View();
        }

        [HttpPost]
        public IActionResult In(string login, string password)
        {
            var auth = HttpContext.LoginAuth(login, password);
            if (auth.Exist)
                if (auth.IsAdmin)
                    return Redirect("~/Admin");
                else
                    return Redirect("~/Account");
            else
                return Redirect("~/Login?error=1");
        }
    }
}