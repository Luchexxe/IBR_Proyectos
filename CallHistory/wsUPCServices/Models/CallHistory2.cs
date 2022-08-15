namespace rCallHistory2
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CallHistoryResponse2
    {
        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; }

        [JsonProperty("pageSize")]
        public long PageSize { get; set; }

        [JsonProperty("pageNumber")]
        public long PageNumber { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("firstUri")]
        public string FirstUri { get; set; }

        [JsonProperty("selfUri")]
        public string SelfUri { get; set; }

        [JsonProperty("lastUri")]
        public string LastUri { get; set; }

        [JsonProperty("pageCount")]
        public long PageCount { get; set; }
    }

    public partial class Entity
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("reportId")]
        public Guid ReportId { get; set; }

        [JsonProperty("runTime")]
        public DateTimeOffset RunTime { get; set; }

        [JsonProperty("runStatus")]
        public string RunStatus { get; set; }

        [JsonProperty("runDurationMsec")]
        public long RunDurationMsec { get; set; }

        [JsonProperty("reportUrl")]
        public Uri ReportUrl { get; set; }

        [JsonProperty("reportFormat")]
        public string ReportFormat { get; set; }

        [JsonProperty("scheduleUri")]
        public string ScheduleUri { get; set; }

        [JsonProperty("selfUri")]
        public string SelfUri { get; set; }
    }

    public partial class CallHistoryResponse2
    {
        public static CallHistoryResponse2 FromJson(string json) => JsonConvert.DeserializeObject<CallHistoryResponse2>(json, rCallHistory2.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CallHistoryResponse2 self) => JsonConvert.SerializeObject(self, rCallHistory2.Converter.Settings);
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