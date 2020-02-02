using Blockcore.Explorer.Models.ApiModels;
using Blockcore.Explorer.Settings;
using Microsoft.Extensions.Options;

namespace Blockcore.Explorer.Services
{
   public class BlockIndexService : ServiceBase
   {
      private readonly ExplorerSettings settings;

      public BlockIndexService() : base(string.Empty)
      {

      }

      public BlockIndexService(IOptions<ExplorerSettings> settings) : base(settings.Value.Indexer.ApiUrl)
      {
         this.settings = settings.Value;
      }

      public BlockModel GetBlockByHeight(long blockHeight)
      {
         return Execute<BlockModel>(GetRequest($"/query/block/index/{blockHeight}/transactions"));
      }

      public BlockModel GetBlockByHash(string blockHash)
      {
         return Execute<BlockModel>(GetRequest($"/query/block/{blockHash}/transactions"));
      }

      public BlockModel GetLatestBlock()
      {
         return Execute<BlockModel>(GetRequest("/query/block/Latest/transactions"));
      }

      public AddressModel GetTransactionsByAddress(string adddress)
      {
         return Execute<AddressModel>(GetRequest($"/query/address/{adddress}/transactions"));
      }

      public TransactionDetailsModel GetTransaction(string transactionId)
      {
         return Execute<TransactionDetailsModel>(GetRequest($"/query/transaction/{transactionId}"));
      }

      public string GetStatistics()
      {
         string result = Execute(GetRequest("/stats"));
         return result;
      }
   }
}
