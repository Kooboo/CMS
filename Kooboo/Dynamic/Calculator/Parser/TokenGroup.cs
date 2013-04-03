using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Dynamic.Calculator.Parser
{
    public class TokenGroup
    {
        #region Local Variables

        private Tokens tokens = null;
        private Variables variables = null;

        #endregion

        #region Constructor

        public TokenGroup()
        {
            tokens = new Tokens(this);
            variables = new Variables();
        }

        #endregion

        #region Public Properties

        public Tokens Tokens
        {
            get
            {
                return tokens;
            }
        }

        public Variables Variables
        {
            get
            {
                return variables;
            }
        }

        #endregion

        #region Private/Internal Methods


        /// <summary>
        /// This method is called when we add a token to Group.Tokens.DeleteOnSubmit(tk).  This methods allows us to collect the 
        /// unique variables.
        /// </summary>
        /// <param name="tk"></param>
        internal void UpdateVariables(Token tk)
        {

            // loop through the variables in the current token
            foreach (Parser.Variable var in tk.Variables)
            {

                // check if the variable exists it the Group.Variables collection
                if (this.variables.VariableExists(var.VariableName) == false)
                {

                    // we found a new variable to add to our Group.Variables collection
                    this.variables.Add(var.VariableName);
                }

            }


        }

        #endregion

        #region Public Methods

        public bool EvaluateGroup()
        {

            string value = "";
            string ErrorMsg = "";
            bool anyFailures = false;

            // first, populate the variable values in each of the tokens
            foreach (Parser.Token tk in this.tokens)
            {
                // loop through each tokens variables
                foreach (Parser.Variable var in tk.Variables)
                {
                    if (this.variables.VariableExists(var.VariableName) == true)
                    {
                        var.VariableValue = this.variables[var.VariableName].VariableValue;
                    }
                }

                // see if we have tokens and an RPN queue
                if ((tk.TokenItems.Count > 0) && (tk.RPNQueue.Count > 0)) tk.LastErrorMessage = ""; // reset the last error message
                
                // the variables for the token have been populated; evaluate the token
                Evaluate.Evaluator eval = new Evaluate.Evaluator(tk);

                if (eval.Evaluate(out value, out ErrorMsg) == false)
                {
                    tk.LastErrorMessage = ErrorMsg;
                    anyFailures = true;
                }

            }

            return anyFailures;
            
        }

        #endregion
    }
}
