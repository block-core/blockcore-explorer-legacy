//using System.Threading.Tasks;
//using MongoDB.Driver;

//namespace Blockcore.Explorer.Models
//{
//   public class Settings : ISettings
//   {
//      private readonly DatabaseContext _databaseContext;

//      public Settings(DatabaseContext databaseContext)
//      {
//         _databaseContext = databaseContext;
//      }

//      public uint GetIterator() => _databaseContext.Settings.Find(x => true).FirstOrDefault().PublicKeyIterator;

//      public void IncrementIterator()
//      {
//         MongoDB.Bson.ObjectId itemId = _databaseContext.Settings.Find(x => true).FirstOrDefault().Id;
//         _databaseContext.Settings.FindOneAndUpdate(Builders<Setting>.Filter.Eq("_id", itemId), Builders<Setting>.Update.Inc(c => c.PublicKeyIterator, (uint)1), new FindOneAndUpdateOptions<Setting> { IsUpsert = true });
//      }

//      public async Task InitAsync()
//      {
//         if (!_databaseContext.Settings.Find(x => true).Any())
//         {
//            await _databaseContext.Settings.InsertOneAsync(new Setting
//            {
//               PublicKeyIterator = 0
//            });
//         }
//      }
//   }
//}
