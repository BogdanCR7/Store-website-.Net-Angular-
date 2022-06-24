using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
       
        public int Price { get; set; }
       
        public string ImagePath { get; set; }
       
        public string Title  { get; set; }
        public string ProductColor { get; set; }
        public Category category { get; set; }
        
        public Brand BrandProduct { get; set; }
        public List<PropertyParameter> characteristics { get; set; } = new List<PropertyParameter>();

    }
}
