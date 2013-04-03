using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    class ObjectKey
    {
        public ObjectKey(Type type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public Type Type
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }


        public override bool Equals(object obj)
        {
            if (obj is ObjectKey)
            {
                var key = obj as ObjectKey;
                return this.Type == key.Type && this.Name == key.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            //TODO: should improve hash algorithm  
            var hashcode = "ObjectKey.Type=" + this.Type == null ? "null" : this.Type.ToString();
            hashcode += ";ObjectKey.Name=" + this.Name == null ? "null" : this.Name;

            return hashcode.GetHashCode();

            
        }

    }
}
