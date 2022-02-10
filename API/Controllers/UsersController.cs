using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    /* [ApiController]
    [Route("api/[controller]")] */
   [Authorize]
   
    public class UsersController :BaseApiController/* ControllerBase*/
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /* private readonly DataContext _context;

public UsersController(DataContext context)
{
_context = context;

} ( Kada se uvede repository ovo ne treba)*/





        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [HttpGet]
        /* [AllowAnonymous] */
        /*  public ActionResult<IEnumerable<AppUser>> GetUsers()
          {
              var users = _context.Users.ToList();
              return users;

          }*/
       /*  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() */
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users= await _userRepository.GetMembersAsync();
            /* var usersToReturn= _mapper.Map<IEnumerable<MemberDto>>(users); */
            return Ok(users) ;
           
           
           /*  return await _context.Users.ToListAsync(); */
           /*  return Ok(await _userRepository.GetUsersAsync()) ; */
           /*  var users= await _userRepository.GetUsersAsync();
            var usersToReturn= _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(usersToReturn) ; */
        }

        // api/users/id
       /*  [Authorize] */
       /*  [HttpGet("{id}")] */

        [HttpGet("{username}")]
        /*public ActionResult<AppUser> GetUser(int id)
        {
           / var user = _context.Users.Find(id);
             return user;     umesto linija 40 i41 može se napisati direktno kao u 42         /
             return _context.Users.Find(id);
        }*/
        /* public async Task<ActionResult<AppUser>> GetUser(int id) */
        /* public async Task<ActionResult<AppUser>> GetUser(string username) */
       /*  public async Task<ActionResult<MemberDto>> GetUser(string username) */
             public async Task<ActionResult<MemberDto>> GetMember(string username)
        
        {
            /* return await _context.Users.FindAsync(id); */
            /*  return await _userRepository.GetUsersByUserNameAsync(username); */
           /*  var users = await _userRepository.GetUsersByUserNameAsync(username); */
             return  await _userRepository.GetMemberAsync(username);
           /*  return _mapper.Map<MemberDto>(user); */
        }


    }

}