using System;
using System.Linq;
using TabletopRpg.Core.DiceNotation.ThrowExpression;
using TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions;
using TabletopRpg.Utils.Exceptions;

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
    internal class ThrowRuleParserV0
    {
        static readonly char[] _visualDelimeters = new char[] { ' ' };

        static readonly char[] _skippableDelimeters = _visualDelimeters
            .Concat(BinaryOperationExpression._operationsDelimeters)
            .Concat(BracketExpression._bracketsCharacters)
            .ToArray();

        public static IThrowExpression ParseRule(string rule)
        {
            var parser = new ThrowRuleParserV0();
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

            while (pos < rule.Length)
            {
                buffer[bufferPos++] = rule[pos++];
                if (_visualDelimeters.Contains(buffer[bufferPos - 1]))
                {
                    bufferPos--;
                    continue;
                }

                reading = true;

                var currentChar = rule[pos - 1];
                var nextChar = pos < rule.Length ? rule[pos] : ' ';
                var couldBeOperationOrBracketToken = bufferPos == 1; // if only one character was read, this might be a operation (+ - * /)

                if (couldBeOperationOrBracketToken && BinaryOperationExpression.TryParseOperation(new string(buffer, 0, bufferPos), out var binaryOperation))
                {
                    operation = binaryOperation;
                    if (leftExpr != null)
                    {
                        var rightExpr = ParseRuleInternal(rule[pos..]);
                        leftExpr = new BinaryOperationExpression(leftExpr, rightExpr, operation.Value);
                        return leftExpr;
                    }

                    // reset reader
                    reading = false;
                    bufferPos = 0;
                    continue;
                }

                //if (couldBeOperationOrBracketToken && BracketExpression.IsExpressionStart(currentChar))
                //{
                //    var closingBracketIndex = BracketExpression.GetClosingBracketIndex(rule, pos - 1);
                //    if (closingBracketIndex < 0)
                //        throw new ParsingException("Unable to find closing bracket", typeof(BracketExpression));

                //    if (leftExpr == null)
                //    {
                //        var bracketInnerExpression = ParseRuleInternal(rule[pos..(closingBracketIndex)]);
                //        leftExpr = new BracketExpression(bracketInnerExpression);
                //        pos = closingBracketIndex + 1;

                //        // reset reader
                //        reading = false;
                //        bufferPos = 0;
                //        continue;
                //    }
                //    else
                //        throw new InvalidOperationException("Parsed new expression, but without operation"); // (1+1)(2+2)
                //}

                if (_skippableDelimeters.Contains(nextChar))
                {
                    // trying to parse what was read
                    var str = new string(buffer, 0, bufferPos);
                    var parsedExpression = TryParse(str);
                    if (parsedExpression == null)
                        throw new ParsingException($"Unable to parse following expression: {str}");

                    if (leftExpr == null)
                        leftExpr = parsedExpression;
                    else
                        throw new InvalidOperationException("Parsed new expression, but without operation"); // (1+1)(2+2)

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
                throw new ParsingException("No proper ending of expression");

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
