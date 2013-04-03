using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Dynamic.Calculator.Parser
{
    public class Variables : IEnumerable<Variable>
    {
        #region Local Variables

        // save the variable objects in a sorted list
        private System.Collections.Generic.SortedList<String, Variable> items;

        #endregion

        #region Public Constructor

        public Variables()
        {
            items = new SortedList<string, Variable>();
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

        public Variable Add(string Name)
        {
            // clean the name for the key
            Name = Name.Trim();
            string key = Name.ToLower();

            // check if the variable exists first
            if (VariableExists(key) == true)
                return this[key];
            else
            {
                // create the variable
                Variable var = new Variable(Name);
                items.Add(key, var);
                return var;
            }
        }

        public bool VariableExists(string Name)
        {
            Name = Name.Trim();
            string key = Name.ToLower();
            return items.ContainsKey(key);
        }

        public void Clear()
        {
            items.Clear();
        }

        #endregion

        #region Public Indexer

        public Variable this[string Name]
        {
            get
            {
                Name = Name.Trim().ToLower();                
                return items[Name];
            }
        }

        public Variable this[int index]
        {
            get
            {
                return this.items[this.items.Keys[index]];
            }
        }

        #endregion

        #region IEnumerable<Variable> Members

        public IEnumerator<Variable> GetEnumerator()
        {
            return new VariablesEnumerator(items);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new VariablesEnumerator(items);
        }

        #endregion
    }

    public class VariablesEnumerator : IEnumerator<Variable>
    {

        #region Local Variables

        private System.Collections.Generic.SortedList<String, Variable> items;
        int location;

        #endregion

        #region Constructor

        public VariablesEnumerator(System.Collections.Generic.SortedList<String, Variable> Items)
        {
            items = Items;
            location = -1;
        }

        #endregion

        #region IEnumerator<Variable> Members

        public Variable Current
        {
            get 
            {
                if (location > 0 || location < items.Count)
                {
                    return items[items.Keys[location]];
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
                    return (object)items[items.Keys[location]];
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
