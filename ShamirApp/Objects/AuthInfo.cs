namespace ShamirApp.Objects
{
    public class TokenAuthInfo
    {
        public TokenAuthInfo(bool exist, bool isAdmin, int id, string login)
        {
            Id = id;
            Exist = exist;
            IsAdmin = isAdmin;
            Login = login;
        }

        public TokenAuthInfo() { }

        public int Id { get; set; }
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
