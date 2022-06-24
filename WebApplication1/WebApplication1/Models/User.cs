using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class User:IdentityUser<Guid>
    {
        public string City { get; set; }

        public string Address { get; set; }
        public string FirstName { get; set; }

    }
}
