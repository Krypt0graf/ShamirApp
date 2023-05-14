namespace ShamirApp.Services
{
    public static class Auth
    {
        private static readonly (string login, string password, string token) user =  ("user", "user", "12345");
        private static readonly (string login, string password, string token) admin = ("admin", "admin", "54321");

        public static (bool isAuth, bool isAdmin) LoginAuth(this HttpContext context, string login, string password)
        {
            if ($"{login}+{password}" == $"{user.login}+{user.password}")
            {
                context.Response.Cookies.Append("token", user.token);
                return (true, false);
            }
            else
            if ($"{login}+{password}" == $"{admin.login}+{admin.password}")
            {
                context.Response.Cookies.Append("token", admin.token);
                return (true, true);
            }
            else
                return (false, false);
        }
        public static (bool isAuth, bool isAdmin) TokenAuth(this HttpContext context)
        {
            var hasCookie = context.Request.Cookies.TryGetValue("token", out string? token);
            if (hasCookie && token == user.token)
                return (true, false);
            else
            if (hasCookie && token == admin.token)
                return (true, true);
            return (false, false);
        }

        public static void Logout(this HttpContext context)
        {
            context.Response.Cookies.Delete("token");
        }
    }
}
