using System.ComponentModel.DataAnnotations;

namespace SahiHisabAPI.Model
{
    public class Register
    {
       
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required  string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required  string Email { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
