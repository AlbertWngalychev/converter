using converter.Models;
using Microsoft.EntityFrameworkCore;

namespace converter.Data
{
    public class ConvertRepository : IConvertRepository, IDisposable
    {
        private readonly convertContext _context;
        private readonly ModelRepositoryCache? _cache;
        public bool UsedCache => _cache != null;

        public ConvertRepository(convertContext context, ModelRepositoryCache? cache = null)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Models.Convert?> GetAsync(long id)
        {
            Models.Convert? model = null;
            bool useContext = false;

            if (UsedCache)
            {
                model = await _cache.GetAsync<Models.Convert>(id);
            }

            if (model == null)
            {
                model = await _context.FindAsync<Models.Convert>(id);
                useContext = true;
            }

            if (UsedCache && useContext && model is not null)
            {
                return await _cache.SetAsync(id, model, TimeSpan.FromSeconds(30 * 60));
            }

            return model;
        }
        public async Task<Models.Convert> AddAsync(Models.Convert convert)
        {
            var temp = await _context.Converts.AddAsync(convert);
            await SaveChangesAsync();

            var entity = temp.Entity;
            
            return UsedCache ? await _cache.SetAsync(entity.Id, entity) : entity;
        }
        public async Task<IEnumerable<Models.Convert>> GetAllAsync()
        {
            return await _context.Converts.ToArrayAsync();
        }

        public async Task<IEnumerable<Models.Convert?>> GetAsync(params long[] ids)
        {
            Models.Convert?[] converts = new Models.Convert[ids.Length];

            for (int i = 0; i < ids.Length; i++)
            {
                converts[i] = await GetAsync(ids[i]);
            }

            return converts;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
