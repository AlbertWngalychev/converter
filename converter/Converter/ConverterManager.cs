using converter.Converter.Algorithms;
using converter.Converter.Core;
using Microsoft.VisualStudio.Threading;
using System.Xml;
using Convert = converter.Models.Convert;

namespace converter.Converter
{
    public class ConverterManager : ConverterFileManagerBase<Convert>
    {
        private readonly AsyncQueue<Convert> _queue;
        private bool _isStart;
        public override int Count => _queue.Count;
        public override string Directory { get; init; }
        public override bool AutoStart { get; set; }
        public override bool IsStart => _isStart;
        public ConverterManager(string path, bool autoStart = true)
        {
            Directory = path;
            AutoStart = autoStart;
            _queue = new();
        }
        public ConverterManager(string path, IEnumerable<Convert> collection, bool autoStart = true)
            : this(path, autoStart)
        {
            foreach (var item in collection)
                _queue.Enqueue(item);
        }
        public override Task AddAsync(params Convert[] item)
        {
            if (item.Length <= 0)
            {
                throw new ArgumentNullException(item.ToString(), "IsEmpty");
            }

            return Task.Run(async () =>
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (_queue.IsEmpty)
                    {
                        _queue.Enqueue(item[i]);
                        continue;
                    }

                    _queue.TryEnqueue(item[i]);

                    await EventInvokeAsync(item[i], Result.Added);
                }
                if (AutoStart && !_isStart)
                {
                    await ConvertAllAsync();
                }
            });
        }
        public override Task<IEnumerable<Convert>> ToArrayAsync()
        {
            return Task.Run<IEnumerable<Convert>>(() => _queue.ToArray());
        }
        public override async Task ConvertFirstAsync()
        {
            if (_queue.Count <= 0)
            {
                return;
            }
            await TryFirstConvertAsync();
        }
        public override async Task ConvertAllAsync()
        {
            if (_isStart) return;

            _isStart = true;

            while (Count > 0)
            {
                await ConvertFirstAsync();
            }

            _isStart = false;
        }
        private async Task TryFirstConvertAsync()
        {
            Convert first = await _queue.DequeueAsync();
            Result result = Result.Processed;

            await EventInvokeAsync(first, result);

            string format = first.ContentType.Split('/').Last();

            if (format != "json" || format != "xml")
            {
                throw new Exception($"I don't know this type. File: {first.Id} {first.FileName}");
            }

            bool json2xml = format=="xml";

            string fileIn = $"{Directory}\\{first.Id}.{(json2xml ? "json" : "xml")}";
            string fileOut = $"{Directory}\\{first.Id}.{(!json2xml ? "json" : "xml")}";

            string inputString = "";

            try
            {
                using (StreamReader reader = new(fileIn))
                {
                    inputString = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrEmpty(inputString))
                {
                    throw new ArgumentNullException(nameof(inputString));
                }
            }
            catch (Exception ex)
            {
                //log ex
                await EventInvokeAsync(first, Result.Error);
                return;
            }

            try
            {
                if (json2xml)
                {
                    XmlDocument? outDoc = await ConvertAsync(inputString, Json2Xml.NewtonsoftJsonAsync);

                    await SaveXmlAsync(fileOut, outDoc);
                }
                else
                {
                    XmlDocument xml = new();
                    xml.LoadXml(inputString);

                    string json = await ConvertAsync(xml, Xml2Json.NewtonsoftJsonAsync);

                    await SaveJsonAsync(fileOut, json);
                }

                await EventInvokeAsync(first, Result.Done);
            }
            catch (Exception ex)
            {
                //log ex
                await EventInvokeAsync(first, Result.Error);
            }


        }
        private static async Task SaveJsonAsync(string fileOut, string json)
        {
            await new StreamWriter(fileOut).WriteAsync(json);
        }
        private static async Task SaveXmlAsync(string fileOut, XmlDocument? outDoc)
        {
            await Task.Run(() => outDoc?.Save(new StreamWriter(fileOut)));
        }

    }
}
