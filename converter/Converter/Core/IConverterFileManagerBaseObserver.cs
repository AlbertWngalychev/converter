namespace converter.Converter.Core
{
    public interface IConverterFileManagerBaseObserver<TItem>
    {
        void Subscribe(ConverterFileManagerBase<TItem> manager);
    }
}
