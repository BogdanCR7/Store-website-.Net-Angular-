using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }

    }
}
