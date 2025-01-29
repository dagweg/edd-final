namespace HouseRentalSystem.Services.MongoDB;

public interface IMongoContext<T>
    where T : class
{
    Task CreateAsync(T entity);
    Task<List<T>> GetAllAsync();
    Task<T> GetAsync(string id);
    Task UpdateAsync(string id, T entity);
    Task<bool> DeleteAsync(string id);
}
