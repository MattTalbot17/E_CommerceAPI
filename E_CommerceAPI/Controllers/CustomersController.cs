using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EncyrptionDLL;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class CustomersController : ControllerBase
    {
        private readonly E_Commerce_StoreContext _context;
        Class1 encrypt = new Class1();
        public CustomersController(E_Commerce_StoreContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
            return await _context.Customers.Include(c => c.User).ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomer(int id)
        {
            var customer =   _context.Customers.Where(a => a.UserId == id).Include(c => c.User).FirstOrDefault();

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // POST: api/Sales
        [HttpPost]
        public async Task<ActionResult<Customers>> PostCustomer(Customers customer)
        {
            try
            {
                customer.User.Password = encrypt.EncryptData(customer.User.Password, "ffhhgfgh");
                _context.Users.Add(customer.User);
                await _context.SaveChangesAsync();

                customer.UserId = _context.Users.Where(u => u.Username == customer.User.Username).Select(a => a.UserId).FirstOrDefault();
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                {
                    return Conflict();
                }
                else
                {

                    throw;
                }
            }
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customers>> EditCustomer(Customers customer)
        {
            customer.CustomerId = _context.Customers.Where(c => c.UserId == customer.UserId).Select(a => a.CustomerId).Single();
            customer.User.Password = _context.Users.Where(b => b.UserId == customer.User.UserId).Select(a => a.Password).Single();
            _context.Update(customer.User);
            _context.Update(customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw;
            }

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }


        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
