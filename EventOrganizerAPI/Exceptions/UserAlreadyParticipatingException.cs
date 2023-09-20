namespace EventOrganizerAPI.Exceptions
{
    public class UserAlreadyParticipatingException : Exception
    {
        public UserAlreadyParticipatingException(string message) : base(message) { }
    }
}
