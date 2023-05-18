using Microsoft.AspNetCore.Mvc;
using ShamirApp.Services;

namespace ShamirApp.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Анкеты для пользователей
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (CheckAuth())
                return View();
            return Redirect("~/Login");
        }
        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.Logout();
            return Redirect("~/Login");
        }
        
        private bool CheckAuth()
        {
            (bool isAuth, bool isAdmin) = HttpContext.TokenAuth();
            if (isAuth && !isAdmin)
                return true;
            return false;
        }
    }
}
