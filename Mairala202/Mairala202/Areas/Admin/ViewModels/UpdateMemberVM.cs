using System.ComponentModel.DataAnnotations;

namespace Mairala202.Areas.Admin.ViewModels
{
    public class UpdateMemberVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Position { get; set; } = null!;
        [Required]
        public string Image { get; set; } = null!;
        public IFormFile? Photo { get; set; }
    }
}
