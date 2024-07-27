using System.ComponentModel.DataAnnotations;

namespace Rat_Server.Model.DTOs
{
    public class RegisterDeviceRequestBodyDto
    {
        [Required]
        public Guid Hwid {get; set;}
        
        [Required]        
        public string DeviceName { get;set;}
    }
}
