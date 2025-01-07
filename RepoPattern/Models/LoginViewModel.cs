using System.ComponentModel.DataAnnotations;

namespace RepoPattern.Models
{
    public class LoginViewModel
    {
        public string LoginId { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class AuthViewModel
    {
        public int UserId { get; set; }
        public List<string> Roles { get; set; }
        public DateTime ValidUpto { get; set; }
        public string Token { get; set; } = Path.GetRandomFileName();
    }
}
