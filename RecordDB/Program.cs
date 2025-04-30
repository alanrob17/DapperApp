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

namespace RecordDB
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

                    services.AddScoped<IDbConnection>(sp =>
                        sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());

                    services.AddScoped<IArtistRepository, ArtistRepository>();
                    services.AddScoped<ArtistDbService>();
                    services.AddScoped<IRecordRepository, RecordRepository>();
                    services.AddScoped<RecordDbService>();

                    services.AddSingleton<IOutputService, ConsoleOutputService>();
                    services.AddScoped<ArtistDbService>();

                })
                .Build();

            var artistDbService = host.Services.GetRequiredService<ArtistDbService>();
            await artistDbService.RunAllDatabaseOperations();

            var recordDbService = host.Services.GetRequiredService<RecordDbService>();
            await recordDbService.RunAllDatabaseOperations();
        }
    }
}
