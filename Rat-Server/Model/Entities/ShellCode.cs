using Org.BouncyCastle.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.Entities
{
    public class ShellCode
    {
        [Key]
        public int ShellCodeId { get; set; }
        public string Name { get; set; }
        public byte[] Code { get; set; }
    }
}
