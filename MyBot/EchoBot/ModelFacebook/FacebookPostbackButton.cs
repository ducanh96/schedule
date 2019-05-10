using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.ModelFacebook
{
    public class FacebookPostbackButton
    {
        public FacebookPostbackButton()
        {
            this.Type = "postback";
            this.Title = "Postback Title";
            this.Payload = "Postback Payload";
        }

        public FacebookPostbackButton(string title, string payload)
        {
            this.Type = "postback";
            this.Title = title;
            this.Payload = payload;
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        public override string ToString()
        {
            return $"type: {this.Type}";
        }
    }
}
