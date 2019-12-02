using FluentAssertions;
using Microservices.WebApi.Controllers;
using Microservices.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Microservices.Tests.Controllers
{
   [TestClass]
    public class ForecastsControllerTest
    {
        private Mock<IForecastService> _forecastService;
        private ForecastsController _controller;

        [TestInitialize]
        public void Init()
        {
            _forecastService = new Mock<IForecastService>();
            _controller = new ForecastsController(_forecastService.Object);
        }

        [TestMethod]
        public void Has_GetAll()
        {
            //Arrange
            var controller = new ForecastsController(_forecastService.Object);

            //Act
            var response = controller.GetAll();

            //Assert
            Assert.IsNotNull(response);
            response.Should().NotBeNull();
        }

        [TestMethod]
        public void Calls_IForecastService()
        {
            //Arrange            
            var controller = new ForecastsController(_forecastService.Object);

            //Act
            var response = controller.GetAll();

            //Assert
            _forecastService.Verify(mn => mn.QueryAll(), Times.Once);

        }

        [TestMethod]
        public void Has_SaveForecast()
        {
            //Arrange            
            var controller = new ForecastsController(_forecastService.Object);
            var forecast = new WeatherForecast();

            //Act
            var response = controller.SaveForecast(forecast);

            //Assert

            response.Should().NotBeNull();
        }

        //HTTP Status Codes
        //200 - Success
        //404 - Resource not found
        //500 - Internal Server Error (never use)
        //503 - Service unavailable

        [TestMethod]
        public void Returns_503_When_IForecastService_Throws_Exception()
        {
            //Arrange
            _forecastService.Setup(mn => mn.QueryAll()).Throws(new Exception("DB down"));

            //Act
            var result = _controller.GetAll();

            //Assert
            Assert.IsTrue(result is StatusCodeResult);
            var statusCode = (StatusCodeResult)result;
            statusCode.StatusCode.Should().Be((int)HttpStatusCode.ServiceUnavailable);
        }

        // 400 = Bad Request
        [TestMethod]
        public void Returns_BadRequest_If_RequiredFields_Missing()
        {
            //Arrange
            var emptyForecast = new WeatherForecast();

            //Act
            var result = _controller.SaveForecast(emptyForecast);

            //Assert
            Assert.IsTrue(result is StatusCodeResult);
            var statusCode = (StatusCodeResult)result;
            statusCode.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        }
    }
}
