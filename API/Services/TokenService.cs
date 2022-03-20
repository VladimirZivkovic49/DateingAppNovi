using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
      
      private readonly SymmetricSecurityKey _key;
//(L207)
        private readonly UserManager<AppUser> _userManager;
//(L207)
        /*  public TokenService(IConfiguration config)(L207) */
        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
      
      {
          _key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
          _userManager = userManager;
        }
      
      
      
       /*  public async string CreateToken(AppUser user) (L207) mora da bude async */
        public async Task< string> CreateToken(AppUser user)
        
        {
              var claims= new List<Claim>
           {

           /*  new Claim(JwtRegisteredClaimNames.NameId,user.UserName)  */
           //(L163)
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
           //(163)

           };
          
          //(L207)
                var roles= await _userManager.GetRolesAsync(user);
               claims.AddRange(roles.Select(role=> new Claim(ClaimTypes.Role, role)));
         
          //(L207)
            var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature );

            var tokenDescripor= new SecurityTokenDescriptor
            {
                Subject =new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(7),
                SigningCredentials=creds
            };
            var tokenHandler= new JwtSecurityTokenHandler();
            
            var token= tokenHandler.CreateToken(tokenDescripor);
            return tokenHandler.WriteToken(token);
         
         
        }
    }
}