using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Data
{
    public class AssociatableQueryableCollection<T> : QueryableCollection<T>, IAssociatableCollection<T>
    {
        public AssociatableQueryableCollection(IQueryable<T> queryable)
        {
            this.Queryable = queryable;
        }

        List<T> _AddedItems;
        List<T> AddedItems
        {
            get
            {
                if (this._AddedItems == null)
                {
                    this._AddedItems = new List<T>();
                }

                return this._AddedItems;
            }
        }

        List<T> _RemovedItems;
        List<T> RemovedItems
        {
            get
            {
                if (this._RemovedItems == null)
                {
                    this._RemovedItems = new List<T>();
                }

                return this._RemovedItems;
            }
        }

        public IEnumerable<T> Added
        {
            get
            {
                return this.AddedItems;
            }
            
        }

        public IEnumerable<T> Removed
        {
            get
            {
                return this.RemovedItems;
            }
        }


        public override void Add(T item)
        {
            this.Add(item, false);
        }

        public override bool Remove(T item)
        {
            this.Remove(item, false);
            return true;
        }

        public virtual void Remove(T item, bool isAssociatable)
        {
            if (isAssociatable)
            {
                if (this.AddedItems.Contains(item))
                {
                    this.AddedItems.Remove(item);
                }
                else
                {
                    if (this.RemovedItems.Contains(item) == false)
                    {
                        this.RemovedItems.Add(item);
                    }
                }
            }
            else
            {
                this.Items.Remove(item);
            }
        }

        public virtual void Add(T item, bool isAssociatable)
        {
            if (isAssociatable)
            {

                if (!this.AddedItems.Contains(item))
                {
                    this.AddedItems.Add(item);
                }
            }
            else
            {
                this.Items.Add(item);
            }
        }      
    }
}
