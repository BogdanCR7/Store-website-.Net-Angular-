using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Category
    {    [Key]
        public int Id { get; set; }

        public List<Product> products { get; set; } = new List<Product>();
        
        public string  Title { get; set; }
        public string  ImagePath{ get; set; }

      
    }
}
