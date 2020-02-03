using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blockcore.Explorer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Blockcore.Explorer.Handlers
{
   public class FavoriteIconHandler
   {
      private readonly RequestDelegate next;

      private readonly ChainSettings settings;

      private readonly HttpClient http;

      public FavoriteIconHandler(RequestDelegate next, IOptions<ChainSettings> settings)
      {
         this.next = next;
         this.settings = settings.Value;
         http = new HttpClient();
      }

      public async Task Invoke(HttpContext context)
      {
         if (string.IsNullOrWhiteSpace(settings.Icon))
         {
            context.Response.StatusCode = 404;
            return;
         }

         // Perhaps in the future this can be improved a tiny bit?
         byte[] bytes = await http.GetByteArrayAsync(settings.Icon);
         await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
      }
   }
}
