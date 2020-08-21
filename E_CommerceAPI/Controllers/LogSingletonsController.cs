using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce_API.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogSingletonsController : ControllerBase
    {
        private readonly E_Commerce_StoreContext _context;

        public LogSingletonsController(E_Commerce_StoreContext context)
        {
            _context = context;
        }

        // GET: api/LogSingletons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogSingleton>>> GetLogSingleton()
        {
            return await _context.LogSingleton.Include(a => a.User).Include(b => b.Product).Include(c => c.User.Employees).Where(l => l.LogType == "Sale Completed").ToListAsync();
        }

        // GET: api/LogSingletons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogSingleton>> GetLogSingleton(int id)
        {
            var logSingleton = await _context.LogSingleton.FindAsync(id);

            if (logSingleton == null)
            {
                return NotFound();
            }

            return logSingleton;
        }

        [HttpPost]
        public async Task<ActionResult<LogSingleton>> PostLogSingleton(LogSingleton logSingleton)
        {
            _context.LogSingleton.Add(logSingleton);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogSingleton", new { id = logSingleton.LogId }, logSingleton);
        }

        // DELETE: api/LogSingletons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LogSingleton>> DeleteLogSingleton(int id)
        {
            var logSingleton = await _context.LogSingleton.FindAsync(id);
            if (logSingleton == null)
            {
                return NotFound();
            }

            _context.LogSingleton.Remove(logSingleton);
            await _context.SaveChangesAsync();

            return logSingleton;
        }

        private bool LogSingletonExists(int id)
        {
            return _context.LogSingleton.Any(e => e.LogId == id);
        }

    }
}
