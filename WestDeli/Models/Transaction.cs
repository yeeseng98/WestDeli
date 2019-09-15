using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestDeli.Models
{
    public class Transaction
    {
        [JsonProperty(PropertyName ="id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "transactDate")]
        public string TransactDate { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "identifier")]
        public string Identifier { get; set; }

        [JsonProperty(PropertyName = "totalPrice")]
        public string TotalPrice { get; set; }

        [JsonProperty(PropertyName = "totalTime")]
        public int TotalTime { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "items")]
        public OrderObject[] items { get; set; }
    }
}
