using System;

namespace TabletopRpg.Utils.Exceptions
{
    [Serializable]
    public class ParsingException : Exception
    {
        public Type ExpressionType;

        public ParsingException() { }
        public ParsingException(string message) : base(message) { }
        public ParsingException(string message, Type expressionType) : base(message) { Data["ExpressionType"] = expressionType.Name; ExpressionType = expressionType; }
        protected ParsingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
