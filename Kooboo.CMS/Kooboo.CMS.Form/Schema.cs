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
    public class Schema : ISchema
    {
        private List<IColumn> columns = new List<IColumn>();
        #region ISchema Members

        public string Name
        {
            get;
            set;
        }

        public IEnumerable<IColumn> Columns
        {
            get { return columns; }
        }

        public IColumn this[string name]
        {
            get { return columns.Where(it => string.Compare(it.Name, name, true) == 0).FirstOrDefault(); }
        }

        #endregion
    }
}
