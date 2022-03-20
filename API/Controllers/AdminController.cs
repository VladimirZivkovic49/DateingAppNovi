using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController:BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
      
      //(L209)
       public AdminController(UserManager<AppUser> userManager)
       {
            _userManager = userManager;

        

       }
        
        //(L209)
        
        
        
        [Authorize(Policy="RequreAdminRole")]
        [HttpGet("users-with-roles")]
       
            /*  public  ActionResult GetUsersWithRoles() (L209) */
        public async Task  <ActionResult> GetUsersWithRoles()
               
        {
              /*   return Ok("Only admin users can see this"); (L209) */

              var users = await _userManager.Users
              .Include(r=> r.UserRoles)
              .ThenInclude(r=>r.Role)
              .OrderBy(u=>u.UserName)
              .Select(u => new
              {
                u.Id,
                Username=u.UserName,
                Roles= u.UserRoles.Select(r=>r.Role.Name).ToList()

              })
              .ToListAsync();
        
                return Ok(users);
        }
       //(L210)
       
        [Authorize(Policy="RequreAdminRole")]
        [HttpPost("edit-roles/{username}")]
       
            /*  public  ActionResult GetUsersWithRoles() (L209) */
        public async Task  <ActionResult> EditRoles(string username, [FromQuery] string roles)
               
        {
            var selectedRoles = roles.Split(",").ToArray();             
            var user = await _userManager.FindByNameAsync(username);
            if(user==null)
            {
            
            return NotFound("Could not find user");

            }
            var userRoles= await _userManager.GetRolesAsync(user);
            var result= await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
       
            if(!result.Succeeded)
            {
            
            return BadRequest("Faild to add to roles");

            }
            result= await _userManager.RemoveFromRolesAsync(user, selectedRoles.Except(selectedRoles));
       
            if(!result.Succeeded)
            {
            
            return BadRequest("Faild to remove from roles");

            }
           
            return Ok( await _userManager.GetRolesAsync(user));
        
        }


      //(L210) 
       
        [Authorize(Policy="ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
       
             public  ActionResult GetPhotosForModeration()
        
        {
                return Ok("Admin or  moderator users can see this");
        }
    
    
    }
}