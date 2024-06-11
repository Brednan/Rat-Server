using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.DTOs
{
    public class UserLoginRequestBodyDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
