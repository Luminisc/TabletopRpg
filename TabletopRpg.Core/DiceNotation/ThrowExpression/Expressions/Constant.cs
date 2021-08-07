using TabletopRpg.Core.DiceNotation.ThrowContext;

namespace TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions
{
    internal class Constant : IThrowExpression
    {
        private readonly int _constant;

        public Constant(int constant)
        {
            _constant = constant;
        }

        public int GetValue(IThrowContext context)
        {
            return _constant;
        }

        public static bool TryParse(string rule, out Constant expression)
        {
            if (int.TryParse(rule, out var result))
            {
                expression = new Constant(result);
                return true;
            }

            expression = null;
            return false;
        }
    }
}
