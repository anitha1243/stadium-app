using System;

namespace StadiumAnalytics.Api.Models
{
    public class SensorEvent
    {
        public int Id { get; set; }
        public string Gate { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int NumberOfPeople { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}