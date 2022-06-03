using Newtonsoft.Json;
using System.Xml;

namespace converter.Converter.Algorithms
{
    public static class Xml2Json
    {
        public static Task<string> NewtonsoftJsonAsync(XmlDocument xmlDocument)
        {
            return Task.Run(() => JsonConvert.SerializeXmlNode(xmlDocument, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
