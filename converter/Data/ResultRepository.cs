using converter.Models;
using Microsoft.EntityFrameworkCore;

namespace converter.Data
{
    public class ResultRepository : IResultRepository, IDisposable
    {
        private readonly convertContext _convertContext;
        private readonly ModelRepositoryCache? _cache;
        public bool UsedCache => _cache != null;

        public ResultRepository(convertContext convertContext, ModelRepositoryCache? modelRepositoryCache = null)
        {
            _convertContext = convertContext;
            _cache = modelRepositoryCache;
        }

        public async Task<Result?> GetAsync(long id)
        {
            Result? Result = null;
            bool useContext = false;

            if (UsedCache)
            {
                Result = await _cache.GetAsync<Result>(id);
            }

            if (Result == null)
            {
                Result = await _convertContext.FindAsync<Result>(id);
                useContext = true;
            }

            if (UsedCache && useContext && Result is not null)
            {
                return await _cache.SetAsync(id, Result, TimeSpan.FromSeconds(30 * 60));
            }

            return Result;
        }
        public async Task<Result?> GetAsync(string title)
        {
            Result? result = null;
            bool useContext = false;

            if (UsedCache)
            {
                result = await _cache.GetFromKeyAsync<Result>(typeof(Results).Name + title);
            }

            if (result == null)
            {
                result = await _convertContext.Results.FirstOrDefaultAsync(x => x.Title == title);
                useContext = true;
            }

            if (UsedCache && useContext && result is not null)
            {
                return await _cache.SetFromKeyAsync(typeof(Results).Name + title, result, TimeSpan.FromSeconds(30 * 60));
            }

            return result;
        }

        public async Task<IEnumerable<Result>> GetAllAsync()
        {
            return await _convertContext.Results.ToArrayAsync();
        }
        public void Dispose()
        {
            _convertContext.Dispose();
        }

        public async Task<Result?> AddAsync(Result result)
        {
            var temp = await _convertContext.Results.AddAsync(result);

            await SaveChangesAsync();

            return UsedCache ? await _cache.SetAsync(temp.Entity.Id, temp.Entity) : temp.Entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _convertContext.SaveChangesAsync();
        }
    }
}
