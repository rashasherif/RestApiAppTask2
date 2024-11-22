using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRESTAPI.Data;
using TestRESTAPI.Models;

namespace TestRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductesController(AppDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.ToListAsync();
        }





        [HttpPost]
        public async Task<IActionResult> AddProduct(string product)
        {
            Product p = new() { Name = product };
            await _context.Products.AddAsync(p);
            _context.SaveChanges();
            return Ok(p);
        }

        [HttpPut]

        public async Task<IActionResult> UpdateProductes(Product product)
        {
            var p= await _context.Products.SingleOrDefaultAsync(x => x.Id == product.Id);
            if (p == null)
            {
                return NotFound($"product Id {product.Id} not exists");
            }
            p.Name = product.Name;
            _context.SaveChanges();
            return Ok(p);

        }

        [HttpDelete("id")]

        public async Task<IActionResult> RemoveProduct(int id)
        {
            var p = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
            if (p == null)
            {
                return NotFound($"product Id {id} not exists");
            }
            _context.Products.Remove(p);
            _context.SaveChanges();
            return Ok(p);

        }



    }
}
