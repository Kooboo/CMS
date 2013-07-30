#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Drawing;

namespace Kooboo.Drawing.Filters
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Executes the filter.
        /// </summary>
        /// <param name="inputImage">The input image.</param>
        /// <returns></returns>
        Image ExecuteFilter(Image inputImage);
    }
}
