using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using WeatherForecastingApp.Application.Common.Exceptions;
using WeatherForecastingApp.Infrastructure.Services;

namespace WeatherForecastingApp.Infrastructure.IntegrationTests;

    [TestFixture]
    public class WeatherForecastServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<IOptions<WeatherApiOptions>> _optionsMock;
        private WeatherForeCastService _service;

        [SetUp]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _optionsMock = new Mock<IOptions<WeatherApiOptions>>();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://example.com")
            };

            var weatherApiOptions = new WeatherApiOptions
            {
                BaseUrl = "https://example.com/weather",
                ApiKey = "fake-api-key"
            };

            _optionsMock.Setup(x => x.Value).Returns(weatherApiOptions);

            _service = new WeatherForeCastService(httpClient, _optionsMock.Object);
        }

        [Test]
        public async Task GetWeatherForecasts_ShouldReturnWeatherResponse_WhenApiCallIsSuccessful()
        {
            // Arrange
            var city = "New York";
            var date = "2024-08-30";

            var weatherResponseJson = "{\"resolvedAddress\":\"New York\",\"days\":[{\"datetime\":\"2024-08-30\",\"tempmax\":25,\"tempmin\":18,\"temp\":22,\"humidity\":60,\"precip\":5,\"windspeed\":15,\"conditions\":\"Partly cloudy\"}]}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(weatherResponseJson)
                });

            // Act
            var result = await _service.GetWeatherForecasts(date, city);

            // Assert
            result.Should().NotBeNull();
            result.Location.Should().Be(city);
            result.Days.Should().HaveCount(1);
            result.Day.Should().NotBeNull();
        }

        [Test]
        public void GetWeatherForecasts_ShouldThrowWeatherServiceException_WhenApiReturnsNotFound()
        {
            // Arrange
            var city = "UnknownCity";
            var date = "2024-08-30";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Resource not found")
                });

            // Act
            Func<Task> act = async () => await _service.GetWeatherForecasts(date, city);

            // Assert
            act.Should().ThrowAsync<WeatherServiceException>()
                .WithMessage($"The requested resource or endpoint does not exist or cannot be found. Details: Resource not found");
        }

        [Test]
        public void GetWeatherForecasts_ShouldThrowWeatherServiceException_WhenApiReturnsBadRequest()
        {
            // Arrange
            var city = "InvalidCity";
            var date = "InvalidDate";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Bad request")
                });

            // Act
            Func<Task> act = async () => await _service.GetWeatherForecasts(date, city);

            // Assert
            act.Should().ThrowAsync<WeatherServiceException>()
                .WithMessage($"Check the city name '{city}' or date '{date}'. Details: Bad request");
        }

        [Test]
        public void GetWeatherForecasts_ShouldThrowWeatherServiceException_WhenHttpRequestExceptionOccurs()
        {
            // Arrange
            var city = "New York";
            var date = "2024-08-30";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            Func<Task> act = async () => await _service.GetWeatherForecasts(date, city);

            // Assert
            act.Should().ThrowAsync<WeatherServiceException>()
                .WithMessage("Error communicating with weather service");
        }
    }
