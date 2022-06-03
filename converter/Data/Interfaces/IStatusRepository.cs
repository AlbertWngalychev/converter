using converter.Models;

namespace converter.Data
{
    public interface IStatusRepository : ISaveChangesAsync
    {
        Task<Status?> GetAsync(long id);
        Task<IEnumerable<Status>> GetAllAsync();
        Task<Status> AddStatusAsync(long convertId, string title, DateTime dateTimeMilliseconds);
    }
}
