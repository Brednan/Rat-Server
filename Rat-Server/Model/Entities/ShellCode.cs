using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class ShellCode
    {
        [Key]
        public int ShellCodeId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
