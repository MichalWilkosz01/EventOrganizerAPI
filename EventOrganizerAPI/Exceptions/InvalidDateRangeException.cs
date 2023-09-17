namespace EventOrganizerAPI.Exceptions
{
    public class InvalidDateRangeException : Exception
    {
        public InvalidDateRangeException(string message) : base(message) { } 
    }
}
