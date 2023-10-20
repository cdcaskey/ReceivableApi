using Newtonsoft.Json.Linq;

namespace ReceivableApi.Data
{
    public class JsonFileLoader : IJsonFileLoader
    {
        public JToken LoadFile(string path)
        {
            using var reader = new StreamReader(path);
            var contents = reader.ReadToEnd();

            return JToken.Parse(contents);
        }
    }
}
