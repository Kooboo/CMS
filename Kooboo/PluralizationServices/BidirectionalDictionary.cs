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
    /// <typeparam name="TFirst">The type of the first.</typeparam>
    /// <typeparam name="TSecond">The type of the second.</typeparam>
    public class BidirectionalDictionary<TFirst, TSecond>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the first to second dictionary.
        /// </summary>
        /// <value>
        /// The first to second dictionary.
        /// </value>
        public Dictionary<TFirst, TSecond> FirstToSecondDictionary
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the second to first dictionary.
        /// </summary>
        /// <value>
        /// The second to first dictionary.
        /// </value>
        public Dictionary<TSecond, TFirst> SecondToFirstDictionary
        {
            get;
            set;
        }

        #endregion

        #region .ctor
        /// <summary>
        /// 
        /// </summary>
        public BidirectionalDictionary()
        {
            this.FirstToSecondDictionary = new Dictionary<TFirst, TSecond>();
            this.SecondToFirstDictionary = new Dictionary<TSecond, TFirst>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstToSecondDictionary">The first to second dictionary.</param>
        public BidirectionalDictionary(Dictionary<TFirst, TSecond> firstToSecondDictionary)
            : this()
        {
            foreach (TFirst current in firstToSecondDictionary.Keys)
            {
                this.AddValue(current, firstToSecondDictionary[current]);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Existses the in first.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual bool ExistsInFirst(TFirst value)
        {
            return this.FirstToSecondDictionary.ContainsKey(value);
        }
        /// <summary>
        /// Existses the in second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual bool ExistsInSecond(TSecond value)
        {
            return this.SecondToFirstDictionary.ContainsKey(value);
        }
        /// <summary>
        /// Gets the second value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual TSecond GetSecondValue(TFirst value)
        {
            if (this.ExistsInFirst(value))
            {
                return this.FirstToSecondDictionary[value];
            }
            return default(TSecond);
        }
        /// <summary>
        /// Gets the first value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual TFirst GetFirstValue(TSecond value)
        {
            if (this.ExistsInSecond(value))
            {
                return this.SecondToFirstDictionary[value];
            }
            return default(TFirst);
        }
        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="firstValue">The first value.</param>
        /// <param name="secondValue">The second value.</param>
        public void AddValue(TFirst firstValue, TSecond secondValue)
        {
            this.FirstToSecondDictionary.Add(firstValue, secondValue);
            if (!this.SecondToFirstDictionary.ContainsKey(secondValue))
            {
                this.SecondToFirstDictionary.Add(secondValue, firstValue);
            }
        }
        #endregion
    }
}
