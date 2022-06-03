namespace converter.Converter.Core
{
    public static class ConverterFileManagerBaseExtensions
    {
        public static IServiceCollection AddConverterManager<T, K>(this IServiceCollection services)
            where T : class
            where K : ConverterFileManagerBase<T>
        {
            return services.AddSingleton<ConverterFileManagerBase<T>, K>();
        }
    }
}

