using CompanyCalculator.Api.Interfaces;
using CompanyCalculator.Core.Interfaces;
using CompanyCalculator.Core.Models;

namespace CompanyCalculator.Api.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ICalculator _calculator;
        private readonly ILoggingService _loggingService;

        public CalculatorService(ICalculator calculator, ILoggingService loggingService)
        {
            _calculator = calculator;
            _loggingService = loggingService;
        }

        public CalculationResult Calculate(CalculationRequest request)
        {
            var result = _calculator.Calculate(request);
            // Log the calculation details
            _loggingService.LogCalculation(request, result);
            return result;
        }
    }
}
