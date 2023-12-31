﻿namespace EventOrganizerAPI.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public virtual Event Event { get; set; }
    }
}
