using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoreLibrary.Utilities
{
    public static class Helpers
    {
        public static string JsonSerialize(object obj)
        {
            return JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

    }
}
