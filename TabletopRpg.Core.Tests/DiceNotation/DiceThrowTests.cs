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
            Assert.Equal(typeof(Constant).FullName, dice.GetExpression().GetType().FullName);
        }
    }
}
