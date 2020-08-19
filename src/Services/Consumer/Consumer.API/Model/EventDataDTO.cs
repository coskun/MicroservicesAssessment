using System;

namespace Consumer.API.Model
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

        public EventDataDTO(Guid app, string type, DateTime actionDate, bool isSucceeded, object metadata, UserEventDataDTO userData, object attributes)
        {
            App = app;
            Type = type;
            ActionDate = actionDate;
            IsSucceeded = isSucceeded;
            Metadata = metadata;
            UserData = userData;
            Attributes = attributes;
        }
    }
}