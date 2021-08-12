using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopRpg.Core.DiceNotation.ThrowContext;
using TabletopRpg.Utils.Exceptions;

namespace TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions
{
    internal class BinaryOperationExpression : IThrowExpression
    {
        internal static readonly char[] _operationsDelimeters = new char[] { '+', '-', '*', '/' };

        private readonly IThrowExpression left;
        private readonly IThrowExpression right;
        private readonly BinaryOperation operation;

        public BinaryOperationExpression(IThrowExpression left, IThrowExpression right, BinaryOperation operation)
        {
            this.left = left;
            this.right = right;
            this.operation = operation;
        }

        public static bool TryParseOperation(string text, out BinaryOperation operation)
        {
            operation = BinaryOperation.Plus;
            switch (text)
            {
                case "+":
                    operation = BinaryOperation.Plus;
                    return true;
                case "-":
                    operation = BinaryOperation.Minus;
                    return true;
                case "*":
                    operation = BinaryOperation.Multiply;
                    return true;
                case "/":
                    operation = BinaryOperation.Divide;
                    return true;
                default:
                    return false;
            }
        }

        public int GetValue(IThrowContext context)
        {
            var leftResult = left.GetValue(context);
            var rightResult = right.GetValue(context);
            switch (operation)
            {
                case BinaryOperation.Plus:
                    return leftResult + rightResult;
                case BinaryOperation.Minus:
                    return leftResult - rightResult;
                case BinaryOperation.Multiply:
                    return leftResult * rightResult;
                case BinaryOperation.Divide:
                    return leftResult / rightResult;
                default:
                    throw new ParsingException($"Unknown operation: {operation}", typeof(BinaryOperation));
            }
        }
    }

    internal enum BinaryOperation
    { 
        Plus,
        Minus,
        Multiply,
        Divide
    }
}
