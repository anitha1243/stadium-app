using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StadiumAnalytics.Api.DTOs;
using StadiumAnalytics.Api.Services;
using StadiumAnalytics.Api.Models;

namespace StadiumAnalytics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorService _service;

        public SensorsController(ISensorService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<IEnumerable<SensorResultDto>> Get([FromQuery] string gate, [FromQuery] string type, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            return _service.GetSensorResultsAsync(gate, type, start, end);
        }

        [HttpPost("events")]
        public async Task<IActionResult> PostEvent([FromBody] SensorEvent evt)
        {
            await _service.ProcessSensorEvent(evt);
            return Accepted();
        }
    }
}