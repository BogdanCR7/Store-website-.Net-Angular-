using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using WebApplication1.Data;
using WebApplication1.Extention;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IDistributedCache _cache;
        public CategoryController(ApplicationContext context, IWebHostEnvironment env, IDistributedCache cache)
        {
            _context = context;
            _env = env;
            _cache = cache;
        }

       

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
        //    string recordkey="Categories"+ DateTime.Now.ToString("dd/MM/yyyy");
            
            
         //   Category[] categories =await _cache.GetRecordAsync<Category[]>(recordkey);
        //    if (categories is null)
        //    {
               Category[] data= await _context.Categories.ToArrayAsync();
             //  await _cache.SetRecordAsync<Category[]>(recordkey, data, null, null);
                return data;
        //    }

           // return categories;
        }
        [HttpGet("GetCrud")]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCrud()
        {
            //    string recordkey="Categories"+ DateTime.Now.ToString("dd/MM/yyyy");


            //   Category[] categories =await _cache.GetRecordAsync<Category[]>(recordkey);
            //    if (categories is null)
            //    {
            List<CategoryViewModel> data = await _context.Categories.Select(x=>new CategoryViewModel
            {
                Id=x.Id,
                Count=x.products.Count(),
                Title=x.Title,
                ImagePath = x.ImagePath
            }).ToListAsync();
            //  await _cache.SetRecordAsync<Category[]>(recordkey, data, null, null);
            return data;
            //    }

            // return categories;
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutCategory(int id, [FromForm] CategoryAdd category)
        {

           var editcategory= _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            _context.Entry(editcategory).State = EntityState.Modified;
            if (editcategory == null)
            {
                return NotFound();
            }
            if (category.Picture != null)
            {
                string path = editcategory.ImagePath;



                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
                var folderName = Path.Combine("Resources", "Images");

                var dbPath = Path.Combine(folderName, Guid.NewGuid().ToString() + System.IO.Path.GetExtension(category.Picture.FileName)).Replace("\\", "/");
                // var filePath = _env.ContentRootPath + "\\Files\\" + Guid.NewGuid() + System.IO.Path.GetExtension(category.Picture.FileName);
                using (var stream = System.IO.File.Create(dbPath))
                {
                    await category.Picture.CopyToAsync(stream);
                }
                editcategory.ImagePath = dbPath;
            }
            editcategory.Title = category.Title;
            _context.Categories.Update(editcategory);
            await _context.SaveChangesAsync();

            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles ="admin")]
        [HttpPost]    
        public async Task<ActionResult<Category>> PostCategory([FromForm] CategoryAdd category)
        {
            if (ModelState.IsValid)
            {

                var folderName = Path.Combine("Resources", "Images");
               
                var dbPath = Path.Combine(folderName, Guid.NewGuid().ToString() + System.IO.Path.GetExtension(category.Picture.FileName)).Replace("\\", "/");
                // var filePath = _env.ContentRootPath + "\\Files\\" + Guid.NewGuid() + System.IO.Path.GetExtension(category.Picture.FileName);
                using (var stream = System.IO.File.Create(dbPath))
                {
                    await category.Picture.CopyToAsync(stream);
                }

                _context.Categories.Add(new Category() { ImagePath = dbPath, Title = category.Title });
                await _context.SaveChangesAsync();
                string recordkey = "Categories" + DateTime.Now.ToString("dd/MM/yyyy");


            //    Category[] categories = await _cache.GetRecordAsync<Category[]>(recordkey);
              //  if (categories is not null)
             //   {
               //     
               //     await _cache.SetRecordAsync<Category[]>(recordkey, null, null, null);
                    
               // }
                return Ok();
            }
            return BadRequest();

            //            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }
        // DELETE: api/Category/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            if (category.products.Count()>0)
            {
                return BadRequest();
            }
            string path=category.ImagePath;
            

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
