using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public  MessageRepository(DataContext context,IMapper mapper)
       {
            _mapper = mapper;
            _context=context;
       }
       
       
        public void AddMessage(Message message)
        {
            _context.Add(message);
        }

        public void DeliteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
           /*  return await _context.Messages.FindAsync(id);(L197) */
               return await _context.Messages
               .Include(u=>u.Sender)
               .Include(u=>u.Recipient)
               .SingleOrDefaultAsync(x=>x.Id==id);
        
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query= _context.Messages.
            OrderByDescending(m=>m.MessageSent).AsQueryable();
            /*  query = messageParams.Container switch
             {
                 "Inbox"=> query.Where(u=>u.Recipient.UserName==messageParams.UserName),
                 "Outbox"=> query.Where(u=>u.Sender.UserName==messageParams.UserName),
                 _ => query.Where(u=>u.Recipient.UserName==messageParams.UserName && u.DateRead==null) 

             }; (L196)*/
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.UserName
                                        && u.RecipientDelited==false),
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.UserName
                                          && u.SenderDelited==false),   
                _ => query.Where(u => u.Recipient.UserName == messageParams.UserName &&  u.RecipientDelited==false && u.DateRead == null)

            }; 

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider); 
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber,messageParams.PageSize);
        
        }

        public async Task<IEnumerable<MessageDto>>
         GetMessagesThread(string currentUserName, string recipientUserName)
        {
            var messages= await _context.Messages
            .Include(u=>u.Sender).ThenInclude(p=>p.Photos)
            .Include(u=>u.Recipient).ThenInclude(p=>p.Photos)
            /* .Where(m=>m.Recipient.UserName==recipientUserName (L196)*/
           .Where(m=>m.Recipient.UserName==recipientUserName && m.RecipientDelited==false 
            && m.Sender.UserName==currentUserName
            || m.Recipient.UserName==currentUserName
            /* && m.Sender.UserName==recipientUserName) (L196)*/
            && m.Sender.UserName==recipientUserName && m.SenderDelited==false )
            .OrderBy(m=> m.MessageSent)
            .ToListAsync();
        
        var unreadMessages= messages
        .Where(m=>m.DateRead==null && m.Recipient.UserName==currentUserName)
        .ToList();
       
       if(unreadMessages.Any())
       {
         foreach(var message  in unreadMessages)
         {
                message.DateRead=DateTime.Now;

         }
             await _context.SaveChangesAsync();
       }
       
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
       
        }

        public async  Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync()>0;
        }
    }
}