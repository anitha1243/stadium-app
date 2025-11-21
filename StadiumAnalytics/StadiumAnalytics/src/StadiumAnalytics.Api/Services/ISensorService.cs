using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StadiumAnalytics.Api.Models;
using StadiumAnalytics.Api.DTOs;

namespace StadiumAnalytics.Api.Services
{
    public interface ISensorService
    {
        Task ProcessSensorEvent(SensorEvent evt);
        IEnumerable<SensorResultDto> GetSensorResults(string gate, string type, DateTime? start, DateTime? end);
        Task<IEnumerable<SensorResultDto>> GetSensorResultsAsync(string gate, string type, DateTime? start, DateTime? end);
    }
}