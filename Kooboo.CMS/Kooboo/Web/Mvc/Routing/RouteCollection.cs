#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
/*
Kooboo is a content management system based on ASP.NET MVC framework. Copyright 2009 Yardi Technology Limited.

This program is free software: you can redistribute it and/or modify it under the terms of the
GNU General Public License version 3 as published by the Free Software Foundation.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.
If not, see http://www.kooboo.com/gpl3/.
*/
using System;
using System.Configuration;
using System.Collections;


namespace Kooboo.Web.Mvc.Routing
{

    [ConfigurationCollection(typeof(RouteConfigElement))]
    public class RouteElementCollection : ConfigurationElementCollection
    {
        public RouteElementCollection()
        {
        }


        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new RouteConfigElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new RouteConfigElement(elementName);
        }


        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((RouteConfigElement)element).Name;
        }


        public new string AddElementName
        {
            get { return base.AddElementName; }
            set { base.AddElementName = value; }
        }


        public new string ClearElementName
        {
            get { return base.ClearElementName; }
            set { base.AddElementName = value; }
        }

        
        public new string RemoveElementName
        {
            get { return base.RemoveElementName; }
        }


        public new int Count
        {
            get { return base.Count; }
        }


        public RouteConfigElement this[int index]
        {
            get { return (RouteConfigElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        
        new public RouteConfigElement this[string Name]
        {
            get { return (RouteConfigElement)BaseGet(Name); }
        }


        public int IndexOf(RouteConfigElement url)
        {
            return BaseIndexOf(url);
        }


        public void Add(RouteConfigElement url)
        {
            BaseAdd(url);
        }

        
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }


        public void Remove(RouteConfigElement url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Name);
        }

        
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }


        public void Remove(string name)
        {
            BaseRemove(name);
        }

        
        public void Clear()
        {
            BaseClear();
        }
    }
}
