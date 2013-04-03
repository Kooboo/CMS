using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Data
{
    public class Collection<T> : ICollection<T>
    {
        List<T> _Items;
        protected virtual List<T> Items
        {
            get
            {
                if (this._Items == null)
                {
                    this.Items = new List<T>();
                }
                return this._Items;
            }
            set
            {
                this._Items = value;
            }
        }


        public virtual void Add(T item)
        {
            this.Items.Add(item);
        }

        public virtual void Clear()
        {
            this.Items.Clear();

        }

        public virtual bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get
            {
                return this.Items.Count;

            }
        }

        public virtual bool IsReadOnly
        {
            get
            {

                return false;
            }
        }


        public virtual bool Remove(T item)
        {
            return this.Items.Remove(item);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }     
    }
}