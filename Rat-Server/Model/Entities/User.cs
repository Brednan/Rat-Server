using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public Admin? Admin { get; set; }
    }
}
