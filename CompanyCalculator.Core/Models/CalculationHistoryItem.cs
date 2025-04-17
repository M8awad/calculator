using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCalculator.Core.Models
{
    public class CalculationHistoryItem
    {
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }
        public CalculationOperation Operation { get; set; }
        public double Result { get; set; }
        public DateTime Timestamp { get; set; }

        public string DisplayText
        {
            get
            {
                string operationSymbol = GetOperationSymbol();
                return $"{Operand1} {operationSymbol} {Operand2} = {Result}";
            }
        }

        private string GetOperationSymbol()
        {
            switch (Operation)
            {
                case CalculationOperation.Add: return "+";
                case CalculationOperation.Subtract: return "−";
                case CalculationOperation.Multiply: return "×";
                case CalculationOperation.Divide: return "÷";
                default: return "?";
            }
        }
    }

}
