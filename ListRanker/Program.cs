using CsvHelper;
using ListRanker.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ListRanker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dataStore = services.GetRequiredService<IDataStore>();
                    if(dataStore.InitialisationRequired())
                    {
                        FileInfo fileInfo = new FileInfo("./Data/disney_movies.csv");
                        using (var reader = new StreamReader(fileInfo.FullName))
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            List<string> others = new List<string>()
                            { 
                                "Encanto",
                                "Treasure Planet",
                                "Hercules"
                            };
                            IEnumerable<DisneyFilm> records = csv.GetRecords<DisneyFilm>();
                            var items = records
                                .OrderByDescending(i => i.inflation_adjusted_gross)
                                .Take(60)
                                .Select(r => r.movie_title)
                                .Concat(others)
                                .ToList();
                            dataStore.InitialiseDatabase(items);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
