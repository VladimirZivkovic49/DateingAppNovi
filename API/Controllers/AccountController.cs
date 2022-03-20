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
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
       /*  private readonly DataContext _context; (L205) */
       //(L205)
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
       //(L205)
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        /* public   AccountController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        } L(148) */

       /*  public   AccountController(DataContext context,ITokenService tokenService,IMapper mapper) (L205) */
        public   AccountController(UserManager<AppUser>userManager,SignInManager<AppUser>signInManager,ITokenService tokenService,IMapper mapper)
        
        {
           /* ( _context = context;(L205) */
           _userManager = userManager;
            _signInManager = signInManager;
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
             /*  using var hmac = new HMACSHA512();*/

               user.UserName = registerDto.Username.ToLower();
             /*  user. PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
               user. PasswordSalt = hmac.Key;
                                (L200)    */
             
            /* _context.Users.Add(user);
            await _context.SaveChangesAsync(); (L205)*/
            var result= await  _userManager.CreateAsync(user,registerDto.Password);
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);

            }
           /*  return user; */
            //(L207)
             var roleResult= await _userManager.AddToRoleAsync(user, "Member");
                if(!roleResult.Succeeded)
            {
                return BadRequest(result.Errors);

            }


           //(L207)
           
           
            return  new UserDto
            {
                Username = user.UserName,
               /*  Token = _tokenService.CreateToken(user), (L207) mora await zbog Task */
                Token = await _tokenService.CreateToken(user),
                
                KnownAs=user.KnownAs,
                //(L159)
                Gender=user.Gender
                //(L159)
            };
        }
[HttpPost("login")]
        public async Task<ActionResult<UserDto /* AppUser */>> Login(LoginDto loginDto /* string username, string password */)
        {

            /* var user = await _context.Users (L205) */
            var user = await _userManager.Users

            .Include(p=>p.Photos)
            /* .SingleOrDefaultAsync(x => x.UserName == loginDto.Username); (L205) */
           .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid Username");
           /*  using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password // password ));

            for (int i = 0; i < computedHash.Length; i++)

            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Wrong password");

            }  (L200) */
           /*  return user; */
            //(L205)
                var result =await  _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);
            //(L205)
           if(!result.Succeeded)
            {
                return Unauthorized();

            }
          
            return  new UserDto
            {
                Username = user.UserName,
               /*  Token = _tokenService.CreateToken(user), (L207) mora await zbog Task */
                Token = await _tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs=user.KnownAs,
                //(L159)
                Gender=user.Gender
                //(L159)
           
            };



        }


        private async Task<bool> UserExsists(string username)
        {
           /*  return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); (L205)*/
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        
        }

    }
}