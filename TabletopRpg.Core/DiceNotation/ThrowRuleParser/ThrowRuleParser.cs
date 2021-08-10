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
// ((3 + 6) / (2 + 1))    = 3

namespace TabletopRpg.Core.DiceNotation.ThrowRuleParser
{
    internal class ThrowRuleParser
    {
        static readonly char[] _visualDelimeters = new char[] { ' ' };
        static readonly char[] _bracketsCharacters = new char[] { '(', ')' };

        static readonly char[] _skippableDelimeters = _visualDelimeters
            .Concat(BinaryOperationExpression._operationsDelimeters)
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
            IThrowExpression leftExpr = null;
            BinaryOperation? operation = null;
            IThrowExpression rightExpr = null;

            while (pos < rule.Length)
            {
                buffer[bufferPos++] = rule[pos++];
                if (_visualDelimeters.Contains(buffer[bufferPos - 1]))
                {
                    bufferPos--;
                    continue;
                }

                reading = true;
                var couldBeOperation = bufferPos == 1; // if only one character was read, this might be a operation (+ - * /)
                if (couldBeOperation && BinaryOperationExpression.TryParseOperation(new string(buffer, 0, bufferPos), out var binaryOperation))
                {
                    operation = binaryOperation;
                    if (leftExpr != null)
                    {
                        rightExpr = ParseRuleInternal(rule.Substring(pos));
                        leftExpr = new BinaryOperationExpression(leftExpr, rightExpr, operation.Value);
                        return leftExpr;
                    }

                    // reset reader
                    reading = false;
                    bufferPos = 0;
                    continue;
                }

                var nextChar = pos < rule.Length ? rule[pos] : ' ';

                if (_skippableDelimeters.Contains(nextChar))
                {
                    // try guess
                    var str = new string(buffer, 0, bufferPos);
                    var parsedExpression = TryParse(str);
                    if (parsedExpression == null)
                        throw new ArgumentException($"Unable to parse following expression: {str}");

                    if (leftExpr == null)
                        leftExpr = parsedExpression;
                    else
                        rightExpr = ParseRule(rule.Substring(pos));

                    if (operation.HasValue)
                    {
                        leftExpr = new UnaryOperationExpression(leftExpr, operation.Value);
                        operation = null;
                    }

                    // reset reader
                    reading = false;
                    bufferPos = 0;
                    continue;
                }
            }

            if (reading)
                throw new ArgumentException("No proper ending of expression");

            return leftExpr;
        }

        protected static IThrowExpression TryParse(string text)
        {
            if (ConstantExpression.TryParse(text, out var constExpr))
            {
                return constExpr;
            }

            if (DiceExpression.TryParse(text, out var diceExpr))
            {
                return diceExpr;
            }

            return null;
        }
    }
}
