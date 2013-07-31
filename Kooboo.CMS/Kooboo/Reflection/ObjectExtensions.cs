#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

namespace Kooboo.Reflection
{

    /// <summary>
    /// 
    /// </summary>
    public class Members
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Members" /> class.
        /// </summary>
        /// <param name="o">The o.</param>
        public Members(object o)
        {
            this.Properties = new Properties(o);
        }
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public Properties Properties { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Memberses the specified o.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static Members Members(this object o)
        {
            return new Members(o);
        }
    }
}
