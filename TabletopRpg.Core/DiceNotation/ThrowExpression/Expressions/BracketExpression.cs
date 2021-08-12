using TabletopRpg.Core.DiceNotation.ThrowContext;

namespace TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions
{
    internal class BracketExpression : IThrowExpression
    {
        public static readonly char[] _bracketsCharacters = new char[] { '(', ')' };

        private readonly IThrowExpression expression;

        public BracketExpression(IThrowExpression expression)
        {
            this.expression = expression;
        }

        public int GetValue(IThrowContext context)
        {
            return expression.GetValue(context);
        }

        // str like = "5 + (6 + 3) - 1)" without first bracket
        public static int GetClosingBracketIndex(string rule, int bracketIndex)
        {
            var str = rule[(bracketIndex + 1)..];
            var opening = str.IndexOf('(');
            var closing = str.IndexOf(')');
            if (closing < 0) // exception
                return closing;
            if (closing < opening && opening > 0 || opening < 0)
                return closing + bracketIndex + 1;

            var inlinedClosing = GetClosingBracketIndex(rule, opening + bracketIndex + 1);
            closing = -1;
            while (inlinedClosing >= 0)
            {
                closing = inlinedClosing;
                inlinedClosing = GetClosingBracketIndex(rule, inlinedClosing);
            }

            inlinedClosing = closing;
            str = rule[inlinedClosing..];
            opening = str.IndexOf('(');
            closing = str.IndexOf(')');
            if (closing < 0) // exception
                return closing;
            if (closing < opening && opening > 0 || opening < 0)
                return closing + inlinedClosing + opening + bracketIndex + 1;

            return closing + inlinedClosing + opening + bracketIndex + 1;
            //return inlinedClosing;
        }

        public static bool IsExpressionStart(char ch)
        {
            return ch == '(';
        }
    }
}
