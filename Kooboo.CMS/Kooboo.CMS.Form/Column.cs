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

namespace Kooboo.Form
{
    public class Column : IColumn
    {
        #region IColumn Members

        public string Name
        {
            get;
            set;
        }

        public string Label
        {
            get;
            set;
        }

        public DataType DataType
        {
            get;
            set;
        }

        public string ControlType
        {
            get;
            set;
        }

        public string Tooltip
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem> SelectionItems
        {
            get;
            set;
        }

        public IEnumerable<ColumnValidation> Validations
        {
            get;
            set;
        }

        public string DefaultValue
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public bool Modifiable
        {
            get;
            set;
        }

        public bool ShowInGrid
        {
            get;
            set;
        }

        #endregion
    }
}
