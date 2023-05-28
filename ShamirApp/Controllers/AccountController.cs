using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShamirApp.Models.Account;
using ShamirApp.Services;
using System.Net.WebSockets;

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
            var auth = CheckAuth();
            if (!auth.isAuth)
                return Redirect("~/Login");

            var model = NpgsqlClient.GetInstance().GetAllForms(auth.idUser);
            return View(model);
        }

        public IActionResult Form(int id)
        {
            var auth = CheckAuth();
            if (!auth.isAuth)
                return Redirect("~/Login");

            if (id <= 0)
                return Redirect("~/Account");

            var sqlClient = NpgsqlClient.GetInstance();
            var form = sqlClient.GetForm(id);

            if(form.id <= 0)
                return Redirect("~/Account");

            var questions = sqlClient.GetQuestions(id);
            return View((form, questions));
        }

        [HttpPost]
        public string SendVote(int idform, string info)
        {
            var auth = CheckAuth();
            if (!auth.isAuth)
                return @$"{{ ""status"":{403} }}";

            var voteinfo = JsonConvert.DeserializeObject<List<VoteInfo>>(info);

            var rows = NpgsqlClient.GetInstance().AddNewResults(idform, auth.idUser, voteinfo);

            return @$"{{ ""status"":{200}, ""result"":{rows} }}";
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
        
        private (bool isAuth, int idUser, string login) CheckAuth()
        {
            var auth = HttpContext.TokenAuth();
            if (auth.Exist && !auth.IsAdmin)
            {
                ViewData["login"] = auth.Login;
                return (true, auth.Id, auth.Login);
            }
            return (false, 0, string.Empty);
        }
    }
}
