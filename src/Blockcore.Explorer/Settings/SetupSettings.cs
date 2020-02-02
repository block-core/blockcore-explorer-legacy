namespace Blockcore.Explorer.Settings
{
   public class SetupSettings
   {
      public SetupSettings()
      {
         Title = "Blockcore Explorer";
         Chain = "BTC";
         DocumentationUrl = "/docs";
      }

      public string Title { get; set; }

      public string Chain { get; set; }

      public string Coin { get; set; }

      public string Footer { get; set; }

      public string Icon { get; set; }

      public string DocumentationUrl { get; set; }
   }
}
