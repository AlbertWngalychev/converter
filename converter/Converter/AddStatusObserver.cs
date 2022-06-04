using converter.Converter.Core;
using converter.Data;
using Convert = converter.Models.Convert;

namespace converter
{
    public class AddStatusObserver : IConverterFileManagerBaseObserver<Convert>
    {
        private readonly IStatusRepository _repository;
        public AddStatusObserver(IStatusRepository repository)
        {
            _repository = repository;

        }
        public AddStatusObserver(IStatusRepository repository, ConverterFileManagerBase<Convert> manager)
        {
            _repository = repository;

            Subscribe(manager);
        }
        public void Subscribe(ConverterFileManagerBase<Convert> manager)
        {
            manager.Process += (s, e) =>
            {
                _ = OnProccessedAsync(s, e);
            };
        }
        private async Task OnProccessedAsync(object? sender, ConverterFileManagerBaseEventArgs<Convert> e)
        {
            await _repository.AddStatusAsync(e.Item.Id, e.Result.ToString(), e.DateTime);
        }
    }
}
