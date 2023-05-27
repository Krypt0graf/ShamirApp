using System.Diagnostics.Contracts;

namespace ShamirApp.Services
{
    public class TokenAuthInfo
    {
        public TokenAuthInfo(bool exist, bool isAdmin, string login)
        {
            Exist = exist;
            IsAdmin = isAdmin;
            Login = login;
        }

        public TokenAuthInfo() { }

        public bool Exist { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public string Login { get; set; } = string.Empty;
    }
    public class LoginAuthInfo
    {
        public LoginAuthInfo(bool exist, bool isAdmin, string token)
        {
            Exist = exist;
            IsAdmin = isAdmin;
            Token = token;
        }

        public LoginAuthInfo() { }

        public bool Exist { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public string Token { get; set; } = string.Empty;
    }
}
