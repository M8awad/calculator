using CompanyCalculator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCalculator.Core.Interfaces
{
    public interface ICalculator
    {
        CalculationResult Calculate(CalculationRequest request);
    }
}
