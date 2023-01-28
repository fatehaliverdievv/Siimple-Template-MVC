using System.ComponentModel.DataAnnotations;

namespace Siimple.ViewModels
{
    public class UpdateCardVM
    {
        [Required]
        [StringLength(15)]
        public string Title { get; set; }
        [StringLength(40)]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        [StringLength(60)]
        public string? IconUrl { get; set; }
    }
}
