using converter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace converter.Data
{
    public class StatusRepository : IStatusRepository, IDisposable
    {
        private readonly convertContext _convertContext;
        private readonly ModelRepositoryCache? _cache;
        public bool UsedCache => _cache != null;
        public StatusRepository(convertContext convertContext, ModelRepositoryCache? cache = null)
        {
            _convertContext = convertContext;
            _cache = cache;
        }

        public Task<Status?> GetAsync(long id)
        {
            return GetModelAsync<Status>(id);
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _convertContext.Statuses.ToArrayAsync();
        }
        public async Task<Status> AddStatusAsync(long convertId, long resultId, DateTime dateTime)
        {
            
            Result? result = await GetModelAsync<Result>(resultId) ?? throw new ArgumentNullException(nameof(resultId));
            Models.Convert? convert = await GetModelAsync<Models.Convert>(convertId) ?? throw new ArgumentNullException(nameof(convertId));

            var temp = await _convertContext.Statuses.AddAsync(new()
            {
                DateTime = new DateTimeOffset(dateTime).ToUnixTimeMilliseconds(),
                ResultId = result.Id,
                ConvertId = convert.Id
            });

            await SaveChangesAsync();

            if (UsedCache)
            {
                await _cache.SetAsync(temp.Entity.Id, temp.Entity);
            }

            return temp.Entity;
        }       
        public async Task<Status> AddStatusAsync(long convertId, string resultTitle, DateTime dateTime)
        {

            Result? result = await GetResultAsync(resultTitle) ?? throw new ArgumentNullException(nameof(resultTitle));
            Models.Convert? convert = await GetModelAsync<Models.Convert>(convertId) ?? throw new ArgumentNullException(nameof(convertId));

            var temp = await _convertContext.Statuses.AddAsync(new()
            {
                DateTime = new DateTimeOffset(dateTime).ToUnixTimeMilliseconds(),
                ResultId = result.Id,
                ConvertId = convert.Id
            });

            await SaveChangesAsync();

            if (UsedCache)
            {
                await _cache.SetAsync(temp.Entity.Id, temp.Entity);
            }

            return temp.Entity;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _convertContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _convertContext.Dispose();
        }
        private async Task<Model?> GetModelAsync<Model>(long id) where Model : class
        {
            Model? model = null;
            bool useContext = false;

            if (UsedCache)
            {
                model = await _cache.GetAsync<Model>(id);
            }

            if (model == null)
            {
                model = await _convertContext.FindAsync<Model>(id);
                useContext = true;
            }

            if (UsedCache && useContext && model is not null)
            {
                return await _cache.SetAsync(id, model, TimeSpan.FromSeconds(30 * 60));
            }

            return model;
        }
        private async Task<Result?> GetResultAsync(string title)
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
    }
}
