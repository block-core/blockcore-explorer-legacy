using NBitcoin.Networks;

namespace Blockcore.Explorer
{
   public class ExplorerSetup
   {
      private static NBitcoin.Network network;

      public static NBitcoin.Network CurrentNetwork
      {
         get
         {
            if (network == null)
            {
               network = NetworkRegistration.GetNetwork("StratisMain");
            }

            return network;
         }
      }
   }
}
