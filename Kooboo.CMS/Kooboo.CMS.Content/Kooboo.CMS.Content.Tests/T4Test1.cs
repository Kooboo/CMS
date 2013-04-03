using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;
using System.CodeDom.Compiler;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Tests
{
    public class ParameterProcessor : DirectiveProcessor
    {

        public override void FinishProcessingRun()
        {

        }

        public override string GetClassCodeForProcessingRun()
        {
            return "GetClassCodeForProcessingRun";
        }

        public override string[] GetImportsForProcessingRun()
        {
            return new string[0];
        }

        public override string GetPostInitializationCodeForProcessingRun()
        {
            return "GetPostInitializationCodeForProcessingRun";
        }

        public override string GetPreInitializationCodeForProcessingRun()
        {
            return "GetPostInitializationCodeForProcessingRun";
        }

        public override string[] GetReferencesForProcessingRun()
        {
            return new string[0];
        }

        public override bool IsDirectiveSupported(string directiveName)
        {
            return true;
        }

        public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
        {

        }
    }
    //The text template transformation engine is responsible for running 
    //the transformation process.
    //The host is responsible for all input and output, locating files, 
    //and anything else related to the external environment.
    //-------------------------------------------------------------------------
    [Serializable]
    public class CustomCmdLineHost : ITextTemplatingEngineHost, ITextTemplatingSessionHost
    {
        #region ITextTemplatingEngineHost
        internal string TemplateFileValue = "a.tt";
        public string TemplateFile
        {
            get { return TemplateFileValue; }
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
                    typeof(System.Uri).Assembly.Location
                };
            }
        }

        public IList<string> StandardImports
        {
            get
            {
                return new string[]
                {
                    "System"
                };
            }
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = System.String.Empty;
            location = System.String.Empty;

            if (File.Exists(requestFileName))
            {
                content = File.ReadAllText(requestFileName);
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
            if (File.Exists(assemblyReference))
            {
                return assemblyReference;
            }
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), assemblyReference);
            if (File.Exists(candidate))
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
            //If the argument is the fully qualified path of an existing file,
            //then we are done
            //----------------------------------------------------------------
            if (File.Exists(fileName))
            {
                return fileName;
            }
            //Maybe the file is in the same folder as the text template that 
            //called the directive.
            //----------------------------------------------------------------
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }
            //Look more places.
            //----------------------------------------------------------------
            //More code can go here...
            //If we cannot do better, return the original file name.
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
            //Code to provide "hard-coded" parameter values goes here.
            //This code depends on the directive processors this host will interact with.
            //If we cannot do better, return the empty string.
            return string.Empty;
        }
        public void SetFileExtension(string extension)
        {
            //The parameter extension has a '.' in front of it already.
            //--------------------------------------------------------
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
            return AppDomain.CreateDomain("Generation App Domain");
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
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class T4Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var templateString = @"
<#@ template debug=""false"" hostspecific=""false"" language=""C#"" #>
<#@ parameter name=""Column"" type=""Kooboo.CMS.Content.Models.Column"" #>
<input name=""{0}"" type=""{1}"" value=""<%= Model["" <#= Column.Name #>""] %>"" />";

            Engine engine = new Engine();

            var host = new CustomCmdLineHost();
            host.Session = new TextTemplatingSession();
            host.Session["Column"] = new Column() { Name = "Column1" };            
            string output = engine.ProcessTemplate(templateString, host);

            foreach (CompilerError item in host.Errors)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(output);

        }
    }
}
