using System;
using System.Threading;
using System.Threading.Tasks;
using Blockcore.Explorer.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blockcore.Explorer.Services
{
   public class DataUpdateService : IHostedService, IDisposable
   {
      private readonly ILogger<DataUpdateService> log;
      private readonly BlockIndexService blockIndexService;
      private readonly TickerService tickerService;
      private readonly IMemoryCache memoryCache;
      private readonly ExplorerSettings settings;
      private System.Timers.Timer indexerTimer;
      private System.Timers.Timer tickerTimer;

      public DataUpdateService(
         ILogger<DataUpdateService> log,
          BlockIndexService blockIndexService,
          TickerService tickerService,
          IMemoryCache memoryCache,
          IOptions<ExplorerSettings> settings)
      {
         this.log = log;
         this.tickerService = tickerService;
         this.blockIndexService = blockIndexService;
         this.memoryCache = memoryCache;
         this.settings = settings.Value;
      }

      public Task StartAsync(CancellationToken cancellationToken)
      {
         if (settings.Features.Explorer)
         {
            try
            {
               indexerTimer = new System.Timers.Timer();
               StartIndexerTimer();
            }
            catch (Exception ex)
            {
               log.LogCritical(ex, "Failed to start indexer timer.");
            }
         }

         if (settings.Features.Ticker)
         {
            try
            {
               tickerTimer = new System.Timers.Timer();
               StartTickerTimer(cancellationToken);
            }
            catch (Exception ex)
            {
               log.LogCritical(ex, "Failed to start ticker timer.");
            }
         }

         return Task.CompletedTask;
      }

      private void StartIndexerTimer()
      {
         indexerTimer.AutoReset = false; // Make sure it only trigger once initially.

         indexerTimer.Elapsed += (sender, args) =>
         {
            if (indexerTimer.AutoReset == false)
            {
               indexerTimer.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
               indexerTimer.AutoReset = true;
            }

            try
            {
               // Get statistics and cache it.
               memoryCache.Set("BlockchainStats", blockIndexService.GetStatistics());
            }
            catch (Exception ex)
            {
               log.LogCritical(ex, "Failed to set blockchain stats cache.");
            }
         };

         indexerTimer.Start();
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

            try
            {
               // Get ticker and cache it.
               memoryCache.Set("Ticker", tickerService.DownloadTicker());
               memoryCache.Set("Rates", tickerService.DownloadRates());
            }
            catch (Exception ex)
            {
               log.LogCritical(ex, "Failed to set ticker or rates.");
            }
         };

         tickerTimer.Start();
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         return Task.CompletedTask;
      }

      public void Dispose()
      {
         indexerTimer?.Stop();
         indexerTimer?.Dispose();

         tickerTimer?.Stop();
         tickerTimer?.Dispose();
      }
   }
}
