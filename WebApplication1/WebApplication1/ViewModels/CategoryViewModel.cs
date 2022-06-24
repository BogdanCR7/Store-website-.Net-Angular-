using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
}
