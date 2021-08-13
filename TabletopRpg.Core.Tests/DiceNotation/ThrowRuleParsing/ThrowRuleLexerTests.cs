using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopRpg.Core.DiceNotation.ThrowRuleParsing;
using Xunit;
using TabletopRpg.Core.DiceNotation.ThrowRuleParsing.Lexer;
using static TabletopRpg.Core.Tests.DiceNotation.ThrowRuleParsing.TokensSequenceDescription;

namespace TabletopRpg.Core.Tests.DiceNotation.ThrowRuleParsing
{
    public class ThrowRuleLexerTests
    {
        private readonly ThrowRuleLexer _lexer;

        public ThrowRuleLexerTests()
        {
            _lexer = new ThrowRuleLexer();
        }

        [Theory]
        [MemberData(nameof(GetTokenizeTestCases))]
        public void Tokenizer_SuccessfullyTokenizedString(string rule, TokensSequenceDescription description)
        {
            var tokens = _lexer.Tokenize(rule);
            Assert.True(description.ValidateTokensList(tokens));
        }

        public static IEnumerable<object[]> GetTokenizeTestCases()
        {
            static TokenExpectation te(TokenType expectedType, string expectedValue = null) => new(expectedType, expectedValue);

            var testCases = new List<object[]>
            {
                new object[] { "5", ExpectValues(te(TokenType.Digit, "5")) },
                new object[] { "5+3", ExpectValues(te(TokenType.Digit, "5"), te(TokenType.Plus, "+"), te(TokenType.Digit, "3")) },
                new object[] { "5 + 3", ExpectValues(te(TokenType.Digit, "5"), te(TokenType.Whitespace), te(TokenType.Plus, "+"), te(TokenType.Whitespace, null), te(TokenType.Digit, "3")) },
                new object[] { "2d6", ExpectValues(te(TokenType.DiceThrow, "2d6")) },
                new object[] { "3d8-5", ExpectValues(te(TokenType.DiceThrow, "3d8"), te(TokenType.Minus, "-"), te(TokenType.Digit, "5")) },
                new object[] { 
                    "3d8-(5-3)*STR", 
                    ExpectValues(
                        te(TokenType.DiceThrow, "3d8"), te(TokenType.Minus, "-"), 
                        te(TokenType.OpenParenthesis, "("), te(TokenType.Digit, "5"), te(TokenType.Minus, "-"), te(TokenType.Digit, "3"), te(TokenType.CloseParenthesis, ")"),
                        te(TokenType.Multiply, "*"), te(TokenType.StringLiteral, "STR")
                    )
                }
            };

            return testCases;
        }
    }
}
