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
    public class Matrix<T> : IEnumerable<T[]>
    {
        #region Fields
        List<T[]> _Items;

        List<int> _Nodes = new List<int>();

        dynamic Values;
        #endregion

        #region Properties
        public dynamic this[int i]
        {
            get
            {
                return this.Values[i];
            }
        }
        #endregion

        #region .ctor
        Matrix(int deep, List<int> nodes, List<T[]> items, params T[][] members)
        {
            this._Items = items;
            this._Nodes.AddRange(nodes);
            var len = members[deep].Length;

            if (deep < members.Length - 1)
            {

                Values = new Matrix<T>[len];

                for (var i = 0; i < len; i++)
                {
                    List<int> indexs2 = new List<int>();
                    indexs2.AddRange(nodes);
                    indexs2.Add(i);

                    Values[i] = new Matrix<T>(deep + 1, indexs2, this._Items, members);
                }
            }
            else
            {
                Values = new T[len][];

                for (var i = 0; i < len; i++)
                {
                    Values[i] = new T[members.Length];

                    for (var n = 0; n < this._Nodes.Count; n++)
                    {
                        Values[i][n] = members[n][this._Nodes[n]];
                    }

                    Values[i][members.Length - 1] = members[members.Length - 1][i];

                    this._Items.Add(Values[i]);
                }

            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the specified members.
        /// </summary>
        /// <param name="members">The members.</param>
        /// <returns></returns>
        public static Matrix<T> Create(params T[][] members)
        {
            return new Matrix<T>(0, new List<int>(), new List<T[]>(), members);
        } 
        #endregion

        #region IEnumerable<T[]> Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T[]> GetEnumerator()
        {
            foreach (var i in this._Items)
            {
                yield return i;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (this._Items as System.Collections.IEnumerable).GetEnumerator();
        }

        #endregion
    }
}
