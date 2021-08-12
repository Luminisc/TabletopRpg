namespace TabletopRpg.Core.DiceNotation.ThrowRuleParsing.Lexer
{
    public enum TokenType
    {
        Unknown,
        Invalid,
        Digit,
        Plus,
        Minus,
        Multiply,
        Divide,
        OpenParenthesis,
        CloseParenthesis,
        DiceThrow, // 1d6, 2d20
        StringLiteral,
        Whitespace
    }
}
