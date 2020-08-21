using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce_API.Models;
using Microsoft.AspNetCore.Authorization;
using EncyrptionDLL;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly E_Commerce_StoreContext _context;
        Class1 encrypt = new Class1();
        public EmployeesController(E_Commerce_StoreContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employees>>> GetEmployees()
        {
            return await _context.Employees.Include(c => c.User).ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employees>> GetEmployees(int id)
        {
            var employees =  _context.Employees.Where(a => a.UserId == id).Include(c => c.User).FirstOrDefault();

            if (employees == null)
            {
                return NotFound();
            }

            return employees;
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employees>> PostEmployees(Employees employees)
        {
            try
            {
                employees.User.Password = encrypt.EncryptData(employees.User.Password, "ffhhgfgh");
                _context.Users.Add(employees.User);
                await _context.SaveChangesAsync();

                employees.UserId = _context.Users.Where(u => u.Username == employees.User.Username).Select(a => a.UserId).FirstOrDefault();
                _context.Employees.Add(employees);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeesExists(employees.EmployeeId))
                {
                    return Conflict();
                }
                else
                {

                    throw;
                }
            }

            return CreatedAtAction("GetEmployees", new { id = employees.EmployeeId }, employees);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employees>> DeleteEmployees(int id)
        {
            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();

            return employees;
        }

        [HttpPost]
        public async Task<ActionResult<Employees>> EditEmployee(Employees employees)
        {
            employees.EmployeeId = _context.Employees.Where(c => c.UserId == employees.UserId).Select(a => a.EmployeeId).Single();
            employees.User.Password = _context.Users.Where(b => b.UserId == employees.User.UserId).Select(a => a.Password).Single();
            _context.Update(employees.User);
            _context.Update(employees);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw;
            }

            return CreatedAtAction("GetEmployees", new { id = employees.EmployeeId }, employees);
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
