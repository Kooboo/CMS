#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
    /// <summary>
    /// 
    /// </summary>
	public class StringBidirectionalDictionary : BidirectionalDictionary<string, string>
	{
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="StringBidirectionalDictionary" /> class.
        /// </summary>
        public StringBidirectionalDictionary()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StringBidirectionalDictionary" /> class.
        /// </summary>
        /// <param name="firstToSecondDictionary">The first to second dictionary.</param>
        public StringBidirectionalDictionary(Dictionary<string, string> firstToSecondDictionary)
            : base(firstToSecondDictionary)
        {
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Existses the in first.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override bool ExistsInFirst(string value)
        {
            return base.ExistsInFirst(value.ToLowerInvariant());
        }
        /// <summary>
        /// Existses the in second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override bool ExistsInSecond(string value)
        {
            return base.ExistsInSecond(value.ToLowerInvariant());
        }
        /// <summary>
        /// Gets the first value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override string GetFirstValue(string value)
        {
            return base.GetFirstValue(value.ToLowerInvariant());
        }
        /// <summary>
        /// Gets the second value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override string GetSecondValue(string value)
        {
            return base.GetSecondValue(value.ToLowerInvariant());
        } 
        #endregion
	}
}
