using System.Collections.Generic;
using System.Globalization;
using Blockcore.Explorer.Settings;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Blockcore.Explorer.Services
{
   public class CurrencyService : ServiceBase
   {
      private readonly IMemoryCache memoryCache;
      private readonly ExplorerSettings settings;

      public static readonly Dictionary<string, string> CustomCultures = new Dictionary<string, string>() { { "EN", "EN-US" }, { "NO", "NB-NO" }, { "NB", "NB-NO" } };

      public CurrencyService() : base(string.Empty)
      {

      }

      public CurrencyService(
          IMemoryCache memoryCache,
          IOptions<ExplorerSettings> settings) : base(settings.Value.Currency.ApiUrl)
      {
         this.memoryCache = memoryCache;
         this.settings = settings.Value;
      }

      public RegionInfo GetRegionaInfo(IRequestCultureFeature rqf)
      {
         // Whenever the culture has been specified in the URL, write it to a cookie. This ensures that the culture selection is
         // available in the REST API/Web Socket call and updates, and when the user visits the website next time.
         //if (!string.IsNullOrWhiteSpace(this.Request.Query["culture"]))
         //{
         //    CookieRequestCultureProvider.MakeCookieValue(rqf.RequestCulture);
         //}

         var regionInfo = new RegionInfo("EN-US");

         if (!rqf.RequestCulture.UICulture.Name.Equals("en-US") && !rqf.RequestCulture.UICulture.Name.Equals("en"))
         {
            try
            {

               string culture = rqf.RequestCulture.UICulture.Name.ToUpper();

               if (CustomCultures.ContainsKey(culture))
               {
                  culture = CustomCultures[culture];
               }

               regionInfo = new RegionInfo(culture);
            }
            catch { }
         }

         return regionInfo;
      }
   }
}
