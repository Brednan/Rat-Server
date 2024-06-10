using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Rat_Server.Model
{
    [Index(nameof(Hwid), nameof(Name))]
    public class Device
    {
        [Key]
        [Required]
        [Column("DeviceHwid")]
        public Guid Hwid { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Precision(5)]
        public DateTime LastActive { get; set; }

        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}
