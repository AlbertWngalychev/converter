namespace converter.Converter.Core
{
    public interface IConverterFileManagerBaseObserver<TId>
    {
        void Subscribe(ConverterFileManagerBase<TId> manager);
    }
}
