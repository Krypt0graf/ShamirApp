namespace ShamirApp.Services
{
    public static class Auth
    {
        public static (bool isAuth, bool isAdmin) LoginAuth(this HttpContext context, string login, string password)
        {
            (bool exist, bool isAdmin, string token)  = NpgsqlClient.GetInstance().GetUserFromLogin(login, password);
            if (exist)
            {
                if (isAdmin)
                {
                    context.Response.Cookies.Append("token", token);
                    return (true, false);
                }
                else
                {
                    context.Response.Cookies.Append("token", token);
                    return (true, true);
                }
            }
            else
                return (false, false);
        }
        public static (bool isAuth, bool isAdmin) TokenAuth(this HttpContext context)
        {
            var hasCookie = context.Request.Cookies.TryGetValue("token", out string? token);
            if (hasCookie)
            {
                (bool exist, bool isAdmin) = NpgsqlClient.GetInstance().GetUserFromToken(token);
                if (exist)
                {
                    if (isAdmin)
                    {
                        return (true, true);
                    }
                    else
                    {
                        return (true, false);
                    }
                }
                else
                    context.Response.Cookies.Delete("token");
            }
            return (false, false);         
        }

        public static void Logout(this HttpContext context)
        {
            context.Response.Cookies.Delete("token");
        }
    }
}
