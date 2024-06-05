using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Rat_Server.Model
{
    [Index(nameof(Hwid), nameof(Username))]
    public class Device
    {
        [Key]
        [Required]
        public Guid Hwid { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Precision(5)]
        public DateTime LastActive { get; set; }

        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}
