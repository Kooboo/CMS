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
using System.Text;

namespace Kooboo.Dynamic.Calculator.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenItems : IEnumerable<TokenItem>
    {
        #region Local Variables

        // save the variable objects in a list
        private System.Collections.Generic.List<TokenItem> items;

        // The tokenitems collection object has a token as a parent
        private Token parent = null;

        #endregion

        #region Public Constructor

        public TokenItems(Token Parent)
        {
            parent = Parent;
            items = new List<TokenItem>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The tokenitems collection object has a token as a parent
        /// </summary>
        public Token Parent
        {
            get
            {
                return parent;
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        #endregion

        #region Public Methods

        public void Add(TokenItem TItem)
        {
            items.Add(TItem);
            TItem.parent = this;  // set the parent of this token item
        }

        public void AddToFront(TokenItem TItem)
        {
            items.Insert(0, TItem);
            TItem.parent = this;  // set the parent of this token item
        }

        #endregion

        #region Public Indexer

        public TokenItem this[int index]
        {
            get
            {
                return this.items[index];
            }
        }

        #endregion

        #region IEnumerable<TokenItem> Members

        public IEnumerator<TokenItem> GetEnumerator()
        {
            return new TokemItemsEnumerator(items);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new TokemItemsEnumerator(items);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class TokemItemsEnumerator : IEnumerator<TokenItem>
    {
        #region Local Variables

        private System.Collections.Generic.List<TokenItem> items;
        int location;

        #endregion

        #region Constructor

        public TokemItemsEnumerator(System.Collections.Generic.List<TokenItem> Items)
        {
            items = Items;
            location = -1;
        }

        #endregion

        #region IEnumerator<TokenItem> Members

        public TokenItem Current
        {
            get 
            {
                if (location > 0 || location < items.Count)
                {
                    return items[location];
                }
                else
                {
                    // we are outside the bounds					
                    throw new InvalidOperationException("The enumerator is out of bounds");
                }
                
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // do nothing
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get 
            {
                if (location > 0 || location < items.Count)
                {
                    return (object)items[location];
                }
                else
                {
                    // we are outside the bounds					
                    throw new InvalidOperationException("The enumerator is out of bounds");
                }
                
            }
        }

        public bool MoveNext()
        {
            location++;
            return (location < items.Count);
        }

        public void Reset()
        {
            location = -1;
        }

        #endregion
    }
}
