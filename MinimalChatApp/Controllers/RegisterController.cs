using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalChatApp.Data;
using MinimalChatApp.Dtos;
using MinimalChatApp.Models;
using NuGet.Common;

namespace MinimalChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly DataContext _context;
        private IMapper _mapper;

        public RegisterController(DataContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public static User user = new User();
        private readonly IConfiguration _configuration;


        [HttpPost("register")]
        public async Task<ActionResult<IEnumerable<User>>> Register(UserDto adduser)
        {
            CreatePasswordHash(adduser.UserPassword, out byte[] passwordHash, out byte[] passwordSalt);
            
            user.UserName = adduser.UserName;
            user.UserEmail = adduser.UserEmail;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Ok(user);
            //if (_context.Users == null)
            //{
            //    return NotFound();
            //}
            //return await _context.Users.ToListAsync();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto usr) 
        {
            if (user.UserName != usr.UserName) 
            {
                return BadRequest(" Login failed due to validation errors");
            }

            if (!VerifyPasswordHash(usr.UserPassword,user.PasswordHash,user.PasswordSalt)) 
            {
                return Unauthorized(" Login failed due to incorrect credentials");
            }

            string token = CreateToken(user);
            return Ok("Ok message returned.");
        }

      

        private string CreateToken(User user) 
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using (var hmac = new HMACSHA512()) 
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);

            }
        }

        //// GET: api/Register
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //  if (_context.Users == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Users.ToListAsync();
        //}

        //// GET: api/Register/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //  if (_context.Users == null)
        //  {
        //      return NotFound();
        //  }
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        //// PUT: api/Register/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.UserId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Register
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(UserDto user)
        //{
        //    var newUser = _mapper.Map<User>(user);
        //  if (_context.Users == null)
        //  {
        //      return Problem("Entity set 'DataContext.Users'  is null.");
        //  }
        //    _context.Users.Add(newUser);
        //    await _context.SaveChangesAsync();


        //    return CreatedAtAction("GetUser",  user);
        //}

        //// DELETE: api/Register/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    if (_context.Users == null)
        //    {
        //        return NotFound();
        //    }
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
