using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        public OrdersController(ApplicationContext context
           ,UserManager<User> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

        private int Userid => Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{GetUser}")]
        public async Task<ActionResult<IEnumerable<ProductDescription>>> GetOrder()
        {
            
            var userid = User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userManager.FindByIdAsync(userid);
            var orderProducts= await _context.Orders.Where(x => x.user.Id == user.Id).Include(x=>x.OrderProducts).ToListAsync();
            List<ProductDescription> lst = new List<ProductDescription>();
            foreach (var tmp in orderProducts)
            {
                foreach (var item in tmp.OrderProducts)
                {
                    Product product = await _context.Products.Include(x => x.BrandProduct).Where(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                    if (product != null)
                    {
                        lst.Add(new ProductDescription()
                        {
                            Title = product.Title,
                            Brand = product.BrandProduct.BrandName,
                            Color = product.ProductColor,
                            ImagePath = product.ImagePath,
                            Price = product.Price,
                            Id = product.Id
                        });
                    }
                }
            }

            

            return lst;
        }
        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<IEnumerable<AllOrders>>> All()
        {

            var userid = User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
           
            var orderProducts = await _context.Orders.Include(x => x.OrderProducts).Include(x=>x.user).ToListAsync();
            List<AllOrders> lst = new List<AllOrders>();
           
            foreach (var tmp in orderProducts)
            {
                foreach (var item in tmp.OrderProducts)
                {
                    Product product = await _context.Products.Include(x => x.BrandProduct).Where(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                    if (product != null)
                    {
                        lst.Add(new AllOrders()
                        {
                            Address = tmp.user.Address,
                            City = tmp.user.City,
                            Name = tmp.user.FirstName,
                            PhoneNumber = tmp.user.PhoneNumber,
                            
                            product = new ProductViewModel()
                            {
                                Title = product.Title,
                                ImagePath = product.ImagePath,
                                Price = product.Price,
                                Id = product.Id
                            }
                        });

                    }
                       
                    
                }
            }

            return lst;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
       
        public async Task<IActionResult> Create(OrderViewModel order)
        {
            
                Order neworder = new Order();
            
                var userid = User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;

            neworder.user = await _userManager.FindByIdAsync(userid);
            foreach (var item in order.orderProducts)
            {
                OrderProduct orderProduct = new OrderProduct()
                {
                    Count = item.Count,
                    ProductId = item.ProductId
                };
               
                neworder.OrderProducts.Add(orderProduct);
               
            }
            try
            {
                _context.Orders.Add(neworder);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                string str = ex.Message;
            }
                return Ok();
        }
       

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
