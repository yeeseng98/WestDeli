using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WestDeli.Models;
using Microsoft.Extensions.DependencyInjection;
using App.Metrics.Health;
using App.Metrics.Health.Checks.Sql;
using System.Diagnostics;

namespace WestDeli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try { var context = services.GetRequiredService<WestDeliContext>(); context.Database.Migrate(); SeedData.Initialize(services); } catch (Exception ex) { var logger = services.GetRequiredService<ILogger<Program>>(); logger.LogError(ex, "An error occurred seeding the DB."); }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseApplicationInsights()
            .ConfigureHealthWithDefaults(
            builder =>
            {
                // Check that the current amount of private memory in bytes is below a threshold
                builder.HealthChecks.AddProcessPrivateMemorySizeCheck("Private Memory Size", 1000000000);
                // Check that the current amount of virtual memory in bytes is below a threshold
                builder.HealthChecks.AddProcessVirtualMemorySizeCheck("Virtual Memory Size", 1000000000);
                // Check that the current amount of physical memory in bytes is below a threshold
                builder.HealthChecks.AddProcessPhysicalMemoryCheck("Working Set", 1000000000);
                // Check connectivity to the site with a "ping", passes if the result is `IPStatus.Success`
                builder.HealthChecks.AddPingCheck("Site Ping", "https://localhost:44395/", TimeSpan.FromSeconds(10));               
                builder.HealthChecks.AddHttpGetCheck("Azure Storage Check", new Uri("https://westdelistorage.blob.core.windows.net/image-blob-container/20160801-sous-vide-brisket-guide-35-1500x1125.jpg"), TimeSpan.FromSeconds(10));
            builder.HealthChecks.AddCheck("DatabaseConnected",
            () => DBCheck());
            })
            .UseHealthEndpoints()
            .UseStartup<Startup>();

        public async static ValueTask<HealthCheckResult> DBCheck()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WestDeliContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WestDeliContext-e59bba77-a8ac-4fe4-a0d5-ca1afe7f48a0;Trusted_Connection=True;MultipleActiveResultSets=true");

            WestDeliContext context = new WestDeliContext(optionsBuilder.Options);

            try
            {
                context.Database.OpenConnection();
                context.Database.CloseConnection();

                return await new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("Database Connection OK"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return await new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy("Database Connection Failed"));
            }
        }
    }
}
