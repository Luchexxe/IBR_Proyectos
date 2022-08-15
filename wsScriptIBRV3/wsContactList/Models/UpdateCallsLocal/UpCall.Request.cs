using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wsTeleavenceService.Models.UpdateCallsLocal
{
    public class UpCallRequest
    {
        public string agentId { get; set; }
        public string campaignId { get; set; }
        public string contactId { get; set; }
        public string conversationId { get; set; }
        public string contactlistId { get; set; }
        public string customString1 { get; set; }
        public string customString2 { get; set; }
        public string customString3 { get; set; }
        public string participantCustId { get; set; }
        public string phonenumber { get; set; }
        public string queueId { get; set; }
        public string scriptId { get; set; }
        public string wrapupcodeAgent { get; set; }
        public string wrapupcodeSystem { get; set; }

    }
}