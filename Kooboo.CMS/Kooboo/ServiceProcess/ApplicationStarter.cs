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
using System.ServiceProcess;
using System.Configuration.Install;

namespace Kooboo.ServiceProcess
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationStarter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStarter" /> class.
        /// </summary>
        public ApplicationStarter()
        {
            Commands.Add(new InstallCommand());
            Commands.Add(new UninstallCommand());
            Commands.Add(new ServiceCommand());
            Commands.Add(new ConsoleCommand());
        }

        private IList<ICommand> _commands;
        /// <summary>
        /// Gets the commands.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        public IList<ICommand> Commands
        {
            get
            {
                if (_commands == null)
                {
                    _commands = new CommandCollection(this);
                }
                return _commands;
            }
        }

        /// <summary>
        /// Runs the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Run(string[] args)
        {
            foreach (var each in Commands)
            {
                if (each.Execute(args))
                    break;
            }
        }

        /// <summary>
        /// Creates the installer.
        /// </summary>
        /// <param name="args">The <see cref="ServiceInstallEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        public Installer CreateInstaller(ServiceInstallEventArgs args)
        {
            var installArgs = new List<string>
            {
                "/LogFile=" + args.Install.LogFile,
                "/LogToConsole=false" + args.Install.LogToConsole,
            };
            if (!String.IsNullOrEmpty(args.Install.StateDir))
            {
                installArgs.Add("/InstallStateDir=" + args.Install.StateDir);
            }
            var installer = new IntegratedInstaller(installArgs.ToArray());
            installer.Installers.Add(CreateServiceInstaller(args));
            installer.Installers.Add(CreateServiceProcessInstaller(args));
            return installer;
        }

        /// <summary>
        /// Creates the service installer.
        /// </summary>
        /// <param name="args">The <see cref="ServiceInstallEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        public ServiceInstaller CreateServiceInstaller(ServiceInstallEventArgs args)
        {
            return new ServiceInstaller
            {
                ServiceName = args.Service.Name,
                Description = args.Service.Description,
                StartType = args.Service.StartMode,
            };
        }

        /// <summary>
        /// Creates the service process installer.
        /// </summary>
        /// <param name="args">The <see cref="ServiceInstallEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        public ServiceProcessInstaller CreateServiceProcessInstaller(ServiceInstallEventArgs args)
        {
            return new ServiceProcessInstaller
            {
                Account = args.Service.Account
            };
        }

        /// <summary>
        /// Raises the <see cref="E:ServiceInstalling" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ServiceInstallEventArgs" /> instance containing the event data.</param>
        public virtual void OnServiceInstalling(ServiceInstallEventArgs args)
        {
            if (ServiceInstalling != null)
            {
                ServiceInstalling(this, args);
            }
        }

        /// <summary>
        /// Called when [start].
        /// </summary>
        public virtual void OnStart()
        {
            if (Start != null)
            {
                Start(this, new EventArgs());
            }
        }

        /// <summary>
        /// Called when [stop].
        /// </summary>
        public virtual void OnStop()
        {
            if (Stop != null)
            {
                Stop(this, new EventArgs());
            }
        }

        /// <summary>
        /// Occurs when [service installing].
        /// </summary>
        public event EventHandler<ServiceInstallEventArgs> ServiceInstalling;

        /// <summary>
        /// Occurs when [start].
        /// </summary>
        public event EventHandler<EventArgs> Start;

        /// <summary>
        /// Occurs when [stop].
        /// </summary>
        public event EventHandler<EventArgs> Stop;
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommandCollection : List<ICommand>, IList<ICommand>, ICollection<ICommand>
    {
        private ApplicationStarter _starter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCollection" /> class.
        /// </summary>
        /// <param name="starter">The starter.</param>
        public CommandCollection(ApplicationStarter starter)
        {
            _starter = starter;
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public new void Add(ICommand item)
        {
            item.Starter = _starter;
            base.Add(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public new void Insert(int index, ICommand item)
        {
            item.Starter = _starter;
            base.Insert(index, item);
        }

        /// <summary>
        /// Gets or sets the <see cref="ICommand" /> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="ICommand" />.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public new ICommand this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                value.Starter = _starter;
                base[index] = value;
            }
        }

        void ICollection<ICommand>.Add(ICommand item)
        {
            Add(item);
        }

        void IList<ICommand>.Insert(int index, ICommand item)
        {
            Insert(index, item);
        }

        ICommand IList<ICommand>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ServiceInstallEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInstallEventArgs" /> class.
        /// </summary>
        public ServiceInstallEventArgs()
        {
            Install = new InstallArgs();
            Service = new ServiceArgs();
        }

        /// <summary>
        /// Gets or sets the install.
        /// </summary>
        /// <value>
        /// The install.
        /// </value>
        public InstallArgs Install { get; set; }

        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public ServiceArgs Service { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class InstallArgs
        {
            /// <summary>
            /// Gets or sets the state dir.
            /// </summary>
            /// <value>
            /// The state dir.
            /// </value>
            public string StateDir { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [log to console].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [log to console]; otherwise, <c>false</c>.
            /// </value>
            public bool LogToConsole { get; set; }

            /// <summary>
            /// Gets or sets the log file.
            /// </summary>
            /// <value>
            /// The log file.
            /// </value>
            public string LogFile { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ServiceArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ServiceArgs" /> class.
            /// </summary>
            public ServiceArgs()
            {
                Account = ServiceAccount.NetworkService;
                StartMode = ServiceStartMode.Automatic;
            }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            /// <value>
            /// The description.
            /// </value>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the account.
            /// </summary>
            /// <value>
            /// The account.
            /// </value>
            public ServiceAccount Account { get; set; }

            /// <summary>
            /// Gets or sets the start mode.
            /// </summary>
            /// <value>
            /// The start mode.
            /// </value>
            public ServiceStartMode StartMode { get; set; }
        }
    }
}
