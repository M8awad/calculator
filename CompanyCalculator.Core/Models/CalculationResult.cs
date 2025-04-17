using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCalculator.Core.Models
{
    public class CalculationResult
    {
        public double Result { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
    }
}
