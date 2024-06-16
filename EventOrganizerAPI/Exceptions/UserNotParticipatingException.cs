namespace EventOrganizerAPI.Exceptions
{
    public class UserNotParticipatingException : Exception
    {
        public UserNotParticipatingException(string message) : base(message) { }
    }
}
