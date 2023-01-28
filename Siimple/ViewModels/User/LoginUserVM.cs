using System.ComponentModel.DataAnnotations;

namespace Siimple.ViewModels
{
    public class LoginUserVM
    {
        [Required]
        [StringLength(15)]
        public string UsernameorEmail { get; set; }
        [Required]
        [StringLength(15)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }
}
