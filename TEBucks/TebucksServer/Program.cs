using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TEBucksServer
{
    public class Program
    {
        //private const string apiUrl = "https://te-pgh-api.azurewebsites.net/";
        //private const string apiKey = "03013";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}