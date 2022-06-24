using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public User user { get; set; }
        public List<OrderProduct>  OrderProducts { get; set; } = new List<OrderProduct>();

    }
}
