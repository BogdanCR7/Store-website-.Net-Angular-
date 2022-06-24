using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class GetProduct
    {
        public int CategoryId { get; set; }
        public int Page { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
       public List<string> colors { get; set; } = new List<string>();
       public List<string> brand { get; set; } = new List<string>();
    }
}
