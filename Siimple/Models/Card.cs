using System.ComponentModel.DataAnnotations;

namespace Siimple.Models
{
    public class Card:BaseEntity
    {
        [Required]
        [StringLength(15)]
        public string Title { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
        [Required]
        [StringLength(60)]
        public string ImageUrl{ get; set;}
        [StringLength(60)]
        public string? IconUrl { get; set;}

    }
}
