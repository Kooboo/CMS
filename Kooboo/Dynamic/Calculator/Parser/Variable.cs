using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Dynamic.Calculator.Parser
{
    public class Variable
    {
        #region Local Variables

        // the name of the variable
        private string variableName;
        private string variableValue;

        // the token items that correspond with tis variable
        private System.Collections.Generic.List<TokenItem> tokenItems;

        #endregion

        #region Constructor

        public Variable(string VarName)
        {
            tokenItems = new List<TokenItem>();
            variableName = VarName;
            variableValue = "";
            
        }

        public Variable(string VarName, string VarValue)
        {
            tokenItems = new List<TokenItem>();
            variableName = VarName;
            variableValue = VarValue;
        }

        #endregion

        #region Public Properties

        public string VariableName
        {
            get
            {
                return variableName;
            }
        }

        public string VariableValue
        {
            get
            {

                return variableValue;
            }
            set
            {
                variableValue = value;
            }
        }


        /// <summary>
        /// This variables key in the collection
        /// </summary>
        public string CollectionKey
        {
            get
            {
                return variableName.Trim().ToLower();
            }
        }

        public System.Collections.Generic.List<TokenItem> TokenItems
        {
            get
            {
                return tokenItems;
            }
        }


        #endregion

        #region Public Methods

        public Variable Clone()
        {
            // create a new variable using the current name and value
            return new Variable(this.variableName, this.variableValue);
            
        }

        #endregion

    }
}
