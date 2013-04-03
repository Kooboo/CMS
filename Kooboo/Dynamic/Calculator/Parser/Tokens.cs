using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Dynamic.Calculator.Parser
{
    public class Tokens : IEnumerable<Token>
    {

        #region Local Variables

        // save the token objects in a sorted list
        private System.Collections.Generic.List<Token> items;

        private TokenGroup tokenGroup;  // a reference to the parent object....a group of tokens

        #endregion

        #region Public Constructor

        public Tokens(TokenGroup Group)
        {
            items = new List<Token>();
            tokenGroup = Group;
        }

        #endregion

        #region Public Properties

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        #endregion

        #region Public Methods

        public bool Add(Token tk)
        {
            items.Add(tk);

            // the token has been added to the collection....set the  token object group
            tk.TokenGroup = tokenGroup;

            // update the token group list of variables
            tokenGroup.UpdateVariables(tk);

            return true;
        }

        public void Clear()
        {
            items.Clear();
        }

        #endregion

        #region Public Indexer

        public Token this[int index]
        {
            get
            {
                return this.items[index];
            }
        }

        #endregion

        #region IEnumerable<Token> Members

        public IEnumerator<Token> GetEnumerator()
        {
            return new TokensEnumerator(items);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new TokensEnumerator(items);
        }

        #endregion
    }


    public class TokensEnumerator : IEnumerator<Token>
    {

        #region Local Variables

        private System.Collections.Generic.List<Token> items;
        int location;

        #endregion

        #region Constructor

        public TokensEnumerator(System.Collections.Generic.List<Token> Items)
        {
            items = Items;
            location = -1;
        }

        #endregion

        #region IEnumerator<Token> Members

        public Token Current
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
