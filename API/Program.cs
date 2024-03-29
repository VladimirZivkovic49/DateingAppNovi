using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API
{
    public class Program
    {
        /* public static void Main(string[] args) */
       
       public static async Task Main(string[] args)
        {
            /* CreateHostBuilder(args).Build().Run(); */
            var host=CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services=scope.ServiceProvider;
            try
            {

                    var context =services.GetRequiredService<DataContext>();
                   //(L204)
                    var userManager= services.GetRequiredService<UserManager<AppUser>>();

                   //(L204)
                   //(L206)
                        var roleManager= services.GetRequiredService<RoleManager<AppRole>>();

                   //(L206)
                   
                    await context.Database.MigrateAsync(); 
                   /*  await Seed.SeedUsers(context); (L204)*/
                    /* await Seed.SeedUsers(userManager);  (L206)*/
                    await Seed.SeedUsers(userManager,roleManager);
           
            }
            catch(Exception ex)
            {

                var logger =services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Doslo je do greske");
            }
       
                await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
