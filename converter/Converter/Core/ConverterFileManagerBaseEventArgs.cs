namespace converter.Converter.Core
{
    public class ConverterFileManagerBaseEventArgs<TId> : EventArgs
    {
        public TId Item { get; init; }
        public Result Result { get; init; }
        public DateTime DateTime { get; init; }
        public bool IsError => Result is Result.Error;
        public bool IsDone => Result is Result.Done;
        public bool IsAdded => Result is Result.Added;
        public bool IsProcessed => Result is Result.Processed;


        public ConverterFileManagerBaseEventArgs(TId item, Result result)
        {
            Item = item;
            Result = result;
            DateTime = DateTime.Now;
        }
    }
}

