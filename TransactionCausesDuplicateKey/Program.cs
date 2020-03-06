using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TransactionCausesDuplicateKey.Db;
using TransactionCausesDuplicateKey.Helpers;

namespace TransactionCausesDuplicateKey
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase<TestDbContext>()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
