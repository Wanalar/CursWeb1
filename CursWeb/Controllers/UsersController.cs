﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CursLib.Models;

namespace CursWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Avto_VakzalContext _context;
        private int input;
        public UsersController(Avto_VakzalContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpPost("ListUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.Include(s => s.RoleNavigation).ToListAsync();
        }

        [HttpPost("Auth")]
        public async Task<User> AuthUser(Auth auth)
        {
            var login = await _context.Users.Include(s => s.RoleNavigation).FirstOrDefaultAsync(s => s.Login == auth.Login && s.Password == auth.Password);

            return login ?? new User();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("put")]
        public async Task<IActionResult> PutUser([FromBody] User user)
        {

            input = user.UserId;

            var origin = _context.Users.Find(input);

            _context.Entry(origin).CurrentValues.SetValues(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(input))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public User User { get; set; }
        [HttpPost("SaveUser")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var origin = await _context.Users.FirstOrDefaultAsync( s => s.Email == user.Email || s.Login == user.Login);
            
            if(origin == null)
            {
                this.User = new User() { Role = 3, Password = user.Password, FirstName = user.FirstName, SecondName = user.SecondName, Patronymic = user.Patronymic, Email= user.Email, PhonNumber = user.PhonNumber, Login = user.Login };
                _context.Users.Add(User);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("Пользователь с таким логином или почтой уже существует!");
            }
            return User;
        }

        // DELETE: api/Users/5
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
