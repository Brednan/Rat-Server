using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.Entities
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
