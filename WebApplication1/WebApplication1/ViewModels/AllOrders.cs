using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class AllOrders
    {

        public string City { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public string Name { get; set; }
        public ProductViewModel product { get; set; }
    }
}
