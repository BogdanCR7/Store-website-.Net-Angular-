using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class OrderViewModel
    {
        
        public List<OrderProductViewModel> orderProducts { get; set; } = new List<OrderProductViewModel>();
    }
}
