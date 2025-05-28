using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using RecordDB.Repositories;
using RecordDB.Services;
using RecordDB.Data.Interfaces;
using RecordDB.Data;
using RecordDB.Services.Output;
using Microsoft.Extensions.Logging;
using Serilog;

namespace RecordDB
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Configure Serilog early to capture startup logs
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Async(a => a.Console())
                .CreateLogger();

            try
            {
                Log.Information("Application starting up...");

                var host = Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", optional: false);

                        // Optional: Add environment-specific config
                        var env = hostingContext.HostingEnvironment;
                        config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                    })
                    .ConfigureServices((context, services) =>
                    {
                        // Your existing service registrations
                        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
                        services.AddScoped<IDbConnection>(sp =>
                            sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());

                        services.AddScoped<IDataAccess, DataAccess>();
                        services.AddScoped<IArtistRepository, ArtistRepository>();
                        services.AddScoped<IRecordRepository, RecordRepository>();
                        services.AddScoped<IStatisticRepository, StatisticRepository>();

                        services.AddScoped<ArtistDbService>();
                        services.AddScoped<RecordDbService>();
                        services.AddScoped<StatisticDbService>();

                        services.AddSingleton<IOutputService, ConsoleOutputService>();
                    })
                    .UseSerilog() // This will use the logger we configured above
                    .Build();

                await host.Services.GetRequiredService<ArtistDbService>().RunAllDatabaseOperations();
                await host.Services.GetRequiredService<RecordDbService>().RunAllDatabaseOperations();
                await host.Services.GetRequiredService<StatisticDbService>().RunAllDatabaseOperations();

                Log.Information("All database operations completed successfully");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush(); // Ensure all logs are written before exiting
            }
        }
    }
}
