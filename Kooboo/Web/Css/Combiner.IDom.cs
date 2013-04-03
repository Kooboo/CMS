using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    partial class Combiner<T>
    {
        public interface IDom<TNode>
        {
            IEnumerable<TNode> Select(string selector);

            TNode InsertBefore(TNode node, string html);

            TNode InsertAfter(TNode node, string html);
        }
    }
}
