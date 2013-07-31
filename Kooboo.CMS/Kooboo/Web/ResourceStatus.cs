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

namespace Kooboo.Web
{
    public enum ResourceStatus
    {
        OK = 200,

        Empty = 204,

        MovedPermanently = 301,

        SeeOther = 303,

        NotModified = 304,

        BadRequest = 400,

        NotFound = 404,

        NotAcceptable = 406,

        InternalServerError = 500,

        ServiceUnavailable = 503,
    }
}
