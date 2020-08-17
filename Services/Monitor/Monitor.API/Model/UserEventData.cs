using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace Monitor.API.Model
{
    public class UserEventData
    {
        /// <summary>
        /// Is user authenticated ?
        /// </summary>
        [JsonProperty("isAuthenticated"), Required]
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// User's provider information
        /// </summary>
        [JsonProperty("provider"), Required]
        public string Provider { get; set; }

        /// <summary>
        /// User's id information
        /// </summary>
        [JsonProperty("id"), Required]
        public object? Id { get; set; }

        /// <summary>
        /// User's email address information
        /// </summary>
        [JsonProperty("e-mail"), EmailAddress]
        public string EmailAddress { get; set; }
    }
}