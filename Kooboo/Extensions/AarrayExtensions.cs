using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Extensions
{
    public static class AarrayExtensions
    {
        public static string Join(this IEnumerable<string> array, string separator)
        {
            return string.Join(separator, array);
        }
    }
}
