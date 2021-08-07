using System;
using System.IO;
using System.Linq;
using TabletopRpg.Core.DiceNotation.ThrowExpression;
using TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions;

// testing phrases:

// 5            constant
// 3d8          dice throw
// 3+8        just sum
// STR          context
// 3 + (5 - 1)  brackets
// 3d4 + (5d2 - 1) + STR + INT - (WIS * AGI)*5 / 2 + (-2) - (+2)
// 3 + 6 / 2 + 5        = 11
// (3 + 6) / (2 + 1)    = 3

namespace TabletopRpg.Core.DiceNotation.ThrowRuleParser
{
    internal class ThrowRuleParser
    {
        static readonly char[] _visualDelimeters = new char[] { ' ' };
        static readonly char[] _operationsDelimeters = new char[] { '+', '-', '*', '/' };
        static readonly char[] _bracketsCharacters = new char[] { '(', ')' };
        static readonly char[] _diceCharacters = new char[] { 'd' };

        static readonly char[] _skippableDelimeters = _visualDelimeters
            .Concat(_operationsDelimeters)
            .Concat(_bracketsCharacters)
            .ToArray();

        public static IThrowExpression ParseRule(string rule)
        {
            var parser = new ThrowRuleParser();
            return parser.ParseRuleInternal(rule.ToLower());
        }

        protected IThrowExpression ParseRuleInternal(string rule)
        {
            var buffer = new char[100];
            var bufferPos = 0;
            var pos = 0;
            var reading = false;
            IThrowExpression expr = null;

            while (pos < rule.Length)
            {
                buffer[bufferPos++] = rule[pos++];

                reading = true;
                var nextChar = pos < rule.Length ? rule[pos] : ' ';

                if (_skippableDelimeters.Contains(nextChar))
                {
                    // try guess
                    var str = new string(buffer, 0, bufferPos);
                    var parsedExpression = TryParse(str);
                    if (parsedExpression == null)
                        throw new ArgumentException($"Unable to parse following expression: {str}");

                    expr = parsedExpression;
                    // reset reader
                    reading = false;
                    bufferPos = 0;
                    continue;
                }
            }

            if (reading)
                throw new ArgumentException("No proper ending of expression");

            return expr;
        }

        protected static IThrowExpression TryParse(string text)
        {
            if (Constant.TryParse(text, out var expr))
            {
                return expr;
            }

            return null;
        }
    }
}
