using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopRpg.Core.DiceNotation.ThrowContext
{
    internal class ThrowContext : IThrowContext
    {
        private readonly Dictionary<string, int> _values;

        public ThrowContext(Dictionary<string, int> values)
        {
            _values = values;
        }

        public int GetContextValue(string key)
        {
            if (_values.TryGetValue(key, out var val))
            {
                return val;
            }

            throw new ArgumentException("No such value in context");
        }
    }
}
