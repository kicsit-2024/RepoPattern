using RepoPattern.Handlers;
using System.ComponentModel.DataAnnotations;

namespace RepoPattern.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public List<AppRole> Roles { get; set; }

        public string EncryptPassword()
        {
            Password = EncryptionHelper.EncryptString(Id + Password);
            return Password;
        }

        public bool ValidatePassword(string password)
        {
            var encryptedUserPassword = EncryptionHelper.EncryptString(Id + password);
            return Password == encryptedUserPassword;
        }
    }

    public class AppRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AppUser> Users { get; set; }
    }

}
