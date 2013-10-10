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
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.WcfExtensions
{
    public class ExceptionHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return false;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            Kooboo.HealthMonitoring.Log.LogException(error);
        }
    }
}
