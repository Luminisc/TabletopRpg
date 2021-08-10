using System;
using TabletopRpg.Core.DiceNotation;
using TabletopRpg.Core.DiceNotation.ThrowExpression.Expressions;
using Xunit;

namespace TabletopRpg.Core.Tests.DiceNotation
{
    public class DiceThrowTests
    {
        [Fact]
        public void Throw_Constant_ReturnConstant()
        {
            var dice = new DiceThrow("5");
            var throwResult = dice.Throw(null);
            Assert.Equal(5, throwResult);
            Assert.Equal(typeof(ConstantExpression), dice.GetExpression().GetType());
        }

        [Fact]
        public void Throw_Dice_ReturnRandomValue()
        {
            var dice = new DiceThrow("1d6");
            var diceWhitespaces = new DiceThrow(" 1d6 ");

            for (int i = 0; i < 10; i++)
            {
                var throwResult = dice.Throw(null);
                Assert.InRange(throwResult, 1, 6);

                var throw2Result = diceWhitespaces.Throw(null);
                Assert.InRange(throw2Result, 1, 6);
            }

            Assert.Equal(typeof(DiceExpression), dice.GetExpression().GetType());
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
    }
}
