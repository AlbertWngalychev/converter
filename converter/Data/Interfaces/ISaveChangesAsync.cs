namespace converter.Data
{
    public interface ISaveChangesAsync
    {
        Task<int> SaveChangesAsync();
    }
}
