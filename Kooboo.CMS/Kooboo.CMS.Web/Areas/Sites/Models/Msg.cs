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

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class Msg
    {
        public bool Success { get; set; }
        public string ErrMsg { get; set; }
    }
    public class Msg<T> : Msg
    {
        public T Data { get; set; }
    }
}
