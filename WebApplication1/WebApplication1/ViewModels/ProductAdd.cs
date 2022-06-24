
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class ProductAdd
    {
        public PropertyAdd[] properties { get; set; }

        public IFormFile Picture { get; set; }
        public string Color  { get; set; }
        public string Title { get; set; }

        public int Price { get; set; }

        public string Brand { get; set; }

        public int CategoryId { get; set; }

    }
}
