using TabletopRpg.Core.DiceNotation.ThrowContext;
using TabletopRpg.Core.DiceNotation.ThrowExpression;

namespace TabletopRpg.Core.DiceNotation
{
    public class DiceThrow
    {
        private readonly IThrowExpression throwExpression; 

        public DiceThrow(string rule)
        {
            throwExpression = ThrowRuleParser.ThrowRuleParserV0.ParseRule(rule);
        }

        /// <summary>Returns result of dice throw</summary>
        /// <param name="context">Context of throw. Used to get context-based values, like character modifiers, bonuses, etc.</param>
        public int Throw(IThrowContext context)
        {
            return throwExpression.GetValue(context);
        }

        /// <summary>Used for testing</summary>
        /// <returns>ThrowExpression of this throw rule</returns>
        internal IThrowExpression GetExpression()
        {
            return throwExpression;
        }
    }
}
