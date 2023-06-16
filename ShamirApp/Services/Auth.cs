using ShamirApp.Objects;

namespace ShamirApp.Services
{
    public static class Auth
    {
        public static LoginAuthInfo LoginAuth(this HttpContext context, string login, string password)
        {
            var auth  = NpgsqlClient.GetInstance().GetUserFromLogin(login, password);
            if (auth.Exist)
                context.Response.Cookies.Append("token", auth.Token);
            return auth;
        }
        public static TokenAuthInfo TokenAuth(this HttpContext context)
        {
            var hasCookie = context.Request.Cookies.TryGetValue("token", out string? token);
            if (hasCookie)
            {
                var auth = NpgsqlClient.GetInstance().GetUserFromToken(token);
                if (auth.Exist)
                {
                    return auth;
                }
                else
                    context.Response.Cookies.Delete("token");
            }
            return new TokenAuthInfo();         
        }

        public static void Logout(this HttpContext context)
        {
            context.Response.Cookies.Delete("token");
        }
    }
}
