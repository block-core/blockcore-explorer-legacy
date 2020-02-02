using System;
using System.Threading;
using System.Threading.Tasks;
using Blockcore.Explorer.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Blockcore.Explorer.Services
{
   public class DataUpdateService : IHostedService, IDisposable
   {
      private readonly BlockIndexService blockIndexService;
      private readonly TickerService tickerService;
      private readonly IMemoryCache memoryCache;
      private readonly ExplorerSettings settings;
      private System.Timers.Timer nakoTimer;
      private System.Timers.Timer tickerTimer;

      public DataUpdateService(
          BlockIndexService blockIndexService,
          TickerService tickerService,
          IMemoryCache memoryCache,
          IOptions<ExplorerSettings> settings)
      {
         this.tickerService = tickerService;
         this.blockIndexService = blockIndexService;
         this.memoryCache = memoryCache;
         this.settings = settings.Value;
      }

      public Task StartAsync(CancellationToken cancellationToken)
      {
         if (settings.Features.Explorer)
         {
            nakoTimer = new System.Timers.Timer();
            StartNakoTimer();
         }

         if (settings.Features.Ticker)
         {
            tickerTimer = new System.Timers.Timer();
            StartTickerTimer(cancellationToken);
         }

         return Task.CompletedTask;
      }

      private void StartNakoTimer()
      {
         nakoTimer.AutoReset = false; // Make sure it only trigger once initially.

         nakoTimer.Elapsed += (sender, args) =>
         {
            if (nakoTimer.AutoReset == false)
            {
               nakoTimer.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
               nakoTimer.AutoReset = true;
            }

            // Get statistics and cache it.
            memoryCache.Set("BlockchainStats", blockIndexService.GetStatistics());
         };

         nakoTimer.Start();
      }

      private void StartTickerTimer(CancellationToken cancellationToken)
      {
         tickerTimer.AutoReset = false; // Make sure it only trigger once initially.

         tickerTimer.Elapsed += async (sender, args) =>
         {
            if (tickerTimer.AutoReset == false)
            {
               tickerTimer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
               tickerTimer.AutoReset = true;
            }

            // Get ticker and cache it.
            memoryCache.Set("Ticker", tickerService.DownloadTicker());
            memoryCache.Set("Rates", tickerService.DownloadRates());

            //await hubContext.Clients.All.SendAsync("UpdateTicker", cancellationToken);
         };

         tickerTimer.Start();
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         return Task.CompletedTask;
      }

      public void Dispose()
      {
         nakoTimer?.Stop();
         nakoTimer?.Dispose();

         tickerTimer?.Stop();
         tickerTimer?.Dispose();
      }
   }
}
