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
using System.Threading.Tasks;

namespace Kooboo.CMS.Common.Extension.Tab
{
    public interface ITabPlugin : IApplyTo
    {
        string Name { get; }
        string DisplayText { get; }
        string VirtualPath { get; }

        bool Order { get; }

        Type ModelType { get; }

        void LoadData(TabContext context);

        void Submit(TabContext context);
    }
}
