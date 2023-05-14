using Microsoft.AspNetCore.Mvc;
using ShamirApp.Services;

namespace ShamirApp.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// Анкеты
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return GetView();
        }
        /// <summary>
        /// Пользователи
        /// </summary>
        /// <returns></returns>
        public IActionResult Users()
        {
            return GetView();
        }
        /// <summary>
        /// Выход
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
                    return View();
                else
                    return Redirect("~/Admin");
            else
                return Redirect("~/Login");
        }
    }
}
