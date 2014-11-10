#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Services
{
    public class Entry<T>
    {
        public Entry()
        {

        }
        public Entry(string fullName, T entryObject)
        {
            FullName = fullName;
            var names = fullName.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Name = names.Last();

            this.EntryObject = entryObject;
        }
        public string Name { get; set; }

        public string FullName { get; set; }

        public T EntryObject { get; set; }
    }
    public class Namespace<T>
    {
        public Namespace()
        {
            ChildNamespaces = new Namespace<T>[0];
            Entries = new Entry<T>[0];
        }
        public string Name { get; set; }
        public string FullName { get; set; }
        public IEnumerable<Namespace<T>> ChildNamespaces { get; set; }
        public IEnumerable<Entry<T>> Entries { get; set; }
        public void AddNamespace(Namespace<T> ns)
        {
            ChildNamespaces = ChildNamespaces.Concat(new[] { ns });
        }
        public void AddEntry(string fullName, T entry)
        {
            Entries = Entries.Concat(new[] { new Entry<T>(fullName, entry) });
        }

        public Namespace<T> GetNamespaceNode(string ns)
        {
            if (string.IsNullOrEmpty(ns))
            {
                return this;
            }
            string[] nsSplit = ns.Split(new char[] { '.' });

            Namespace<T> node = this;
            foreach (var name in nsSplit)
            {
                node = node.ChildNamespaces.Where(it => it.Name.EqualsOrNullEmpty(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (node == null)
                {
                    return node;
                }
            }
            return node;
        }

    }

    public static class NamespaceParser
    {
        public static Namespace<T> Extract<T>(IEnumerable<T> list)
            where T : PathResource
        {
            Dictionary<string, Namespace<T>> namespaces = new Dictionary<string, Namespace<T>>(StringComparer.CurrentCultureIgnoreCase);
            Namespace<T> root = new Namespace<T>();
            foreach (var item in list)
            {
                string parentName = "";
                Namespace<T> last = null;
                var names = item.Name.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var name in names.Take(names.Length - 1))
                {
                    string fullName = "";
                    if (string.IsNullOrEmpty(parentName))
                    {
                        fullName = name;
                    }
                    else
                    {
                        fullName = parentName + "." + name;
                    }
                    if (!namespaces.ContainsKey(fullName))
                    {
                        last = new Namespace<T>()
                        {
                            Name = name,
                            FullName = fullName
                        };
                        namespaces[fullName] = last;
                        if (!string.IsNullOrEmpty(parentName))
                        {
                            namespaces[parentName].AddNamespace(last);
                        }
                        else
                        {
                            root.AddNamespace(last);
                        }
                    }
                    else
                    {
                        last = namespaces[fullName];
                    }
                    parentName = fullName;
                }
                if (last == null)
                {
                    root.AddEntry(item.Name, item);
                }
                else
                {
                    last.AddEntry(item.Name, item);
                }
            }
            return root;
        }
    }
}
