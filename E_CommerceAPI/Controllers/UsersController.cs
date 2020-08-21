using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce_API.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using EncyrptionDLL;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IConfiguration _config;
        private readonly E_Commerce_StoreContext _context;
        static int userID;
        Class1 encrypt = new Class1();
        public UsersController(IConfiguration config, E_Commerce_StoreContext context)
        {
            _config = config;
            _context = context;
        }
        [HttpPost]
        public ActionResult<String> LoginCheck(Users user)
        {
            if (user != null && user.Username != null && user.Password != null)
            {
                try
                {
                    string password = encrypt.EncryptData(user.Password, "ffhhgfgh");
                    Users temp = _context.Users.Where(u => u.Username== user.Username && u.Password== encrypt.EncryptData(user.Password, "ffhhgfgh")).Single();
                    //create claims details based on the user information
                     var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    userID = temp.UserId;
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                catch (InvalidOperationException e)
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            else
            {
                return BadRequest("Missing Credentials");
            }
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.Include(a => a.Customers).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        private bool UsersExist(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        [HttpGet]
        public async Task<ActionResult<Users>> UserID()
        {
            var user =  _context.Users.Include(a => a.Employees).Include(a => a.Customers).Where(a => a.UserId == userID).Single();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
