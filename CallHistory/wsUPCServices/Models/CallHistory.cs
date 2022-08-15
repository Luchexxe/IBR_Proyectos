namespace rCallHistory
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CallHistoryResponse
    {
        [JsonProperty("conversations")]
        public List<Conversation> Conversations { get; set; }
    }

    public partial class Conversation
    {
        [JsonProperty("conversationId")]
        public string ConversationId { get; set; }

        [JsonProperty("conversationStart")]
        public string ConversationStart { get; set; }

         [JsonProperty("campaignID")]
        public string CampaignID { get; set; }

        [JsonProperty("contactID")]
        public string ContactID { get; set; }

        [JsonProperty("contactListID")]
        public string ContactListID { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("wrapupcode")]
        public string WrapUpCode { get; set; }

        
       // [JsonProperty("participants")]
     //   public List<Participant> Participants { get; set; }
    }
   
    public partial class Participant
    {
        [JsonProperty("purpose")]
        public string Purpose { get; set; }

       

        [JsonProperty("sessions")]
        public List<Session> Sessions { get; set; }
    }

    public partial class Session
    {
        [JsonProperty("ani")]
        public string Ani { get; set; }

        [JsonProperty("dnis")]
        public string Dnis { get; set; }

        [JsonProperty("outboundCampaignId")]
        public string OutboundCampaignId { get; set; }

        [JsonProperty("outboundContactId")]
        public string OutboundContactId { get; set; }

        [JsonProperty("outboundContactListId")]
        public string OutboundContactListId { get; set; }

        [JsonProperty("segments")]
        public List<Segment> Segments { get; set; }
    }

    public partial class Segment
    {
        [JsonProperty("queueId")]
        public string QueueId { get; set; }

        [JsonProperty("segmentType")]
        public string SegmentType { get; set; }

        [JsonProperty("wrapUpCode", NullValueHandling = NullValueHandling.Ignore)]
        public string WrapUpCode { get; set; }
    }

    public partial class CallHistoryResponse
    {
        public static rCallHistory.CallHistoryResponse FromJson(string json) => JsonConvert.DeserializeObject<rCallHistory.CallHistoryResponse>(json);
    
    }



    public static class Serialize
    {
        public static string ToJson(this CallHistoryResponse self) => JsonConvert.SerializeObject(self);
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
