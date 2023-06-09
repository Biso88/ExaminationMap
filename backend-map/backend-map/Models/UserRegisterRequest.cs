using System.ComponentModel.DataAnnotations;

namespace backend_map.Models
{
    public class UserRegisterRequest
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8, ErrorMessage = "Plaese, enter at lwast 8 characters")]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        public UserRegisterRequest(string name, string email, string password, string confirmPassword)
        {
            Name = name;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
