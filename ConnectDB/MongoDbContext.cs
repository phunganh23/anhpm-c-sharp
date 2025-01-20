using cloud.core;
using cloud.core.mongodb;
using danh_gia_csharp.model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace danh_gia_csharp.ConnectDB
{
    public class MongoDbContext<T> : BaseMongoObjectIdDbContext where T : IEntityObjectId
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;
        public MongoDbContext(IConfiguration configuration) : base(AppSettingsHelper.GetValueByKey("SampleMongodbConnect:ConnectionString"))
        {
            var connectionString = configuration.GetValue<string>("SampleMongodbConnect:ConnectionString");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("SampleMongodbConnect");
        }

        public DbSetObjectId<T> DbContext => new DbSetObjectId<T>(_client, _database, typeof(T).Name.ToLower());

        // Kiểm tra kết nối
        public bool CheckConnection()
        {
            try
            {
                var collections = _database.ListCollectionNames().ToList();
                return collections != null && collections.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MongoDB: {ex.Message}");
                return false;
            }
        }
    }
}