using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Data
{
    public class CascadableCollection<T> : AssociatableCollection<T>, ICascadableCollection<T>
    {
   
        public IEnumerable<T> Updated
        {
            get
            {
                return this.Items.Where(i => this.AddedItems.Contains(i) == false && this.RemovedItems.Contains(i) == false);
            }
        }
    }
}
