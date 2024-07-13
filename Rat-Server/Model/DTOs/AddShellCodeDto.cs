using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.DTOs
{
    public class AddShellCodeDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
