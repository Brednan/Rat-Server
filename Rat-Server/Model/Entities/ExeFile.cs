using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class ExeFile
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
