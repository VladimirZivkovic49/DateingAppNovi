using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub:Hub
    {
        /* private readonly IMessageRepository _messageRepository; (L239) */

        private readonly PresenceTracker _tracker ;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       /*  private readonly IUserRepository _userRepository; (L239) */
        private readonly IHubContext<PresenceHub> _presenceHub;

        /* public  MessageHub(IMessageRepository messageRepository, IMapper mapper)
{
_mapper = mapper;
_messageRepository = messageRepository;


}  (L226)*/
        /* public MessageHub(IMessageRepository messageRepository, IMapper mapper,
        IUserRepository userRepository) (L233) */
        //(L233)
       /*  public MessageHub(IMessageRepository messageRepository, IMapper mapper,
        IUserRepository userRepository,IHubContext<PresenceHub> presenceHub, PresenceTracker tracker)
      
       
        {
            _tracker=tracker;
            _mapper = mapper;
            _userRepository = userRepository;
            _presenceHub = presenceHub;
            _messageRepository = messageRepository;


        } */
        //(L233)
//(L239)
 public MessageHub( IMapper mapper,
        IHubContext<PresenceHub> presenceHub, PresenceTracker tracker , IUnitOfWork unitOfWork)
      
       
        {
            _tracker = tracker;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            _presenceHub = presenceHub;
            


        }
//(L239)
            public override async Task OnConnectedAsync()
        {
            var httpContext= Context.GetHttpContext();
            var otherUser= httpContext.Request.Query["user"].ToString();
            var groupName=GetGroupName(Context.User.GetUsername(),otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);
            //(231)
               /*  await AddToGroup(Context, groupName); (L235) */
            //(231)
//(L235)
            var group=await AddToGroup(groupName);
            await Clients.Group(group.Name).SendAsync("UpdatedGroup",group);
//(L235)
            /* var messages = await _messageRepository.GetMessagesThread(Context.User.GetUsername(),otherUser); (L239) */
            var messages = await _unitOfWork.MessageRepository.GetMessagesThread(Context.User.GetUsername(),otherUser); 
           /*  await Clients.Group(groupName).SendAsync("ReciveMessageThread",messages);(L235) */
            //(L235)
          //(L239)
          if( _unitOfWork.HasChanges())
          {
              await _unitOfWork.Complete();

          }
          //(L239)
           await Clients.Caller.SendAsync("ReciveMessageThread",messages);
            //(L235)
       
        }
              public override async Task OnDisconnectedAsync(Exception exception)
        {
          //(231)
               /*  await RemoveFromMessageGroup(Context.ConnectionId); (L235*/
            //(231)
          //(L235)
             var group= await RemoveFromMessageGroup();
             await Clients.Group(group.Name).SendAsync("UpdatedGroup",group);
          //(L235)
          
           await base.OnDisconnectedAsync(exception);
        }
          //(L226)
           public  async Task SendMessage(CreateMessageDto createMessageDto )
           {
            /* var username = User.GetUsername(); */
            var username = Context.User.GetUsername();
            if (username == createMessageDto.RecipientUsername.ToLower())
            {
               /*  return BadRequest("You cant send message to yourself"); */
                throw new HubException("You cant send message to yourself");
            
            }
            /* var sender = await _userRepository.GetUsersByUserNameAsync(username);  (L239)*/
            var sender = await _unitOfWork.UserRepository.GetUsersByUserNameAsync(username);
            /* var recipient = await _userRepository.GetUsersByUserNameAsync(createMessageDto.RecipientUsername); (L239) */
            var recipient = await _unitOfWork.UserRepository.GetUsersByUserNameAsync(createMessageDto.RecipientUsername);
            if (recipient == null)
            {
                /* return NotFound(); (L226) */
                 throw new HubException("Not found user");
            }
            var message = new Message // dodati Api.Enteties
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content

            };

            //(231)
                var groupName  = GetGroupName(sender.UserName,recipient.UserName);
                /* var group=await _messageRepository.GetMessageGroup(groupName); (L239) */
                var group=await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
                if(group.Connections.Any(x=> x.Username==recipient.UserName))
                {
                    message.DateRead=DateTime.UtcNow;

                }
            //(L231)


            //(L233)
                else
                {
                    var connections=await _tracker.GetConnectionsForUser(recipient.UserName);
                    if(connections !=null) 
                    {
                    await _presenceHub.Clients.Clients(connections).SendAsync
                    ("NewMessageRecived", new {username=sender.UserName, knownAs=sender.KnownAs});


                    }

                }
            //(L233)
           /*  _messageRepository.AddMessage(message); (L239)*/
                _unitOfWork.MessageRepository.AddMessage(message);
            /* if (await _messageRepository.SaveAllAsync()) (L239) */
            if (await _unitOfWork.Complete())
            {
               //(L226)
                  /*  var group  = GetGroupName(sender.UserName,recipient.UserName);(L231) */
               //(L226)
               
               /*  return Ok(_mapper.Map<MessageDto>(message));(L226) */
               /* await Clients.Group(group).SendAsync("NewMessage",_mapper.Map<MessageDto>(message));(L231) */
                //(L231)
                await Clients.Group(groupName).SendAsync("NewMessage",_mapper.Map<MessageDto>(message));
                //(L231)
            }
            /* return BadRequest("Faild to send message"); */

           }
          //(L226)
          
          //(231)
           /*  private async Task<bool>AddToGroup(//HubCallerContext context ne treba/string groupName)  (L235*/
            private async Task<Group>AddToGroup(string groupName)
            
            {
                   /*  var group= await _messageRepository.GetMessageGroup(groupName); (L239)*/
                    var group= await _unitOfWork.MessageRepository.GetMessageGroup(groupName);

                    var connection= new Connection(Context.ConnectionId, Context.User.GetUsername());

                    if(group==null)
                    {
                        group=new Group(groupName);
                        /* _messageRepository.AddGroup(group);(L239) */
                        _unitOfWork.MessageRepository.AddGroup(group);
                    }
                    group.Connections.Add(connection);
                    /* return await _messageRepository.SaveAllAsync(); (L235) */
           //(L235)
                /* if( await _messageRepository.SaveAllAsync())  (L239)*/
                if( await _unitOfWork.Complete())
                {
                        return group;
                       
                }
           //(L235)
                    throw new HubException("Failed to join the group");
            
            }
         /* private async Task RemoveFromMessageGroup(//string connectionId kasnije izbrisano jer može kroz Context) (L235) */
        private async Task<Group> RemoveFromMessageGroup()
        
         {
                /* var connection= await _messageRepository.GetConnection(connectionId); (L235) */
                //(L235)
                /* var connection= await _messageRepository.GetConnection(Context.ConnectionId);(L235) */
               /*  var group= await _messageRepository.GetGroupForConnection(Context.ConnectionId); (L239)*/
               var group= await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
               
                var connection = group.Connections.FirstOrDefault(x=>x.ConnectionId==Context.ConnectionId);
                //(L235)
               /*  _messageRepository.RemoveConnection(connection); (L239) */
               _unitOfWork.MessageRepository.RemoveConnection(connection);
              /* if(await _messageRepository.SaveAllAsync()) (L239) */
              if(await _unitOfWork.Complete())
              {
                  return group;

              }
               throw new HubException("Failed to remove from the group");
                /* await _messageRepository.SaveAllAsync(); (L235)*/
         }
         
         
          //(231)
            private string GetGroupName(string caller,string other)
            {

            var stringComapare= string.CompareOrdinal(caller,other)<0;
            return stringComapare ? $"{caller}-{other}":$"{other}-{caller}";
            }
    }
    
    }

