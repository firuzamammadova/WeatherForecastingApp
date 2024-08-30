using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WeatherForecastingApp.Application.Common.Interfaces;
using WeatherForecastingApp.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using WeatherForecastingApp.Domain.Entities;

namespace WeatherForecastingApp.Application.FunctionalTests.WeatherForecasts.Queries
{
    [TestFixture]
    public class GetWeatherForecastsQueryHandlerTests
    {
        private Mock<IWeatherForecastService> _mockWeatherService;
        private Mock<IMapper> _mockMapper;
        private GetWeatherForecastsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockWeatherService = new Mock<IWeatherForecastService>();
            _mockMapper = new Mock<IMapper>();

            _handler = new GetWeatherForecastsQueryHandler(_mockWeatherService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnWeatherForecast_WhenValidRequest()
        {
            // Arrange
            var query = new GetWeatherForecastsQuery { Date = "2023-08-30", City = "New York" };
            var weatherResponse = new WeatherResponse { Location = "New York" }; // Assume a simple weather response
            var expectedWeatherForecast = new WeatherForecast { Location = "New York", Date = DateTime.Parse(query.Date) };

            _mockWeatherService.Setup(x => x.GetWeatherForecasts(query.Date, query.City))
                .ReturnsAsync(weatherResponse);

            _mockMapper.Setup(m => m.Map<WeatherForecast>(weatherResponse))
                .Returns(expectedWeatherForecast);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedWeatherForecast);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenServiceFails()
        {
            // Arrange
            var query = new GetWeatherForecastsQuery { Date = "2023-08-30", City = "New York" };

            _mockWeatherService.Setup(x => x.GetWeatherForecasts(query.Date, query.City))
                .ThrowsAsync(new Exception("Service failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("Service failed");
        }
    }
}
