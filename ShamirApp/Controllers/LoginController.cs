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
            if (HttpContext.TokenAuth().isAuth)
                return Redirect("~/Account");
            else
                return View();
        }

        [HttpPost]
        public IActionResult In(string login, string password)
        {
            if (HttpContext.LoginAuth(login, password).isAuth)
                return Redirect("~/Account");
            else
                return Redirect("~/Login");
        }
    }
}