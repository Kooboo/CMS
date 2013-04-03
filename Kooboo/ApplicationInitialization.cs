using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo
{
    public static class ApplicationInitialization
    {
        private class InitializationItem
        {
            public Action InitializeMethod { get; set; }
            public int Priority { get; set; }
        }
        private static List<InitializationItem> items = new List<InitializationItem>();
        public static void RegisterInitializerMethod(Action method, int priority)
        {
            items.Add(new InitializationItem() { InitializeMethod = method, Priority = priority });
        }
        public static void Execute()
        {
            lock (items)
            {
                foreach (var item in items.OrderBy(it => it.Priority))
                {
                    item.InitializeMethod();
                }
                items.Clear();
            }

        }
    }
}
