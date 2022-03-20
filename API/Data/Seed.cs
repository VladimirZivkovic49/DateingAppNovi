using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
       /*  public static async Task SeedUsers(DataContext context) (L204) */
        /* public static async Task SeedUsers(UserManager<AppUser> userManager) L(205)*/
        public static async Task SeedUsers(UserManager<AppUser> userManager,
         RoleManager<AppRole> roleManager) 
        {
           /*  if(await context.Users.AnyAsync()) return;(L204) */
           if(await userManager.Users.AnyAsync())
           {

                return;
           }
           
           
           
           
            var userData=  await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if(users==null)
            {
                  return;

            }
               var roles= new List<AppRole>
               {
                  new AppRole{Name = "Member"},
                  new AppRole{Name = "Admin"},
                  new AppRole{Name = "Moderator"}

               };
            foreach (var role in roles)
            {
               
               {
                  await roleManager.CreateAsync(role);

               }
           
            }
            foreach (var user in users)
            {
               /*  using var hmac = new HMACSHA512(); (L200) */
           
                user.UserName= user.UserName.ToLower();
                /* user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes("PaSSw0rd"));
                user.PasswordSalt=hmac.Key;  L(200)*/
             /*  await  context.Users.AddAsync(user); (L204) */
                 await  userManager.CreateAsync(user, "PaSSw0rd");
              //(L205)
                 await  userManager.AddToRoleAsync(user,"Member"); 
              //(L205)
            }
               /*  await context.SaveChangesAsync(); (L204) preuzima userManager u liniji 42*/
        
            var admin= new AppUser
            {
                  UserName="admin"

            };
            
            await userManager.CreateAsync(admin, "PaSSw0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Admin","Moderator"});
        
        }
           
    }
}