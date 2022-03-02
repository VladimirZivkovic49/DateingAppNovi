using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool>SaveAllAsync();
        Task <IEnumerable<AppUser>>GetUsersAsync();
        Task <AppUser>GetUsersByIdAsync(int id);
         Task <AppUser>GetUsersByUserNameAsync(string username);
         /* Task <IEnumerable<MemberDto>>GetMembersAsync(); */
         Task <PagedList<MemberDto>>GetMembersAsync(UserParams userParams);
         Task<MemberDto> GetMemberAsync(string username);
    
    
    }
}