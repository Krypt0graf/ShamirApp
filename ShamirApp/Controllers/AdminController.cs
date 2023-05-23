using Microsoft.AspNetCore.Mvc;
using ShamirApp.Services;

namespace ShamirApp.Controllers
{
    public class AdminController : Controller
    {
        #region [IActionResult]
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
            var model = NpgsqlClient.GetInstance().GetUsers();
            return GetView(model);
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
        private IActionResult GetView(object? model = null)
        {
            if (CheckAuth())
                return View(model);
            return Redirect("~/Login");
        }
        #endregion
        #region [Private]
        private bool CheckAuth()
        {
            (bool isAuth, bool isAdmin) = HttpContext.TokenAuth();
            if(isAuth && isAdmin)
                return true;
            return false;
        }
        #endregion
        #region [API]
        [HttpPost]
        public string AddNewUser(string login, string password, string fio)
        {
            var id = NpgsqlClient.GetInstance().AddNewUser(login, password, fio);
            return @$"{{ ""id"":{id} }}";
        }

        [HttpPut]
        public string EditUser(int id, string login, string password, string fio)
        {
            var rows = NpgsqlClient.GetInstance().EditUser(id, login, password, fio);
            return @$"{{ ""rows"":{rows} }}";
        }

        [HttpDelete]
        public string DeleteUser(int id, string login, string password, string fio)
        {
            var rows = NpgsqlClient.GetInstance().DeleteUser(id);
            return @$"{{ ""rows"":{rows} }}";
        }
        #endregion
    }
}
