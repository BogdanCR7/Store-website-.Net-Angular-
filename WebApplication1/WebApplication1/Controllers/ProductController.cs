using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ProductController(ApplicationContext context)
        {
            _context = context;
        }

     
        [HttpPost("GetByCategory")]
        public async Task<ActionResult<ProductPageViewModel>> GetProducts([FromBody] GetProduct getProduct)
        {
            int PageSize = 6;

            IQueryable<Product> products = _context.Products.Where(x => x.category.Id == getProduct.CategoryId);


            if (getProduct.Page == 0)
            {
                getProduct.Page = 1;
            }
            if (getProduct.Max!=0)
            {
                products=products.Where(x => x.Price <= getProduct.Max);
            }
            if (getProduct.Min != 0)
            {
               products= products.Where(x => x.Price >= getProduct.Min);
            }
            if (getProduct.brand.Count()>0)
            {
                 products=products.Where(x => getProduct.brand.Contains(x.BrandProduct.BrandName));
            }
            if (getProduct.colors.Count() > 0)
            {
                products = products.Where(x => getProduct.colors.Contains(x.ProductColor));
            }
            ProductPageViewModel productPageViewModel = new ProductPageViewModel();


            productPageViewModel.PageCount=(int)Math.Ceiling((double)products.Count() / PageSize);

            productPageViewModel.Max =  _context.Products.Where(x => x.category.Id == getProduct.CategoryId).Select(x=>x.Price).Max();
            productPageViewModel.Min =  _context.Products.Where(x => x.category.Id == getProduct.CategoryId).Select(x=>x.Price).Min();


            productPageViewModel.productViewModels=await products.Where(x => x.category.Id == getProduct.CategoryId).OrderBy(x=>x.Id)
                .Skip((int)((getProduct.Page - 1) * PageSize))
                .Take(PageSize)
                .Select(x => new ProductViewModel()
            {
                Title = x.Title,
                Price = x.Price,
                ImagePath=x.ImagePath,
                Id=x.Id                            
            }).ToListAsync();

            productPageViewModel.Brands = await _context.Products.Where(x => x.category.Id == getProduct.CategoryId).Select(x => x.BrandProduct.BrandName).Distinct().ToListAsync();
            productPageViewModel.Colors = await _context.Products.Where(x => x.category.Id == getProduct.CategoryId).Select(x => x.ProductColor).Distinct().ToListAsync();

            return productPageViewModel;
        }
        [HttpGet("GetAdd/{categoryId}")]
        public async Task<ActionResult<ProductAddViewModel>> GetAdd([FromRoute] int categoryId)
        {

            ProductAddViewModel productViewModel = new ProductAddViewModel();
            productViewModel.brands =await _context.Products.Where(x=>x.category.Id==categoryId).Select(x=>x.BrandProduct.BrandName).Distinct().ToListAsync();
           var props  = _context.Products
                .Include(x => x.characteristics).ThenInclude(x => x.property)
                .Where(x => x.category.Id == categoryId)
                .Select(x => x.characteristics).ToList();
            foreach (var item in props)
            {
                foreach (var prop in item)
                {
                    if (productViewModel.properties.Where(x=>x.Id==prop.property.Id).Count()==0) {
                        productViewModel.properties.Add(new PropertyAdd() {
                            Id = prop.property.Id,
                            Name = prop.property.Name
                        });
                    }
                }
            }

           
            return productViewModel;
        }
        [HttpGet("GetCrud")]
        public async Task<ActionResult<IEnumerable<ProductCrud>>> GetCrud()
        {
            var product = await _context.Products.Include(x=>x.category).Select(x => new ProductCrud()
            {
                Id=x.Id,
                Title = x.Title,
                Brand = x.BrandProduct.BrandName,
                Color = x.ProductColor,
                ImagePath = x.ImagePath,
                Price = x.Price,
                Category=x.category.Title

            }).ToListAsync();
            return product;
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDescription>> GetProduct(int id)
        {
            var product =await _context.Products.Where(x=>x.Id==id).Select(x=>new ProductDescription()
            {
                Title=x.Title,
                Brand=x.BrandProduct.BrandName,
                Color=x.ProductColor,
                ImagePath=x.ImagePath,
                Price=x.Price
              
            }).FirstOrDefaultAsync();
           
                List<PropertyParameter> props =  await _context.Products.Where(x => x.Id == id).Include(x=>x.characteristics).ThenInclude(x=>x.property)
                .Include(x => x.characteristics).ThenInclude(x => x.parameter)
                    .Select(x => x.characteristics).FirstOrDefaultAsync();
                foreach (var item in props)
                {
                    product.properties.Add(new PropertyAdd()
                    {
                        Name = item.property.Name,
                        Parameter = item.parameter.Value
                    });
                }
            
          
           
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductAdd ProductAdd)
        {

                Product _product = new Product();
                _product.ProductColor = ProductAdd.Color.Trim();
                _product.Title = ProductAdd.Title.Trim();
                _product.Price = ProductAdd.Price;
                _product.category = _context.Categories.Where(x => x.Id == ProductAdd.CategoryId).FirstOrDefault();
           
                Brand brand = _context.Brands.Where(x => x.BrandName == ProductAdd.Brand).FirstOrDefault();
                if (brand is null) {
                brand = new Brand();
                    brand.BrandName = ProductAdd.Brand;
                    _context.Brands.Add(brand);
                    _product.BrandProduct = brand;
                }
                else
                {
                    _product.BrandProduct = brand;
                }
            await _context.SaveChangesAsync();
            var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var fullPath = Path.Combine(pathToSave, Guid.NewGuid().ToString() + System.IO.Path.GetExtension(ProductAdd.Picture.FileName));
                var dbPath = Path.Combine(folderName, Guid.NewGuid().ToString() + System.IO.Path.GetExtension(ProductAdd.Picture.FileName));
                // var filePath = _env.ContentRootPath + "\\Files\\" + Guid.NewGuid() + System.IO.Path.GetExtension(category.Picture.FileName);
                using (var stream = System.IO.File.Create(dbPath))
                {
                    await ProductAdd.Picture.CopyToAsync(stream);
                }
                _product.ImagePath = dbPath;


            for (int i = 0; i < ProductAdd.properties.Count(); i++)
            {
               
                PropertyParameter propertyParameter = new PropertyParameter();
                if (ProductAdd.properties[i].Id == 0 )
                {

                    Parameters parameter = new Parameters()
                    {
                        Value = ProductAdd.properties[i].Parameter.Trim()
                    };
                    Property property = new Property()
                    {
                        Name = ProductAdd.properties[i].Name.Trim(),
                    };
                    propertyParameter.parameter = parameter;
                    propertyParameter.property = property;
                    _context.PropertyParameters.Add(propertyParameter);
                   
                }
                else
                {
                    Property prop = _context.properties.Where(x => x.Id == ProductAdd.properties[i].Id).FirstOrDefault();
                    Parameters parameter = new Parameters()
                    {
                        Value = ProductAdd.properties[i].Parameter.Trim()
                    };
                    propertyParameter.parameter = parameter;
                    propertyParameter.property = prop;

                }
                
                _product.characteristics.Add(propertyParameter);
            }
            _context.Products.Add(_product);
            _context.SaveChanges();
            
            
            
            return Ok(_product.Id);
            
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product =  _context.Products.Where(x=>x.Id==id).Include(x=>x.BrandProduct).Include(x => x.characteristics).ThenInclude(x => x.property)
                .Include(x => x.characteristics).ThenInclude(x => x.parameter).FirstOrDefault();
                if (product == null)
                {
                    return NotFound();
                }
                int count = _context.Products.Where(x => x.BrandProduct.Id == product.BrandProduct.Id).Count();
                if (count == 1)
                {
                    _context.Brands.Remove(_context.Brands.Where(x => x.Id == product.BrandProduct.Id).FirstOrDefault());
                }
                foreach (var item in product.characteristics)
                {
                    _context.parameters.Remove(item.parameter);
                   
                    if (_context.PropertyParameters.Include(x=>x.property).Where(x=>x.property.Id==item.property.Id).Count()==1)
                    {
                        _context.properties.Remove(item.property);
                    }
                    _context.PropertyParameters.Remove(item);
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                string path = product.ImagePath;
                _context.Products.Add(product);
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch(Exception ex)
            {
                string str = ex.Message;
            }

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
