using System.ComponentModel.DataAnnotations;

namespace backend_map.Models
{
    public class UserLoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8, ErrorMessage = "Please, enter at least 8 characters")]
        public string Password { get; set; }

        public UserLoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
