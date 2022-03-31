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
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context,IMapper mapper)
       {
            _mapper = mapper;
            _context=context;
           
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
             
             
              return  await _context.Users.
              Where(x=>x.UserName==username).
              ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
              
              
              /* Select(user=>new MemberDto
              {
              }).SingleOrDefaultAsync(); */
        }

        /* public async Task<IEnumerable<MemberDto>> GetMembersAsync(UserParams userParams)(L153) */
       public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
       
        {
            /* return await _context.Users.

             ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync(); (L154)*/
            /* ( var query=  _context.Users.
                  ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking();(L157) */
             //(L157)

                var query=  _context.Users.AsQueryable();
                query= query.Where(u=> u.UserName!=userParams.CurrentUserName);
                query=query.Where(u=>u.Gender==userParams.Gender);
             //(L157)
             //(L158)
               var minDob= DateTime.Now.AddYears(-userParams.maxAge-1);
               var maxDob= DateTime.Now.AddYears(-userParams.minAge);
               query= query.Where(u=> u.DateOfBirth>=minDob && u.DateOfBirth<=maxDob);
             //(L158)
             //(L161)
                query= userParams.OrderBy switch
                {

                    "created"=>query.OrderByDescending(u=>u.Created),
                    _=>query.OrderByDescending(u=>u.LastActive)
                };

             //(161)

            /*  return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize );(L157) */
        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                                                   .AsNoTracking(),userParams.PageNumber, userParams.PageSize );
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            
            
            /* return  await _context.Users.ToListAsync(); */
              return  await _context.Users.
              Include(p=>p.Photos).
              ToListAsync();
        }

        public async Task<AppUser> GetUsersByIdAsync(int id)
        {
           return  await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUsersByUserNameAsync(string username)
        {
            /*  return  await _context.Users.SingleOrDefaultAsync(x=>x.UserName==username ); */
             return  await _context.Users.
             Include(p=>p.Photos).
             SingleOrDefaultAsync(x=>x.UserName==username );
        
        }

       /*  public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync()>0;
        } (L238) */

        public void Update(AppUser user)
        {
           _context.Entry(user).State=EntityState.Modified;
        }
    }
}