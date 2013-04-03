using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Dynamic.Calculator.Parser
{

    public enum TokenType
    {
        Token_Operand,                      // indicates the token is an operand 
        Token_Operand_Function_Start,       // indicates the token is the start of an operand function
        Token_Open_Parenthesis,             // indicates the token is an open parenthesis
        Token_Close_Parenthesis,            // indicates the token is a closed parenthesis
        Token_Operand_Function_Stop,        // indicates the token is the end of an operand function
        Token_Operand_Function_Delimiter,   // indicates the token is an operand function delimiter
        Token_Operator,                     // indicates the token is an operator
        Token_Assignemt_Start,              // indicates the token is the start of an assignment 
        Token_Assignment_Stop               // indicates the token is the stop of an assignment.
    };

    public enum TokenDataType
    {
        Token_DataType_None,            // the token does not have a data type (for example, an operator)
        Token_DataType_Variable,         // indicates that the token is a variable - unknown data type
        Token_DataType_Int,              // indicates that the token is an integer
        Token_DataType_Date,             // indicates that the token is date
        Token_DataType_Double,           // indicates the token is a double
        Token_DataType_String,           // indicates the token is a string
        Token_DataType_Boolean,          // indicates the token is a boolean
        Token_DataType_NULL              // indicates the token is a null value
    };



    public class TokenItem
    {
        #region Local Variables

        private string tokenName;
        private TokenType tokenType;
        private TokenDataType tokenDataType;
        private bool inOperandFunction = false;

        private bool willBeAssigned = false;  // Indicates if this token item is in an assignment

        // if this token type is Token_Operand_Function_Start and the 
        // operand function is iif[, this property indicates if the 
        // iif[ operand function can be short circiuted
        private bool canShortCircuit = false;
        private IIFShortCircuit shortCircuit = null;  // if we can short curcuit, this is the object

        // This token item will reference the TokenItems collectopn
        internal TokenItems parent = null;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the Token Item and set the name and token type
        /// </summary>
        /// <param name="TokenName"></param>
        /// <param name="TokenType"></param>
        public TokenItem(string TokenName, TokenType TokenType, bool InOperandFunction)
        {
            tokenName = TokenName;
            tokenType = TokenType;
            tokenDataType = TokenDataType.Token_DataType_None;
            inOperandFunction = InOperandFunction;
        }

        /// <summary>
        /// Initialize the Token Item and set the name, type, and data type
        /// </summary>
        /// <param name="TokenName"></param>
        /// <param name="TokenType"></param>
        /// <param name="TokenDataType"></param>
        public TokenItem(string TokenName, TokenType TokenType, TokenDataType TokenDataType, bool InOperandFunction)
        {
            tokenName = TokenName;
            tokenType = TokenType;
            tokenDataType = TokenDataType;
            inOperandFunction = InOperandFunction;
        }

        #endregion

        #region Public Properties

        public string TokenName
        {
            get
            {
                return tokenName;
            }
        }

        public TokenType TokenType
        {
            get
            {
                return tokenType;
            }
        }

        public TokenDataType TokenDataType
        {
            get
            {
                return tokenDataType;
            }
        }

        public bool InOperandFunction
        {
            get
            {
                return inOperandFunction;
            }
            set
            {
                inOperandFunction = value;
            }
        }

        /// <summary>
        /// Indicates if the variable will be assigned to in the rule
        /// </summary>
        public bool WillBeAssigned
        {
            get
            {
                return willBeAssigned;
            }
            set
            {
                willBeAssigned = value;
            }
        }

        // if this token type is Token_Operand_Function_Start and the 
        // operand function is iif[, this property indicates if the 
        // iif[ operand function is embedded within other operand functions.
        public bool CanShortCircuit
        {
            get
            {
                return canShortCircuit;
            }
            set
            {
                canShortCircuit = value;
            }
        }

        // if we can short curcuit, this is the object
        public IIFShortCircuit ShortCircuit
        {
            get
            {
                if (this.canShortCircuit == false) return null;

                if (shortCircuit == null) shortCircuit = new IIFShortCircuit(this);

                return shortCircuit;
            }
        }

        /// <summary>
        /// A tokenitem object parent is a tokenitems collection
        /// </summary>
        public TokenItems Parent
        {
            get
            {
                return parent;
            }
        }

        #endregion

        #region Data Type Properties

        public int TokenName_Int
        {
            get
            {
                int result = 0;

                if (Int32.TryParse(tokenName, out result) == true)
                    return result;
                else
                    return 0;

            }
        }

        public bool TokenName_Boolean
        {
            get
            {
                if (String.IsNullOrEmpty(tokenName) == true) return false;
                return (tokenName.Trim().ToLower() == "true");
            }
        }

        public double TokenName_Double
        {
            get
            {
                double result = 0;

                if (Double.TryParse(tokenName, out result) == true)
                    return result;
                else
                    return 0;

            }
        }

        public DateTime TokenName_DateTime
        {
            get
            {
                DateTime result = DateTime.MinValue;

                if (DateTime.TryParse(tokenName, out result) == true)
                    return result;
                else
                    return DateTime.MinValue;

            }
        }


        public int OrderOfOperationPrecedence
        {
            get
            {
                int ooo = 1000;

                switch (this.tokenName.Trim().ToLower())
                {

                    case "^":
                        ooo = 1;
                        break;

                    case "*":
                        ooo = 2;
                        break;

                    case "/":
                        ooo = 2;
                        break;

                    case "%":
                        ooo = 2;
                        break;

                    case "+":
                        ooo = 3;
                        break;

                    case "-":
                        ooo = 3;
                        break;

                    case "<":
                        ooo = 4;
                        break;

                    case "<=":
                        ooo = 4;
                        break;

                    case ">":
                        ooo = 4;
                        break;

                    case ">=":
                        ooo = 4;
                        break;

                    case "<>":
                        ooo = 4;
                        break;

                    case "=":
                        ooo = 4;
                        break;

                    case "and":
                        ooo = 5;
                        break;

                    case "or":
                        ooo = 6;
                        break;

                }

                return ooo;

            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return tokenName;
        }
        #endregion
       
    }
}
