using CompanyCalculator.Api.Interfaces;
using CompanyCalculator.Core.Models;

namespace CompanyCalculator.Api.Services
{
    public class LoggingService : ILoggingService
    {
        public void LogCalculation(CalculationRequest request, CalculationResult result)
        {
            
            // In a production system, you might persist this in a file or database.
            Console.WriteLine($"[{DateTime.Now}] Calculation: {request.Operand1} {request.Operation} {request.Operand2} = {result.Result}");
        }
    }
}
