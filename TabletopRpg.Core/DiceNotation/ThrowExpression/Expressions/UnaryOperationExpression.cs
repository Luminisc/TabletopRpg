using TabletopRpg.Core.DiceNotation.ThrowContext;

namespace TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions
{
    internal class UnaryOperationExpression : IThrowExpression
    {
        private readonly IThrowExpression expression;
        private readonly BinaryOperation operation;

        public UnaryOperationExpression(IThrowExpression expression, BinaryOperation operation)
        {
            this.expression = expression;
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
                default:
                    return false;
            }
        }

        public int GetValue(IThrowContext context)
        {
            return expression.GetValue(context) * (operation == BinaryOperation.Minus ? -1 : 1);
        }
    }
}
