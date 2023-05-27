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

            var model = NpgsqlClient.GetInstance().GetAllForms();
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
            var arr = new List<double>(); 

            if(voteinfo is not null)
                foreach (var vote in voteinfo)
                {   
                    if(vote.Points is not null)
                        arr.Add(Interpolate(vote.Points, 0));
                }
            var result = arr.Select(x => (int)Math.Round(x, 0)).ToArray();
            
            return @$"{{ ""status"":{200}, ""result"":{result.Length} }}";
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
        
        private (bool isAuth, string login) CheckAuth()
        {
            var auth = HttpContext.TokenAuth();
            if (auth.Exist && !auth.IsAdmin)
                return (true, auth.Login);
            return (false, string.Empty);
        }

        private double Interpolate(List<Point> points, int x)
        {
            var x1 = points[0].X;
            var x2 = points[1].X;
            var x3 = points[2].X;
            var y1 = points[0].Y;
            var y2 = points[1].Y;
            var y3 = points[2].Y;

            double y =
                y1 * (double)((x - x2) * (x - x3)) / ((x1 - x2) * (x1 - x3)) +
                y2 * (double)((x - x1) * (x - x3)) / ((x2 - x1) * (x2 - x3)) +
                y3 * (double)((x - x1) * (x - x2)) / ((x3 - x1) * (x3 - x2));

            return y;
        }
    }
}
