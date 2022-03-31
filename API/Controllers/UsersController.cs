using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    /* [ApiController]
    [Route("api/[controller]")] */
   [Authorize]
   
    public class UsersController :BaseApiController/* ControllerBase*/
    {
       /*  private readonly IUserRepository _userRepository; (L239) */
        private readonly IMapper _mapper;
       private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;

        /* private readonly DataContext _context;

public UsersController(DataContext context)
{
_context = context;

} ( Kada se uvede repository ovo ne treba)*/


        /*  public UsersController(IUserRepository userRepository,IMapper mapper)
         {
             _userRepository = userRepository;
             _mapper = mapper;
         } */
        /*  public UsersController(IUserRepository userRepository,
         IMapper mapper, IPhotoService photoService)
         {
             _userRepository = userRepository;
             _mapper = mapper;
             _photoService=photoService;

         }  (L239)*/

        public UsersController(
        IMapper mapper, IPhotoService photoService, IUnitOfWork unitOfWork)
        {
           
            _mapper = mapper;
            _photoService=photoService;
            _unitOfWork = unitOfWork;
        } 
       
       /* [Authorize(Roles ="Member")] //bilo "Admin" */
        [HttpGet]
        /* [AllowAnonymous] */
        /*  public ActionResult<IEnumerable<AppUser>> GetUsers()
          {
              var users = _context.Users.ToList();
              return users;

          }*/
       /*  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() */
        /* public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()(L154) */
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            /* var users= await _userRepository.GetMembersAsync() (L154); */
           //(L157)
           /*  var user=await _userRepository.GetUsersByUserNameAsync(User.GetUsername()); (L239) */
             var user=await _unitOfWork.UserRepository.GetUsersByUserNameAsync(User.GetUsername());
            userParams.CurrentUserName=user.UserName;
            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender=user.Gender=="female" ? "male": "female";

            }
           //(L157)
           
            /* var users= await _userRepository.GetMembersAsync(userParams);(L239) */
                    var users= await _unitOfWork.UserRepository.GetMembersAsync(userParams);
            /* var usersToReturn= _mapper.Map<IEnumerable<MemberDto>>(users); */
           
           Response.AddPaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount
                                        ,users.TotalPages);
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

        /* [HttpGet("{username}")] (L129) */
       
      /*  [Authorize(Roles ="Member")] */
        [HttpGet("{username}",Name ="GetMember")]
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
             /* return  await _userRepository.GetMemberAsync(username);  (L239)*/
             return  await _unitOfWork.UserRepository.GetMemberAsync(username);
           /*  return _mapper.Map<MemberDto>(user); */
        }
[HttpPut]
 public async Task<ActionResult<MemberUpdateDto>> UpdateUser(MemberUpdateDto memberUpdateDto)
      {
         /*  var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value; */
         var username=User.GetUsername();
          /* var user=await _userRepository.GetUsersByUserNameAsync(username);  (L239)*/
           var user=await _unitOfWork.UserRepository.GetUsersByUserNameAsync(username); 
         _mapper.Map(memberUpdateDto,user);
         /* _userRepository.Update(user);  (239)*/
         _unitOfWork.UserRepository.Update(user);

         /* if(await _userRepository.SaveAllAsync()) (L239) */
         if(await _unitOfWork.Complete())
         
         {
             return NoContent();

         }
            else
            {
                return BadRequest("Fail tio update user");

            }


       }
    [HttpPost("add-photo")]
    public async Task <ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {

           /*  var user = await _userRepository.GetUsersByUserNameAsync(User.GetUsername()); (L239)
            */
            var user = await _unitOfWork.UserRepository.GetUsersByUserNameAsync(User.GetUsername());
            var result = await _photoService.AddPhotosAsync(file);
            if(result.Error!= null)
            {

            return BadRequest(result.Error.Message);

            }
            var photo= new Photo
            {

                Url = result.SecureUrl.AbsoluteUri,
                PublicId=result.PublicId
            };
       
            if(user.Photos.Count==0)
            {
                photo.IsMain=true;

            }
            user.Photos.Add(photo);
           /*  if(await _userRepository.SaveAllAsync()) (L239)*/
           if(await _unitOfWork.Complete())
            {
                /* return _mapper.Map<PhotoDto>(photo); (L129) */
                /* return CreatedAtRoute("GetMember",_mapper.Map<PhotoDto>(photo));(L129a) */
            return CreatedAtRoute("GetMember",new {username=user.UserName} ,_mapper.Map<PhotoDto>(photo));
            }
            else
            {
               return BadRequest ("Problem with uploading photos");
            }
       
        }
    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        /* var user= await _userRepository.GetUsersByUserNameAsync(User.GetUsername()); (L239) */
      var user= await _unitOfWork.UserRepository.GetUsersByUserNameAsync(User.GetUsername());
        var photo=user.Photos.FirstOrDefault(x=> x.Id==photoId);
        if(photo.IsMain)
        {
            return BadRequest("Already is main photo");

        }
        var currentMain=user.Photos.FirstOrDefault(x=>x.IsMain);
        if(currentMain!=null)
        {
            currentMain.IsMain=false;

        }
    
        photo.IsMain=true;
   
    /* if(await _userRepository.SaveAllAsync())  (L239)*/
    if(await _unitOfWork.Complete())
         {
             return NoContent();

         }
            else
            {
                return BadRequest("Failed to set main photo");

            }
    
    
    }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DelitePhoto(int photoId)
        {
            /* var user = await _userRepository.GetUsersByUserNameAsync(User.GetUsername()); (L239)*/
            var user = await _unitOfWork.UserRepository.GetUsersByUserNameAsync(User.GetUsername());
            
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null)
            {
                return NotFound();
            }

            if (photo.IsMain)
            {
                return BadRequest("This is main photo");
            }

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotosAsync(photo.PublicId);

                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);

                }

            }

            user.Photos.Remove(photo);

             /* if (await _userRepository.SaveAllAsync())  (L239)*/
             if (await _unitOfWork.Complete())
            {
                   return Ok();  
            }
             return BadRequest("Faild to delite photo");
      
      }

   }
}
