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
using System.Xml.Linq;

namespace Kooboo.CMS.Modules.CMIS.Models
{
    /// <summary>
    /// Defines CMIS namespace references.
    /// </summary>
    public static class CmisNs
    {
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the appns URI (http://www.w3.org/2007/app).
        /// CMIS suggested prefix: app.
        /// </summary>
        public const string App = "http://www.w3.org/2007/app";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the atomns URI (http://www.w3.org/2005/Atom).
        /// CMIS suggested prefix: atom.
        /// </summary>
        public const string Atom = "http://www.w3.org/2005/Atom";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the cmisns URI (http://docs.oasis-open.org/ns/cmis/core/200908/).
        /// CMIS suggested prefix: cmis.
        /// </summary>
        public const string Cmis = "http://docs.oasis-open.org/ns/cmis/core/200908/";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the cmismns URI (http://docs.oasis-open.org/ns/cmis/messaging/200908/).
        /// CMIS suggested prefix: cmism.
        /// </summary>
        public const string Cmism = "http://docs.oasis-open.org/ns/cmis/messaging/200908/";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the cmisrans URI (http://docs.oasis-open.org/ns/cmis/restatom/200908/).
        /// CMIS suggested prefix: cmisra.
        /// </summary>
        public const string Cmisra = "http://docs.oasis-open.org/ns/cmis/restatom/200908/";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the W3 Schema instance URI (http://wwww.w3.org/2001/XMLSchema-instance).
        /// </summary>
        public const string W3instance = "http://wwww.w3.org/2001/XMLSchema-instance";

        /// <summary>
        /// Gets the <see cref="System.Xml.Linq.XNamespace"/> object that corresponds to the atomns URI
        /// (http://www.w3.org/2005/Atom). CMIS suggested prefix: atom.
        /// </summary>
        public static XNamespace XAtom
        {
            get { return (XNamespace)Atom; }
        }
        /// <summary>
        /// Gets the <see cref="System.Xml.Linq.XNamespace"/> object that corresponds to the cmisns URI
        /// (http://docs.oasis-open.org/ns/cmis/core/200908/). CMIS suggested prefix: cmis.
        /// </summary>
        public static XNamespace XCmis
        {
            get { return (XNamespace)Cmis; }
        }
        /// <summary>
        /// Gets the <see cref="System.Xml.Linq.XNamespace"/> object that corresponds to the cmisrans URI
        /// (http://docs.oasis-open.org/ns/cmis/restatom/200908/).
        /// </summary>
        public static XNamespace XCmisra
        {
            get { return (XNamespace)Cmisra; }
        }
        /// <summary>
        /// Gets the <see cref="System.Xml.Linq.XNamespace"/> object that corresponds to the W3 Schema instance URI
        /// (http://wwww.w3.org/2001/XMLSchema-instance). CMIS suggested prefix: cmisra.
        /// </summary>
        public static XNamespace XW3instance
        {
            get { return (XNamespace)W3instance; }
        }
    }
}
