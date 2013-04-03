using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Dynamic.Calculator.Parser;
using Kooboo.Dynamic.Calculator.Evaluate;

namespace Kooboo.Dynamic.Calculator
{
    public static class Calculator
    {
        public static string Calculate(string expression)
        {
            Token token = new Kooboo.Dynamic.Calculator.Parser.Token(expression);
            Evaluator evaluator = new Evaluator(token);
            string value;
            string errorMsg;
            if (!evaluator.Evaluate(out value, out errorMsg))
            {
                throw new CalculateExpression(errorMsg);
            }
            return value;
        }
    }
}
