using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Reflection
{

    /// <summary>
    /// 
    /// </summary>
    public class Members
    {
        public Members(object o)
        {
            this.Properties = new Properties(o);
        }
        public Properties Properties { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtensions
    {
        public static Members Members(this object o)
        {
            return new Members(o);
        }
    }
}
