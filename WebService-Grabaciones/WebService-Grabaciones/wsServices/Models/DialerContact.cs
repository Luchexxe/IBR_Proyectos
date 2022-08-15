using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;

namespace wsUPCServices.Models
{
    public partial class DialercontactOut
    {
        public string Id { get; set; }
        // public string Name { get; set; }
        public string ContactListId { get; set; }
        //  public string Numero { get; set; }
        public Dictionary<string, object> Data { get; set; }
        // public Dictionary<string,CallRecord> CallRecords { get; set; }
        public bool? Callable { get; set; }
        //     public Dictionary<string,PhoneNumberStats> PhoneNumberStatus { get; set; }
        //    public Dictionary<string,ContactColumnTimeZone> ContactColumnTimeZones { get; set; }
        //  public string SelfUri { get; set; }
    }


    /*  public class PhoneNumberStats
       {
           public bool? Callable { get; set; }
       }

       public class ContactColumnTimeZone
       {
           public string TimeZone { get; set; }
           public string ColumnType { get; set; }
       }*/

    public partial class DialercontactOut
    {
        public static DialercontactOut FromJson(string json) => JsonConvert.DeserializeObject<DialercontactOut>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this DialercontactOut self) => JsonConvert.SerializeObject(self, Converter.Settings);
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