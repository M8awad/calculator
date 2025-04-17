using CompanyCalculator.Core.Models;

namespace CompanyCalculator.Api.Interfaces
{
    public interface ICalculatorService
    {
        CalculationResult Calculate(CalculationRequest request);
    }
}
