namespace Blockcore.Explorer.Models.ApiModels
{
   public class Status
   {
      public string CoinTag { get; set; }

      public string Progress { get; set; }

      public long TransactionsInPool { get; set; }

      public long SyncBlockIndex { get; set; }

      public long BlocksPerMinute { get; set; }

      public decimal AvgBlockPersistInSeconds { get; set; }

      public string Error { get; set; }

      public BlockChainInfo BlockChainInfo { get; set; }

      public NetworkInfo NetworkInfo { get; set; }
   }
}
