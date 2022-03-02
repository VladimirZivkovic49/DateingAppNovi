using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext=await next();

            if(!resultContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }
            /* var username= resultContext.HttpContext.User.GetUsername();
            var repo =resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user= await repo.GetUsersByUserNameAsync(username);
            user.LastActive=DateTime.Now;
            await repo.SaveAllAsync(); */
           
       //(L163)
            var userId = resultContext.HttpContext.User.GetUserId();
            var repo =resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user= await repo.GetUsersByIdAsync(userId);
            user.LastActive=DateTime.Now;
            await repo.SaveAllAsync();
       //(L163)
       
        }
    }
}