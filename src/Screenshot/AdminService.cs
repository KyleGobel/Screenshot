using Screenshot.Models;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Screenshot
{
    public class AdminService : Service
    {
        public object Post(LoginRequest request)
        {
            if (Db.Exists<Admin>(x => x.Username == request.Username && x.Password == request.Password))
                return new LoginResponse {Success = true};
            else
            {
                return new LoginResponse {Success = false};
            }
        }
    }

    [Route("/login", "POST")]
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
    }
}