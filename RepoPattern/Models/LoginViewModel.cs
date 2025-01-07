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
}
