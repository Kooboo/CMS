using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.CMS.Account.Services
{
    public static class ServiceFactory
    {
        static Hashtable services = new Hashtable();
        static ServiceFactory()
        {
            services.Add(typeof(RoleManager), new RoleManager());
            services.Add(typeof(UserManager), new UserManager());
        }
        public static RoleManager RoleManager
        {
            get
            {
                return (RoleManager)services[typeof(RoleManager)];
            }
            set
            {
                services[typeof(RoleManager)] = value;
            }
        }

        public static UserManager UserManager
        {
            get
            {
                return (UserManager)services[typeof(UserManager)];
            }
            set
            {
                services[typeof(UserManager)] = value;
            }
        }
        public static T GetService<T>()
        {
            foreach (var service in services.Values)
            {
                if (service is T)
                {
                    return (T)service;
                }
            }
            return default(T);
        }
    }
}
