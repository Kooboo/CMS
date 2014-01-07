#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.ServiceProcess
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleCommand : ICommand
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
            try
            {
                if (!Environment.UserInteractive)
                    return false;

                Starter.OnStart();
                while (true)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
            finally
            {
                Starter.OnStop();
            }
            return true;
        }
    }
}
