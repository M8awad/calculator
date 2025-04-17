using CompanyCalculator.Core.Models;

namespace CompanyCalculator.Api.Interfaces
{
    public interface ILoggingService
    {
        void LogCalculation(CalculationRequest request, CalculationResult result);
    }
}
