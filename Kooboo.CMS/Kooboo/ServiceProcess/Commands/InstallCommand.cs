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
using System.Reflection;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Kooboo.ServiceProcess
{
    /// <summary>
    /// 
    /// </summary>
    public class InstallCommand : ICommand
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
            if (!args.Contains("-i"))
                return false;


            var installArgs = new ServiceInstallEventArgs();
            Starter.OnServiceInstalling(installArgs);

            InstallService(installArgs);
            StartService(installArgs);

            return true;
        }

        /// <summary>
        /// Installs the service.
        /// </summary>
        /// <param name="args">The <see cref="ServiceInstallEventArgs" /> instance containing the event data.</param>
        public void InstallService(ServiceInstallEventArgs args)
        {
            Console.WriteLine("Installing service......");

            using (var installer = Starter.CreateInstaller(args))
            {
                IDictionary state = new Hashtable();
                try
                {
                    // Install service
                    installer.Install(state);
                    installer.Commit(state);
                    Console.WriteLine("Service installed.");
                }
                catch
                {
                    installer.Rollback(state);
                    throw;
                }
            }
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="args">The <see cref="ServiceInstallEventArgs" /> instance containing the event data.</param>
        protected void StartService(ServiceInstallEventArgs args)
        {
            using (var sc = new ServiceController(args.Service.Name))
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                Console.WriteLine("Service started.");
            }
        }
    }


}
