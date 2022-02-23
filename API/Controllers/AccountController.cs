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
using AutoMapper;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        /* public   AccountController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        } L(148) */

        public   AccountController(DataContext context,ITokenService tokenService,IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        } 


        [HttpPost("register")]
        public async Task<ActionResult<UserDto /* AppUser */>> Register(RegisterDto registerDto /*string username, string password */)
        {

            if (await UserExsists(registerDto.Username)) return BadRequest("Username is taken");


           /*  using var hmac = new HMACSHA512();

            var user = new AppUser
            {

                UserName = registerDto.Username.ToLower() //username ,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password   //password )),
                PasswordSalt = hmac.Key

            }; (L148) */
              var user =_mapper.Map<AppUser>(registerDto);
              using var hmac = new HMACSHA512();

               user.UserName = registerDto.Username.ToLower();
               user. PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
               user. PasswordSalt = hmac.Key;

             
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
           /*  return user; */
           
           
           
            return  new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs=user.KnownAs

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
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs=user.KnownAs
            };



        }


        private async Task<bool> UserExsists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}