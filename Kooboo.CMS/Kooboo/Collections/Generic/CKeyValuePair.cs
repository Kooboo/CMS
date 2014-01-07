using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Collections.Generic
{
    public class CKeyValuePair<TKey, TValue>
    {
        public CKeyValuePair()
        {
        }
        public CKeyValuePair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }
}
