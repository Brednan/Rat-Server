using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Rat_Server.Model
{
    public class Command
    {
        [Required]
        [Key]
        public Guid commandId { get; set; }

        [Required]
        public Guid DeviceHwid { get; set; }

        [Required]
        [MaxLength(600)]
        public string CommandValue { get; set; }

        [Precision(5)]
        public DateTime DateAdded { get; set; }

        [Required]
        public Device Device { get; set; }
    }
}
