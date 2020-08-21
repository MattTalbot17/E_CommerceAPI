using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly E_Commerce_StoreContext _context;

        public SalesController(E_Commerce_StoreContext context)
        {
            _context = context;
        }

        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSale()
        {
            return await _context.Sale.Include(c => c.User).Include(c => c.Product).ToListAsync();
        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            var sale = await _context.Sale.FindAsync(id);

            if (sale == null)
            {
                return NotFound();
            }

            return sale;
        }

        // POST: api/Sales
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            sale.Product = _context.Product.Where(p => p.ProductId == sale.ProductId).SingleOrDefault();
            sale.User = _context.Users.Where(u => u.UserId == sale.UserId).SingleOrDefault();

            _context.Sale.Add(sale);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SaleExists(sale.SaleId))
                {
                    return Conflict();
                }
                else
                {

                    throw;
                }
            }
            LogSingleton log = new LogSingleton();
            LogSingletonsController logController = new LogSingletonsController(_context);

            log.LogDate = DateTime.Now;
            log.LogType = "Sale Completed";
            log.Product = sale.Product;
            log.ProductId = sale.ProductId;
            log.Quantity = sale.Quantity;
            log.User = sale.User;
            log.UserId = sale.UserId;

            await logController.PostLogSingleton(log);

            return CreatedAtAction("GetSale", new { id = sale.SaleId }, sale);
        }


        private bool SaleExists(int id)
        {
            return _context.Sale.Any(e => e.SaleId == id);
        }
    }
}
