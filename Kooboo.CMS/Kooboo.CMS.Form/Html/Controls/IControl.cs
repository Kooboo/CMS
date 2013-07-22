﻿#region License
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


namespace Kooboo.CMS.Form.Html.Controls
{
    public interface IControl
    {
        string Name { get; }

        string Render(ISchema schema, IColumn column);

        bool IsFile { get; }

        bool HasDataSource { get; }

        string GetValue(object oldValue, string newValue);

        string DataType { get; }
    }
}
