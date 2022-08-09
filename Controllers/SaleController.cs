using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CashRegister.Models;

namespace CashRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly CashRegisterContext _context;

        public SaleController(CashRegisterContext context)
        {
            _context = context;
        }

        // GET: api/Sale
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            if (_context.Sales == null)
            {
                return NotFound();
            }
            return await _context.Sales.Include("ProductSales.Product").ToListAsync();
        }

        // GET: api/Sale/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            if (_context.Sales == null)
            {
                return NotFound();
            }
            var sale = await _context.Sales.Include(s => s.ProductSales).FirstOrDefaultAsync(s => s.SaleId == id);

            if (sale == null)
            {
                return NotFound();
            }

            return sale;
        }

        // PUT: api/Sale/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale sale)
        {
            if (id != sale.SaleId)
            {
                return BadRequest();
            }

            _context.Entry(sale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
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

        // POST: api/Sale
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(SaleRequest saleRequest)
        {
            try
            {
                if (_context.Sales == null)
                {
                    return Problem("Entity set 'CashRegisterContext.Sales'  is null.");
                }
                var sale = new Sale
                {
                    ApartmentNumber = saleRequest.ApartmentNumber,
                    IsLoan = saleRequest.IsLoan,
                    Payment = saleRequest.Payment
                };

                var productIds = saleRequest.ProductSales.Select(ps => ps.ProductId).ToList();
                var products = await _context.Products.Where(p => productIds.Contains(p.ProductId))
                                                      .ToListAsync();

                sale.ProductSales = saleRequest.ProductSales.Select(productSaleRequest => GetProductSale(productSaleRequest, products)).ToList();
                sale.Total = sale.ProductSales.Sum(ps => ps.Quantity * ps.Price);
                sale.Date = DateTime.Now;

                if (sale.Payment < sale.Total || sale.IsLoan)
                {
                    throw new Exception("The total value is higher than the payment");
                }

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSale), new { id = sale.SaleId }, sale);
            }
            catch (System.Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }

        private ProductSale GetProductSale(ProductSaleRequest productSaleRequest, List<Product> products)
        {
            var product = products.Find(p => p.ProductId == productSaleRequest.ProductId && p.IsActive);
            if (product == null)
            {
                throw new Exception("Invalid product");
            }

            if (product.Quantity < productSaleRequest.Quantity)
            {
                throw new Exception($"Insuficcient inventory for product {product.Name}");
            }

            product.Quantity -= productSaleRequest.Quantity;
            _context.Entry(product).State = EntityState.Modified;

            return new ProductSale
            {
                Price = product.SalePrice,
                ProductId = productSaleRequest.ProductId,
                Quantity = productSaleRequest.Quantity
            };
        }

        // DELETE: api/Sale/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            if (_context.Sales == null)
            {
                return NotFound();
            }
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaleExists(int id)
        {
            return (_context.Sales?.Any(e => e.SaleId == id)).GetValueOrDefault();
        }
    }
}
