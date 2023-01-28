using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Siimple.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [StringLength(20),MinLength(3)]
        public string Name { get; set; }
        [StringLength(20),MinLength(3)]
        public string? Surname { get; set; } = "XXXX";
    }
}
