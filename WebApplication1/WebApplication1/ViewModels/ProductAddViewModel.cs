using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class ProductAddViewModel
    {
        public List<string> brands { get; set; }

        public List<PropertyAdd> properties { get; set; } = new();
    }
}
