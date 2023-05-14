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
            return GetView();
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
        
        private IActionResult GetView()
        {
            (bool isAuth, bool isAdmin) = HttpContext.TokenAuth();
            if (isAuth)
                if (isAdmin)
                    return Redirect("~/Admin");
                else
                    return View();
            else
                return Redirect("~/Login");
        }
    }
}
