using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController:BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private ILikeRepository _likesRepository;

        public   LikesController(IUserRepository userRepository,ILikeRepository likesRepository)
        {
            
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        } 
    
     [HttpPost("{username}")]
       public async Task <ActionResult> AddLike(string username)
        {
        var sourceUserId=User.GetUserId();
        var likeUser= await _userRepository.GetUsersByUserNameAsync(username);
        var sourceUser=await _likesRepository.GetUserWithLikes(sourceUserId);
        
        if(likeUser==null)
        {
            return NotFound();
        }
        if(sourceUser.UserName==username)
        {
            return BadRequest("You cannot like Yourself");
        }
         var userLike= await _likesRepository.GetUserLike(sourceUserId,likeUser.Id);
            if (userLike != null)
            {
                return BadRequest("You already liked this user");
            }
             userLike = new UserLike
                {
                    SourceUserId=sourceUserId,
                    LikedUserId=likeUser.Id
                };
            sourceUser.LikedUsers.Add(userLike);
        if (await _userRepository.SaveAllAsync())
            {
                return Ok();
            }
             return BadRequest("Failed to like User");
       
        }
       [HttpGet]
       
        /* public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
        {
      
           var users= await _likesRepository.GetUserLikes(predicate , User.GetUserId());
            return Ok(users);
        
        } (L177)*/
       public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId=User.GetUserId();
            var users= await _likesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);
            return Ok(users);
        
        } 
       
       }
        
     
    }