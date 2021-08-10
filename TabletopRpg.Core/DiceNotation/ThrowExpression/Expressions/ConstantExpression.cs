using TabletopRpg.Core.DiceNotation.ThrowContext;

namespace TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions
{
    internal class ConstantExpression : IThrowExpression
    {
        private readonly int _constant;

        public ConstantExpression(int constant)
        {
            _constant = constant;
        }

        public int GetValue(IThrowContext context)
        {
            return _constant;
        }

        public static bool TryParse(string rule, out ConstantExpression expression)
        {
            if (int.TryParse(rule, out var result))
            {
                expression = new ConstantExpression(result);
                return true;
            }

            expression = null;
            return false;
        }
    }
}
