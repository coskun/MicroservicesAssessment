namespace Monitor.API.Model
{
    public class UserEventDataDTO
    {
        public bool IsAuthenticated { get; set; }
        public string Provider { get; set; }
        public object? Id { get; set; }
        public string EmailAddress { get; set; }
    }
}