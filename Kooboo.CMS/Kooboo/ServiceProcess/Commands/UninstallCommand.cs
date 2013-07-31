#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;

namespace Kooboo.ServiceProcess
{
    /// <summary>
    /// 
    /// </summary>
    public class UninstallCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the starter.
        /// </summary>
        /// <value>
        /// The starter.
        /// </value>
        public ApplicationStarter Starter { get; set; }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public bool Execute(string[] args)
        {
            if (!args.Contains("-u"))
                return false;

            var installArgs = new ServiceInstallEventArgs();
            Starter.OnServiceInstalling(installArgs);

            UninstallService(installArgs);

            return true;
        }

        /// <summary>
        /// Uninstalls the service.
        /// </summary>
        /// <param name="args">The <see cref="ServiceInstallEventArgs" /> instance containing the event data.</param>
        public void UninstallService(ServiceInstallEventArgs args)
        {
            Console.WriteLine("Uninstalling service ......");
            using (var installer = Starter.CreateInstaller(args))
            {
                IDictionary state = new Hashtable();
                try
                {
                    installer.Uninstall(state);
                    Console.WriteLine("Service uninstalled.");
                }
                catch
                {
                    installer.Rollback(state);
                    throw;
                }
            }
        }

    }
}
