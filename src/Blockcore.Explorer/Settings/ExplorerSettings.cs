namespace Blockcore.Explorer.Settings
{
   public class ExplorerSettings
   {
      public CurrencySettings Currency { get; set; }

      public IndexerSettings Indexer { get; set; }

      public TickerSettings Ticker { get; set; }

      public SetupSettings Setup { get; set; }

      public FeaturesSettings Features { get; set; }
   }
}
