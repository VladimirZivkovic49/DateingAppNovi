using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Helpers;
using API.DTOs;
namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);

        void DeliteMessage(Message message);
        Task<Message> GetMessage(int id);
       /*  Task<PagedList<MessageDto>> GetMessagesForUser(); */
       //(L185)
         Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
       //(L185)
        /* Task<IEnumerable<MessageDto>> GetMessagesThread(int currentUserId, int recipientId); */
        //(L186)
        Task<IEnumerable<MessageDto>> GetMessagesThread(string currentUsername, string recipientUsername);
        //(L186)
        
        Task<bool> SaveAllAsync();
    }
}