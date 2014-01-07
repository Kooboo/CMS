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
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    public class Declaration : ICollection<Property>
    {
        private ISet<Property> _set;

        protected ISet<Property> Set
        {
            get
            {
                if (_set == null)
                {
                    _set = new HashSet<Property>();
                }
                return _set;
            }
        }
 
        public void AddRange(IEnumerable<Property> items)
        {
            foreach (var each in items)
            {
                Add(each);
            }
        }

        public void Remove(string name)
        {
            var item = Set.FirstOrDefault(o => o.Name == name);
            if (item == null)
                return;

            Set.Remove(item);
        }

        public override string ToString()
        {
            return String.Join(";", Set.Select(o => o.ToString()));
        }

        public static Declaration Parse(string str)
        {
            Declaration result = new Declaration();

            foreach (var each in str.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Property property;
                if (Property.TryParse(each, out property))
                {
                    if (!property.IsInitial && !property.IsBrowserGenerated)
                    {
                        // We ignore browser generated property and inital property, it's not set by user.
                        result.Add(property);
                    }
                }
            }

            return result;
        }

        #region ICollection<Property> Members

        public void Add(Property item)
        {
            if (Set.Contains(item))
            {
                Set.Remove(item);
            }

            Set.Add(item);
        }

        public void Clear()
        {
            Set.Clear();
        }

        public bool Contains(Property item)
        {
            return Set.Contains(item);
        }

        public void CopyTo(Property[] array, int arrayIndex)
        {
            Set.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Set.Count; }
        }

        public bool IsReadOnly
        {
            get { return Set.IsReadOnly; }
        }

        public bool Remove(Property item)
        {
            return Set.Remove(item);
        }

        #endregion

        #region IEnumerable<Property> Members

        public IEnumerator<Property> GetEnumerator()
        {
            return Set.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
