using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class ProductPageViewModel
    {

        public List<ProductViewModel> productViewModels { get; set; } = new List<ProductViewModel>();

        public List<string> Brands { get; set; } = new List<string>();
        public List<string> Colors { get; set; } = new List<string>();
        public int Max { get; set; }
        public int Min { get; set; }
        public int PageCount { get; set; }


    }
}
