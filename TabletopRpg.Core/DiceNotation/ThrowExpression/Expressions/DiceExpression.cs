using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopRpg.Core.DiceNotation.ThrowContext;

namespace TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions
{
    internal class DiceExpression : IThrowExpression
    {
        private readonly int throws;
        private readonly int dice;

        public DiceExpression(int throws, int dice)
        {
            this.throws = throws;
            this.dice = dice;
        }

        public int GetValue(IThrowContext context)
        {
            return Enumerable.Range(0, throws)
                .Select(x => GameRandom.Random.Next(1, dice + 1))
                .Sum();
        }

        public static bool TryParse(string rule, out DiceExpression expression)
        {
            var elements = rule.Split('d');
            if (elements.Length == 2 && int.TryParse(elements[0], out var throws) && int.TryParse(elements[1], out var dice))
            {
                expression = new DiceExpression(throws, dice);
                return true;
            }

            expression = null;
            return false;
        }
    }
}
