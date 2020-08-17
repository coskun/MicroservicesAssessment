using Newtonsoft.Json;

using System;
using System.ComponentModel.DataAnnotations;

namespace Monitor.API.Model.BindingModels
{
    public class EventData
    {
        /// <summary>
        /// Application identity
        /// </summary>
        [JsonProperty("app"), Required]
        public Guid App { get; set; }

        /// <summary>
        /// Category of event
        /// </summary>
        [JsonProperty("type"), Required(AllowEmptyStrings = false)]
        public string Type { get; set; }

        /// <summary>
        /// Time of event
        /// </summary>
        [JsonProperty("time"), Required]
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Is event successfull ?
        /// </summary>
        [JsonProperty("isSucceeded"), Required]
        public bool IsSucceeded { get; set; }

        /// <summary>
        /// Metadata of event
        /// </summary>
        [JsonProperty("meta")]
        public object Metadata { get; set; }

        /// <summary>
        /// User's information of event
        /// </summary>
        [JsonProperty("user"), Required]
        public UserEventData UserData { get; set; }

        /// <summary>
        /// Attribute data of event
        /// </summary>
        [JsonProperty("attributes")]
        public object Attributes { get; set; }
    }
}