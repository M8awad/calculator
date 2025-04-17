using CompanyCalculator.Core.Interfaces;
using CompanyCalculator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCalculator.Core.Services
{
    public class Calculator : ICalculator
    {
        public CalculationResult Calculate(CalculationRequest request)
        {
            var result = new CalculationResult();

            try
            {
                switch (request.Operation)
                {
                    case CalculationOperation.Add:
                        result.Result = request.Operand1 + request.Operand2;
                        break;
                    case CalculationOperation.Subtract:
                        result.Result = request.Operand1 - request.Operand2;
                        break;
                    case CalculationOperation.Multiply:
                        result.Result = request.Operand1 * request.Operand2;
                        break;
                    case CalculationOperation.Divide:
                        if (request.Operand2 == 0)
                        {
                            result.Success = false;
                            result.Message = "Division by zero is not allowed.";
                        }
                        else
                        {
                            result.Result = request.Operand1 / request.Operand2;
                        }
                        break;
                    default:
                        result.Success = false;
                        result.Message = "Invalid operation.";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
