using System.Text.RegularExpressions;

namespace TabletopRpg.Core.DiceNotation.ThrowRuleParsing.Lexer
{
    public class TokenDefinition
    {
        private readonly Regex _regex;
        private readonly TokenType _returnsToken;

        public TokenDefinition(TokenType returnsToken, string regexPattern)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            _returnsToken = returnsToken;
        }

        public TokenMatch Match(string inputString)
        {
            var match = _regex.Match(inputString);
            if (match.Success)
            {
                string remainingText = string.Empty;
                if (match.Length != inputString.Length)
                    remainingText = inputString[match.Length..];

                return new TokenMatch()
                {
                    IsMatch = true,
                    RemainingText = remainingText,
                    TokenType = _returnsToken,
                    Value = match.Value
                };
            }
            else
            {
                return new TokenMatch() { IsMatch = false };
            }

        }
    }

    public struct TokenMatch
    {
        public bool IsMatch { get; set; }
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
        public string RemainingText { get; set; }
    }

    public struct RuleToken
    {
        public RuleToken(TokenType tokenType)
        {
            TokenType = tokenType;
            Value = string.Empty;
        }

        public RuleToken(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TokenType TokenType { get; }
        public string Value { get; }

        public RuleToken Clone()
        {
            return new RuleToken(TokenType, Value);
        }
    }
}
