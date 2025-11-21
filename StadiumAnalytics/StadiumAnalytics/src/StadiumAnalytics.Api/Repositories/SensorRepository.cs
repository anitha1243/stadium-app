using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StadiumAnalytics.Api.Data;
using StadiumAnalytics.Api.DTOs;
using StadiumAnalytics.Api.Models;

namespace StadiumAnalytics.Api.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly StadiumContext _context;

        public SensorRepository(StadiumContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SensorResultDto>> GetSensorResultsAsync(string gate, string type, DateTime? start, DateTime? end)
        {
            var query = _context.SensorEvents.AsQueryable();

            if (!string.IsNullOrWhiteSpace(gate)) query = query.Where(s => s.Gate == gate);
            if (!string.IsNullOrWhiteSpace(type)) query = query.Where(s => s.Type == type);
            if (start.HasValue) query = query.Where(s => s.Timestamp >= start.Value);
            if (end.HasValue) query = query.Where(s => s.Timestamp <= end.Value);

            var results = await query
                .GroupBy(s => new { s.Gate, s.Type })
                .Select(g => new SensorResultDto
                {
                    Gate = g.Key.Gate,
                    Type = g.Key.Type,
                    NumberOfPeople = g.Sum(x => x.NumberOfPeople)
                })
                .ToListAsync();

            return results;
        }

        public async Task AddSensorEventAsync(SensorEvent evt)
        {
            await _context.SensorEvents.AddAsync(evt);
            await _context.SaveChangesAsync();
        }
    }
}