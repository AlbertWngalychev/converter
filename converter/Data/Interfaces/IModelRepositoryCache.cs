namespace converter.Data
{
    public interface IModelRepositoryCache
    {
        Task<Model?> GetAsync<Model>(long id) where Model : class;
        Task<Model?> GetFromKeyAsync<Model>(object key) where Model : class;
        Task RemoveAsync<Model>(long id) where Model : class;
        Task<Model> SetAsync<Model>(long id, Model model) where Model : class;
        Task<Model> SetAsync<Model>(long id, Model model, TimeSpan time) where Model : class;
        Task<Model> SetFromKeyAsync<Model>(object key, Model model, TimeSpan time) where Model : class;
    }
}