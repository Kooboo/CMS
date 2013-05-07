#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

namespace Kooboo.CMS.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public class CachedNullValue
    {
        #region .ctor
        public CachedNullValue()
        {

        } 
        #endregion

        #region statics
        public volatile static CachedNullValue Value = new CachedNullValue(); 
        #endregion

        #region Methods
        public override bool Equals(object obj)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return 0;
        }
        public static bool operator ==(CachedNullValue obj1, CachedNullValue obj2)
        {
            return true;
        }
        public static bool operator !=(CachedNullValue obj1, CachedNullValue obj2)
        {
            return false;
        } 
        #endregion
    }
}
