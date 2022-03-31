using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
          public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
         {
              //(L223)
              services.AddSingleton<PresenceTracker>();
              //(L223)
               services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
               services.AddScoped<ITokenService,TokenService>();
               services.AddScoped<IPhotoService,PhotoService>();
                //(L238)
              services.AddScoped<IUnitOfWork,UnitOfWork>();
                //(L238)
               //(L172)
                /* services.AddScoped<ILikeRepository,LikesRepository>();  (L238)*/
               //(L172)
              //(182)
                /* services.AddScoped<IMessageRepository,MessageRepository>(); (L238)*/

              //(182)
               //(L162)
                services.AddScoped<LogUserActivity>();
               //(L162)
               /* services.AddScoped<IUserRepository,UserRepository>();(L238) */
               services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
               string mySqlConnectionStr=configuration.GetConnectionString("DefaultConnection");
               services.AddDbContextPool<DataContext>(options=>options.UseMySQL(mySqlConnectionStr));
             
                 return services;
         }
    }
}