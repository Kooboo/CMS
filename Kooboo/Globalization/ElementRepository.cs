#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Globalization.Repository;

namespace Kooboo.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public static class ElementRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public static IElementRepository DefaultRepository = new CacheElementRepository(new XmlElementRepository());
    }
}
