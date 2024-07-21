using Org.BouncyCastle.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.Entities
{
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
