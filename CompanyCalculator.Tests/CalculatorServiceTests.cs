using CompanyCalculator.Api.Interfaces;
using CompanyCalculator.Api.Services;
using CompanyCalculator.Core.Interfaces;
using CompanyCalculator.Core.Models;
using Moq;
using Xunit;

namespace CompanyCalculator.Tests
{
    public class CalculatorServiceTests
    {
        private readonly Mock<ICalculator> _mockCalculator;
        private readonly Mock<ILoggingService> _mockLoggingService;
        private readonly CalculatorService _calculatorService;

        public CalculatorServiceTests()
        {
            // Arrange - Setup mocks and service for all tests
            _mockCalculator = new Mock<ICalculator>();
            _mockLoggingService = new Mock<ILoggingService>();
            _calculatorService = new CalculatorService(_mockCalculator.Object, _mockLoggingService.Object);
        }

        [Fact]
        public void Calculate_CallsUnderlyingCalculator_WithCorrectParameters()
        {
            // Arrange
            var request = new CalculationRequest
            {
                Operand1 = 10,
                Operand2 = 5,
                Operation = CalculationOperation.Add
            };

            var expectedResult = new CalculationResult
            {
                Result = 15,
                Success = true
            };

            _mockCalculator.Setup(c => c.Calculate(It.IsAny<CalculationRequest>()))
                .Returns(expectedResult);

            // Act
            var result = _calculatorService.Calculate(request);

            // Assert
            _mockCalculator.Verify(c => c.Calculate(It.Is<CalculationRequest>(r =>
                r.Operand1 == request.Operand1 &&
                r.Operand2 == request.Operand2 &&
                r.Operation == request.Operation)), Times.Once);
        }



        [Fact]
        public void Calculate_CallsLoggingService_WithCorrectParameters()
        {
            // Arrange
            var request = new CalculationRequest
            {
                Operand1 = 10,
                Operand2 = 5,
                Operation = CalculationOperation.Add
            };

            var expectedResult = new CalculationResult
            {
                Result = 15,
                Success = true
            };

            _mockCalculator.Setup(c => c.Calculate(It.IsAny<CalculationRequest>()))
                .Returns(expectedResult);

            // Act
            var result = _calculatorService.Calculate(request);

            // Assert
            _mockLoggingService.Verify(l => l.LogCalculation(
                It.Is<CalculationRequest>(r =>
                    r.Operand1 == request.Operand1 &&
                    r.Operand2 == request.Operand2 &&
                    r.Operation == request.Operation),
                It.Is<CalculationResult>(r =>
                    r.Result == expectedResult.Result &&
                    r.Success == expectedResult.Success)),
                Times.Once);
        }



        [Fact]
        public void Calculate_ReturnsCorrectResult_FromCalculator()
        {
            // Arrange
            var request = new CalculationRequest
            {
                Operand1 = 10,
                Operand2 = 5,
                Operation = CalculationOperation.Add
            };

            var expectedResult = new CalculationResult
            {
                Result = 15,
                Success = true
            };

            _mockCalculator.Setup(c => c.Calculate(It.IsAny<CalculationRequest>()))
                .Returns(expectedResult);

            // Act
            var result = _calculatorService.Calculate(request);

            // Assert
            Assert.Equal(expectedResult.Result, result.Result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.Equal(expectedResult.Message, result.Message);
        }


        [Fact]
        public void Calculate_HandlesFailureResult_FromCalculator()
        {
            // Arrange
            var request = new CalculationRequest
            {
                Operand1 = 10,
                Operand2 = 0,
                Operation = CalculationOperation.Divide
            };

            var expectedResult = new CalculationResult
            {
                Success = false,
                Message = "Division by zero is not allowed."
            };

            _mockCalculator.Setup(c => c.Calculate(It.IsAny<CalculationRequest>()))
                .Returns(expectedResult);

            // Act
            var result = _calculatorService.Calculate(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(expectedResult.Message, result.Message);
        }



    }
}
