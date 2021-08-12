using System.Collections.Generic;

namespace TabletopRpg.Core.DiceNotation.ThrowRuleParsing.Lexer
{
    internal class TokensList
    {
        public static List<TokenDefinition> TokenDefinitions = new()
        {
            new TokenDefinition(TokenType.DiceThrow, @"^\d+d\d+"),
            new TokenDefinition(TokenType.Digit, @"^\d+"),
            new TokenDefinition(TokenType.Plus, @"^\+"),
            new TokenDefinition(TokenType.Minus, @"^\-"),
            new TokenDefinition(TokenType.Multiply, @"^\*"),
            new TokenDefinition(TokenType.Divide, @"^\/"),
            new TokenDefinition(TokenType.OpenParenthesis, @"^\("),
            new TokenDefinition(TokenType.CloseParenthesis, @"^\)"),
            new TokenDefinition(TokenType.StringLiteral, @"^[\w]+"),
            new TokenDefinition(TokenType.Whitespace, @"^\s+"),
            new TokenDefinition(TokenType.Invalid, @"(^\S+\s)|^\S+")
        };
    }
}
