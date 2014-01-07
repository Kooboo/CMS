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
using System.Web;

namespace Kooboo.Web
{
    public class TrustLevelUtility
    {
        private static object lockObj = new object();
        private static AspNetHostingPermissionLevel? _trustLevel;

        public static AspNetHostingPermissionLevel CurrentTrustLevel
        {
            get
            {
                if (_trustLevel == null)
                {
                    lock(lockObj)
                    {
                        if (_trustLevel == null)
                        {
                            _trustLevel = GetCurrentTrustLevel();
                        }
                    }
                }
                return _trustLevel.Value;
            }
        }

        private static AspNetHostingPermissionLevel GetCurrentTrustLevel()
        {
            foreach (AspNetHostingPermissionLevel trustLevel in
                    new AspNetHostingPermissionLevel[] {
                AspNetHostingPermissionLevel.Unrestricted,
                AspNetHostingPermissionLevel.High,
                AspNetHostingPermissionLevel.Medium,
                AspNetHostingPermissionLevel.Low,
                AspNetHostingPermissionLevel.Minimal 
            })
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                }
                catch (System.Security.SecurityException)
                {
                    continue;
                }

                return trustLevel;
            }

            return AspNetHostingPermissionLevel.None;
        }
    }
}
