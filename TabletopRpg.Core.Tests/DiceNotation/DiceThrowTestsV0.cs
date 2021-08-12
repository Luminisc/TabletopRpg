using System;
using TabletopRpg.Core.DiceNotation;
using TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions;
using Xunit;

namespace TabletopRpg.Core.Tests.DiceNotation
{
    public class DiceThrowTestsV0
    {
        [Fact]
        public void Throw_Constant_ReturnConstant()
        {
            var dice = new DiceThrow("5");
            var throwResult = dice.Throw(null);
            Assert.Equal(5, throwResult);
            Assert.Equal(typeof(ConstantExpression), dice.GetExpression().GetType());
        }

        [Theory]
        [InlineData("1d6", 1, 6, typeof(DiceExpression))]
        [InlineData(" 1d6 ", 1, 6, typeof(DiceExpression))]
        [InlineData(" 1d6", 1, 6, typeof(DiceExpression))]
        [InlineData("1d6 ", 1, 6, typeof(DiceExpression))]

        [InlineData("1d1", 1, 1, typeof(DiceExpression))]
        [InlineData("1d2", 1, 2, typeof(DiceExpression))]
        [InlineData("2d1", 2, 2, typeof(DiceExpression))]
        [InlineData("2d2", 2, 4, typeof(DiceExpression))]
        [InlineData("2d8", 2, 16, typeof(DiceExpression))]
        public void Throw_Dice_ReturnRandomValue(string diceRule, int expectedMin, int expectedMax, Type expectedExpressionType)
        {
            var dice = new DiceThrow(diceRule);

            for (int i = 0; i < 10; i++)
            {
                var throwResult = dice.Throw(null);
                Assert.InRange(throwResult, expectedMin, expectedMax);
            }

            Assert.Equal(expectedExpressionType, dice.GetExpression().GetType());
        }

        [Theory]
        [InlineData("5+3", 8, typeof(BinaryOperationExpression))]
        [InlineData("5 + 3", 8, typeof(BinaryOperationExpression))]
        [InlineData("3 + 5", 8, typeof(BinaryOperationExpression))]
        [InlineData("-3 + 5", 2, typeof(BinaryOperationExpression))]
        [InlineData("-3 -5", -8, typeof(BinaryOperationExpression))]
        [InlineData("-3- 5", -8, typeof(BinaryOperationExpression))]
        [InlineData("-3-5", -8, typeof(BinaryOperationExpression))]
        [InlineData("-3+-5", -8, typeof(BinaryOperationExpression))]
        [InlineData("-3", -3, typeof(UnaryOperationExpression))]
        public void Throw_ConstantOperations_ReturnConstantResult(string diceRule, int expectedResult, Type expectedExpressionType)
        {
            var dice = new DiceThrow(diceRule);
            var throwResult = dice.Throw(null);
            Assert.Equal(expectedResult, throwResult);
            Assert.Equal(expectedExpressionType, dice.GetExpression().GetType());
        }

        //[Theory]
        //[InlineData("((5+3))", 8, typeof(BracketExpression))]
        //[InlineData("(5+3)", 8, typeof(BracketExpression))]
        //[InlineData("5+(3)", 8, typeof(BinaryOperationExpression))]
        //[InlineData("(5)+(3)", 8, typeof(BinaryOperationExpression))]
        //[InlineData("(5)+(3)+(1)", 9, typeof(BinaryOperationExpression))]
        //[InlineData("((5)+(3))", 8, typeof(BracketExpression))]
        //[InlineData("(5)+3", 8, typeof(BinaryOperationExpression))]
        //public void Throw_ParenOperations_ReturnConstantResult(string diceRule, int expectedResult, Type expectedExpressionType)
        //{
        //    var dice = new DiceThrow(diceRule);
        //    var throwResult = dice.Throw(null);
        //    Assert.Equal(expectedResult, throwResult);
        //    Assert.Equal(expectedExpressionType, dice.GetExpression().GetType());
        //}
    }
}
