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
            var testCases = new List<object[]>
            {
                new object[] { "5", ExpectValues(new TokenExpectation(TokenType.Digit, "5")) }
            };

            return testCases;
        }
    }
}
