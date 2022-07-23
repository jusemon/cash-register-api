using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CashRegister.Models;

namespace CashRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSaleController : ControllerBase
    {
        private readonly CashRegisterContext _context;

        public ProductSaleController(CashRegisterContext context)
        {
            _context = context;
        }

        // GET: api/ProductSale
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSale>>> GetProductSales()
        {
          if (_context.ProductSales == null)
          {
              return NotFound();
          }
            return await _context.ProductSales.ToListAsync();
        }

        // GET: api/ProductSale/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSale>> GetProductSale(int id)
        {
          if (_context.ProductSales == null)
          {
              return NotFound();
          }
            var productSale = await _context.ProductSales.FindAsync(id);

            if (productSale == null)
            {
                return NotFound();
            }

            return productSale;
        }

        // PUT: api/ProductSale/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductSale(int id, ProductSale productSale)
        {
            if (id != productSale.ProductSaleId)
            {
                return BadRequest();
            }

            _context.Entry(productSale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductSaleExists(id))
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

        // POST: api/ProductSale
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductSale>> PostProductSale(ProductSale productSale)
        {
          if (_context.ProductSales == null)
          {
              return Problem("Entity set 'CashRegisterContext.ProductSales'  is null.");
          }
            _context.ProductSales.Add(productSale);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductSale), new { id = productSale.ProductSaleId }, productSale);
        }

        // DELETE: api/ProductSale/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductSale(int id)
        {
            if (_context.ProductSales == null)
            {
                return NotFound();
            }
            var productSale = await _context.ProductSales.FindAsync(id);
            if (productSale == null)
            {
                return NotFound();
            }

            _context.ProductSales.Remove(productSale);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductSaleExists(int id)
        {
            return (_context.ProductSales?.Any(e => e.ProductSaleId == id)).GetValueOrDefault();
        }
    }
}
