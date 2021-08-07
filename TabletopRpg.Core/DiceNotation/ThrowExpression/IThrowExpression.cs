using TabletopRpg.Core.DiceNotation.ThrowContext;

namespace TabletopRpg.Core.DiceNotation.ThrowExpression
{
    /// <summary>Describes throw rule</summary>
    internal interface IThrowExpression
    {
        /// <summary>Returns value of expression based on context</summary>
        /// <param name="context">Context of throw. Used to get context-based values, like character modifiers, bonuses, etc.</param>
        /// <returns>Calculated value of expression</returns>
        int GetValue(IThrowContext context);
    }
}
