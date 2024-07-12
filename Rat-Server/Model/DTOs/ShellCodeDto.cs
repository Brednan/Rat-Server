using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.DTOs
{
    public class ShellCodeDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
