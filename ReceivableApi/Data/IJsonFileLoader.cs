using Newtonsoft.Json.Linq;

namespace ReceivableApi.Data
{
    public interface IJsonFileLoader
    {
        JToken LoadFile(string path);
    }
}
