using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace rCallHistory3
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class DialerContactOut2
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("contactListId")]
        public Guid ContactListId { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }

        [JsonProperty("callable")]
        public bool Callable { get; set; }

        [JsonProperty("selfUri")]
        public string SelfUri { get; set; }
    }

    
    public partial class DialerContactOut2
    {
        public static List<DialerContactOut2> FromJson(string json) => JsonConvert.DeserializeObject<List<DialerContactOut2>>(json, rCallHistory2.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<DialerContactOut2> self) => JsonConvert.SerializeObject(self, rCallHistory2.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
