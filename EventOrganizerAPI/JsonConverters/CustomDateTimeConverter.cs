using Newtonsoft.Json.Converters;

namespace EventOrganizerAPI.JsonConverters
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm";
        }
    }
}
