using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.IoC;

namespace Kooboo.Reflection
{
    public static class Binder
    {
        public static object Bind(this object obj, string fieldName)
        {
            if (obj == null)
            {
                return "";
            }

            var binders = ContextContainer.Current.Resolve<Binders>();//keep one request one instance

            Members binder;

            
            if (binders.ContainsKey(obj))
            {
                binder = binders[obj];
            }
            else
            {
                binder = new Members(obj);
                binders.Add(obj,binder);
            }

            return binder.Properties[fieldName];
        }
    }


    public class Binders : Dictionary<object, Members>
    {
    }
  
}
