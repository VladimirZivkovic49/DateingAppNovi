using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public   AccountController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto /* AppUser */>> Register(RegisterDto registerDto /*string username, string password */)
        {

            if (await UserExsists(registerDto.Username)) return BadRequest("Username is taken");


            using var hmac = new HMACSHA512();

            var user = new AppUser
            {

                UserName = registerDto.Username.ToLower()/* username */,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password /*  password */)),
                PasswordSalt = hmac.Key

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
           /*  return user; */
           
           
           
            return  new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)

            };
        }
[HttpPost("login")]
        public async Task<ActionResult<UserDto /* AppUser */>> Login(LoginDto loginDto /* string username, string password */)
        {

            var user = await _context.Users
            .Include(p=>p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null) return Unauthorized("Invalid Username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password /* password */));

            for (int i = 0; i < computedHash.Length; i++)

            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Wrong password");

            }
           /*  return user; */
            
            return  new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url

            };



        }


        private async Task<bool> UserExsists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}