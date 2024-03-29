using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
               //(L202)
               services.AddIdentityCore<AppUser>(opt =>
           {
                opt.Password.RequireNonAlphanumeric=false;
                  
               
           })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddRoleValidator<RoleValidator<AppRole>>()
                .AddEntityFrameworkStores<DataContext>();
              //(L202)
            
               services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
           {
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer=false,
                    ValidateAudience=false
                };
          //(L221)
                    options.Events= new JwtBearerEvents
                    {
                        OnMessageReceived = context => 
                        {
                        var  accessToken = context.Request.Query["access_token"]; 

                        var path = context.Request.Path;

                        if(!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                                context.Token=accessToken;

                        }
                            return  Task.CompletedTask;
                    
                        }
                    
                    };
                    //(L221)
         
         
           });
           //(L208)
            services.AddAuthorization(opt =>
           {
                opt.AddPolicy("RequreAdminRole", policy=>policy.RequireRole("Admin"));
                opt.AddPolicy("ModeratePhotoRole", policy=>policy.RequireRole("Admin","Moderator"));
           });
            //(L208)

            return services;
       
       
       
       
        }
    }
}