#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.Dynamic.Calculator.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public class IIFShortCircuit
    {

        #region Local Variables

        // the rpn queue for the condition statement is saved in this queue object
        private Support.ExQueue<TokenItem> rpn_condition = null;

        // the rpn queue for the true porition of the iif[] statement
        private Support.ExQueue<TokenItem> rpn_true = null;

        // the rpn queue for the false portion of the iif[] statement
        private Support.ExQueue<TokenItem> rpn_false = null;

        // This short circuit object will have a token item as the parent
        private TokenItem parent;

        #endregion

        #region Constructor

        public IIFShortCircuit(TokenItem Parent)
        {
            parent = Parent;

            rpn_condition = new Support.ExQueue<TokenItem>();
            rpn_true = new Support.ExQueue<TokenItem>();
            rpn_false = new Support.ExQueue<TokenItem>();
        }

        #endregion

        #region Public Properties


        /// <summary>
        /// This short circuit object will have a token item as the parent
        /// </summary>
        public TokenItem Parent
        {
            get
            {
                return parent;
            }
        }


        /// <summary>
        /// This is the RPN for the condition parameter of the iif[] Operand function
        /// </summary>
        public Support.ExQueue<TokenItem> RPNCondition
        {
            get
            {
                return rpn_condition;
            }
            set
            {
                rpn_condition = value;
            }
        }

        /// <summary>
        /// This is the RPN for the "true" parameter of the iif[] Operand function
        /// </summary>
        public Support.ExQueue<TokenItem> RPNTrue
        {
            get
            {
                return rpn_true;
            }
            set
            {
                rpn_true = value;
            }
        }

        /// <summary>
        /// This is the RPN for the "false" parameter of the iif[] Operand function
        /// </summary>
        public Support.ExQueue<TokenItem> RPNFalse
        {
            get
            {
                return rpn_false;
            }
            set
            {
                rpn_false = value;
            }
        }


        #endregion

        #region Public Methods

        public TokenItem Evaluate(out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";

            // local variables
            string result = "";
            
            // get the parent token object
            Token token = this.Parent.Parent.Parent;

            // create the evaluator object to evaluate the condition
            Evaluate.Evaluator eval = null;
            try
            {
                eval = new Evaluate.Evaluator(token);
            }
            catch (Exception err)
            {
                ErrorMsg = "Failed to evaluation the condition in IIFShortCircuit(): " + err.Message;
                return null;
            }


            // evaluate the condition
            if (eval.Evaluate(this.rpn_condition, out result, out ErrorMsg) == false) return null;

            // determine if we need to run the "true" or "false" result
            Support.ExQueue<TokenItem> resultQueue = null;
            if (result.Trim().ToLower() == "true")
            {
                // evaluate the true parameter
                resultQueue = this.rpn_true;
            }
            else
            {
                // evaluate the false parameter
                resultQueue = this.rpn_false;
            }

            // do the final evaluation
            if (eval.Evaluate(resultQueue, out result, out ErrorMsg) == false) return null;

            // all done
            return new TokenItem(result, TokenType.Token_Operand, TokenDataType.Token_DataType_String, false);            
        }

        #endregion


    }
}
