#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Collections.Generic;

namespace Kooboo.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComparedResult<T>
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="ComparedResult{T}" /> class.
        /// </summary>
        public ComparedResult()
        {
            this.Added = new List<T>();
            this.Deleted = new List<T>();
            this.Updated = new List<T>();
        }
        #endregion

        #region Propterties
        /// <summary>
        /// Gets or sets the added.
        /// </summary>
        /// <value>
        /// The added.
        /// </value>
        public List<T> Added
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the deleted.
        /// </summary>
        /// <value>
        /// The deleted.
        /// </value>
        public List<T> Deleted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        /// <value>
        /// The updated.
        /// </value>
        public List<T> Updated
        {
            get;
            set;
        }
        #endregion
    }
}
