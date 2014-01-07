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

namespace Kooboo
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete]
    public static class ApplicationInitialization
    {
        #region Fields
        private class InitializationItem
        {
            public Action InitializeMethod { get; set; }
            public int Priority { get; set; }
        }
        private static List<InitializationItem> items = new List<InitializationItem>();
        #endregion

        #region Methods
        /// <summary>
        /// Registers the initializer method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="priority">The priority.</param>
        public static void RegisterInitializerMethod(Action method, int priority)
        {
            items.Add(new InitializationItem() { InitializeMethod = method, Priority = priority });
        }
        /// <summary>
        /// Executes this instance.
        /// </summary>
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
        #endregion
    }
}
