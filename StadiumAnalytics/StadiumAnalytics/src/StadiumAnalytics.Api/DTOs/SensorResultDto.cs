using System;

namespace StadiumAnalytics.Api.DTOs
{
    public class SensorResultDto
    {
        public string Gate { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int NumberOfPeople { get; set; }
    }
}