using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.ComponentModel
{
    /// <summary>
    /// Indicate reverse mapping from meta data type to data type, it's used in meta data type class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
    public class MetadataForAttribute : Attribute
    {
        /// <param name="type">The type current type describe meta data for.</param>
        public MetadataForAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Get the type current type describe meta data for.
        /// </summary>
        public Type Type { get; private set; }
    }
}
