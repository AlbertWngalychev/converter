using converter.Models;

namespace converter.Data
{
    public interface IResultRepository : ISaveChangesAsync
    {
        Task<Result?> GetAsync(long id);
        Task<Result?> GetAsync(string title);
        Task<IEnumerable<Result>> GetAllAsync();
        Task<Result?> AddAsync(Result result);
    }
}
