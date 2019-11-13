using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();

            //The execution of the Run method is delayed until we have seeded data
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()) {
                
                var services = scope.ServiceProvider;

                try {
                    
                    //Get the Database Context
                    var context = services.GetRequiredService<DataContext>();
                    
                    // Apply any pending migration and create db if it does not exist
                    context.Database.Migrate();

                    //Seed Users
                    Seed.SeedUsers(context);

                }
                catch (Exception ex) 
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }

            host.Run();
      
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
