namespace converter.Converter.Core
{
    public abstract class ConverterFileManagerBase<TId>
    {
        public event EventHandler<ConverterFileManagerBaseEventArgs<TId>>? Process;
        public event EventHandler<ConverterFileManagerBaseEventArgs<TId>>? Added;
        public event EventHandler<ConverterFileManagerBaseEventArgs<TId>>? Done;
        public event EventHandler<ConverterFileManagerBaseEventArgs<TId>>? Error;
        public abstract string Directory { get; init; }
        public abstract bool AutoStart { get; set; }
        public abstract int Count { get; }
        public abstract bool IsStart { get; }
        public abstract Task<IEnumerable<TId>> ToArrayAsync();
        public abstract Task AddAsync(params TId[] item);
        public abstract Task ConvertFirstAsync();
        public abstract Task ConvertAllAsync();
        public virtual async Task<TOutput> ConvertAsync<TInput, TOutput>(TInput input, Converter<TInput, Task<TOutput>> func)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return await func(input);
        }
        private protected virtual void EventInvoke(TId item, Result result)
        {
            var args = new ConverterFileManagerBaseEventArgs<TId>(item, result);

            Process?.Invoke(this, args);

            if (result is Result.Done)
            {
                Done?.Invoke(this, args);
            }

            if (result is Result.Added)
            {
                Added?.Invoke(this, args);
            }

            if (result is Result.Error)
            {
                Error?.Invoke(this, args);
            }
        }
        private protected virtual Task EventInvokeAsync(TId item, Result result)
        {
            return Task.Run(() =>
            {
                EventInvoke(item, result);
            });
        }

    }
}

