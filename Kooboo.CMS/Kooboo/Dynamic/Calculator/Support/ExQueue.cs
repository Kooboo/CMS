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

namespace Kooboo.Dynamic.Calculator.Support
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExQueue<T> : IEnumerable<T>
    {

        #region Local Varibles

        private System.Collections.Generic.List<T> queue = null;

        #endregion

        #region Constructor


        public ExQueue()
        {
            queue = new List<T>(); 
        }

        public ExQueue(int Capacity)
        {
            queue = new List<T>(Capacity);
        }

        #endregion

        #region Public Methods

        public void Add(T item)
        {
            queue.Add(item);
        }

        public void Enqueue(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            queue.Clear();
        }

        public T Dequeue()
        {
            if (queue.Count == 0) return default(T);

            T t = queue[0];
            queue.RemoveAt(0);

            return t;
        }

        #endregion

        #region Public Properties

        public int Count
        {
            get
            {
                return queue.Count;
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

        #region Indexer

        public T this[int index]
        {
            get
            {
                return queue[index];
            }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new ExQueueEnumerator<T>(queue);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ExQueueEnumerator<T>(queue);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExQueueEnumerator<T> : IEnumerator<T>
    {

        #region Local Variables

        private System.Collections.Generic.List<T> items = null;

        private int location;

        #endregion

        #region Constructor

        public ExQueueEnumerator(System.Collections.Generic.List<T> Items)
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
