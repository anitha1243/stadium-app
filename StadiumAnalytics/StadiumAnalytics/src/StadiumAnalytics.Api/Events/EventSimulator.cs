using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StadiumAnalytics.Api.Models;
using StadiumAnalytics.Api.Services;

namespace StadiumAnalytics.Api.Events
{
    public class EventSimulator : BackgroundService
    {
        private readonly ISensorService _sensorService;
        private readonly ILogger<EventSimulator> _logger;
        private readonly Random _rnd = new();

        private readonly string[] _gates = new[] { "Gate A", "Gate B", "Gate C" };
        private readonly string[] _types = new[] { "enter", "leave" };
        private readonly TimeSpan _interval;

        public EventSimulator(ISensorService sensorService, ILogger<EventSimulator> logger)
        {
            _sensorService = sensorService;
            _logger = logger;
            _interval = TimeSpan.FromSeconds(5); // adjust or read from config
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EventSimulator starting with interval {Interval}", _interval);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var evt = new SensorEvent
                    {
                        Gate = _gates[_rnd.Next(_gates.Length)],
                        Type = _types[_rnd.Next(_types.Length)],
                        NumberOfPeople = _rnd.Next(1, 11), // 1..10 people
                        Timestamp = DateTime.UtcNow
                    };

                    await _sensorService.ProcessSensorEvent(evt);
                    _logger.LogDebug("Simulated event: {@Event}", evt);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while simulating sensor event");
                }

                try
                {
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (TaskCanceledException) { /* shutdown requested */ }
            }

            _logger.LogInformation("EventSimulator stopping");
        }
    }
}