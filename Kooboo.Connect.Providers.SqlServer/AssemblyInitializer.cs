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
using System.IO;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.Connect.Providers.SqlServer.AssemblyInitializer), "Initialize")]
namespace Kooboo.Connect.Providers.SqlServer
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            ApplicationInitialization.RegisterInitializerMethod(delegate()
            {
                Kooboo.Connect.UserServices.DefaultProvider = new MSSQLProvider();
            }, 0);
        }
    }
}
