using Microsoft.Extensions.Caching.Memory;

namespace converter.Data
{
    public class ModelRepositoryCache : IModelRepositoryCache
    {
        private readonly IMemoryCache _cache;
        public ModelRepositoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }
        public Task<Model?> GetAsync<Model>(long id) where Model : class
        {
            string k = typeof(Model).Name + id;

            return Task.Run(() => _cache.Get<Model?>(k));
        }
        public Task<Model> SetAsync<Model>(long id, Model model) where Model : class
        {
            return Task.Run(() => _cache.Set(typeof(Model).Name + id, model));
        }
        public Task<Model> SetAsync<Model>(long id, Model model, TimeSpan time) where Model : class
        {
            return Task.Run(() => _cache.Set(typeof(Model).Name + id, model, time));
        }
        public Task RemoveAsync<Model>(long id) where Model : class
        {
            return Task.Run(() => _cache.Remove(typeof(Model).Name + id));
        }
        public Task<Model?> GetFromKeyAsync<Model>(object key) where Model : class
        {
            return Task.Run(() => _cache.Get<Model?>(key));
        }
        public Task<Model> SetFromKeyAsync<Model>(object key, Model model, TimeSpan time) where Model : class
        {
            return Task.Run(() => _cache.Set(key, model, time));
        }
    }
}
