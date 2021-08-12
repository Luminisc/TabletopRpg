using System;
using System.Collections.Generic;
using System.Linq;
using TabletopRpg.Core.DiceNotation.ThrowRuleParsing.Lexer;

namespace TabletopRpg.Core.Tests.DiceNotation.ThrowRuleParsing
{
    public class TokensSequenceDescription
    {
        private readonly List<TokenExpectation> ExpectedTokens = new();

        public static TokensSequenceDescription ExpectTypes(params TokenType[] types)
        {
            return new TokensSequenceDescription().AndTypes(types);
        }

        public static TokensSequenceDescription ExpectValues(params TokenExpectation[] expectations)
        {
            return new TokensSequenceDescription().AndValues(expectations);
        }

        public TokensSequenceDescription AndTypes(params TokenType[] types)
        {
            ExpectedTokens.AddRange(types.Select(t => new TokenExpectation(t)));
            return this;
        }

        public TokensSequenceDescription AndValues(params TokenExpectation[] expectations)
        {
            ExpectedTokens.AddRange(expectations);
            return this;
        }

        public bool ValidateTokensList(List<RuleToken> tokens)
        {
            if (tokens.Count != ExpectedTokens.Count)
            {
                return false;
            }

            for (int i = 0; i < ExpectedTokens.Count; i++)
            {
                if (!ExpectedTokens[i].MeetsExpectation(tokens[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public struct TokenExpectation
        {
            public TokenType ExpectedType;
            public string ExpectedValue;

            public TokenExpectation(TokenType expectedType) : this(expectedType, null) { }

            public TokenExpectation(TokenType expectedType, string expectedValue)
            {
                ExpectedType = expectedType;
                ExpectedValue = expectedValue;
            }

            public bool MeetsExpectation(RuleToken token)
            {
                return token.TokenType == ExpectedType &&
                    (ExpectedValue == null || token.Value == ExpectedValue);
            }
        }
    }
}
