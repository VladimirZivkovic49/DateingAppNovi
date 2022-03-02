using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Token{ get; set; }
   
        public string PhotoUrl { get; set; }
        public string KnownAs { get; set; } 
    //(L159)
        public string Gender { get; set; }
    //(L159)
    
    }
}