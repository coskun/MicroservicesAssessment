namespace Consumer.API.Model
{
    public class UserEventDataDTO
    {
        public bool IsAuthenticated { get; set; }
        public string Provider { get; set; }
        public object? Id { get; set; }
        public string EmailAddress { get; set; }

        public UserEventDataDTO(bool isAuthenticated, string provider, object id, string emailAddress)
        {
            IsAuthenticated = isAuthenticated;
            Provider = provider;
            Id = id;
            EmailAddress = emailAddress;
        }
    }
}