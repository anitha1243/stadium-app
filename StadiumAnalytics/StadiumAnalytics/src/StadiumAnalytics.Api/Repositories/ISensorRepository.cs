using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StadiumAnalytics.Api.DTOs;

namespace StadiumAnalytics.Api.Repositories
{
    public interface ISensorRepository
    {
        Task<IEnumerable<SensorResultDto>> GetSensorResultsAsync(string gate, string type, DateTime? start, DateTime? end);
        Task AddSensorEventAsync(StadiumAnalytics.Api.Models.SensorEvent evt);
    }
}