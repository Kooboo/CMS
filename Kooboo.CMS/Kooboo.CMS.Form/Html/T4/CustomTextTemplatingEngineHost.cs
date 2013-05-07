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
using Microsoft.VisualStudio.TextTemplating;
using System.CodeDom.Compiler;
using System.IO;

namespace Kooboo.Form.Html.T4
{
    public class CustomTextTemplatingEngineHost : ITextTemplatingEngineHost, ITextTemplatingSessionHost
    {
        #region ITextTemplatingEngineHost
        private string templateFileValue = "";
        public string TemplateFile
        {
            get { return templateFileValue; }
            set { templateFileValue = value; }
        }
        private string fileExtensionValue = ".txt";
        public string FileExtension
        {
            get { return fileExtensionValue; }
        }
        private Encoding fileEncodingValue = Encoding.UTF8;
        public Encoding FileEncoding
        {
            get { return fileEncodingValue; }
        }

        private CompilerErrorCollection errorsValue;
        public CompilerErrorCollection Errors
        {
            get { return errorsValue; }
        }

        public IList<string> StandardAssemblyReferences
        {
            get
            {
                return new string[]
                {
                    typeof(System.Uri).Assembly.Location,
                    typeof(System.Linq.Enumerable).Assembly.Location,
                    typeof(Kooboo.SR).Assembly.Location,
                    typeof(Kooboo.Form.IColumn).Assembly.Location
                };
            }
        }

        public IList<string> StandardImports
        {
            get
            {
                return new string[]
                {
                    "System",
                    "System.Linq",
                    "Kooboo.Form"
                };
            }
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = System.String.Empty;
            location = System.String.Empty;

            if (System.IO.File.Exists(requestFileName))
            {
                content = System.IO.File.ReadAllText(requestFileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public object GetHostOption(string optionName)
        {
            object returnObject;
            switch (optionName)
            {
                case "CacheAssemblies":
                    returnObject = true;
                    break;
                default:
                    returnObject = null;
                    break;
            }
            return returnObject;
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (System.IO.File.Exists(assemblyReference))
            {
                return assemblyReference;
            }
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), assemblyReference);
            if (System.IO.File.Exists(candidate))
            {
                return candidate;
            }

            return "";
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            //if (string.Compare(processorName, "ParameterDirectiveProcessor", StringComparison.OrdinalIgnoreCase) == 0)
            //{
            //    // return typeof(ParameterProcessor);
            //}          
            throw new Exception("Directive Processor not found");
        }
        public string ResolvePath(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("the file name cannot be null");
            }

            if (System.IO.File.Exists(fileName))
            {
                return fileName;
            }

            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), fileName);
            if (System.IO.File.Exists(candidate))
            {
                return candidate;
            }

            return fileName;
        }
        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (directiveId == null)
            {
                throw new ArgumentNullException("the directiveId cannot be null");
            }
            if (processorName == null)
            {
                throw new ArgumentNullException("the processorName cannot be null");
            }
            if (parameterName == null)
            {
                throw new ArgumentNullException("the parameterName cannot be null");
            }
            return string.Empty;
        }
        public void SetFileExtension(string extension)
        {

            fileExtensionValue = extension;
        }

        public void SetOutputEncoding(System.Text.Encoding encoding, bool fromOutputDirective)
        {
            fileEncodingValue = encoding;
        }
        public void LogErrors(CompilerErrorCollection errors)
        {
            errorsValue = errors;
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain.CreateDomain("Generation App Domain", null, AppDomain.CurrentDomain.SetupInformation);
        }
        #endregion

        #region ITextTemplatingSessionHost Members

        public ITextTemplatingSession CreateSession()
        {
            return this.Session;
        }

        public ITextTemplatingSession Session
        {
            get;
            set;
        }

        #endregion
    }
}
