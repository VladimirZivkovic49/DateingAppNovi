using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

       public UsersController(DataContext context)
        {
            _context = context;

        }
        [HttpGet]
        /* [AllowAnonymous]*/
        /*  public ActionResult<IEnumerable<AppUser>> GetUsers()
          {
              var users = _context.Users.ToList();
              return users;

          }*/
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();


        }

        // api/users/id
        /*[Authorize]*/
        [HttpGet("{id}")]


        /*public ActionResult<AppUser> GetUser(int id)
        {
           / var user = _context.Users.Find(id);
             return user;     umesto linija 40 i41 može se napisati direktno kao u 42         /
           return _context.Users.Find(id);
        }*/
public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);


        }


    }

}