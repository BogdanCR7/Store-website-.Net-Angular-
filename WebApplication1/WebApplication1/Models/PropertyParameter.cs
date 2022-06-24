using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class PropertyParameter
    {
        [Key]
        public int Id { get; set; }

        public Property property { get; set; }

        public Parameters parameter { get; set; }

    }
}
