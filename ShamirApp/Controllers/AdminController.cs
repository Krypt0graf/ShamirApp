using Microsoft.AspNetCore.Mvc;
using ShamirApp.Services;
using Newtonsoft.Json;

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
            var auth = CheckAuth();
            if (!auth.isAuth)
                return Redirect("~/Login");

            var model = NpgsqlClient.GetInstance().GetAllForms();
            return GetView(model);
        }
        /// <summary>
        /// Пользователи
        /// </summary>
        /// <returns></returns>
        public IActionResult Users()
        {
            var auth = CheckAuth();
            if (!auth.isAuth)
                return Redirect("~/Login");

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
            var auth = CheckAuth();
            if (!auth.isAuth)
                return Redirect("~/Login");

            return View(model);
            
        }
        #endregion
        #region [Private]
        private (bool isAuth, string login) CheckAuth()
        {
            TokenAuthInfo auth = HttpContext.TokenAuth();
            if(auth.Exist && auth.IsAdmin)
                return (true, auth.Login);
            return (false, string.Empty);
        }
        #endregion
        #region [USERS]
        [HttpPost]
        public string AddNewUser(string login, string password, string fio)
        {
            var auth = CheckAuth();
            if (!auth.isAuth)
                return @$"{{ ""status"":{403} }}";

            var id = NpgsqlClient.GetInstance().AddNewUser(login, password, fio);
            return @$"{{ ""id"":{id} }}";
        }

        [HttpPut]
        public string EditUser(int id, string login, string password, string fio)
        {
            var auth = CheckAuth();
            if (!auth.isAuth)
                return @$"{{ ""status"":{403} }}";

            var rows = NpgsqlClient.GetInstance().EditUser(id, login, password, fio);
            return @$"{{ ""rows"":{rows} }}";
        }

        [HttpDelete]
        public string DeleteUser(int id, string login, string password, string fio)
        {
            var auth = CheckAuth();
            if (!auth.isAuth)
                return @$"{{ ""status"":{403} }}";

            var rows = NpgsqlClient.GetInstance().DeleteUser(id);
            return @$"{{ ""rows"":{rows} }}";
        }
        #endregion
        #region [FORMS]
        [HttpPost]
        public string AddNewForm(string title, string[] qs)
        {
            var auth = CheckAuth();
            if (!auth.isAuth)
                return @$"{{ ""status"":{403} }}";

            var id = NpgsqlClient.GetInstance().AddNewForm(title);
            var count = NpgsqlClient.GetInstance().AddNewQuestions(qs, id);
            return @$"{{ ""id"": {id}, ""count"": {count} }}";
        }
        public string GetQuestions(int idform)
        {
            var auth = CheckAuth();
            if (!auth.isAuth)
                return @$"{{ ""status"":{403} }}";

            var qs = NpgsqlClient.GetInstance().GetQuestions(idform);
            var json = JsonConvert.SerializeObject(qs);
            return json;
        }
        [HttpDelete]
        public string DeleteForm(int id)
        {
            var auth = CheckAuth();
            if (!auth.isAuth)
                return @$"{{ ""status"":{403} }}";

            var sqlClient = NpgsqlClient.GetInstance();
            sqlClient.DeleteQuestions(id);
            var rows = sqlClient.DeleteForm(id);
            return @$"{{ ""rows"":{rows} }}";
        }
        #endregion
    }
}
