using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
          public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
         {
               services.AddScoped<ITokenService,TokenService>();
               services.AddScoped<IUserRepository,UserRepository>();
               services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
               string mySqlConnectionStr=configuration.GetConnectionString("DefaultConnection");
               services.AddDbContextPool<DataContext>(options=>options.UseMySQL(mySqlConnectionStr));
             
                 return services;
         }
    }
}