using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StadiumAnalytics.Api.DTOs;
using StadiumAnalytics.Api.Models;
using StadiumAnalytics.Api.Repositories;

namespace StadiumAnalytics.Api.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _repository;

        public SensorService(ISensorRepository repository)
        {
            _repository = repository;
        }

        public async Task ProcessSensorEvent(SensorEvent evt)
        {
            // persist the incoming sensor event
            await _repository.AddSensorEventAsync(evt);
        }

        public IEnumerable<SensorResultDto> GetSensorResults(string gate, string type, DateTime? start, DateTime? end)
        {
            // synchronous convenience wrapper over async call
            return GetSensorResultsAsync(gate, type, start, end).GetAwaiter().GetResult();
        }

        public Task<IEnumerable<SensorResultDto>> GetSensorResultsAsync(string gate, string type, DateTime? start, DateTime? end)
        {
            return _repository.GetSensorResultsAsync(gate, type, start, end);
        }
    }
}