using System;
using System.Globalization;
using Blockcore.Explorer.Settings;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blockcore.Explorer.TagHelpers
{
   public class TimeTagHelper : TagHelper
   {
      private readonly ExplorerSettings settings;

      private readonly ChainSettings chainSettings;

      private readonly ILogger<TimeTagHelper> log;

      public TimeTagHelper(IOptions<ExplorerSettings> settings, IOptions<ChainSettings> chainSettings, ILogger<TimeTagHelper> log)
      {
         this.settings = settings.Value;
         this.chainSettings = chainSettings.Value;
         this.log = log;
      }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         // TODO: When the Blockcore Indexer returns correct time for transactions, enable the code below!
         output.Content.SetContent("-");
         return;
         
         string input = output.GetChildContentAsync().Result.GetContent();
         bool success = long.TryParse(input, out long value);

         if (success)
         {
            try
            {
               string text = DateTimeOffset.FromUnixTimeSeconds(value).ToString("F");
               string html = $"<span class=\"time-value\">{text}</span>";
               output.Content.SetHtmlContent(html);
            }
            catch (Exception ex)
            {
               log.LogError(ex, $"Failed to parse in TimeTagHelper. Input was {input} and parsed value was {value}.");
               output.Content.SetContent(input);
            }
         }
         else
         {
            output.Content.SetContent(input);
         }
      }
   }
}
