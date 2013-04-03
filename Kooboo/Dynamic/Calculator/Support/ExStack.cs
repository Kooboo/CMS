using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Dynamic.Calculator.Support
{
    public class ExStack<T> : IEnumerable<T>
    {
        #region Local Variables

        System.Collections.Generic.List<T> list = null;

        #endregion

        #region Constructor

        public ExStack()
        {
            list = new List<T>();
        }

        public ExStack(int Capacity)
        {
            list = new List<T>(Capacity);
        }

        #endregion

        #region Public Propertes

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (Count == 0);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reverse the stack
        /// </summary>
        public void Reverse()
        {
            list.Reverse();
        }

        public void Swap()
        {
            if (this.Count < 2) return;
             
            T Item1 = Pop();
            T Item2 = Pop();

            Push(Item1);
            Push(Item2);
        }

        
        /// <summary>
        /// Push a single item on the stack
        /// </summary>
        /// <param name="Node"></param>
        public void Push(T Item)
        {
            list.Add(Item);
        }

        
        /// <summary>
        /// Push an array of items on the stack
        /// </summary>
        /// <param name="Nodes"></param>
        public void Push(T[] Items)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Push(Items[i]);
            }
        }
       
        /// <summary>
        /// Push the items from a stack onto a stack.  
        /// </summary>
        /// <param name="Stack"></param>
        public void Push(ExStack<T> Stack)
        {
            // reverse the incoming stack
            Stack.Reverse();

            // pop the incoming stack and push on to this stack
            for (int i = 0; i < Stack.Count; i++)
            {
                Push(Stack.Pop());
            }
        }
        
        public T Pop()
        {
            // see if there are any items to pop
            if (Count == 0) return default(T);

            // get the lst item
            T item = list[Count - 1];

            // remove the item
            list.RemoveAt(Count - 1);

            // return the last node
            return item;
        }

        
        public T[] Pop(int PopCount)
        {
            // see if there are any items to pop
            if (Count == 0) return null;

            // adjust the pop count if popping too many
            if (PopCount > Count) PopCount = Count;

            // create the array that returns the items
            T[] items = new T[PopCount];

            // start looping through the list adding the item
            for (int i = 0; i < PopCount; i++)
            {
                // save the item
                items[i] = Pop();
            }

            // all done
            return items;
        }
        
        public T[] PopAll()
        {
            return Pop(Count);
        }


        /// <summary>
        /// Pops the items from the stack and places them in their own stack
        /// </summary>
        /// <param name="PopCount"></param>
        /// <returns></returns>
        public ExStack<T> PopStack(int PopCount)
        {
            // create a new stack
            ExStack<T> stack = new ExStack<T>(PopCount);

            for (int i = 0; i < PopCount; i++)
            {
                stack.Push(this.Pop());
            }

            stack.Reverse();

            return stack;

        }

       
        public T Peek()
        {
            // see if there are any items to peek
            if (Count == 0) return default(T);

            return list[Count - 1];
        }
        
        public T[] Peek(int PeekCount)
        {
            // see if there are any items to peek
            if (Count == 0) return null;

            // adjust the pop count if peeking too many
            if (PeekCount > Count) PeekCount = Count;

            // create the array that returns the items
            T[] items = new T[PeekCount];

            // start looping through the list adding the item
            for (int i = 0; i < PeekCount; i++)
            {
                // save the item
                items[i] = list[Count - 1 - i];
            }

            // all done
            return items;
        }

        
        public T[] PeekAll()
        {
            return Peek(Count);
        }
        
        /// <summary>
        /// Search the stack and find a matching T.  
        /// </summary>
        /// <param name="Item">The item to search for</param>
        /// <returns>This method returns an integer that indicates the number of Pop()'s required
        /// to retrieve the found item.  If the item is not found, it returns 0</returns>
        public int Search(T Item)
        {
            int numPops = 0;
            bool found = false;

            for (int i = 0; i < this.Count; i++)
            {
                numPops++;
                if (list[Count - 1 - i].Equals(Item) == true)
                {
                    // we found a match
                    found = true;
                    break;
                }
            }

            if (found == true)
                return numPops;
            else
                return 0;
        }

        
        public int Search(string Item)
        {
            int numPops = 0;
            bool found = false;

            for (int i = 0; i < this.Count; i++)
            {
                numPops++;
                if (list[Count - 1 - i].ToString() == Item)
                {
                    // we found a match
                    found = true;
                    break;
                }
            }

            if (found == true)
                return numPops;
            else
                return 0;

        }
        
        /// <summary>
        /// InsertOnSubmit the item from the stack.  For example, to remove the "oldest" (or first item) use Stack.RemoveItem(0);
        /// </summary>
        /// <param name="index"></param>
        public void RemoveItem(int index)
        {
            if (index < 0) return;
            if (index >= Count) return;

            this.list.RemoveAt(index);
        }
        

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public class ExStackEnumerator<T> : IEnumerator<T>
    {
        #region Local Variables

        private System.Collections.Generic.List<T> items = null;

        private int location;

        #endregion

        #region Constructor

        public ExStackEnumerator(System.Collections.Generic.List<T> Items)
        {
            items = Items;
            location = -1;
        }

        #endregion

        #region IEnumerator<T> Members

        public T Current
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
