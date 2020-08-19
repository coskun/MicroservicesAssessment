using System;

namespace Monitor.API.Model
{
    public class EventDataDTO
    {
        public Guid App { get; set; }
        public string Type { get; set; }
        public DateTime ActionDate { get; set; }
        public bool IsSucceeded { get; set; }
        public object Metadata { get; set; }
        public UserEventDataDTO UserData { get; set; }
        public object Attributes { get; set; }
    }
}