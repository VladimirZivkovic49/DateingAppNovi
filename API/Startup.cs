using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;
using API.Extensions;
using API.MiddleWare;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration /*configuration*/config)
        {
            _config = config;

            /* Configuration = configuration;*/

        }

        /*   public IConfiguration Configuration { get; }*/

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddApplicationServices(_config);
            /* services.AddScoped<ITokenService,TokenService>();*/
            /*string mySqlConnectionStr=Configuration.GetConnectionString("DefaultConnection");
             services.AddDbContextPool<DataContext>(options=>options.UseMySQL(mySqlConnectionStr)); */
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        
         services.AddCors();
         services.AddIdentityServices(_config);
         /*services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),
                    ValidateIssuer=false,
                    ValidateAudience=false
                };
           });*/
        
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
               /*  app.UseDeveloperExceptionPage(); */
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }
            
            app.UseMiddleware<ExeptionMiddleWare>();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
