using CompanyCalculator.Core.Models;
using CompanyCalculator.Core.Services;
using Xunit;

namespace CompanyCalculator.Tests
{
    public class CalculatorTests
    {
        private readonly Calculator _calculator;

        public CalculatorTests()
        {
            // Arrange - Setup the calculator instance for all tests
            _calculator = new Calculator();
        }

        [Fact]
        public void Calculate_Addition_ReturnsCorrectResult()
        {
            // Arrange
            var request = new CalculationRequest
            {
                Operand1 = 5,
                Operand2 = 3,
                Operation = CalculationOperation.Add
            };

            // Act
            var result = _calculator.Calculate(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(8, result.Result);
            Assert.Null(result.Message);
        }

        [Fact]
        public void Calculate_Subtraction_ReturnsCorrectResult()
        {
            // Arrange
            var request = new CalculationRequest
            {
                Operand1 = 5,
                Operand2 = 3,
                Operation = CalculationOperation.Subtract
            };

            // Act
            var result = _calculator.Calculate(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Result);
            Assert.Null(result.Message);
        }

        [Fact]
        public void Calculate_Division_ReturnsCorrectResult()
        {
            // Arrange
            var request = new CalculationRequest
            {
                Operand1 = 4,
                Operand2 = 2,
                Operation = CalculationOperation.Subtract
            };

            // Act
            var result = _calculator.Calculate(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Result);
            Assert.Null(result.Message);
        }


    }
}
