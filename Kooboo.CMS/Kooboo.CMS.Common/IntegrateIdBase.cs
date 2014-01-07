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

namespace Kooboo.CMS.Common
{
    public abstract class IntegrateIdBase
    {
        public string[] Segments { get; protected set; }
        public IntegrateIdBase() { }
        public IntegrateIdBase(string id)
        {
            Splite(id);
        }
        private void Splite(string contentId)
        {
            if (!string.IsNullOrEmpty(contentId))
            {
                Segments = contentId.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        public string Id
        {
            get
            {
                if (Segments != null)
                {
                    return string.Join("#", Segments);
                }
                return null;
            }
        }
        public override string ToString()
        {
            return Id;
        }
    }
}
