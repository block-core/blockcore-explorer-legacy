namespace Blockcore.Explorer
{
   using System;
   using System.Globalization;
   using System.Linq;
   using System.Net.Http;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.Extensions.Configuration;
   using Microsoft.Extensions.Hosting;

   /// <summary>
   /// The application program.
   /// </summary>
   public class Program
   {
      public static void Main(string[] args)
      {
         Console.WriteLine("Starting Blockcore Explorer...");

         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
         .ConfigureAppConfiguration(config =>
         {
            string chain = args
               .DefaultIfEmpty("--chain=BTC")
               .Where(arg => arg.StartsWith("--chain", ignoreCase: true, CultureInfo.InvariantCulture))
               .Select(arg => arg.Replace("--chain=", string.Empty, ignoreCase: true, CultureInfo.InvariantCulture))
               .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(chain))
            {
               throw new ArgumentNullException("--chain", "You must specify the --chain argument. It can be either chain name, or URL to a json configuration.");
            }

            Console.WriteLine("CHAIN: " + chain);

            if (chain == "Development")
            {
               // Skip external loading as we'll only rely on the local appsettings.json and appsettings.Development.json.
               return;
            }

            string url;

            if (chain.Contains("/"))
            {
               url = chain;
            }
            else
            {
               url = $"https://chains.blockcore.net/chains/{chain}.json";
            }

            Console.WriteLine("SETUP: " + url);

            var http = new HttpClient();
            HttpResponseMessage result = http.GetAsync(url).Result;

            if (result.IsSuccessStatusCode)
            {
               System.IO.Stream stream = result.Content.ReadAsStreamAsync().Result;
               config.AddJsonStream(stream);

               // We will re-add command line to ensure command line can override.
               config.AddCommandLine(args);
            }
            else
            {
               throw new ApplicationException("Unable to read the supplied configuration.");
            }
         })
            .ConfigureWebHostDefaults(webBuilder =>
            {
               webBuilder.UseStartup<Startup>();
            });
   }
}
