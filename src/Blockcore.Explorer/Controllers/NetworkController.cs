using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using Blockcore.Explorer.Models;
using Blockcore.Explorer.Services;
using Blockcore.Explorer.Settings;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRCoder;

namespace Blockcore.Explorer.Controllers
{
   [ApiExplorerSettings(IgnoreApi = true)]
   public class NetworkController : Controller
   {
      private readonly IMemoryCache memoryCache;
      private readonly TickerService tickerService;
      private readonly WeightService weightService;
      private readonly CurrencyService currencyService;
      private readonly ExplorerSettings settings;
      private readonly ChainSettings chainSettings;
      private readonly ILogger<HomeController> log;
      private readonly BlockIndexService indexService;

      public NetworkController(IMemoryCache memoryCache,
          ILogger<HomeController> log,
          TickerService tickerService,
          BlockIndexService indexService,
          WeightService weightService,
          CurrencyService currencyService,
          IOptions<ExplorerSettings> settings,
          IOptions<ChainSettings> chainSettings)
      {
         this.memoryCache = memoryCache;
         this.log = log;
         this.indexService = indexService;
         this.settings = settings.Value;
         this.chainSettings = chainSettings.Value;
         this.tickerService = tickerService;
         this.weightService = weightService;
         this.currencyService = currencyService;
      }

      public IActionResult Index()
      {
         if (!settings.Features.Home)
         {
            return Redirect("/block-explorer");
         }

         ViewBag.Features = settings.Features;
         ViewBag.Setup = settings.Setup;
         ViewBag.Chain = chainSettings;
         ViewBag.Ticker = settings.Ticker;
         
         ViewBag.Url = Request.Host.ToString();

         Models.ApiModels.CoinInfo nodeInfo = indexService.GetNodeInfo();
         ViewBag.Peers = indexService.GetPeers(DateTime.UtcNow);

         return View(nodeInfo);
      }
   }
}
