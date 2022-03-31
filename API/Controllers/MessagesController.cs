using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController:BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        /*  private readonly IUserRepository _userRepository;
private readonly IMessageRepository _messageRepository;  (L239)*/
        private readonly IMapper _mapper;

       /*  public  MessagesController(IUserRepository userRepository, IMessageRepository messageRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        } (L239) */
   public  MessagesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
  [HttpPost]
       public async Task <ActionResult<MessageDto>>CreateMessage(CreateMessageDto createMessageDto)
        {
                var username = User.GetUsername();

                if(username==createMessageDto.RecipientUsername.ToLower())
                {
                    return BadRequest("You cant send message to yourself");

                }
               /*  var sender =await _userRepository .GetUsersByUserNameAsync(username);
                var recipient =await _userRepository .GetUsersByUserNameAsync(createMessageDto.RecipientUsername);(L239) */
                var sender =await _unitOfWork.UserRepository .GetUsersByUserNameAsync(username);
                var recipient =await _unitOfWork.UserRepository.GetUsersByUserNameAsync(createMessageDto.RecipientUsername);
                if(recipient==null)
                {
                    return NotFound();

                }
                var message =new Message
                {
                    Sender=sender,
                    Recipient=recipient,
                    SenderUsername=sender.UserName,
                    RecipientUsername=recipient.UserName,
                    Content=createMessageDto.Content
                
                };
                /* _messageRepository.AddMessage(message); (L239)*/
                   _unitOfWork.MessageRepository.AddMessage(message);
                /* if(await _messageRepository.SaveAllAsync())  (L239)*/
                if(await _unitOfWork.Complete())
                {
                    return Ok(_mapper.Map<MessageDto>(message));

                }
                    return BadRequest("Faild to send message");
        }
            [HttpGet]
       
       public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser(
           [FromQuery]MessageParams messageParams)
        {
            messageParams.UserName=User.GetUsername();
           /*  var messages= await _messageRepository.GetMessagesForUser(messageParams); (L239) */
             var messages= await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage,messages.PageSize,
            messages.TotalCount,messages.TotalPages);
            return messages;

        }
       /*  [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesThread(
        string username)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _messageRepository.GetMessagesThread(currentUsername, username));

        }  (L239) funkcionalnost obezbeđena kroz SignalR*/
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeliteMessage(
            int id)
        {

                var username=User.GetUsername() ;
               /*  var message= await _messageRepository.GetMessage(id);  (L239)*/
                var message= await _unitOfWork.MessageRepository.GetMessage(id);
                if(message.Sender.UserName != username && message.Recipient.UserName != username)
                {
                    return Unauthorized();

                }
                if(message.Sender.UserName == username )
                {
                    message.SenderDelited=true;

                }
            if (message.Recipient.UserName == username)
            {
                message.RecipientDelited= true;

            }

            if (message.SenderDelited  &&  message.RecipientDelited)
            {
              /*  _messageRepository.DeliteMessage(message); (L239) */
                    _unitOfWork.MessageRepository.DeliteMessage(message);
            }
            /* if (await _messageRepository.SaveAllAsync()) (L239) */
           if (await _unitOfWork.Complete())
           
            {
                return Ok();

            }
        else
        {
            return BadRequest("problem with delitig  messages ");

        }
        
        }


    }
}