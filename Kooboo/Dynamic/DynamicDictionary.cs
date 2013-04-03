using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections;

namespace Kooboo.Dynamic
{

    /// <summary>
    /// DynamicDictionary can not be serialized in MONO.
    /// </summary>
    public class DynamicDictionary : DynamicObject, IDictionary<string, object>
    {
        public DynamicDictionary()
        {
            dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        public DynamicDictionary(IDictionary<string, object> dictionary)
        {
            this.dictionary = new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
        }
        IDictionary<string, object> dictionary;

        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return dictionary.Keys;
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            string name = binder.Name;

            if (!dictionary.TryGetValue(name, out result))
            {
                result = null;
            }
            return true;
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            dictionary[binder.Name] = value;

            return true;
        }

        #region IDictionary<string,object> Members

        public void Add(string key, object value)
        {
            this.dictionary[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return this.dictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return this.dictionary.Values; }
        }

        public object this[string key]
        {
            get
            {
                if (this.dictionary.ContainsKey(key))
                {
                    return dictionary[key];
                }
                return null;
            }
            set
            {
                dictionary[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<string,object>> Members

        public void Add(KeyValuePair<string, object> item)
        {
            ((ICollection<KeyValuePair<string, object>>)this.dictionary).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, object>>)this.dictionary).Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)this.dictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, object>>)this.dictionary).CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<string, object>>)this.dictionary).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)this.dictionary).Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>> Members

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return ((ICollection<KeyValuePair<string, object>>)this.dictionary).GetEnumerator();
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
