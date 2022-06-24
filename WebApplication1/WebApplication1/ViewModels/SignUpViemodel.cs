using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class SignUpViemodel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        public string City { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        
    }
}
