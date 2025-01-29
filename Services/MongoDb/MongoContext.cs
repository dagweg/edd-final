namespace HouseRentalSystem.Services.MongoDB;

using global::MongoDB.Driver;
using HouseRentalSystem.Options;
using Microsoft.Extensions.Options;

public class MongoContext<T> : IMongoContext<T>
    where T : class
{
    protected readonly IMongoCollection<T> _collection;

    public MongoContext(IOptions<MongoDbOptions> options, string collectionName)
    {
        try
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            _collection = database.GetCollection<T>(collectionName);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        return result.DeletedCount > 0;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
    }

    public async Task<T> GetAsync(string id)
    {
        return await _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string id, T entity)
    {
        await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);
    }
}
