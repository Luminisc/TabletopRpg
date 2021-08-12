using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopRpg.Core.DiceNotation.ThrowRuleParsing.Lexer;
using TabletopRpg.Utils.Exceptions;

// testing phrases:

// 5            constant
// 3d8          dice throw
// 3+8          just sum
// STR          context
// 3 + (5 - 1)  brackets
// 3d4 + (5d2 - 1) + STR + INT - (WIS * AGI)*5 / 2 + (-2) - (+2)
// 3 + 6 / 2 + 5        = 11
// (3 + 6) / (2 + 1)    = 3
// ((3 + 6) / (2 + 1))    = 3
// 1 + 5 - (3 * 4 + len) + (-2) + 5 + 8 * (1 + 5)

namespace TabletopRpg.Core.DiceNotation.ThrowRuleParsing
{
    internal class ThrowRuleLexer
    {
        public List<TokenDefinition> _tokenDefinitions;

        public ThrowRuleLexer(List<TokenDefinition> tokenDefinitions)
        {
            _tokenDefinitions = tokenDefinitions;
        }

        public ThrowRuleLexer() : this(TokensList.TokenDefinitions) {}

        public List<RuleToken> Tokenize(string lqlText)
        {
            var tokens = new List<RuleToken>();

            string remainingText = lqlText;

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);
                if (match.IsMatch)
                {
                    tokens.Add(new RuleToken(match.TokenType, match.Value));
                    remainingText = match.RemainingText;
                }
                else
                {
                    throw new ParsingException("Failed to generate invalid token");
                }
            }

            return tokens;
        }

        private TokenMatch FindMatch(string lqlText)
        {
            foreach (var tokenDefinition in _tokenDefinitions)
            {
                var match = tokenDefinition.Match(lqlText);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch() { IsMatch = false };
        }
    }
}
