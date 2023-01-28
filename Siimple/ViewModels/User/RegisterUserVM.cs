using System.ComponentModel.DataAnnotations;

namespace Siimple.ViewModels
{
    public class RegisterUserVM
    {
        [Required]
        [StringLength(20), MinLength(3)]
        public string Name { get; set; }
        [StringLength(20), MinLength(3)]
        public string? Surname { get; set; } = "XXXX";
        [Required]
        [StringLength(20), MinLength(3)]
        public string Username { get; set; }
        [Required]
        [StringLength(25), MinLength(3)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(15)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(15)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
