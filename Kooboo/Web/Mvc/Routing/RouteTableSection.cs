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
using System.IO;
using System.Xml;
using System.Configuration;
using System.Collections;


namespace Kooboo.Web.Mvc.Routing
{
    public class RouteTableSection : ConfigurationSection
    {
        public RouteTableSection()
        {
        }

        [ConfigurationProperty("ignores", IsDefaultCollection = false)]
        public IgnoreRouteCollection Ignores
        {
            get
            {
                IgnoreRouteCollection ignoresCollection =
                (IgnoreRouteCollection)base["ignores"];
                return ignoresCollection;
            }
        }

        [ConfigurationProperty("routes", IsDefaultCollection = false)]
        public RouteElementCollection Routes
        {
            get
            {
                RouteElementCollection urlsCollection =
                (RouteElementCollection)base["routes"];
                return urlsCollection;
            }
        }


        protected override void DeserializeSection(System.Xml.XmlReader reader)
        {
            base.DeserializeSection(reader);
        }


        protected override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
        {
            return base.SerializeSection(parentElement, name, saveMode);
        }


        #region IStandaloneConfigurationSection Members

        public void DeserializeSection(string config)
        {
            this.DeserializeSection(new XmlTextReader(new StringReader(config)));
        }

        #endregion
    }
}

