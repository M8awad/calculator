using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCalculator.Core.Models
{
    public class CalculationRequest
    {
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }
        public CalculationOperation Operation { get; set; }
    }
}
