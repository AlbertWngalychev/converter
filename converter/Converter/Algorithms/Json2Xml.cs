using Newtonsoft.Json;
using System.Xml;

namespace converter.Converter.Algorithms
{
    public static class Json2Xml
    {
        public static Task<XmlDocument?> NewtonsoftJsonAsync(string json)
        {
            return Task.Run(() => JsonConvert.DeserializeXmlNode(json, "root", true));
        }
    }
}
