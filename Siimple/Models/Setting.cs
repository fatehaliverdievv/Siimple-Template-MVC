using System.ComponentModel.DataAnnotations;

namespace Siimple.Models
{
    public class Setting:BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string Key { get; set; }
        [StringLength(50)]
        public string? Value { get; set; }
    }
}
