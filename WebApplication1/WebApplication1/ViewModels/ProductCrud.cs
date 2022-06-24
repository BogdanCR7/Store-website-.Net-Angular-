using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class ProductCrud
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
    }
}
