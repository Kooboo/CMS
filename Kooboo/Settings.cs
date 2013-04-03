using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Extensions;
namespace Kooboo
{
    public static class Settings
    {
        static bool isHostByIIS = false;
        static Settings()
        {
            isHostByIIS = AppDomain.CurrentDomain.FriendlyName.ToUpper().StartsWith("/LM/W3SVC/");
        }
        public static string BaseDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
        }
        public static string BinDirectory
        {
            get
            {
                if (IsWebApplication)
                {
                    return AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
                }
                else
                {
                    return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                }
            }
        }
        public static string ComponentsDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Components";
            }
        }

        public static bool IsWebApplication
        {
            get
            {
                return System.IO.Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile).EqualsOrNullEmpty("Web.config", StringComparison.OrdinalIgnoreCase);
            }
        }

        public static bool IsHostByIIS
        {
            get
            {
                return isHostByIIS;
            }
        }
    }


}
