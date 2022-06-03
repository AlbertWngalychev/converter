namespace converter.Data
{
    public interface IConvertRepository : ISaveChangesAsync
    {
        Task<Models.Convert?> GetAsync(long id);
        Task<IEnumerable<Models.Convert>> GetAllAsync();
        Task<Models.Convert> AddAsync(Models.Convert convert);
        Task<IEnumerable<Models.Convert?>> GetAsync(params long[] ids);
    }
}
