using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    class ObjectPool:IDisposable
    {
        public ObjectPool()
        {
            this.Items = new Dictionary<ObjectKey, List<object>>();
        }

        Dictionary<ObjectKey, List<object>> Items
        {
            get;
            set;
        }

        public List<object> this[ObjectKey key]
        {
            get
            {
                if (this.Items.ContainsKey(key))
                {
                    return this.Items[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public void AddRange(ObjectKey key,List<object> items)
        {
            if (this.Items.ContainsKey(key))
            {
                this.Items[key].AddRange(items);
            }
            else
            {
                this.Items.Add(key, items);
            }
        }

        public void Add(ObjectKey key, object value)
        {
            if (this.Items.ContainsKey(key) == false)
            {
                this.Items.Add(key, new List<object>());
            }

            this.Items[key].Add(value);
        }

        public void Remove(ObjectKey key, object value)
        {
            if (this.Items.ContainsKey(key))
            {
                this.Items[key].Remove(value);
            }
        }

        #region Dispose
        ~ObjectPool()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var list in Items)
                {
                    foreach (var n in list.Value.OfType<IDisposable>())
                    {
                        n.Dispose();                     
                    }
                    list.Value.Clear();
                }

                this.Items.Clear();
            }

            //free unmanaged objects

            if (disposing)
            {
                //remove me from finalization list
                GC.SuppressFinalize(this);
            }


        }


        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
