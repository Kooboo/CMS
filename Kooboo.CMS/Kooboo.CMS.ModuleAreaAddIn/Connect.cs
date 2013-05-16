using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
namespace Kooboo.CMS.ModuleAreaAddIn
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2
    {
        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;
            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_applicationObject.Commands;
                string toolsMenuName;

                try
                {
                    ResourceManager resourceManager = new
                      ResourceManager("MyAddin1.CommandBar",
                      Assembly.GetExecutingAssembly());
                    CultureInfo cultureInfo = new
                      System.Globalization.CultureInfo
                      (_applicationObject.LocaleID);
                    string resourceName = String.Concat(cultureInfo.
                      TwoLetterISOLanguageName, "Tools");
                    toolsMenuName = resourceManager.GetString(resourceName);
                }
                catch
                {
                    toolsMenuName = "Tools";
                }

                CommandBar menuBarCommandBar =
                  ((CommandBars)_applicationObject.CommandBars)
                  ["MenuBar"];

                CommandBarControl toolsControl =
                  menuBarCommandBar.Controls[toolsMenuName];
                CommandBarPopup toolsPopup =
                  (CommandBarPopup)toolsControl;

                try
                {
                    Command command = commands.AddNamedCommand2
                      (_addInInstance, "MyAddin1", "MyAddin1", @"Executes  
                the command for MyAddin1", true, 59, ref 
                contextGUIDS, (int)vsCommandStatus.
                      vsCommandStatusSupported + (int)vsCommandStatus.
                      vsCommandStatusEnabled, (int)vsCommandStyle.
                      vsCommandStylePictAndText, vsCommandControlType.
                      vsCommandControlTypeButton);

                    if ((command != null) && (toolsPopup != null))
                    {
                        command.AddControl(toolsPopup.CommandBar, 1);
                    }
                }
                catch (System.ArgumentException)
                {
                }
            }

        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        //public void QueryStatus(string commandName,
        //    vsCommandStatusTextWanted neededText, ref vsCommandStatus status,ref object commandText)
        //{
        //    if (neededText ==
        //      vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
        //    {
        //        if (commandName == "MyAddin1.Connect.MyAddin1")
        //        {
        //            status = (vsCommandStatus)vsCommandStatus.
        //              vsCommandStatusSupported | vsCommandStatus.
        //              vsCommandStatusEnabled;
        //            return;
        //        }
        //    }
        //}

        //public void Exec(string commandName, vsCommandExecOption
        //    executeOption, ref object varIn, ref object varOut, ref bool handled)
        //{
        //    handled = false;
        //    if (executeOption ==
        //      vsCommandExecOption.vsCommandExecOptionDoDefault)
        //    {
        //        if (commandName == "MyAddin1.Connect.MyAddin1")
        //        {
        //            handled = true;
        //            System.Windows.Forms.MessageBox.
        //              Show("add-in running.");
        //            return;
        //        }
        //    }
        //}

        private DTE2 _applicationObject;
        private AddIn _addInInstance;
    }
}