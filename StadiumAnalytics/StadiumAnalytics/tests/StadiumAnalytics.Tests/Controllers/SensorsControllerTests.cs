using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StadiumAnalytics.Api.Controllers;
using StadiumAnalytics.Api.Services;
using StadiumAnalytics.Api.DTOs;

namespace StadiumAnalytics.Tests.Controllers
{
    [TestFixture]
    public class SensorsControllerTests
    {
        private SensorsController _controller;
        private Mock<ISensorService> _sensorServiceMock;

        [SetUp]
        public void SetUp()
        {
            _sensorServiceMock = new Mock<ISensorService>();
            _controller = new SensorsController(_sensorServiceMock.Object);
        }

        [Test]
        public async Task Get_ReturnsResults_WhenDataExists()
        {
            // Arrange
            var gate = "Gate A";
            var type = "enter";
            var startTime = DateTime.UtcNow.AddMinutes(-10);
            var endTime = DateTime.UtcNow;
            var expectedResults = new List<SensorResultDto>
            {
                new SensorResultDto { Gate = gate, Type = type, NumberOfPeople = 100 }
            };

            _sensorServiceMock
                .Setup(s => s.GetSensorResultsAsync(gate, type, startTime, endTime))
                .ReturnsAsync(expectedResults);

            // Act
            var result = (await _controller.Get(gate, type, startTime, endTime)).ToList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Gate, Is.EqualTo(expectedResults[0].Gate));
            Assert.That(result[0].Type, Is.EqualTo(expectedResults[0].Type));
            Assert.That(result[0].NumberOfPeople, Is.EqualTo(expectedResults[0].NumberOfPeople));
        }

        [Test]
        public async Task Get_ReturnsEmptyCollection_WhenNoDataExists()
        {
            // Arrange
            var gate = "Gate A";
            var type = "enter";
            var startTime = DateTime.UtcNow.AddMinutes(-10);
            var endTime = DateTime.UtcNow;

            _sensorServiceMock
                .Setup(s => s.GetSensorResultsAsync(gate, type, startTime, endTime))
                .ReturnsAsync(new List<SensorResultDto>());

            // Act
            var result = (await _controller.Get(gate, type, startTime, endTime)).ToList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Get_AllowsNullGateParameter_NoExceptionThrown()
        {
            // Arrange
            string gate = null;
            var type = "enter";
            var startTime = DateTime.UtcNow.AddMinutes(-10);
            var endTime = DateTime.UtcNow;
            var expectedResults = new List<SensorResultDto>
            {
                new SensorResultDto { Gate = "Any", Type = type, NumberOfPeople = 5 }
            };

            _sensorServiceMock
                .Setup(s => s.GetSensorResultsAsync(gate, type, startTime, endTime))
                .ReturnsAsync(expectedResults);

            // Act
            var result = (await _controller.Get(gate, type, startTime, endTime)).ToList();

            // Assert - ensure call succeeds and returns expected count
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedResults.Count));
        }
    }
}