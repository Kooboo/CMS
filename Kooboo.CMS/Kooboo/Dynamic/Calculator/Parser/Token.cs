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
using System.Text;

namespace Kooboo.Dynamic.Calculator.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public enum ParseState
    {
        Parse_State_Operand,            // the parser is looking for an operand
        Parse_State_Operator,           // the parser is looking for an operator
        Parse_State_Quote,              // the parser is looking for a quote
        Parse_State_OperandFunction,    // the parser is in an operand function
        Parse_State_Comment             // the parser is in a comment
    };

    /// <summary>
    /// This enumeration represents the current state of the iif[] short circuit
    /// </summary>
    public enum IIFShortCircuitState
    {
        ShortCircuit_Condition,         // We are currently in the condition parameter of the short citcuit
        ShortCircuit_True,              // We are in the true parameter of the iif[] operand
        ShortCircuit_False              // We are in the false parameter of the iif[] operand
    }

    /// <summary>
    /// 
    /// </summary>
    public class Token
    {
        #region Local Variables

        private Parser.TokenItems tokenItems = null;
        private Parser.Variables variables = new Variables();

        private Support.ExQueue<TokenItem> rpn_queue = null;

        private string ruleSyntax = "";
        private string lastErrorMessage = "";

        private double tokenParseTime = 0;
        private double lastEvaluationTime = 0;  // populated by the evaluator

        private int charIndex = 0;  // that index of the character that is being analyzed.  This also tells us the position of the error.

        private string lastEvaluationResult = "";  // this value gets populated by the evaluator object

        private TokenGroup tokenGroup = null;  // a reference to the token group (if any)

        private bool anyAssignments = false;  // indicates if the rule syntax has any assignments

        #endregion

        #region Public Constructor

        public Token(string RuleSyntax)
        {
            tokenItems = new TokenItems(this);
            ruleSyntax = RuleSyntax.Trim();
            lastErrorMessage = "";

            
            GetTokens();
        }

        public Token(System.IO.FileInfo Filename)
        {
            tokenItems = new TokenItems(this);

            string ErrorMsg = "";
            if (Open(Filename, out ErrorMsg) == false)
            {
                throw new Exception(ErrorMsg);
            }
        }

        #endregion

        #region Public Properties

        public string RuleSyntax
        {
            get
            {
                return ruleSyntax;
            }
        }

        public string LastErrorMessage
        {
            get
            {
                return lastErrorMessage;
            }
            set
            {
                lastErrorMessage = value;
            }
        }

        public bool AnyErrors
        {
            get
            {
                return (String.IsNullOrEmpty(lastErrorMessage) == false);
            }
        }

        public Parser.TokenItems TokenItems
        {
            get
            {
                return tokenItems;
            }
        }

        public Support.ExQueue<TokenItem> RPNQueue
        {
            get
            {
                return rpn_queue;
            }
        }

        public double TokenParseTime
        {
            get
            {
                return tokenParseTime;
            }
        }

        public Parser.Variables Variables
        {
            get
            {
                return variables;
            }
        }

        /// <summary>
        /// that index of the character that is being analyzed.  This also tells us the position of the error.
        /// </summary>
        public int CharIndex
        {
            get
            {
                return charIndex;
            }
        }

        /// <summary>
        /// this value gets populated by the evaluator object
        /// </summary>
        public string LastEvaluationResult
        {
            get
            {
                return lastEvaluationResult;
            }
            set
            {
                lastEvaluationResult = value;
            }
        }

        /// <summary>
        /// This property is populated by the evaluator
        /// </summary>
        public double LastEvaluationTime
        {
            get
            {
                return lastEvaluationTime;
            }
            set
            {
                lastEvaluationTime = value;
            }
        }

        /// <summary>
        /// This token object may be part of a token group object.  This is a reference to the token group (if any)
        /// </summary>
        public TokenGroup TokenGroup
        {
            get
            {
                return tokenGroup;
            }
            set
            {
                tokenGroup = value;
            }
        }


        /// <summary>
        /// Indicates if the RuleSyntax has any assignments.
        /// </summary>
        public bool AnyAssignments
        {
            get
            {
                return anyAssignments;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates TokenItem object and adds them to the collection
        /// </summary>
        /// <param name="CurrentToken"></param>
        /// <param name="WaitForCreation"></param>
        /// <param name="NextParseState"></param>
        /// <returns></returns>
        private bool CreateTokenItem(string CurrentToken, bool WaitForCreation, bool InOperandFunction, out ParseState NextParseState, out bool IsError, out string ErrorMsg)
        {
            // initialize the outgoing variable
            NextParseState = ParseState.Parse_State_Comment;
            ErrorMsg = "";
            IsError = false; // assume that no error occur

            // check for an empty token
            if (String.IsNullOrEmpty(CurrentToken) == true) return false;

            // clean the token
            CurrentToken = CurrentToken.Trim();

            // check for an empty clean token
            if (String.IsNullOrEmpty(CurrentToken) == true) return false;

            // local variables
            string tempOperand = "";
            string tempOperator = "";

            // check if the token is an operator
            if (Support.DataTypeCheck.IsOperator(CurrentToken) == true)
            {

                // add the operator, but check the next character first

                // the problem is, the next character in the token
                // may also make an operator.  for example, if the
                // current token is < we need to see if the next token is =                                

                tempOperator = CurrentToken;
                switch (CurrentToken)
                {
                    case "<":
                        // see if the next character is = or >
                        try
                        {
                            if (charIndex < ruleSyntax.Length - 1)
                            {
                                char nextChar = ruleSyntax[charIndex]; //charIndex has already been incremented

                                if (nextChar == '=')
                                {
                                    tempOperator += '=';  // adjust the current token
                                    charIndex++; // cause the '=' to be skipped on the next iteration
                                }
                                else if (nextChar == '>')
                                {
                                    tempOperator += '>';  // adjust the current token
                                    charIndex++; // cause the '>' to be skipped on the next iteration
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            IsError = true;
                            ErrorMsg = "Error while determining if the next character is <, location = 1, Error message = " + err.Message;
                            return false;
                        }

                        break;

                    case ">":
                        try
                        {
                            // see if the next character is =
                            if (charIndex < ruleSyntax.Length - 1)
                            {
                                char nextChar = ruleSyntax[charIndex];//charIndex has already been incremented

                                if (nextChar == '=')
                                {
                                    tempOperator += '=';  // adjust the current token
                                    charIndex++; // cause the '=' to be skipped on the next iteration
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            IsError = true;
                            ErrorMsg = "Error while determining if the next character is <, location = 2, Error message = " + err.Message;
                            return false;
                        }

                        break;
                }

                // create the operator
                tokenItems.Add(new TokenItem(tempOperator, TokenType.Token_Operator, InOperandFunction));

                // we added an operator, so our next parse state should be operand
                NextParseState = ParseState.Parse_State_Operand;

            }
            else if (Support.DataTypeCheck.ContainsOperator(CurrentToken, out tempOperand, out tempOperator) == true)
            {
                // our current token contains an operand and a operator

                // make sure our operand is not a reserved word
                if (Support.DataTypeCheck.IsReservedWord(tempOperand) == true)
                {
                    IsError = true;
                    ErrorMsg = "The operand \"" + tempOperand + "\" is a reserved word.";
                    return false;
                }

                // validate and add the new operand
                if (Support.DataTypeCheck.IsInteger(tempOperand) == true)
                    tokenItems.Add(new TokenItem(tempOperand, TokenType.Token_Operand, TokenDataType.Token_DataType_Int, InOperandFunction));
                else if (Support.DataTypeCheck.IsDouble(tempOperand) == true)
                    tokenItems.Add(new TokenItem(tempOperand, TokenType.Token_Operand, TokenDataType.Token_DataType_Double, InOperandFunction));
                else if (Support.DataTypeCheck.IsDate(tempOperand) == true)
                    tokenItems.Add(new TokenItem(tempOperand, TokenType.Token_Operand, TokenDataType.Token_DataType_Date, InOperandFunction));
                else if (Support.DataTypeCheck.IsBoolean(tempOperand) == true)
                    tokenItems.Add(new TokenItem(tempOperand, TokenType.Token_Operand, TokenDataType.Token_DataType_Boolean, InOperandFunction));
                else if (Support.DataTypeCheck.IsNULL(tempOperand) == true)
                    tokenItems.Add(new TokenItem(tempOperand, TokenType.Token_Operand, TokenDataType.Token_DataType_NULL, InOperandFunction));
                else
                {
                    // add the token as a variable
                    tokenItems.Add(new TokenItem(tempOperand, TokenType.Token_Operand, TokenDataType.Token_DataType_Variable, InOperandFunction));
                    variables.Add(tempOperand);
                }

                // add the operator, but check the next character first

                // the problem is, the next character in the token
                // may also make an operator.  for example, if the
                // current token is < we need to see if the next token is =                                

                // clean the token before we create the operator
                tempOperator = tempOperator.Trim();

                switch (tempOperator.Trim().ToLower())
                {
                    case "<":
                        try
                        {
                            // see if the next character is = or >
                            if (charIndex < ruleSyntax.Length - 1)
                            {
                                char nextChar = ruleSyntax[charIndex];//charIndex has already been incremented

                                if (nextChar == '=')
                                {
                                    tempOperator += '=';  // adjust the current token
                                    charIndex++; // cause the '=' to be skipped on the next iteration
                                }
                                else if (nextChar == '>')
                                {
                                    tempOperator += '>';  // adjust the current token
                                    charIndex++; // cause the '>' to be skipped on the next iteration
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            IsError = true;
                            ErrorMsg = "Error while determining if the next character is <, location = 3, Error message = " + err.Message;
                            return false;
                        }

                        break;

                    case ">":
                        try
                        {
                            // see if the next character is =
                            if (charIndex < ruleSyntax.Length - 1)
                            {
                                char nextChar = ruleSyntax[charIndex];//charIndex has already been incremented

                                if (nextChar == '=')
                                {
                                    tempOperator += '=';  // adjust the current token
                                    charIndex++; // cause the '=' to be skipped on the next iteration
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            IsError = true;
                            ErrorMsg = "Error while determining if the next character is >, location = 4, Error message = " + err.Message;
                            return false;
                        }
                        break;
                }

                // create the operator
                tokenItems.Add(new TokenItem(tempOperator, TokenType.Token_Operator, InOperandFunction));

                // we added an operand and a operator...our next parse state should be operand
                NextParseState = ParseState.Parse_State_Operand;

                //}
            }
            else if (WaitForCreation == false)
            {
                // make sure our operand is not a reserved word
                if (Support.DataTypeCheck.IsReservedWord(CurrentToken) == true)
                {
                    IsError = true;
                    ErrorMsg = "The operand \"" + CurrentToken + "\" is a reserved word.";
                    return false;
                }

                // create a new operand
                if (Support.DataTypeCheck.IsInteger(CurrentToken) == true)
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_Int, InOperandFunction));
                else if (Support.DataTypeCheck.IsDouble(CurrentToken) == true)
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_Double, InOperandFunction));
                else if (Support.DataTypeCheck.IsDate(CurrentToken) == true)
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_Date, InOperandFunction));
                else if (Support.DataTypeCheck.IsBoolean(CurrentToken) == true)
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_Boolean, InOperandFunction));
                else if (Support.DataTypeCheck.IsNULL(CurrentToken) == true)
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_NULL, InOperandFunction));
                else if (Support.DataTypeCheck.IsText(CurrentToken) == true)
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_String, InOperandFunction));
                else
                {
                    // add the token as a variable
                    tokenItems.Add(new TokenItem(CurrentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_Variable, InOperandFunction));
                    variables.Add(CurrentToken);                    
                }

                // we added an operand, our next parse state should be operator
                NextParseState = ParseState.Parse_State_Operator;
            }
            else
            {
                return false;
            }

            return true;


        }

        /// <summary>
        /// Check if we found the assignment token
        /// </summary>
        /// <returns></returns>
        private bool FoundAssignment()
        {

            try
            {
                // see if the next character is =
                if (charIndex < ruleSyntax.Length - 1)
                {
                    char nextChar = ruleSyntax[charIndex];//charIndex has already been incremented
                    return (nextChar == '=');
                }
                else
                    return false;
            }
            catch 
            {
                return false;
            }

        }

        /// <summary>
        /// This is called by the constructor.
        /// </summary>
        private void GetTokens_Old_1()
        {
            // local variables
            ParseState parseState = ParseState.Parse_State_Operand;  // start be searching for an operand
            ParseState nextParseState = ParseState.Parse_State_Comment; // the next parse state after we have processed a comment            
            ParseState opFuncParseState = ParseState.Parse_State_Operand; // The parse state within the operand function
            ParseState tempParseState = ParseState.Parse_State_Operand; // temporary variable to hold a parse state
            string currentToken = "";


            int parenthesisMatch = 0;  // the number of open and close parenthesis should match
            int squareMatch = 0;  // the number of open and close square parenthesis should match
            int tildaMatch = 0; // the open and close tildas should match
            int quoteMatch = 0; // the open and close quotes should match
            bool isError = false;
            bool tokenCreated = false;

            int operandFunctionDepth = 0;  // operand function can contain operand function...This is the depth of the operand functions

            // make sure we have some syntax to parse
            if (String.IsNullOrEmpty(ruleSyntax) == true) return;

            // create a stop watch to time the parse
            System.Diagnostics.Stopwatch parseTime = System.Diagnostics.Stopwatch.StartNew();

            // start at the first character
            charIndex = 0;
            
            do
            {
                //  double check the exit condition, just in case
                if (charIndex >= ruleSyntax.Length) break;

                // the character to be processed
                char c = ruleSyntax[charIndex];

                // Increment the counter for the next character
                charIndex++;                

                System.Diagnostics.Debug.WriteLine("c = " + c.ToString() + "\tcurrentToken = " + currentToken + "\tparse state = " + parseState.ToString() + "\tOp Parse State = " + opFuncParseState.ToString());

                #region New Line and Tab check
                // check for new line and tab
                if ((c == '\n') || (c == '\t'))
                {
                    // new lines and tabs are always ignored.
                    c = ' ';  // force the new line or tab to be a space and continue processing.
                }
                #endregion

                #region Comment Checking

                // check if we are in a comment
                if (parseState == ParseState.Parse_State_Comment)
                {
                    if (c == '~')
                    {
                        // we found the end of our comment, continue with what we were looking for
                        parseState = nextParseState;
                        tildaMatch--;
                    }

                    // continue with the next item in the loop
                    continue;
                }
                else
                {
                    if (c == '~')
                    {
                        // we found the start of a comment
                        tildaMatch++;
                        nextParseState = parseState;  // save the parse state
                        parseState = ParseState.Parse_State_Comment;
                        continue; // continue with the next item in the loop
                    }
                }
                #endregion

                // determine out current parse state
                switch (parseState)
                {
                    case ParseState.Parse_State_Operand:
                        #region Parse_State_Operand
                        if (c == '"')
                        {
                            #region Quote Handling
                            
                            try
                            {
                                // we have a quote in an operand...This is probably the start of a string operand
                                quoteMatch++;
                                currentToken += c;
                                parseState = ParseState.Parse_State_Quote;
                            }
                            catch (Exception err)
                            {
                                lastErrorMessage = "Error in GetTokens() in Operand Quote Handling: " + err.Message;
                                return;
                            }

                            #endregion
                        }
                        else if (c == ' ')
                        {
                            #region Space Handling

                            try
                            {
                                // we are looking for an operand and we found a space.  We are not currently looking for a closing quote.
                                // Assume that the space indicates that the operand is completed and we now need to look for an operator                            
                                tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                                if (isError) return;

                                if (tokenCreated == true)
                                {
                                    // set the next parse state
                                    parseState = tempParseState;

                                    // reset the token
                                    currentToken = "";
                                }
                            }
                            catch (Exception err)
                            {
                                lastErrorMessage = "Error in GetTokens() in Operand Space Handling: " + err.Message;
                                return;
                            }
                            
                            #endregion
                        }
                        else if (c == '(')
                        {
                            #region Parenthesis Handling
                            // increment the parenthesis count
                            parenthesisMatch++;

                            // create the token
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // add the open parenthesis
                            tokenItems.Add(new TokenItem("(", TokenType.Token_Open_Parenthesis, false));

                            // clear the current token
                            currentToken = "";

                            // after a parenthesis we need an operand
                            parseState = ParseState.Parse_State_Operand;
                            #endregion
                        }
                        else if (c == ')')
                        {
                            #region Parenthesis Handling
                            // decrement the parenthesis count
                            parenthesisMatch--;

                            // create the token
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // add the close parenthesis
                            tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, false));

                            // clear the current token
                            currentToken = "";

                            // after a parenthesis we need an operator
                            parseState = ParseState.Parse_State_Operator;

                            #endregion
                        }
                        else if (c == '[')
                        {
                            #region Operand Function Handling
                            // Check that an operand function is starting

                            // increment the square parenthesis count
                            squareMatch++;

                            // clean the token
                            currentToken = currentToken.Trim();

                            if (Support.DataTypeCheck.IsOperandFunction(currentToken) == true)
                            {
                                // we found the start of an operand function

                                // it's possible that we have operand functions within operand functions
                                operandFunctionDepth++;

                                // add the token item
                                currentToken += c;
                                tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand_Function_Start, false));

                                // Indicate that we are now in a operand function
                                parseState = ParseState.Parse_State_OperandFunction;

                                // within the operand function, we are looking for an operand
                                opFuncParseState = ParseState.Parse_State_Operand;

                                // reset the token
                                currentToken = "";

                            }
                            else
                            {
                                // we found a square bracket but we don't have an operand function
                                lastErrorMessage = "Error in Rule Syntax: Found an open square parenthesis without an operand function";
                                return;
                            }
                            #endregion
                        }
                        else if (c == ']')
                        {
                            #region Operand Function Handling
                            // we should never be looking for an operand and find a ]
                            squareMatch--;
                            lastErrorMessage = "Error in Rule Syntax: Found an ] while looking for an operand.";
                            return;
                            #endregion
                        }
                        else if (c == ',')
                        {
                            #region Command Handling
                            // we should never be looking for an operand and find a ,
                            lastErrorMessage = "Error in Rule Syntax: Found a , (comma) while looking for an operand.";
                            return;
                            #endregion
                        }
                        else if (c == '-')
                        {
                            #region Negative Operands
                            if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                            {
                                // we found a negative sign
                                currentToken += c;
                            }
                            else
                            {
                                // try and create the new token
                                currentToken += c;
                                tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                                if (isError == true) return;
                                if (tokenCreated == true)
                                {
                                    // the new tokens were created

                                    // reset the current token
                                    currentToken = "";

                                    // set the next parse state
                                    parseState = tempParseState;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Other Handling
                            // try and create the new token
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // the new tokens were created

                                // reset the current token
                                currentToken = "";

                                // set the next parse state
                                parseState = tempParseState;

                            }
                            #endregion
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_OperandFunction:
                        #region Parse_State_OperandFunction

                        switch (opFuncParseState)
                        {
                            case ParseState.Parse_State_Operand:
                                #region Parse_State_OperandFunction, Parse_State_Operand
                                if (c == '"')
                                {
                                    #region Quote Handling
                                    // we have a quote in the operand function...This is probably the start of a string operand
                                    quoteMatch++;
                                    currentToken += c;
                                    opFuncParseState = ParseState.Parse_State_Quote;
                                    #endregion
                                }
                                else if (c == ' ')
                                {
                                    #region Space Handling
                                    // we are looking for an operand and we found a space.  We are not currently looking for a closing quote.
                                    // Assume that the space indicates that the operand is completed and we now need to look for an operator                            
                                    tokenCreated = CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (tokenCreated == true)
                                    {
                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                        // reset the token
                                        currentToken = "";
                                    }
                                    #endregion
                                }
                                else if (c == '(')
                                {
                                    #region Parenthesis Handling
                                    // increment the parenthesis count
                                    parenthesisMatch++;

                                    // create the token
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the open parenthesis
                                    tokenItems.Add(new TokenItem("(", TokenType.Token_Open_Parenthesis, true));

                                    // clear the current token
                                    currentToken = "";

                                    // after a parenthesis we need an operand
                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else if (c == ')')
                                {
                                    #region Parenthesis Handling
                                    // decrement the parenthesis count
                                    parenthesisMatch--;

                                    // create the token
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the close parenthesis
                                    tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, true));

                                    // clear the current token
                                    currentToken = "";

                                    // after a parenthesis we need an operator
                                    opFuncParseState = ParseState.Parse_State_Operator;

                                    #endregion
                                }
                                else if (c == '[')
                                {
                                    #region Operand Function Handling
                                    // Check that an operand function is starting

                                    // increment the square parenthesis count
                                    squareMatch++;

                                    // clean the token
                                    currentToken = currentToken.Trim();

                                    if (Support.DataTypeCheck.IsOperandFunction(currentToken) == true)
                                    {
                                        // we found the start of an operand function

                                        // it's possible that we have operand functions within operand functions
                                        operandFunctionDepth++;

                                        // add the token item
                                        currentToken += c;
                                        tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand_Function_Start, true));

                                        // within the operand function, we are looking for an operand
                                        opFuncParseState = ParseState.Parse_State_Operand;

                                        // reset the token
                                        currentToken = "";

                                    }
                                    else
                                    {
                                        // we found a square bracket but we don't have an operand function
                                        lastErrorMessage = "Error in Rule Syntax: Found an open square parenthesis without an operand function";
                                        return;
                                    }
                                    #endregion
                                }
                                else if (c == ']')
                                {
                                    #region Operand Function Handling

                                    squareMatch--;

                                    // we found the end of an operator function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the stop operand token
                                    tokenItems.Add(new TokenItem("]", TokenType.Token_Operand_Function_Stop, true));

                                    // decrement the operand depth
                                    operandFunctionDepth--;

                                    // set the parse states
                                    //opFuncParseState = ParseState.Parse_State_Operand;
                                    opFuncParseState = ParseState.Parse_State_Operator;

                                    // check if we are done with operand function and now need an operator
                                    if (operandFunctionDepth <= 0)
                                    {
                                        operandFunctionDepth = 0;
                                        parseState = ParseState.Parse_State_Operator;
                                    }

                                    // reset the current token
                                    currentToken = "";

                                    #endregion
                                }
                                else if (c == ',')
                                {
                                    #region Command Handling
                                    // create a toke
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the delimiter token
                                    tokenItems.Add(new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, true));

                                    // reset the token
                                    currentToken = "";

                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else if (c == '-')
                                {
                                    #region Negative operands
                                    if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                                    {
                                        // we found a negative sign
                                        currentToken += c;
                                    }
                                    else
                                    {
                                        // try and create the new token
                                        currentToken += c;
                                        tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                        if (isError == true) return;
                                        if (tokenCreated == true)
                                        {
                                            // the new tokens were created

                                            // reset the current token
                                            currentToken = "";

                                            // set the next parse state
                                            opFuncParseState = tempParseState;
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    // try and create the new token
                                    currentToken += c;

                                    tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    if (tokenCreated  == true)
                                    {
                                        // the new tokens were created

                                        // reset the current token
                                        currentToken = "";

                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                    }
                                    #endregion
                                }

                                #endregion
                                break;

                            case ParseState.Parse_State_Operator:
                                #region Parse_State_OperandFunction, Parse_State_Operator

                                if (c == '"')
                                {
                                    // we found a quote while looking for an operator...this is a problem
                                    lastErrorMessage = "Error in Rule Syntax: Found a double quote (\") while looking for an operator.";
                                    return;
                                }
                                else if (c == ' ')
                                {
                                    // we found a space while looking for an operator...maybe it's just white space
                                    if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                                    {
                                        // yep, it's just white space; it can be ignored.
                                        currentToken = "";
                                    }
                                    else
                                    {
                                        // we found a space while looking for an operator....this is a problem because
                                        // no operators allow for spaces.
                                        lastErrorMessage = "Error with expression syntax: Found a space while looking for an operator";
                                        return;
                                    }
                                }
                                else if (c == '(')
                                {
                                    // we found an opern parenthesis while searching for an operator....that's a problem
                                    lastErrorMessage = "Error in rule syntax: Found an open parenthesis while searching for an operator";
                                    return;
                                }
                                else if (c == ')')
                                {
                                    // add the closed parenthesis and keep looking for the operator
                                    parenthesisMatch--;
                                    tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, true));
                                }
                                else if (c == '[')
                                {
                                    // we found an open parenthesis while searching for an operator....that's a problem
                                    lastErrorMessage = "Error in rule syntax: Found an open square parenthesis while searching for an operator";
                                    return;
                                }
                                else if (c == ']')
                                {
                                    squareMatch--;
                                    // we found the end of an operator function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the stop operand token
                                    tokenItems.Add(new TokenItem("]", TokenType.Token_Operand_Function_Stop, true));

                                    // decrement the operand depth
                                    operandFunctionDepth--;

                                    // set the parse states
                                    opFuncParseState = ParseState.Parse_State_Operand;

                                    // check if we are done with operand function and now need an operator
                                    if (operandFunctionDepth <= 0)
                                    {
                                        operandFunctionDepth = 0;
                                        parseState = ParseState.Parse_State_Operator;
                                    }

                                    // reset the current token
                                    currentToken = "";
                                }
                                else if (c == ',')
                                {
                                    // we found a comma while searching for an operator....that's alright in an operand function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the comma
                                    tokenItems.Add(new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, true));

                                    // reset the token
                                    currentToken = "";

                                    // set the parse state
                                    opFuncParseState = ParseState.Parse_State_Operand;
                                }
                                else
                                {
                                    // try and create the new token
                                    currentToken += c;
                                    tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;
                                    if (tokenCreated == true)
                                    {
                                        // the new tokens were created

                                        // reset the current token
                                        currentToken = "";

                                        // set the next parse state
                                        opFuncParseState = ParseState.Parse_State_Operand;

                                    }
                                }

                                #endregion
                                break;

                            case ParseState.Parse_State_Quote:
                                #region Parse_State_OperandFunction, Parse_State_Quote

                                if (c == '"')
                                {
                                    quoteMatch--;

                                    // we found the end of the qoute
                                    currentToken += c;
                                    tokenCreated = CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;
                                    if (tokenCreated == true)
                                    {
                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                        // clear the current token
                                        currentToken = "";
                                    }
                                }
                                else
                                {
                                    currentToken += c;
                                }

                                #endregion
                                break;
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_Operator:
                        #region Parse_State_Operator
                        if (c == '"')
                        {
                            #region Quote Handling
                            // we found a quote while looking for an operator...this is a problem
                            lastErrorMessage = "Error in Rule Syntax: Found a double quote (\") while looking for an operator.";
                            return;
                            #endregion
                        }
                        else if (c == ' ')
                        {
                            #region Space Handling
                            // we found a space while looking for an operator...maybe it's just white space
                            if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                            {
                                // yep, it's just white space; it can be ignored.
                                currentToken = "";
                            }
                            else
                            {
                                // we found a space while looking for an operator....this is a problem because
                                // no operators allow for spaces.
                                lastErrorMessage = "Error with expression syntax: Found a space while looking for an operator";
                                return;
                            }
                            #endregion
                        }
                        else if (c == '(')
                        {
                            #region Parenthesis Handling
                            // we found an opern parenthesis while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found an open parenthesis while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ')')
                        {
                            #region Parenthesis Handling
                            parenthesisMatch--;

                            // add the closed parenthesis and keep looking for the operator
                            tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, false));
                            #endregion
                        }
                        else if (c == '[')
                        {
                            #region Operand Function Handling
                            // we found an open parenthesis while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found an open square parenthesis while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ']')
                        {
                            #region operand Function Handling
                            // we found an closed parenthesis while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found a closed square parenthesis while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ',')
                        {
                            #region Comma Handling
                            // we found a comma while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found a comma while searching for an operator";
                            return;
                            #endregion
                        }
                        else
                        {
                            #region Other Handling
                            // try and create the new token
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // the new tokens were created

                                // reset the current token
                                currentToken = "";

                                // set the next parse state
                                parseState = tempParseState;

                            }
                            #endregion
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_Quote:
                        #region Parse_State_Quote
                        if (c == '"')
                        {
                            #region Quote Handling
                            // we found the end of the qoute
                            quoteMatch--;
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // set the next parse state
                                parseState = tempParseState;

                                // clear the current token
                                currentToken = "";
                            }
                            #endregion
                        }
                        else
                        {
                            currentToken += c;
                        }

                        #endregion
                        break;
                }


            } while (charIndex < ruleSyntax.Length);


            #region Final Token Handling

            // see if we have a current token that needs to be processed.
            currentToken = currentToken.Trim();

            if (String.IsNullOrEmpty(currentToken) == false)
            {
                // we have a token that needs to be processed
                if (currentToken == "(")
                    parenthesisMatch++;
                else if (currentToken == ")")
                    parenthesisMatch--;
                else if (currentToken == "[")
                    squareMatch++;
                else if (currentToken == "]")
                    squareMatch--;
                else if (currentToken == "~")
                    tildaMatch--;
                else if (currentToken == "\"")
                    quoteMatch--;

                switch (parseState)
                {
                    case ParseState.Parse_State_Operand:
                        CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                        if (isError == true) return;
                        break;

                    case ParseState.Parse_State_Operator:
                        // there should never be an operator at the end of the rule syntax
                        lastErrorMessage = "Error in Rule Syntax: A rule cannot end with an operator.";
                        break;

                    case ParseState.Parse_State_Quote:
                        // we are looking for a closing quote
                        if (currentToken != "\"")
                        {
                            lastErrorMessage = "Error in RuleSyntax: Double quote mismatch.";
                            return;
                        }
                        else
                        {
                            // add the token as an operand
                            tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_String, false));
                        }
                        break;

                    case ParseState.Parse_State_OperandFunction:
                        break;

                }
            }

            #endregion

            #region RuleSyntax Validation Checks

            if (parenthesisMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a parenthesis mismatch.";
                return;
            }

            if (squareMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is an operand function mismatch.";
                return;
            }

            if (operandFunctionDepth > 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is an operand function mismatch error...Operand function depth is not zero.";
                return;
            }

            if (tildaMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a comment mismatch.";
                return;
            }

            if (quoteMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a quote mismatch.";
                return;
            }

            if (charIndex < ruleSyntax.Length)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem parsing the rule...some of the tokens were not found.";
                return;
            }

            #endregion

            // create the RPN Stack
            if (this.AnyErrors == false) MakeRPNQueue();

            // check that we have tokens in out RPN Queue
            if (rpn_queue == null)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem creating the RPN queue.";
                return;
            }

            if (rpn_queue.Count == 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem creating the RPN queue.";
                return;
            }

            // stop the timer
            parseTime.Stop();
            tokenParseTime = parseTime.Elapsed.TotalMilliseconds;

        }

        /// <summary>
        /// This is called by the constructor.  This new version includes support for assignment :=
        /// </summary>
        private void GetTokens_old_2()
        {
            // local variables
            ParseState parseState = ParseState.Parse_State_Operand;  // start be searching for an operand
            ParseState nextParseState = ParseState.Parse_State_Comment; // the next parse state after we have processed a comment            
            ParseState opFuncParseState = ParseState.Parse_State_Operand; // The parse state within the operand function
            ParseState tempParseState = ParseState.Parse_State_Operand; // temporary variable to hold a parse state
            string currentToken = "";


            int parenthesisMatch = 0;  // the number of open and close parenthesis should match
            int squareMatch = 0;  // the number of open and close square parenthesis should match
            int tildaMatch = 0; // the open and close tildas should match
            int quoteMatch = 0; // the open and close quotes should match
            int assignmentStartCount = 0;  // the number of times an assignment start operator was found;
            int assignmentStopCount = 0;   // the number of times an assignment stop operator was found
            bool isError = false;
            bool tokenCreated = false;

            int operandFunctionDepth = 0;  // operand function can contain operand function...This is the depth of the operand functions

            // make sure we have some syntax to parse
            if (String.IsNullOrEmpty(ruleSyntax) == true) return;

            // create a stop watch to time the parse
            System.Diagnostics.Stopwatch parseTime = System.Diagnostics.Stopwatch.StartNew();

            // start at the first character
            charIndex = 0;

            do
            {
                //  double check the exit condition, just in case
                if (charIndex >= ruleSyntax.Length) break;

                // the character to be processed
                char c = ruleSyntax[charIndex];

                // Increment the counter for the next character
                charIndex++;

                System.Diagnostics.Debug.WriteLine("c = " + c.ToString() + "\tcurrentToken = " + currentToken + "\tparse state = " + parseState.ToString() + "\tOp Parse State = " + opFuncParseState.ToString());

                #region New Line and Tab check
                // check for new line and tab
                if ((c == '\n') || (c == '\t'))
                {
                    // new lines and tabs are always ignored.
                    c = ' ';  // force the new line or tab to be a space and continue processing.
                }
                #endregion

                #region Comment Checking

                // check if we are in a comment
                if (parseState == ParseState.Parse_State_Comment)
                {
                    if (c == '~')
                    {
                        // we found the end of our comment, continue with what we were looking for
                        parseState = nextParseState;
                        tildaMatch--;
                    }

                    // continue with the next item in the loop
                    continue;
                }
                else
                {
                    if (c == '~')
                    {
                        // we found the start of a comment
                        tildaMatch++;
                        nextParseState = parseState;  // save the parse state
                        parseState = ParseState.Parse_State_Comment;
                        continue; // continue with the next item in the loop
                    }
                }
                #endregion

                // determine out current parse state
                switch (parseState)
                {
                    case ParseState.Parse_State_Operand:
                        #region Parse_State_Operand
                        if (c == '"')
                        {
                            #region Quote Handling

                            try
                            {
                                // we have a quote in an operand...This is probably the start of a string operand
                                quoteMatch++;
                                currentToken += c;
                                parseState = ParseState.Parse_State_Quote;
                            }
                            catch (Exception err)
                            {
                                lastErrorMessage = "Error in GetTokens() in Operand Quote Handling: " + err.Message;
                                return;
                            }

                            #endregion
                        }
                        else if (c == ' ')
                        {
                            #region Space Handling

                            try
                            {
                                // we are looking for an operand and we found a space.  We are not currently looking for a closing quote.
                                // Assume that the space indicates that the operand is completed and we now need to look for an operator                            
                                tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                                if (isError) return;

                                if (tokenCreated == true)
                                {
                                    // set the next parse state
                                    parseState = tempParseState;

                                    // reset the token
                                    currentToken = "";
                                }
                            }
                            catch (Exception err)
                            {
                                lastErrorMessage = "Error in GetTokens() in Operand Space Handling: " + err.Message;
                                return;
                            }

                            #endregion
                        }
                        else if (c == '(')
                        {
                            #region Parenthesis Handling
                            // increment the parenthesis count
                            parenthesisMatch++;

                            // create the token
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // add the open parenthesis
                            tokenItems.Add(new TokenItem("(", TokenType.Token_Open_Parenthesis, false));

                            // clear the current token
                            currentToken = "";

                            // after a parenthesis we need an operand
                            parseState = ParseState.Parse_State_Operand;
                            #endregion
                        }
                        else if (c == ')')
                        {
                            #region Parenthesis Handling
                            // decrement the parenthesis count
                            parenthesisMatch--;

                            // create the token
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // add the close parenthesis
                            tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, false));

                            // clear the current token
                            currentToken = "";

                            // after a parenthesis we need an operator
                            parseState = ParseState.Parse_State_Operator;

                            #endregion
                        }
                        else if (c == '[')
                        {
                            #region Operand Function Handling
                            // Check that an operand function is starting

                            // increment the square parenthesis count
                            squareMatch++;

                            // clean the token
                            currentToken = currentToken.Trim();

                            if (Support.DataTypeCheck.IsOperandFunction(currentToken) == true)
                            {
                                // we found the start of an operand function

                                // it's possible that we have operand functions within operand functions
                                operandFunctionDepth++;

                                // add the token item
                                currentToken += c;
                                tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand_Function_Start, false));

                                // Indicate that we are now in a operand function
                                parseState = ParseState.Parse_State_OperandFunction;

                                // within the operand function, we are looking for an operand
                                opFuncParseState = ParseState.Parse_State_Operand;

                                // reset the token
                                currentToken = "";

                            }
                            else
                            {
                                // we found a square bracket but we don't have an operand function
                                lastErrorMessage = "Error in Rule Syntax: Found an open square parenthesis without an operand function";
                                return;
                            }
                            #endregion
                        }
                        else if (c == ']')
                        {
                            #region Operand Function Handling
                            // we should never be looking for an operand and find a ]
                            squareMatch--;
                            lastErrorMessage = "Error in Rule Syntax: Found an ] while looking for an operand.";
                            return;
                            #endregion
                        }
                        else if (c == ',')
                        {
                            #region Command Handling
                            // we should never be looking for an operand and find a ,
                            lastErrorMessage = "Error in Rule Syntax: Found a , (comma) while looking for an operand.";
                            return;
                            #endregion
                        }
                        else if (c == '-')
                        {
                            #region Negative Operands
                            if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                            {
                                // we found a negative sign
                                currentToken += c;
                            }
                            else
                            {
                                // try and create the new token
                                currentToken += c;
                                tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                                if (isError == true) return;
                                if (tokenCreated == true)
                                {
                                    // the new tokens were created

                                    // reset the current token
                                    currentToken = "";

                                    // set the next parse state
                                    parseState = tempParseState;
                                }
                            }
                            #endregion
                        }
                        else if (c == ':')
                        {
                            #region Assignment handling

                            // we may have the start of an assignment.

                            // first check....the next character must be =
                            if (FoundAssignment() == false)
                            {
                                // nope, we did not find an assignment, this is a problem
                                lastErrorMessage = "Error in Rule Syntax: Found a : (colon) but did not find an assignment token.";
                                return;
                            }

                            assignmentStartCount++;

                            // since we were looking for an operand and we found a colon, we may have a current token
                            CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // second check....the assignment can only be made to a variable...
                            // therefore, the previous token must be a variable.
                            if (tokenItems[tokenItems.Count - 1].TokenType == TokenType.Token_Operand)
                            {
                                if (tokenItems[tokenItems.Count - 1].TokenDataType != TokenDataType.Token_DataType_Variable)
                                {
                                    // assignment can only happen to a variable, we have a problem
                                    lastErrorMessage = "Error in Rule Syntax: An assignment can only be made to a variable.";
                                    return;
                                }
                            }
                            else
                            {
                                // We have an error because you can only assign to an operand variableassignment can only happen to a variable, we have a problem
                                lastErrorMessage = "Error in Rule Syntax: An assignment can only be made to a variable.";
                                return;
                            }

                            // indicates that the last token item we created will be assigned to from the rule
                            tokenItems[tokenItems.Count - 1].WillBeAssigned = true;

                            // also indicate that this token items as assignments
                            anyAssignments = true;

                            // ok, we have a valid assignment....add the token
                            tokenItems.Add(new TokenItem(":=", TokenType.Token_Assignemt_Start, false));  // assignments are always flagged as not in operand functions

                            // increment the character index so that the = from the assignment is skipped
                            charIndex++;

                            // clear the token
                            currentToken = "";

                            // after the assignment, we need an operand
                            parseState = ParseState.Parse_State_Operand;

                            #endregion
                        }
                        else if (c == ';')
                        {
                            #region Assignment Handling

                            assignmentStopCount++;

                            // we found the end of an assignment, create the assignment stop token
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // add the stop token 
                            tokenItems.Add(new TokenItem(";", TokenType.Token_Assignment_Stop, false));

                            // reset the current token
                            currentToken = "";

                            // after the end of the assignment, we are starting over again with a new operand
                            parseState = ParseState.Parse_State_Operand;

                            #endregion
                        }
                        else
                        {
                            #region Other Handling
                            // try and create the new token
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // the new tokens were created

                                // reset the current token
                                currentToken = "";

                                // set the next parse state
                                parseState = tempParseState;

                            }
                            #endregion
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_OperandFunction:
                        #region Parse_State_OperandFunction

                        switch (opFuncParseState)
                        {
                            case ParseState.Parse_State_Operand:
                                #region Parse_State_OperandFunction, Parse_State_Operand
                                if (c == '"')
                                {
                                    #region Quote Handling
                                    // we have a quote in the operand function...This is probably the start of a string operand
                                    quoteMatch++;
                                    currentToken += c;
                                    opFuncParseState = ParseState.Parse_State_Quote;
                                    #endregion
                                }
                                else if (c == ' ')
                                {
                                    #region Space Handling
                                    // we are looking for an operand and we found a space.  We are not currently looking for a closing quote.
                                    // Assume that the space indicates that the operand is completed and we now need to look for an operator                            
                                    tokenCreated = CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (tokenCreated == true)
                                    {
                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                        // reset the token
                                        currentToken = "";
                                    }
                                    #endregion
                                }
                                else if (c == '(')
                                {
                                    #region Parenthesis Handling
                                    // increment the parenthesis count
                                    parenthesisMatch++;

                                    // create the token
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the open parenthesis
                                    tokenItems.Add(new TokenItem("(", TokenType.Token_Open_Parenthesis, true));

                                    // clear the current token
                                    currentToken = "";

                                    // after a parenthesis we need an operand
                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else if (c == ')')
                                {
                                    #region Parenthesis Handling
                                    // decrement the parenthesis count
                                    parenthesisMatch--;

                                    // create the token
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the close parenthesis
                                    tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, true));

                                    // clear the current token
                                    currentToken = "";

                                    // after a parenthesis we need an operator
                                    opFuncParseState = ParseState.Parse_State_Operator;

                                    #endregion
                                }
                                else if (c == '[')
                                {
                                    #region Operand Function Handling
                                    // Check that an operand function is starting

                                    // increment the square parenthesis count
                                    squareMatch++;

                                    // clean the token
                                    currentToken = currentToken.Trim();

                                    if (Support.DataTypeCheck.IsOperandFunction(currentToken) == true)
                                    {
                                        // we found the start of an operand function

                                        // it's possible that we have operand functions within operand functions
                                        operandFunctionDepth++;

                                        // add the token item
                                        currentToken += c;
                                        tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand_Function_Start, true));

                                        // within the operand function, we are looking for an operand
                                        opFuncParseState = ParseState.Parse_State_Operand;

                                        // reset the token
                                        currentToken = "";

                                    }
                                    else
                                    {
                                        // we found a square bracket but we don't have an operand function
                                        lastErrorMessage = "Error in Rule Syntax: Found an open square parenthesis without an operand function";
                                        return;
                                    }
                                    #endregion
                                }
                                else if (c == ']')
                                {
                                    #region Operand Function Handling

                                    squareMatch--;

                                    // we found the end of an operator function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the stop operand token
                                    tokenItems.Add(new TokenItem("]", TokenType.Token_Operand_Function_Stop, true));

                                    // decrement the operand depth
                                    operandFunctionDepth--;

                                    // set the parse states
                                    //opFuncParseState = ParseState.Parse_State_Operand;
                                    opFuncParseState = ParseState.Parse_State_Operator;

                                    // check if we are done with operand function and now need an operator
                                    if (operandFunctionDepth <= 0)
                                    {
                                        operandFunctionDepth = 0;
                                        parseState = ParseState.Parse_State_Operator;
                                    }

                                    // reset the current token
                                    currentToken = "";

                                    #endregion
                                }
                                else if (c == ',')
                                {
                                    #region Command Handling
                                    // create a toke
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the delimiter token
                                    tokenItems.Add(new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, true));

                                    // reset the token
                                    currentToken = "";

                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else if (c == '-')
                                {
                                    #region Negative operands
                                    if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                                    {
                                        // we found a negative sign
                                        currentToken += c;
                                    }
                                    else
                                    {
                                        // try and create the new token
                                        currentToken += c;
                                        tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                        if (isError == true) return;
                                        if (tokenCreated == true)
                                        {
                                            // the new tokens were created

                                            // reset the current token
                                            currentToken = "";

                                            // set the next parse state
                                            opFuncParseState = tempParseState;
                                        }
                                    }
                                    #endregion
                                }
                                else if ((c == ':') || (c == ';'))
                                {
                                    #region Assignment Handling

                                    // assignments are not allowed within operand function
                                    lastErrorMessage = "Error in Rule Syntax: Assignments are not allowed within Operand Functions";
                                    return;
                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    // try and create the new token
                                    currentToken += c;

                                    tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    if (tokenCreated == true)
                                    {
                                        // the new tokens were created

                                        // reset the current token
                                        currentToken = "";

                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                    }
                                    #endregion
                                }

                                #endregion
                                break;

                            case ParseState.Parse_State_Operator:
                                #region Parse_State_OperandFunction, Parse_State_Operator

                                if (c == '"')
                                {
                                    #region Quote Handling
                                    // we found a quote while looking for an operator...this is a problem
                                    lastErrorMessage = "Error in Rule Syntax: Found a double quote (\") while looking for an operator.";
                                    return;
                                    #endregion
                                }
                                else if (c == ' ')
                                {
                                    #region Space Handling
                                    // we found a space while looking for an operator...maybe it's just white space
                                    if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                                    {
                                        // yep, it's just white space; it can be ignored.
                                        currentToken = "";
                                    }
                                    else
                                    {
                                        // we found a space while looking for an operator....this is a problem because
                                        // no operators allow for spaces.
                                        lastErrorMessage = "Error with expression syntax: Found a space while looking for an operator";
                                        return;
                                    }
                                    #endregion
                                }
                                else if (c == '(')
                                {
                                    #region Parenthesis Handling
                                    // we found an opern parenthesis while searching for an operator....that's a problem
                                    lastErrorMessage = "Error in rule syntax: Found an open parenthesis while searching for an operator";
                                    return;
                                    #endregion
                                }
                                else if (c == ')')
                                {
                                    #region Parenthesis Handling
                                    // add the closed parenthesis and keep looking for the operator
                                    parenthesisMatch--;
                                    tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, true));
                                    #endregion
                                }
                                else if (c == '[')
                                {
                                    #region Operand Function
                                    // we found an open parenthesis while searching for an operator....that's a problem
                                    lastErrorMessage = "Error in rule syntax: Found an open square parenthesis while searching for an operator";
                                    return;
                                    #endregion
                                }
                                else if (c == ']')
                                {
                                    #region Operand Function
                                    squareMatch--;
                                    // we found the end of an operator function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the stop operand token
                                    tokenItems.Add(new TokenItem("]", TokenType.Token_Operand_Function_Stop, true));

                                    // decrement the operand depth
                                    operandFunctionDepth--;

                                    // set the parse states
                                    opFuncParseState = ParseState.Parse_State_Operand;

                                    // check if we are done with operand function and now need an operator
                                    if (operandFunctionDepth <= 0)
                                    {
                                        operandFunctionDepth = 0;
                                        parseState = ParseState.Parse_State_Operator;
                                    }

                                    // reset the current token
                                    currentToken = "";

                                    #endregion
                                }
                                else if (c == ',')
                                {
                                    #region Command Handling
                                    // we found a comma while searching for an operator....that's alright in an operand function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the comma
                                    tokenItems.Add(new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, true));

                                    // reset the token
                                    currentToken = "";

                                    // set the parse state
                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else if ((c == ':') || (c == ';'))
                                {
                                    #region Assignment Handling

                                    // assignments are not allowed within operand function
                                    lastErrorMessage = "Error in Rule Syntax: Assignments are not allowed within Operand Functions";
                                    return;
                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    // try and create the new token
                                    currentToken += c;
                                    tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;
                                    if (tokenCreated == true)
                                    {
                                        // the new tokens were created

                                        // reset the current token
                                        currentToken = "";

                                        // set the next parse state
                                        opFuncParseState = ParseState.Parse_State_Operand;

                                    }
                                    #endregion
                                }

                                #endregion
                                break;

                            case ParseState.Parse_State_Quote:
                                #region Parse_State_OperandFunction, Parse_State_Quote

                                if (c == '"')
                                {
                                    #region Quote Handling
                                    quoteMatch--;

                                    // we found the end of the qoute
                                    currentToken += c;
                                    tokenCreated = CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;
                                    if (tokenCreated == true)
                                    {
                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                        // clear the current token
                                        currentToken = "";
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    currentToken += c;
                                    #endregion
                                }

                                #endregion
                                break;
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_Operator:
                        #region Parse_State_Operator
                        if (c == '"')
                        {
                            #region Quote Handling
                            // we found a quote while looking for an operator...this is a problem
                            lastErrorMessage = "Error in Rule Syntax: Found a double quote (\") while looking for an operator.";
                            return;
                            #endregion
                        }
                        else if (c == ' ')
                        {
                            #region Space Handling
                            // we found a space while looking for an operator...maybe it's just white space
                            if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                            {
                                // yep, it's just white space; it can be ignored.
                                currentToken = "";
                            }
                            else
                            {
                                // we found a space while looking for an operator....this is a problem because
                                // no operators allow for spaces.
                                lastErrorMessage = "Error with expression syntax: Found a space while looking for an operator";
                                return;
                            }
                            #endregion
                        }
                        else if (c == '(')
                        {
                            #region Parenthesis Handling
                            // we found an opern parenthesis while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found an open parenthesis while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ')')
                        {
                            #region Parenthesis Handling
                            parenthesisMatch--;

                            // add the closed parenthesis and keep looking for the operator
                            tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, false));
                            #endregion
                        }
                        else if (c == '[')
                        {
                            #region Operand Function Handling
                            // we found an open parenthesis while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found an open square parenthesis while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ']')
                        {
                            #region operand Function Handling
                            // we found an closed parenthesis while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found a closed square parenthesis while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ',')
                        {
                            #region Comma Handling
                            // we found a comma while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found a comma while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ':')
                        {
                            #region Assignment handling

                            // we may have the start of an assignment.

                            // first check....the next character must be =
                            if (FoundAssignment() == false)
                            {
                                // nope, we did not find an assignment, this is a problem
                                lastErrorMessage = "Error in Rule Syntax: Found a : (colon) but did not find an assignment token.";
                                return;
                            }

                            assignmentStartCount++;

                            // second check....the assignment can only be made to a variable...
                            // therefore, the previous token must be a variable.
                            if (tokenItems[tokenItems.Count - 1].TokenType == TokenType.Token_Operand)
                            {
                                if (tokenItems[tokenItems.Count - 1].TokenDataType != TokenDataType.Token_DataType_Variable)
                                {
                                    // assignment can only happen to a variable, we have a problem
                                    lastErrorMessage = "Error in Rule Syntax: An assignment can only be made to a variable.";
                                    return;
                                }
                            }
                            else
                            {
                                // We have an error because you can only assign to an operand variableassignment can only happen to a variable, we have a problem
                                lastErrorMessage = "Error in Rule Syntax: An assignment can only be made to a variable.";
                                return;
                            }

                            // indicates that the last token item we created will be assigned to from the rule
                            tokenItems[tokenItems.Count - 1].WillBeAssigned = true;

                            // also indicate that this token items as assignments
                            anyAssignments = true;

                            // ok, we have a valid assignment....add the token
                            tokenItems.Add(new TokenItem(":=", TokenType.Token_Assignemt_Start, false));  // assignments are always flagged as not in operand functions

                            // increment the character index so that the = from the assignment is skipped
                            charIndex++;

                            // clear the token
                            currentToken = "";

                            // after the assignment, we need an operand
                            parseState = ParseState.Parse_State_Operand;

                            #endregion
                        }
                        else if (c == ';')
                        {
                            #region End assignment handling

                            // we found the end of the assignment...add the token
                            tokenItems.Add(new TokenItem(";", TokenType.Token_Assignment_Stop, false));

                            assignmentStopCount++;

                            // clear the token
                            currentToken = "";

                            // after a closing assignment token item, we need a operand
                            parseState = ParseState.Parse_State_Operand;

                            #endregion
                        }
                        else
                        {
                            #region Other Handling
                            // try and create the new token
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // the new tokens were created

                                // reset the current token
                                currentToken = "";

                                // set the next parse state
                                parseState = tempParseState;

                            }
                            #endregion
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_Quote:
                        #region Parse_State_Quote
                        if (c == '"')
                        {
                            #region Quote Handling
                            // we found the end of the qoute
                            quoteMatch--;
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // set the next parse state
                                parseState = tempParseState;

                                // clear the current token
                                currentToken = "";
                            }
                            #endregion
                        }
                        else
                        {
                            currentToken += c;
                        }

                        #endregion
                        break;
                }


            } while (charIndex < ruleSyntax.Length);


            #region Final Token Handling

            // see if we have a current token that needs to be processed.
            currentToken = currentToken.Trim();

            if (String.IsNullOrEmpty(currentToken) == false)
            {
                // we have a token that needs to be processed
                if (currentToken == "(")
                    parenthesisMatch++;
                else if (currentToken == ")")
                    parenthesisMatch--;
                else if (currentToken == "[")
                    squareMatch++;
                else if (currentToken == "]")
                    squareMatch--;
                else if (currentToken == "~")
                    tildaMatch--;
                else if (currentToken == "\"")
                    quoteMatch--;

                switch (parseState)
                {
                    case ParseState.Parse_State_Operand:
                        CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                        if (isError == true) return;
                        break;

                    case ParseState.Parse_State_Operator:
                        // there should never be an operator at the end of the rule syntax
                        lastErrorMessage = "Error in Rule Syntax: A rule cannot end with an operator.";
                        break;

                    case ParseState.Parse_State_Quote:
                        // we are looking for a closing quote
                        if (currentToken != "\"")
                        {
                            lastErrorMessage = "Error in RuleSyntax: Double quote mismatch.";
                            return;
                        }
                        else
                        {
                            // add the token as an operand
                            tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_String, false));
                        }
                        break;

                    case ParseState.Parse_State_OperandFunction:
                        break;

                }
            }

            #endregion

            #region RuleSyntax Validation Checks

            if (parenthesisMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a parenthesis mismatch.";
                return;
            }

            if (squareMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is an operand function mismatch.";
                return;
            }

            if (operandFunctionDepth > 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is an operand function mismatch error...Operand function depth is not zero.";
                return;
            }

            if (tildaMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a comment mismatch.";
                return;
            }

            if (quoteMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a quote mismatch.";
                return;
            }

            if (charIndex < ruleSyntax.Length)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem parsing the rule...some of the tokens were not found.";
                return;
            }

            if (assignmentStartCount != assignmentStopCount)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem parsing the rule...there was an assignment mismatch.";
                return;

            }

            #endregion

            // create the RPN Stack
            if (this.AnyErrors == false) MakeRPNQueue();

            // check that we have tokens in out RPN Queue
            if (rpn_queue == null)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem creating the RPN queue.";
                return;
            }

            if (rpn_queue.Count == 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem creating the RPN queue.";
                return;
            }

            // stop the timer
            parseTime.Stop();
            tokenParseTime = parseTime.Elapsed.TotalMilliseconds;

        }

        /// <summary>
        /// This is called by the constructor.  This new version includes support for assignment :=
        /// This new version also checks for iif[ operand functions embedded within other iif[ statements
        /// </summary>
        private void GetTokens()
        {
            // local variables
            ParseState parseState = ParseState.Parse_State_Operand;  // start be searching for an operand
            ParseState nextParseState = ParseState.Parse_State_Comment; // the next parse state after we have processed a comment            
            ParseState opFuncParseState = ParseState.Parse_State_Operand; // The parse state within the operand function
            ParseState tempParseState = ParseState.Parse_State_Operand; // temporary variable to hold a parse state
            string currentToken = "";


            int parenthesisMatch = 0;  // the number of open and close parenthesis should match
            int squareMatch = 0;  // the number of open and close square parenthesis should match
            int tildaMatch = 0; // the open and close tildas should match
            int quoteMatch = 0; // the open and close quotes should match
            int assignmentStartCount = 0;  // the number of times an assignment start operator was found;
            int assignmentStopCount = 0;   // the number of times an assignment stop operator was found
            bool isError = false;
            bool tokenCreated = false;

            int operandFunctionDepth = 0;  // operand function can contain operand function...This is the depth of the operand functions
          
            // make sure we have some syntax to parse
            if (String.IsNullOrEmpty(ruleSyntax) == true) return;

            // create a stop watch to time the parse
            System.Diagnostics.Stopwatch parseTime = System.Diagnostics.Stopwatch.StartNew();

            // start at the first character
            charIndex = 0;

            do
            {
                //  double check the exit condition, just in case
                if (charIndex >= ruleSyntax.Length) break;

                // the character to be processed
                char c = ruleSyntax[charIndex];

                // Increment the counter for the next character
                charIndex++;

                System.Diagnostics.Debug.WriteLine("c = " + c.ToString() + "\tcurrentToken = " + currentToken + "\tparse state = " + parseState.ToString() + "\tOp Parse State = " + opFuncParseState.ToString());

                #region New Line and Tab check
                // check for new line and tab
                if ((c == '\n') || (c == '\t'))
                {
                    // new lines and tabs are always ignored.
                    c = ' ';  // force the new line or tab to be a space and continue processing.
                }
                #endregion

                #region Comment Checking

                // check if we are in a comment
                if (parseState == ParseState.Parse_State_Comment)
                {
                    if (c == '~')
                    {
                        // we found the end of our comment, continue with what we were looking for
                        parseState = nextParseState;
                        tildaMatch--;
                    }

                    // continue with the next item in the loop
                    continue;
                }
                else
                {
                    if (c == '~')
                    {
                        // we found the start of a comment
                        tildaMatch++;
                        nextParseState = parseState;  // save the parse state
                        parseState = ParseState.Parse_State_Comment;
                        continue; // continue with the next item in the loop
                    }
                }
                #endregion

                // determine out current parse state
                switch (parseState)
                {
                    case ParseState.Parse_State_Operand:
                        #region Parse_State_Operand
                        if (c == '"')
                        {
                            #region Quote Handling

                            try
                            {
                                // we have a quote in an operand...This is probably the start of a string operand
                                quoteMatch++;
                                currentToken += c;
                                parseState = ParseState.Parse_State_Quote;
                            }
                            catch (Exception err)
                            {
                                lastErrorMessage = "Error in GetTokens() in Operand Quote Handling: " + err.Message;
                                return;
                            }

                            #endregion
                        }
                        else if (c == ' ')
                        {
                            #region Space Handling

                            try
                            {
                                // we are looking for an operand and we found a space.  We are not currently looking for a closing quote.
                                // Assume that the space indicates that the operand is completed and we now need to look for an operator                            
                                tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                                if (isError) return;

                                if (tokenCreated == true)
                                {
                                    // set the next parse state
                                    parseState = tempParseState;

                                    // reset the token
                                    currentToken = "";
                                }
                            }
                            catch (Exception err)
                            {
                                lastErrorMessage = "Error in GetTokens() in Operand Space Handling: " + err.Message;
                                return;
                            }

                            #endregion
                        }
                        else if (c == '(')
                        {
                            #region Parenthesis Handling
                            // increment the parenthesis count
                            parenthesisMatch++;

                            // create the token
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // add the open parenthesis
                            tokenItems.Add(new TokenItem("(", TokenType.Token_Open_Parenthesis, false));

                            // clear the current token
                            currentToken = "";

                            // after a parenthesis we need an operand
                            parseState = ParseState.Parse_State_Operand;
                            #endregion
                        }
                        else if (c == ')')
                        {
                            #region Parenthesis Handling
                            // decrement the parenthesis count
                            parenthesisMatch--;

                            // create the token
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // add the close parenthesis
                            tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, false));

                            // clear the current token
                            currentToken = "";

                            // after a parenthesis we need an operator
                            parseState = ParseState.Parse_State_Operator;

                            #endregion
                        }
                        else if (c == '[')
                        {
                            #region Operand Function Handling
                            // Check that an operand function is starting

                            // increment the square parenthesis count
                            squareMatch++;

                            // clean the token
                            currentToken = currentToken.Trim();

                            if (Support.DataTypeCheck.IsOperandFunction(currentToken) == true)
                            {
                                // we found the start of an operand function

                                // it's possible that we have operand functions within operand functions
                                operandFunctionDepth++;

                                // add the token item
                                currentToken += c;
                                tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand_Function_Start, false));

                                // Indicate that we are now in a operand function
                                parseState = ParseState.Parse_State_OperandFunction;

                                // within the operand function, we are looking for an operand
                                opFuncParseState = ParseState.Parse_State_Operand;

                                // reset the token
                                currentToken = "";

                            }
                            else
                            {
                                // we found a square bracket but we don't have an operand function
                                lastErrorMessage = "Error in Rule Syntax: Found an open square parenthesis without an operand function";
                                return;
                            }
                            #endregion
                        }
                        else if (c == ']')
                        {
                            #region Operand Function Handling
                            // we should never be looking for an operand and find a ]
                            squareMatch--;
                            lastErrorMessage = "Error in Rule Syntax: Found an ] while looking for an operand.";
                            return;
                            #endregion
                        }
                        else if (c == ',')
                        {
                            #region Command Handling
                            // we should never be looking for an operand and find a ,
                            lastErrorMessage = "Error in Rule Syntax: Found a , (comma) while looking for an operand.";
                            return;
                            #endregion
                        }
                        else if (c == '-')
                        {
                            #region Negative Operands
                            if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                            {
                                // we found a negative sign
                                currentToken += c;
                            }
                            else
                            {
                                // try and create the new token
                                currentToken += c;
                                tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                                if (isError == true) return;
                                if (tokenCreated == true)
                                {
                                    // the new tokens were created

                                    // reset the current token
                                    currentToken = "";

                                    // set the next parse state
                                    parseState = tempParseState;
                                }
                            }
                            #endregion
                        }
                        else if (c == ':')
                        {
                            #region Assignment handling

                            // we may have the start of an assignment.

                            // first check....the next character must be =
                            if (FoundAssignment() == false)
                            {
                                // nope, we did not find an assignment, this is a problem
                                lastErrorMessage = "Error in Rule Syntax: Found a : (colon) but did not find an assignment token.";
                                return;
                            }

                            assignmentStartCount++;

                            // since we were looking for an operand and we found a colon, we may have a current token
                            CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // second check....the assignment can only be made to a variable...
                            // therefore, the previous token must be a variable.
                            if (tokenItems[tokenItems.Count - 1].TokenType == TokenType.Token_Operand)
                            {
                                if (tokenItems[tokenItems.Count - 1].TokenDataType != TokenDataType.Token_DataType_Variable)
                                {
                                    // assignment can only happen to a variable, we have a problem
                                    lastErrorMessage = "Error in Rule Syntax: An assignment can only be made to a variable.";
                                    return;
                                }
                            }
                            else
                            {
                                // We have an error because you can only assign to an operand variableassignment can only happen to a variable, we have a problem
                                lastErrorMessage = "Error in Rule Syntax: An assignment can only be made to a variable.";
                                return;
                            }

                            // indicates that the last token item we created will be assigned to from the rule
                            tokenItems[tokenItems.Count - 1].WillBeAssigned = true;

                            // also indicate that this token items as assignments
                            anyAssignments = true;

                            // ok, we have a valid assignment....add the token
                            tokenItems.Add(new TokenItem(":=", TokenType.Token_Assignemt_Start, false));  // assignments are always flagged as not in operand functions

                            // increment the character index so that the = from the assignment is skipped
                            charIndex++;

                            // clear the token
                            currentToken = "";

                            // after the assignment, we need an operand
                            parseState = ParseState.Parse_State_Operand;

                            #endregion
                        }
                        else if (c == ';')
                        {
                            #region Assignment Handling

                            assignmentStopCount++;

                            // we found the end of an assignment, create the assignment stop token
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;

                            // add the stop token 
                            tokenItems.Add(new TokenItem(";", TokenType.Token_Assignment_Stop, false));

                            // reset the current token
                            currentToken = "";

                            // after the end of the assignment, we are starting over again with a new operand
                            parseState = ParseState.Parse_State_Operand;

                            #endregion
                        }
                        else
                        {
                            #region Other Handling
                            // try and create the new token
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // the new tokens were created

                                // reset the current token
                                currentToken = "";

                                // set the next parse state
                                parseState = tempParseState;

                            }
                            #endregion
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_OperandFunction:
                        #region Parse_State_OperandFunction

                        switch (opFuncParseState)
                        {
                            case ParseState.Parse_State_Operand:
                                #region Parse_State_OperandFunction, Parse_State_Operand
                                if (c == '"')
                                {
                                    #region Quote Handling
                                    // we have a quote in the operand function...This is probably the start of a string operand
                                    quoteMatch++;
                                    currentToken += c;
                                    opFuncParseState = ParseState.Parse_State_Quote;
                                    #endregion
                                }
                                else if (c == ' ')
                                {
                                    #region Space Handling
                                    // we are looking for an operand and we found a space.  We are not currently looking for a closing quote.
                                    // Assume that the space indicates that the operand is completed and we now need to look for an operator                            
                                    tokenCreated = CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (tokenCreated == true)
                                    {
                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                        // reset the token
                                        currentToken = "";
                                    }
                                    #endregion
                                }
                                else if (c == '(')
                                {
                                    #region Parenthesis Handling
                                    // increment the parenthesis count
                                    parenthesisMatch++;

                                    // create the token
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the open parenthesis
                                    tokenItems.Add(new TokenItem("(", TokenType.Token_Open_Parenthesis, true));

                                    // clear the current token
                                    currentToken = "";

                                    // after a parenthesis we need an operand
                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else if (c == ')')
                                {
                                    #region Parenthesis Handling
                                    // decrement the parenthesis count
                                    parenthesisMatch--;

                                    // create the token
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the close parenthesis
                                    tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, true));

                                    // clear the current token
                                    currentToken = "";

                                    // after a parenthesis we need an operator
                                    opFuncParseState = ParseState.Parse_State_Operator;

                                    #endregion
                                }
                                else if (c == '[')
                                {
                                    #region Operand Function Handling
                                    // Check that an operand function is starting

                                    // increment the square parenthesis count
                                    squareMatch++;

                                    // clean the token
                                    currentToken = currentToken.Trim();

                                    if (Support.DataTypeCheck.IsOperandFunction(currentToken) == true)
                                    {
                                        // we found the start of an operand function

                                        // it's possible that we have operand functions within operand functions
                                        operandFunctionDepth++;

                                        // add the token item
                                        currentToken += c;
                                        tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand_Function_Start, true));

                                        // within the operand function, we are looking for an operand
                                        opFuncParseState = ParseState.Parse_State_Operand;

                                        // reset the token
                                        currentToken = "";

                                    }
                                    else
                                    {
                                        // we found a square bracket but we don't have an operand function
                                        lastErrorMessage = "Error in Rule Syntax: Found an open square parenthesis without an operand function";
                                        return;
                                    }
                                    #endregion
                                }
                                else if (c == ']')
                                {
                                    #region Operand Function Handling

                                    squareMatch--;

                                    // we found the end of an operator function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the stop operand token
                                    tokenItems.Add(new TokenItem("]", TokenType.Token_Operand_Function_Stop, true));

                                    // decrement the operand depth
                                    operandFunctionDepth--;

                                    // set the parse states
                                    //opFuncParseState = ParseState.Parse_State_Operand;
                                    opFuncParseState = ParseState.Parse_State_Operator;

                                    // check if we are done with operand function and now need an operator
                                    if (operandFunctionDepth <= 0)
                                    {
                                        operandFunctionDepth = 0;
                                        parseState = ParseState.Parse_State_Operator;
                                    }

                                    // reset the current token
                                    currentToken = "";

                                    #endregion
                                }
                                else if (c == ',')
                                {
                                    #region Command Handling
                                    // create a toke
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the delimiter token
                                    tokenItems.Add(new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, true));

                                    // reset the token
                                    currentToken = "";

                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else if (c == '-')
                                {
                                    #region Negative operands
                                    if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                                    {
                                        // we found a negative sign
                                        currentToken += c;
                                    }
                                    else
                                    {
                                        // try and create the new token
                                        currentToken += c;
                                        tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                        if (isError == true) return;
                                        if (tokenCreated == true)
                                        {
                                            // the new tokens were created

                                            // reset the current token
                                            currentToken = "";

                                            // set the next parse state
                                            opFuncParseState = tempParseState;
                                        }
                                    }
                                    #endregion
                                }
                                else if ((c == ':') || (c == ';'))
                                {
                                    #region Assignment Handling

                                    // assignments are not allowed within operand function
                                    lastErrorMessage = "Error in Rule Syntax: Assignments are not allowed within Operand Functions";
                                    return;
                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    // try and create the new token
                                    currentToken += c;

                                    tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    if (tokenCreated == true)
                                    {
                                        // the new tokens were created

                                        // reset the current token
                                        currentToken = "";

                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                    }
                                    #endregion
                                }

                                #endregion
                                break;

                            case ParseState.Parse_State_Operator:
                                #region Parse_State_OperandFunction, Parse_State_Operator

                                if (c == '"')
                                {
                                    #region Quote Handling
                                    // we found a quote while looking for an operator...this is a problem
                                    lastErrorMessage = "Error in Rule Syntax: Found a double quote (\") while looking for an operator.";
                                    return;
                                    #endregion
                                }
                                else if (c == ' ')
                                {
                                    #region Space Handling
                                    // we found a space while looking for an operator...maybe it's just white space
                                    if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                                    {
                                        // yep, it's just white space; it can be ignored.
                                        currentToken = "";
                                    }
                                    else
                                    {
                                        // we found a space while looking for an operator....this is a problem because
                                        // no operators allow for spaces.
                                        lastErrorMessage = "Error with expression syntax: Found a space while looking for an operator";
                                        return;
                                    }
                                    #endregion
                                }
                                else if (c == '(')
                                {
                                    #region Parenthesis Handling
                                    // we found an opern parenthesis while searching for an operator....that's a problem
                                    lastErrorMessage = "Error in rule syntax: Found an open parenthesis while searching for an operator";
                                    return;
                                    #endregion
                                }
                                else if (c == ')')
                                {
                                    #region Parenthesis Handling
                                    // add the closed parenthesis and keep looking for the operator
                                    parenthesisMatch--;
                                    tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, true));
                                    #endregion
                                }
                                else if (c == '[')
                                {
                                    #region Operand Function
                                    // we found an open parenthesis while searching for an operator....that's a problem
                                    lastErrorMessage = "Error in rule syntax: Found an open square parenthesis while searching for an operator";
                                    return;
                                    #endregion
                                }
                                else if (c == ']')
                                {
                                    #region Operand Function
                                    squareMatch--;
                                    // we found the end of an operator function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the stop operand token
                                    tokenItems.Add(new TokenItem("]", TokenType.Token_Operand_Function_Stop, true));

                                    // decrement the operand depth
                                    operandFunctionDepth--;

                                    // set the parse states
                                    opFuncParseState = ParseState.Parse_State_Operand;

                                    // check if we are done with operand function and now need an operator
                                    if (operandFunctionDepth <= 0)
                                    {
                                        operandFunctionDepth = 0;
                                        parseState = ParseState.Parse_State_Operator;
                                    }

                                    // reset the current token
                                    currentToken = "";

                                    #endregion
                                }
                                else if (c == ',')
                                {
                                    #region Command Handling
                                    // we found a comma while searching for an operator....that's alright in an operand function
                                    CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;

                                    // add the comma
                                    tokenItems.Add(new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, true));

                                    // reset the token
                                    currentToken = "";

                                    // set the parse state
                                    opFuncParseState = ParseState.Parse_State_Operand;
                                    #endregion
                                }
                                else if ((c == ':') || (c == ';'))
                                {
                                    #region Assignment Handling

                                    // assignments are not allowed within operand function
                                    lastErrorMessage = "Error in Rule Syntax: Assignments are not allowed within Operand Functions";
                                    return;
                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    // try and create the new token
                                    currentToken += c;
                                    tokenCreated = CreateTokenItem(currentToken, true, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;
                                    if (tokenCreated == true)
                                    {
                                        // the new tokens were created

                                        // reset the current token
                                        currentToken = "";

                                        // set the next parse state
                                        opFuncParseState = ParseState.Parse_State_Operand;

                                    }
                                    #endregion
                                }

                                #endregion
                                break;

                            case ParseState.Parse_State_Quote:
                                #region Parse_State_OperandFunction, Parse_State_Quote

                                if (c == '"')
                                {
                                    #region Quote Handling
                                    quoteMatch--;

                                    // we found the end of the qoute
                                    currentToken += c;
                                    tokenCreated = CreateTokenItem(currentToken, false, true, out tempParseState, out isError, out lastErrorMessage);
                                    if (isError == true) return;
                                    if (tokenCreated == true)
                                    {
                                        // set the next parse state
                                        opFuncParseState = tempParseState;

                                        // clear the current token
                                        currentToken = "";
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Other Handling
                                    currentToken += c;
                                    #endregion
                                }

                                #endregion
                                break;
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_Operator:
                        #region Parse_State_Operator
                        if (c == '"')
                        {
                            #region Quote Handling
                            // we found a quote while looking for an operator...this is a problem
                            lastErrorMessage = "Error in Rule Syntax: Found a double quote (\") while looking for an operator.";
                            return;
                            #endregion
                        }
                        else if (c == ' ')
                        {
                            #region Space Handling
                            // we found a space while looking for an operator...maybe it's just white space
                            if (String.IsNullOrEmpty(currentToken.Trim()) == true)
                            {
                                // yep, it's just white space; it can be ignored.
                                currentToken = "";
                            }
                            else
                            {
                                // we found a space while looking for an operator....this is a problem because
                                // no operators allow for spaces.
                                lastErrorMessage = "Error with expression syntax: Found a space while looking for an operator";
                                return;
                            }
                            #endregion
                        }
                        else if (c == '(')
                        {
                            #region Parenthesis Handling
                            // we found an opern parenthesis while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found an open parenthesis while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ')')
                        {
                            #region Parenthesis Handling
                            parenthesisMatch--;

                            // add the closed parenthesis and keep looking for the operator
                            tokenItems.Add(new TokenItem(")", TokenType.Token_Close_Parenthesis, false));
                            #endregion
                        }
                        else if (c == '[')
                        {
                            #region Operand Function Handling
                            // we found an open parenthesis while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found an open square parenthesis while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ']')
                        {
                            #region operand Function Handling
                            // we found an closed parenthesis while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found a closed square parenthesis while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ',')
                        {
                            #region Comma Handling
                            // we found a comma while searching for an operator....that's a problem
                            lastErrorMessage = "Error in rule syntax: Found a comma while searching for an operator";
                            return;
                            #endregion
                        }
                        else if (c == ':')
                        {
                            #region Assignment handling

                            // we may have the start of an assignment.

                            // first check....the next character must be =
                            if (FoundAssignment() == false)
                            {
                                // nope, we did not find an assignment, this is a problem
                                lastErrorMessage = "Error in Rule Syntax: Found a : (colon) but did not find an assignment token.";
                                return;
                            }

                            assignmentStartCount++;

                            // second check....the assignment can only be made to a variable...
                            // therefore, the previous token must be a variable.
                            if (tokenItems[tokenItems.Count - 1].TokenType == TokenType.Token_Operand)
                            {
                                if (tokenItems[tokenItems.Count - 1].TokenDataType != TokenDataType.Token_DataType_Variable)
                                {
                                    // assignment can only happen to a variable, we have a problem
                                    lastErrorMessage = "Error in Rule Syntax: An assignment can only be made to a variable.";
                                    return;
                                }
                            }
                            else
                            {
                                // We have an error because you can only assign to an operand variableassignment can only happen to a variable, we have a problem
                                lastErrorMessage = "Error in Rule Syntax: An assignment can only be made to a variable.";
                                return;
                            }

                            // indicates that the last token item we created will be assigned to from the rule
                            tokenItems[tokenItems.Count - 1].WillBeAssigned = true;

                            // also indicate that this token items as assignments
                            anyAssignments = true;

                            // ok, we have a valid assignment....add the token
                            tokenItems.Add(new TokenItem(":=", TokenType.Token_Assignemt_Start, false));  // assignments are always flagged as not in operand functions

                            // increment the character index so that the = from the assignment is skipped
                            charIndex++;

                            // clear the token
                            currentToken = "";

                            // after the assignment, we need an operand
                            parseState = ParseState.Parse_State_Operand;

                            #endregion
                        }
                        else if (c == ';')
                        {
                            #region End assignment handling

                            // we found the end of the assignment...add the token
                            tokenItems.Add(new TokenItem(";", TokenType.Token_Assignment_Stop, false));

                            assignmentStopCount++;

                            // clear the token
                            currentToken = "";

                            // after a closing assignment token item, we need a operand
                            parseState = ParseState.Parse_State_Operand;

                            #endregion
                        }
                        else
                        {
                            #region Other Handling
                            // try and create the new token
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, true, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // the new tokens were created

                                // reset the current token
                                currentToken = "";

                                // set the next parse state
                                parseState = tempParseState;

                            }
                            #endregion
                        }
                        #endregion
                        break;

                    case ParseState.Parse_State_Quote:
                        #region Parse_State_Quote
                        if (c == '"')
                        {
                            #region Quote Handling
                            // we found the end of the qoute
                            quoteMatch--;
                            currentToken += c;
                            tokenCreated = CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                            if (isError == true) return;
                            if (tokenCreated == true)
                            {
                                // set the next parse state
                                parseState = tempParseState;

                                // clear the current token
                                currentToken = "";
                            }
                            #endregion
                        }
                        else
                        {
                            currentToken += c;
                        }

                        #endregion
                        break;
                }


            } while (charIndex < ruleSyntax.Length);


            #region Final Token Handling

            // see if we have a current token that needs to be processed.
            currentToken = currentToken.Trim();

            if (String.IsNullOrEmpty(currentToken) == false)
            {
                // we have a token that needs to be processed
                if (currentToken == "(")
                    parenthesisMatch++;
                else if (currentToken == ")")
                    parenthesisMatch--;
                else if (currentToken == "[")
                    squareMatch++;
                else if (currentToken == "]")
                    squareMatch--;
                else if (currentToken == "~")
                    tildaMatch--;
                else if (currentToken == "\"")
                    quoteMatch--;

                switch (parseState)
                {
                    case ParseState.Parse_State_Operand:
                        CreateTokenItem(currentToken, false, false, out tempParseState, out isError, out lastErrorMessage);
                        if (isError == true) return;
                        break;

                    case ParseState.Parse_State_Operator:
                        // there should never be an operator at the end of the rule syntax
                        lastErrorMessage = "Error in Rule Syntax: A rule cannot end with an operator.";
                        break;

                    case ParseState.Parse_State_Quote:
                        // we are looking for a closing quote
                        if (currentToken != "\"")
                        {
                            lastErrorMessage = "Error in RuleSyntax: Double quote mismatch.";
                            return;
                        }
                        else
                        {
                            // add the token as an operand
                            tokenItems.Add(new TokenItem(currentToken, TokenType.Token_Operand, TokenDataType.Token_DataType_String, false));
                        }
                        break;

                    case ParseState.Parse_State_OperandFunction:
                        break;

                }
            }

            #endregion

            #region RuleSyntax Validation Checks

            if (parenthesisMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a parenthesis mismatch.";
                return;
            }

            if (squareMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is an operand function mismatch.";
                return;
            }

            if (operandFunctionDepth > 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is an operand function mismatch error...Operand function depth is not zero.";
                return;
            }

            if (tildaMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a comment mismatch.";
                return;
            }

            if (quoteMatch != 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There is a quote mismatch.";
                return;
            }

            if (charIndex < ruleSyntax.Length)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem parsing the rule...some of the tokens were not found.";
                return;
            }

            if (assignmentStartCount != assignmentStopCount)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem parsing the rule...there was an assignment mismatch.";
                return;

            }

            #endregion

            #region Search for the iif[] operand functions that can be short circuited

            // loop through all of the tokens and look for operand functions start and close
            bool anyIIFOperandFuncs = false;
            Support.ExStack<TokenItem> iifStack = new Support.ExStack<TokenItem>();
            foreach (TokenItem item in this.tokenItems)
            {
                System.Diagnostics.Debug.WriteLine(item.TokenName.Trim().ToLower());

                if (item.TokenType == TokenType.Token_Operand_Function_Start)
                {
                    // push the operand on the stack
                    anyIIFOperandFuncs = anyIIFOperandFuncs || (item.TokenName.Trim().ToLower() == "iif[");
                    iifStack.Push(item);
                }
                else if (item.TokenType == TokenType.Token_Operand_Function_Stop)
                {
                    // we want to "throw-away" all operand fungsction other than the iif[].
                    // peek at the stack and see what's on top
                    string peekItem = iifStack.Peek().TokenName.Trim().ToLower();
                    if ((peekItem == "iif[") || (peekItem == "]"))
                    {
                        // we found a complete iif[] statement
                        iifStack.Push(item);
                    }
                    else
                    {
                        // we have the closing bracket that is not for the iif[] statement....
                        // discard the closing ] and the operand function
                        iifStack.Pop();
                    }

                }
            }

            // see if there are an iif[] operand functions
            if (anyIIFOperandFuncs == true)
            {
                // we have iif[] operands, make sure they are NOT nested...This version does not support short circuit of nexted iif[] 

                do
                {
                    TokenItem item1 = null;
                    TokenItem item2 = null;

                    if (iifStack.Count > 0) item1 = iifStack.Pop();
                    if (iifStack.Count > 0) item2 = iifStack.Peek();

                    if (item1 != null)
                    {
                        if (item1.TokenType == TokenType.Token_Operand_Function_Stop)
                        {
                            // we found a ]

                            if (item2 != null)
                            {
                                if (item2.TokenType == TokenType.Token_Operand_Function_Start)
                                {
                                    // we found a iif[ 
                                    // this can be short circuited
                                    item2.CanShortCircuit = true;
                                }
                            }
                        }
                    }

                } while (iifStack.Count > 0);


            }

            #endregion

            #region Make the RPN Queue

            // create the RPN Stack
            if (this.AnyErrors == false) MakeRPNQueue();

            // check that we have tokens in out RPN Queue
            if (rpn_queue == null)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem creating the RPN queue.";
                return;
            }

            if (rpn_queue.Count == 0)
            {
                lastErrorMessage = "Error in RuleSyntax: There was a problem creating the RPN queue.";
                return;
            }

            #endregion
          
            // stop the timer
            parseTime.Stop();
            tokenParseTime = parseTime.Elapsed.TotalMilliseconds;

        }

       
        
        private void MakeRPNQueue()
        {

            /*
            While there are tokens to be read: 
                Read a token. 
                If the token is a number, then add it to the output queue. 
                If the token is a function token, then push it onto the stack. 
                If the token is a function argument separator (e.g., a comma): 
                    Until the topmost element of the stack is a left parenthesis, pop the element onto the output queue. If no left parentheses are encountered, either the separator was misplaced or parentheses were mismatched. 
                If the token is an operator, o1, then: 
                    while there is an operator, o2, at the top of the stack, and either 
                        o1 is associative or left-associative and its precedence is less than (lower precedence) or equal to that of o2, or 
                        o1 is right-associative and its precedence is less than (lower precedence) that of o2,

                        pop o2 off the stack, onto the output queue; 
                    push o1 onto the operator stack. 
                If the token is a left parenthesis, then push it onto the stack. 
                If the token is a right parenthesis: 
                    Until the token at the top of the stack is a left parenthesis, pop operators off the stack onto the output queue. 
                    Pop the left parenthesis from the stack, but not onto the output queue. 
                    If the token at the top of the stack is a function token, pop it and onto the output queue. 
                    If the stack runs out without finding a left parenthesis, then there are mismatched parentheses. 
                When there are no more tokens to read: 
                    While there are still operator tokens in the stack: 
                        If the operator token on the top of the stack is a parenthesis, then there are mismatched parenthesis. 
                        Pop the operator onto the output queue. 
            Exit. 
            */


            // set the last error message
            lastErrorMessage = "";

            // make sure we have parsed tokens
            if (this.tokenItems.Count == 0)
            {
                lastErrorMessage = "No tokens to add to the RPN stack";
                return;
            }

            #region Version 4

            //////////////////////////////////////////////////////////////////////////
            //  This version supports short curcuiting the iif[] operand function
            //////////////////////////////////////////////////////////////////////////
            
            // create the output queue...This is the final queue that is exposed through the public property
            rpn_queue = new Support.ExQueue<TokenItem>(tokenItems.Count);

            // create the operator stack
            Support.ExStack<TokenItem> operators = new Support.ExStack<TokenItem>();


            // create the operator stack for operators
            Support.ExStack<TokenItem> param_operators = new Support.ExStack<TokenItem>();

            // create a temporary queue for expressions that are in operand functions
            Support.ExQueue<TokenItem> param_queue = new Support.ExQueue<TokenItem>();

            // local variables
            //int count = 0;
            bool startedShortCircuit = false;   // indicates if short circuit has started
            TokenItem shortCircuitItem = null;  // pointer to the short circuit item
            IIFShortCircuitState shortCircuitState = IIFShortCircuitState.ShortCircuit_Condition;  // the current parameter in the short circuit iif[] operand
            int matchingSquare = 0;  // used to track the number of closing square parenthesis untile the end of the short circuit item.

            // create a "fake" delimiter token item
            Parser.TokenItem delimiter = new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, false);

            // loop through all the token items
            foreach (TokenItem item in tokenItems)
            {
                System.Diagnostics.Debug.WriteLine(item.TokenName);

                switch (item.TokenType)
                {
                    case TokenType.Token_Close_Parenthesis:
                        #region Token_Close_Parenthesis

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            if (param_operators.Count > 0)
                            {
                                // start peeking at the top operator
                                do
                                {
                                    if (param_operators.Peek().TokenType == TokenType.Token_Open_Parenthesis)
                                    {
                                        // we found the matching open parenthesis...pop it and discard
                                        param_operators.Pop();
                                        break;
                                    }
                                    else
                                    {
                                        // pop the operator and push it on the queue
                                        param_queue.Add(param_operators.Pop());
                                    }

                                    if (param_operators.Count == 0) break;
                                }
                                while (true);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            if (operators.Count > 0)
                            {
                                // start peeking at the top operator
                                do
                                {
                                    System.Diagnostics.Debug.WriteLine("    Peek = " + operators.Peek().TokenName);

                                    // see what is on top of the operator stack
                                    if (operators.Peek().TokenType == TokenType.Token_Open_Parenthesis)
                                    {
                                        // we found the matching open parenthesis...pop it and discard
                                        operators.Pop();
                                        break;
                                    }
                                    else
                                    {
                                        // pop the operator and push it on the queue
                                        rpn_queue.Add(operators.Pop());
                                    }

                                    if (operators.Count == 0) break;
                                }
                                while (true);
                            }
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Open_Parenthesis:
                        #region Token_Open_Parenthesis

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            // add open parenthesis to the operator stack
                            param_operators.Push(item);
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            // add open parenthesis to the operator stack
                            operators.Push(item);
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operand:
                        #region Token_Operand

                        if (item.InOperandFunction == true)
                        {
                            #region Operand Queue
                            param_queue.Add(item); // if token is an operand, then write it to output. 
                            #endregion
                        }
                        else
                        {
                            #region Operand Queue
                            rpn_queue.Add(item); // if token is an operand, then write it to output. 
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Delimiter:
                        #region Token_Operand_Function_Delimiter
                
                        // pop the operators from the parameter stack util it's empty or we get a delimiter
                        do
                        {
                            // check if the parameters operator stack is empty
                            if (param_operators.Count == 0) break;

                            // pop an item from the stack
                            Parser.TokenItem opItem = param_operators.Pop();

                            // if the item is a delimiter...discard it and exit loop
                            if (opItem.TokenType == TokenType.Token_Operand_Function_Delimiter) break;

                            // add the item to the paramter queue
                            param_queue.Add(opItem);
                        }
                        while (true);

                        // start removing items from the paraemters queue and add them to the RPN queue
                        // unill it's empty or we access an operand function start
                        do
                        {
                            // see if the parameter queue is empty
                            if (param_queue.Count == 0) break;

                            // get the item from the queue
                            Parser.TokenItem qItem = param_queue.Dequeue();

                            // add the item to the appropriate rpn queue
                            if (startedShortCircuit == true)
                            {
                                if (qItem.TokenName.Trim().ToLower() != "iif[")
                                {
                                    // we are in short circuit mode
                                    switch (shortCircuitState)
                                    {
                                        case IIFShortCircuitState.ShortCircuit_Condition:
                                            shortCircuitItem.ShortCircuit.RPNCondition.Add(qItem);
                                            break;

                                        case IIFShortCircuitState.ShortCircuit_False:
                                            shortCircuitItem.ShortCircuit.RPNFalse.Add(qItem);
                                            break;

                                        case IIFShortCircuitState.ShortCircuit_True:
                                            shortCircuitItem.ShortCircuit.RPNTrue.Add(qItem);
                                            break;
                                    }
                                }
                            }
                            else
                                rpn_queue.Add(qItem);

                            // if the qItem is the start of another operand function, then we are done
                            if (startedShortCircuit == false)
                            {
                                if (qItem.TokenType == TokenType.Token_Operand_Function_Start) break;
                            }
                        }
                        while (true);

                        // push the delimiter on the parameter operator stack
                        param_operators.Push(item);

                        // add the delimiter to the queue
                        if (startedShortCircuit == true)
                        {
                            if (matchingSquare == 0)
                            {
                                // we are in short circuit mode
                                switch (shortCircuitState)
                                {
                                    case IIFShortCircuitState.ShortCircuit_Condition:
                                        shortCircuitState = IIFShortCircuitState.ShortCircuit_True;
                                        break;

                                    case IIFShortCircuitState.ShortCircuit_False:
                                        // don't change state
                                        break;

                                    case IIFShortCircuitState.ShortCircuit_True:
                                        shortCircuitState = IIFShortCircuitState.ShortCircuit_False;
                                        break;
                                }
                            }
                        }
                        else
                            rpn_queue.Add(item);

                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Start:
                        #region Token_Operand_Function_Start

                        if (item.InOperandFunction == true)
                        {
                            if (item.CanShortCircuit == true)
                            {
                                // add the item to the "regular" queue
                                rpn_queue.Add(item);
                            }
                            else
                            {

                                #region Parameter Operand Queue
                                param_queue.Add(item);

                                // whenever we add a new operand function to the parameter queue,
                                // add a "fake" delimiter to the parameter operator queue
                                param_operators.Push(delimiter);

                                #endregion
                            }
                        }
                        else
                        {
                            #region Operand Queue
                            rpn_queue.Add(item); // if token is an operand function, then write it to output. 

                            #endregion
                        }

                        // check if the item is a short circuit item
                        if (item.CanShortCircuit == true)
                        {
                            // start a new short circuit
                            startedShortCircuit = true;
                            shortCircuitItem = item;
                            shortCircuitState = IIFShortCircuitState.ShortCircuit_Condition;
                        }
                        else
                        {
                            matchingSquare++;  // we have an operand function in a short citcuit IIF[] operand function
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Stop:
                        #region Token_Operand_Function_Stop

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Queue


                            // pop the operators from the parameter stack util it's empty or we get a delimiter
                            do
                            {
                                // check if the parameters operator stack is empty
                                if (param_operators.Count == 0) break;

                                // pop an item from the stack
                                Parser.TokenItem opItem = param_operators.Pop();

                                // if the item is a delimiter...discard it and exit loop
                                if (opItem.TokenType == TokenType.Token_Operand_Function_Delimiter) break;

                                // add the item to the paramter queue
                                param_queue.Add(opItem);
                            }
                            while (true);

                            // add the square bracket to the queue
                            param_queue.Add(item);

                            // start removing items from the paraemters queue and add them to the RPN queue
                            // unill it's empty or we access an operand function start
                            do
                            {
                                // see if the parameter queue is empty
                                if (param_queue.Count == 0) break;

                                // get the item from the queue
                                Parser.TokenItem qItem = param_queue.Dequeue();

                                if (startedShortCircuit == true)
                                {
                                    // we may be at the end of the short circuit....double check the state
                                    if ((matchingSquare == 0) && (shortCircuitState != IIFShortCircuitState.ShortCircuit_False))
                                    {
                                        // this is a problem, we should be on the false condition by now.
                                        lastErrorMessage = "Error parsing the iif[] short circuit operand function: Invalid format: Expecting the false condition";
                                        return;
                                    }

                                    if (qItem.TokenType == TokenType.Token_Operand_Function_Stop)
                                    {
                                        if (matchingSquare == 0)
                                        {
                                            // the stop token will go in the "regualar" queue
                                            rpn_queue.Add(qItem);
                                        }
                                        else
                                        {
                                            // this clocing token is for an operand function embedded in the iif[]  short circuit.
                                            switch (shortCircuitState)
                                            {
                                                case IIFShortCircuitState.ShortCircuit_Condition:
                                                    shortCircuitItem.ShortCircuit.RPNCondition.Add(qItem);
                                                    break;

                                                case IIFShortCircuitState.ShortCircuit_False:
                                                    shortCircuitItem.ShortCircuit.RPNFalse.Add(qItem);
                                                    break;

                                                case IIFShortCircuitState.ShortCircuit_True:
                                                    shortCircuitItem.ShortCircuit.RPNTrue.Add(qItem);
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //shortCircuitItem.ShortCircuit.RPNFalse.DeleteOnSubmit(qItem);
                                        // this clocing token is for an operand function embedded in the iif[]  short circuit.
                                        switch (shortCircuitState)
                                        {
                                            case IIFShortCircuitState.ShortCircuit_Condition:
                                                shortCircuitItem.ShortCircuit.RPNCondition.Add(qItem);
                                                break;

                                            case IIFShortCircuitState.ShortCircuit_False:
                                                shortCircuitItem.ShortCircuit.RPNFalse.Add(qItem);
                                                break;

                                            case IIFShortCircuitState.ShortCircuit_True:
                                                shortCircuitItem.ShortCircuit.RPNTrue.Add(qItem);
                                                break;
                                        }

                                    }

                                }
                                else
                                    rpn_queue.Add(qItem);

                                // if the qItem is the end of an operand function, then we are done
                                if (qItem.TokenType == TokenType.Token_Operand_Function_Stop) break;
                            }
                            while (true);


                            if (startedShortCircuit == true)
                            {                                
                                if (matchingSquare == 0)
                                {
                                    // we are done with the short circuiting
                                    startedShortCircuit = false;
                                    shortCircuitItem = null;
                                    shortCircuitState = IIFShortCircuitState.ShortCircuit_Condition;
                                }
                                else
                                    matchingSquare--;
                            }

                            #endregion
                        }
                        else
                        {
                            #region Operand Queue
                            rpn_queue.Add(item); // if token is an operand function, then write it to output. 
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operator:
                        #region Token_Operator

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            if (param_operators.Count > 0)
                            {
                                // peek at the top item of the operator stack
                                do
                                {
                                    // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                                    if (item.OrderOfOperationPrecedence >= param_operators.Peek().OrderOfOperationPrecedence)
                                    {
                                        param_queue.Add(param_operators.Pop());
                                    }
                                    else
                                        break;


                                    if (param_operators.Count == 0) break;

                                } while (true);

                            }

                            // push the token to the operator stack
                            param_operators.Push(item);
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            if (operators.Count > 0)
                            {
                                // peek at the top item of the operator stack

                                do
                                {
                                    // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                                    if (item.OrderOfOperationPrecedence >= operators.Peek().OrderOfOperationPrecedence)
                                    {
                                        rpn_queue.Add(operators.Pop());
                                    }
                                    else
                                        break;


                                    if (operators.Count == 0) break;

                                } while (true);

                            }

                            // push the token to the operator stack
                            operators.Push(item);
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Assignemt_Start:
                        #region Token_Assignemt_Start

                        if (operators.Count > 0)
                        {
                            // peek at the top item of the operator stack

                            do
                            {
                                // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                                if (item.OrderOfOperationPrecedence >= operators.Peek().OrderOfOperationPrecedence)
                                {
                                    rpn_queue.Add(operators.Pop());
                                }
                                else
                                    break;


                                if (operators.Count == 0) break;

                            } while (true);

                        }

                        // push the token to the operator stack
                        operators.Push(item);


                        #endregion
                        break;

                    case TokenType.Token_Assignment_Stop:
                        #region Token_Assignment_Stop

                        // start popping items from the operator stack until we find the assignemtn start
                        if (operators.Count > 0)
                        {
                            // peek at the top item of the operator stack

                            do
                            {
                                TokenItem t1 = operators.Pop();
                                rpn_queue.Add(t1);

                                if (t1.TokenType == TokenType.Token_Assignemt_Start)
                                {
                                    // we are all don popping operators
                                    break;
                                }

                                if (operators.Count == 0) break;

                            } while (true);

                        }

                        #endregion
                        break;
                }
            }

            // pop the remaining operators
            int opCount = operators.Count;
            for (int i = 0; i < opCount; i++) rpn_queue.Add(operators.Pop());
            
            #endregion

            #region Version 3

            //////////////////////////////////////////////////////////////////////////
            //  This version is working good...however, it does not include
            //  support for short curcuiting the iif[] operand functions
            //////////////////////////////////////////////////////////////////////////

            /*
            // create the output queue...This is the final queue that is exposed through the public property
            rpn_queue = new Support.ExQueue<TokenItem>(tokenItems.Count);

            // create the operator stack
            Support.ExStack<TokenItem> operators = new Support.ExStack<TokenItem>();


            // create the operator stack for operators
            Support.ExStack<TokenItem> param_operators = new Support.ExStack<TokenItem>();

            // create a temporary queue for expressions that are in operand functions
            Support.ExQueue<TokenItem> param_queue = new Support.ExQueue<TokenItem>();

            // local variables
            int count = 0;

            // create a "fake" delimiter token item
            Parser.TokenItem delimiter = new TokenItem(",", TokenType.Token_Operand_Function_Delimiter, false);

            // loop through all the token items
            foreach (TokenItem item in tokenItems)
            {
                System.Diagnostics.Debug.WriteLine(item.TokenName);

                switch (item.TokenType)
                {
                    case TokenType.Token_Close_Parenthesis:
                        #region Token_Close_Parenthesis

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            if (param_operators.Count > 0)
                            {
                                // start peeking at the top operator
                                do
                                {
                                    if (param_operators.Peek().TokenType == TokenType.Token_Open_Parenthesis)
                                    {
                                        // we found the matching open parenthesis...pop it and discard
                                        param_operators.Pop();
                                        break;
                                    }
                                    else
                                    {
                                        // pop the operator and push it on the queue
                                        param_queue.DeleteOnSubmit(param_operators.Pop());
                                    }

                                    if (param_operators.Count == 0) break;
                                }
                                while (true);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            if (operators.Count > 0)
                            {
                                // start peeking at the top operator
                                do
                                {
                                    System.Diagnostics.Debug.WriteLine("    Peek = " + operators.Peek().TokenName);

                                    // see what is on top of the operator stack
                                    if (operators.Peek().TokenType == TokenType.Token_Open_Parenthesis)
                                    {
                                        // we found the matching open parenthesis...pop it and discard
                                        operators.Pop();
                                        break;
                                    }
                                    else
                                    {
                                        // pop the operator and push it on the queue
                                        rpn_queue.DeleteOnSubmit(operators.Pop());
                                    }

                                    if (operators.Count == 0) break;
                                }
                                while (true);
                            }
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Open_Parenthesis:
                        #region Token_Open_Parenthesis

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            // add open parenthesis to the operator stack
                            param_operators.Push(item);
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            // add open parenthesis to the operator stack
                            operators.Push(item);
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operand:
                        #region Token_Operand

                        if (item.InOperandFunction == true)
                        {
                            #region Operand Queue
                            param_queue.DeleteOnSubmit(item); // if token is an operand, then write it to output. 
                            #endregion
                        }
                        else
                        {
                            #region Operand Queue
                            rpn_queue.DeleteOnSubmit(item); // if token is an operand, then write it to output. 
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Delimiter:
                        #region Token_Operand_Function_Delimiter
                
                        // pop the operators from the parameter stack util it's empty or we get a delimiter
                        do
                        {
                            // check if the parameters operator stack is empty
                            if (param_operators.Count == 0) break;

                            // pop an item from the stack
                            Parser.TokenItem opItem = param_operators.Pop();

                            // if the item is a delimiter...discard it and exit loop
                            if (opItem.TokenType == TokenType.Token_Operand_Function_Delimiter) break;

                            // add the item to the paramter queue
                            param_queue.DeleteOnSubmit(opItem);
                        }
                        while (true);

                        // start removing items from the paraemters queue and add them to the RPN queue
                        // unill it's empty or we access an operand function start
                        do
                        {
                            // see if the parameter queue is empty
                            if (param_queue.Count == 0) break;

                            // get the item from the queue
                            Parser.TokenItem qItem = param_queue.Dequeue();

                            rpn_queue.DeleteOnSubmit(qItem);

                            // if the qItem is the start of another operand function, then we are done
                            if (qItem.TokenType == TokenType.Token_Operand_Function_Start) break;
                        }
                        while (true);

                        // push the delimiter on the parameter operator stack
                        param_operators.Push(item);

                        // add the delimiter to the queue
                        rpn_queue.DeleteOnSubmit(item);

                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Start:
                        #region Token_Operand_Function_Start

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operand Queue
                            param_queue.DeleteOnSubmit(item);

                            // whenever we add a new operand function to the parameter queue,
                            // add a "fake" delimiter to the parameter operator queue
                            param_operators.Push(delimiter);

                            #endregion
                        }
                        else
                        {
                            #region Operand Queue
                            rpn_queue.DeleteOnSubmit(item); // if token is an operand function, then write it to output. 
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Stop:
                        #region Token_Operand_Function_Stop

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Queue


                            // pop the operators from the parameter stack util it's empty or we get a delimiter
                            do
                            {
                                // check if the parameters operator stack is empty
                                if (param_operators.Count == 0) break;

                                // pop an item from the stack
                                Parser.TokenItem opItem = param_operators.Pop();

                                // if the item is a delimiter...discard it and exit loop
                                if (opItem.TokenType == TokenType.Token_Operand_Function_Delimiter) break;

                                // add the item to the paramter queue
                                param_queue.DeleteOnSubmit(opItem);
                            }
                            while (true);

                            // add the square bracket to the queue
                            param_queue.DeleteOnSubmit(item);

                            // start removing items from the paraemters queue and add them to the RPN queue
                            // unill it's empty or we access an operand function start
                            do
                            {
                                // see if the parameter queue is empty
                                if (param_queue.Count == 0) break;

                                // get the item from the queue
                                Parser.TokenItem qItem = param_queue.Dequeue();

                                rpn_queue.DeleteOnSubmit(qItem);

                                // if the qItem is the end of an operand function, then we are done
                                if (qItem.TokenType == TokenType.Token_Operand_Function_Stop) break;
                            }
                            while (true);

                            #endregion
                        }
                        else
                        {
                            #region Operand Queue
                            rpn_queue.DeleteOnSubmit(item); // if token is an operand function, then write it to output. 
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Operator:
                        #region Token_Operator

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            if (param_operators.Count > 0)
                            {
                                // peek at the top item of the operator stack
                                do
                                {
                                    // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                                    if (item.OrderOfOperationPrecedence >= param_operators.Peek().OrderOfOperationPrecedence)
                                    {
                                        param_queue.DeleteOnSubmit(param_operators.Pop());
                                    }
                                    else
                                        break;


                                    if (param_operators.Count == 0) break;

                                } while (true);

                            }

                            // push the token to the operator stack
                            param_operators.Push(item);
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            if (operators.Count > 0)
                            {
                                // peek at the top item of the operator stack

                                do
                                {
                                    // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                                    if (item.OrderOfOperationPrecedence >= operators.Peek().OrderOfOperationPrecedence)
                                    {
                                        rpn_queue.DeleteOnSubmit(operators.Pop());
                                    }
                                    else
                                        break;


                                    if (operators.Count == 0) break;

                                } while (true);

                            }

                            // push the token to the operator stack
                            operators.Push(item);
                            #endregion
                        }

                        #endregion
                        break;

                    case TokenType.Token_Assignemt_Start:
                        #region Token_Assignemt_Start

                        if (operators.Count > 0)
                        {
                            // peek at the top item of the operator stack

                            do
                            {
                                // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                                if (item.OrderOfOperationPrecedence >= operators.Peek().OrderOfOperationPrecedence)
                                {
                                    rpn_queue.DeleteOnSubmit(operators.Pop());
                                }
                                else
                                    break;


                                if (operators.Count == 0) break;

                            } while (true);

                        }

                        // push the token to the operator stack
                        operators.Push(item);


                        #endregion
                        break;

                    case TokenType.Token_Assignment_Stop:
                        #region Token_Assignment_Stop

                        // start popping items from the operator stack until we find the assignemtn start
                        if (operators.Count > 0)
                        {
                            // peek at the top item of the operator stack

                            do
                            {
                                TokenItem t1 = operators.Pop();
                                rpn_queue.DeleteOnSubmit(t1);

                                if (t1.TokenType == TokenType.Token_Assignemt_Start)
                                {
                                    // we are all don popping operators
                                    break;
                                }

                                if (operators.Count == 0) break;

                            } while (true);

                        }

                        #endregion
                        break;
                }
            }

            // pop the remaining operators
            int opCount = operators.Count;
            for (int i = 0; i < opCount; i++) rpn_queue.DeleteOnSubmit(operators.Pop());
            */
            #endregion

            #region Version 2


            /*
            // create the output queue
            rpn_queue = new Support.ExQueue<TokenItem>(tokenItems.Count);

            // create the operator stack
            Support.ExStack<TokenItem> operators = new Support.ExStack<TokenItem>();

            // create the operator stack for operators
            Support.ExStack<TokenItem> param_operators = new Support.ExStack<TokenItem>();

            // create a temporary queue for expressions that are in operand functions
            Support.ExQueue<TokenItem> parameter_queue = new Support.ExQueue<TokenItem>();

            // local variables
            int count = 0;


            
            // loop through all the token items
            foreach (TokenItem item in tokenItems)
            {
                //System.Diagnostics.Debug.WriteLine(item.TokenName);

                switch (item.TokenType)
                {
                    case TokenType.Token_Close_Parenthesis:                        
                        #region Parenthesis Handling

                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            if (param_operators.Count > 0)
                            {
                                // start peeking at the top operator
                                do
                                {
                                    // see what is on top of the operator stack
                                    if (param_operators.Peek().TokenType == TokenType.Token_Open_Parenthesis)
                                    {
                                        // we found the matching open parenthesis...pop it and discard
                                        param_operators.Pop();
                                        break;
                                    }
                                    else
                                    {
                                        // pop the operator and push it on the queue
                                        parameter_queue.DeleteOnSubmit(param_operators.Pop());
                                    }

                                    if (param_operators.Count == 0) break;
                                }
                                while (true);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            if (operators.Count > 0)
                            {
                                // start peeking at the top operator
                                do
                                {
                                    // see what is on top of the operator stack
                                    if (operators.Peek().TokenType == TokenType.Token_Open_Parenthesis)
                                    {
                                        // we found the matching open parenthesis...pop it and discard
                                        operators.Pop();
                                        break;
                                    }
                                    else
                                    {
                                        // pop the operator and push it on the queue
                                        rpn_queue.DeleteOnSubmit(operators.Pop());
                                    }

                                    if (operators.Count == 0) break;
                                }
                                while (true);
                            }
                            #endregion
                        }
                        #endregion
                        break;

                    case TokenType.Token_Open_Parenthesis:
                        #region Parenthesis Handling
                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            // add open parenthesis to the operator stack
                            param_operators.Push(item);
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            // add open parenthesis to the operator stack
                            operators.Push(item);
                            #endregion
                        }
                        #endregion
                        break;

                    case TokenType.Token_Operand:
                        #region Operand Handling
                        if (item.InOperandFunction == true)
                        {
                            #region Operand Queue
                            parameter_queue.DeleteOnSubmit(item); // if token is an operand, then write it to output. 
                            #endregion
                        }
                        else
                        {
                            #region Operand Queue
                            rpn_queue.DeleteOnSubmit(item); // if token is an operand, then write it to output. 
                            #endregion
                        }
                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Start:
                        #region Operand Function Handling
                        if (item.InOperandFunction == true)
                        {
                            parameter_queue.DeleteOnSubmit(item);
                        }
                        else
                        {
                            rpn_queue.DeleteOnSubmit(item); // if token is an operand function, then write it to output. 
                        }
                        #endregion
                        break;

                    case TokenType.Token_Operand_Function_Stop:
                        #region Operand Function Handling
                        if (item.InOperandFunction == true)
                        {
                            // pop the operators from the operand function parameter
                            count = param_operators.Count;
                            for (int i = 0; i < count; i++) parameter_queue.DeleteOnSubmit(param_operators.Pop());

                            // add the items from the temporary parameter quue to the RPN queue
                            count = parameter_queue.Count;
                            for (int i = 0; i < count; i++) rpn_queue.DeleteOnSubmit(parameter_queue[i]);

                            // clear the parameter queue
                            parameter_queue.Clear();

                            // add the delimiter or stop operand function
                            rpn_queue.DeleteOnSubmit(item);

                        }
                        else
                        {
                            rpn_queue.DeleteOnSubmit(item); // if token is an operand function, then write it to output. 
                        }
                        #endregion
                        break;

                    case TokenType.Token_Operator:
                        #region Operator Handling
                        if (item.InOperandFunction == true)
                        {
                            #region Parameter Operator Stack
                            if (param_operators.Count > 0)
                            {
                                // peek at the top item of the operator stack
                                do
                                {
                                    // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                                    if (item.OrderOfOperationPrecedence >= param_operators.Peek().OrderOfOperationPrecedence)
                                    {
                                        parameter_queue.DeleteOnSubmit(param_operators.Pop());
                                    }
                                    else
                                        break;


                                    if (param_operators.Count == 0) break;

                                } while (true);

                            }

                            // push the token to the operator stack
                            param_operators.Push(item);
                            #endregion
                        }
                        else
                        {
                            #region Operator Stack
                            if (operators.Count > 0)
                            {
                                // peek at the top item of the operator stack

                                do
                                {
                                    // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                                    if (item.OrderOfOperationPrecedence >= operators.Peek().OrderOfOperationPrecedence)
                                    {
                                        rpn_queue.DeleteOnSubmit(operators.Pop());
                                    }
                                    else
                                        break;


                                    if (operators.Count == 0) break;

                                } while (true);

                            }

                            // push the token to the operator stack
                            operators.Push(item);
                            #endregion
                        }
                        #endregion
                        break;
                    
                    case TokenType.Token_Operand_Function_Delimiter:
                        #region Delimiter Handling
                        // pop the operators from the operand function parameter
                        count = param_operators.Count;
                        for (int i = 0; i < count; i++) parameter_queue.DeleteOnSubmit(param_operators.Pop());

                        // add the items from the temporary parameter quue to the RPN queue
                        count = parameter_queue.Count;
                        for (int i = 0; i < count; i++) rpn_queue.DeleteOnSubmit(parameter_queue[i]);

                        // clear the parameter queue
                        parameter_queue.Clear();

                        // add the delimiter or stop operand function
                        //rpn_queue.DeleteOnSubmit(item);

                        #endregion
                        break;
                }

                
            }


            // pop the operators
            int opCount = operators.Count;
            for (int i = 0; i < opCount; i++) rpn_queue.DeleteOnSubmit(operators.Pop());
            */

            #endregion

            #region Version 1
            /*
            // loop through all the token items
            foreach (TokenItem item in tokenItems)
            {

                if ((item.TokenType == TokenType.Token_Operand) && (item.InOperandFunction == false))
                {
                    // if token is an operand, then write it to output. 
                    rpn_queue.DeleteOnSubmit(item);
                }
                else if (item.TokenType == TokenType.Token_Open_Parenthesis)
                {
                    // add open parenthesis to the operator stack
                    operators.Push(item);
                }
                else if ((item.TokenType == TokenType.Token_Operator) && (item.InOperandFunction == false))
                {
                    #region Operator Handling
                    // peek at the top item of the operator stack
                    if (operators.Count > 0)
                    {
                        do
                        {
                            // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                            if (item.OrderOfOperationPrecedence >= operators.Peek().OrderOfOperationPrecedence)
                            {
                                rpn_queue.DeleteOnSubmit(operators.Pop());
                            }
                            else
                                break;


                            if (operators.Count == 0) break;

                        } while (true);

                    }

                    // push the token to the operator stack
                    operators.Push(item);
                    #endregion
                }
                else if (item.TokenType == TokenType.Token_Close_Parenthesis)
                {
                    #region Parenthesis Handling
                    if (operators.Count > 0)
                    {
                        do
                        {
                            // see when is on top of the operator stack
                            if (operators.Peek().TokenType == TokenType.Token_Open_Parenthesis)
                            {
                                // we found the matching open parenthesis...pop it and discard
                                operators.Pop();
                                break;
                            }
                            else
                            {
                                rpn_queue.DeleteOnSubmit(operators.Pop());
                            }

                            if (operators.Count == 0) break;
                        }
                        while (true);
                    }
                    #endregion
                }
                else if (item.TokenType == TokenType.Token_Operand_Function_Start)
                {
                    // push the operand function onto the output queue
                    rpn_queue.DeleteOnSubmit(item);
                }
                else if ((item.TokenType == TokenType.Token_Operand_Function_Stop) || (item.TokenType == TokenType.Token_Operand_Function_Delimiter))
                {
                    // pop the operators from the operand function parameter
                    int tempCount = param_operators.Count;
                    for (int i = 0; i < tempCount; i++) parameter_queue.DeleteOnSubmit(param_operators.Pop());

                    // add the items from the temporary parameter quue to the RPN queue
                    tempCount = parameter_queue.Count;
                    for (int i = 0; i < tempCount; i++) rpn_queue.DeleteOnSubmit(parameter_queue[i]);

                    // clear the parameter queue
                    parameter_queue.Clear();

                    // add the delimiter or stop operand function
                    rpn_queue.DeleteOnSubmit(item);

                }
                else if ((item.TokenType == TokenType.Token_Operand) && (item.InOperandFunction == true))
                {
                    // push the parameter on the temporary parameter queue
                    parameter_queue.DeleteOnSubmit(item);
                }
                else if ((item.TokenType == TokenType.Token_Operator) && (item.InOperandFunction == true))
                {
                    #region Operand Function Operator Handling
                    // peek at the top item of the operator stack
                    if (param_operators.Count > 0)
                    {
                        do
                        {
                            // see if the current tokens precedence is less than or equal to the tokenItem on the operator stack
                            if (item.OrderOfOperationPrecedence > param_operators.Peek().OrderOfOperationPrecedence)
                            {
                                parameter_queue.DeleteOnSubmit(param_operators.Pop());
                            }
                            else
                                break;


                            if (param_operators.Count == 0) break;

                        } while (true);

                    }

                    // push the token to the operand function operator stack
                    param_operators.Push(item);
                    #endregion
                }
            }
              
            // pop the operators
            int opCount = operators.Count;
            for (int i = 0; i < opCount; i++) rpn_queue.DeleteOnSubmit(operators.Pop());
             
            */
            #endregion

        }

        private bool Open(System.IO.FileInfo File, out string ErrorMsg)
        {
            // initialize the outgoing variable
            ErrorMsg = "";

            // create a text reader
            System.IO.TextReader tr = null;
            try
            {
                tr = new System.IO.StreamReader(File.FullName);
            }
            catch (Exception err)
            {
                ErrorMsg = "Error in Tokens.Open() while reading the file '" + File.FullName + "' : " + err.Message;
                return false;
            }

            // read the whole file
            string content = "";
            bool anyError = false;
            try
            {
                content = tr.ReadToEnd();
            }
            catch (Exception err)
            {
                anyError = true;
                ErrorMsg = "Error in Tokens.Open() while reading the file: " + err.Message;
            }
            finally
            {
                tr.Close();
            }
            if (anyError == true) return false;


            // split the filecontent by the new line
            string[] arrLines = content.Split("\n".ToCharArray());

            // create a collection to hold the variables and the values
            System.Collections.Generic.SortedList<string, string> vars = new SortedList<string, string>();

            // starting looping through the lines of the file
            System.Text.StringBuilder sb = new StringBuilder();
            bool continueLoop = true;
            bool foundVariables = false;

            for (int i = 0; i < arrLines.Length; i++)
            {
                switch (arrLines[i].Trim().ToLower())
                {
                    case ";variables;":
                        foundVariables = true;
                        break;

                    case ";tokens;":
                        // we are done
                        continueLoop = false;
                        break;

                    case ";rpn queue;":
                        // we are done
                        continueLoop = false;
                        break;

                    default:
                        if (foundVariables == false)
                        {
                            // we have part of the rule
                            sb.Append(arrLines[i]);
                            sb.Append("\n");
                        }
                        else
                        {
                            // we have a variable
                            string[] arrData = arrLines[i].Split("=".ToCharArray());

                            if (arrData.Length == 2)
                            {
                                string key = arrData[0].Trim().ToLower();
                                string data = arrData[1];

                                if (vars.ContainsKey(key) == false)
                                {
                                    vars.Add(key, data);
                                }
                            }

                        }
                        break;
                }

                if (continueLoop == false) break;
            }


            // set the rule syntax
            ruleSyntax = sb.ToString();

            // get the tokens
            GetTokens();


            if (AnyErrors == true)
            {
                ErrorMsg = lastErrorMessage;
                return false;
            }

            if (vars.Count > 0)
            {
                for (int i = 0; i < vars.Count; i++)
                {
                    string key = vars.Keys[i];
                    string data = vars[key];
                    data = data.Replace("\r", "");

                    if (this.variables.VariableExists(key) == true) variables[key].VariableValue = data;

                }
            }

            // all done
            return true;
        }

        #endregion

        #region Public Methods

        public bool Save(string Filename, out string ErrorMsg)
        {
            // initialize the outgoing variable
            ErrorMsg = "";

            if (String.IsNullOrEmpty(ruleSyntax) == true)
            {
                ErrorMsg = "Error in Tokens.Save(): No rule to save";
                return false;
            }

            // create a text writer object
            System.IO.TextWriter tw = null;
            try
            {
                tw = new System.IO.StreamWriter(Filename, false);
            }
            catch (Exception err)
            {
                ErrorMsg = "Error in Tokens.Save() while saving the rule: " + err.Message;
                return false;
            }

            // save the rule syntax
            try
            {
                tw.WriteLine(this.ruleSyntax);
            }
            catch (Exception err)
            {
                tw.Close();
                ErrorMsg = "Error in Tokens.Save() while saving the rule syntax: " + err.Message;
                return false;
            }

            // write out the variables and values
            try
            {
                tw.WriteLine("");
                tw.WriteLine(";Variables;");
                if (this.variables != null)
                {

                    foreach (Parser.Variable var in this.variables)
                    {
                        tw.Write(var.VariableName);
                        tw.Write("=");
                        if (var.VariableValue != null)
                            tw.WriteLine(var.VariableValue);
                        else
                            tw.WriteLine("null");
                    }
                }
            }
            catch (Exception err)
            {
                tw.Close();
                ErrorMsg = "Error in Tokens.Save() while saving the varaibles: " + err.Message;
                return false;
            }


            // write out the tokens
            try
            {
                tw.WriteLine("");
                tw.WriteLine(";Tokens;");
                if (this.tokenItems != null)
                {
                    foreach (Parser.TokenItem item in this.tokenItems)
                    {
                        tw.Write(item.TokenName);
                        tw.Write(",");
                        tw.Write(item.TokenType.ToString());
                        tw.Write(",");
                        tw.Write(item.TokenDataType.ToString());                        
                        tw.Write(",");
                        tw.WriteLine(item.InOperandFunction.ToString());
                    }
                }
            }
            catch (Exception err)
            {
                tw.Close();
                ErrorMsg = "Error in Tokens.Save() while saving the tokens: " + err.Message;
                return false;
            }


            // write out the RPN queue
            try
            {
                tw.WriteLine("");
                tw.WriteLine(";RPN Queue;");
                if (this.rpn_queue != null)
                {
                    foreach (Parser.TokenItem item in this.rpn_queue)
                    {
                        tw.Write(item.TokenName);
                        tw.Write(",");
                        tw.Write(item.TokenType.ToString());
                        tw.Write(",");
                        tw.Write(item.TokenDataType.ToString());
                        tw.Write(",");
                        tw.WriteLine(item.InOperandFunction.ToString());
                    }
                }
            }
            catch (Exception err)
            {
                tw.Close();
                ErrorMsg = "Error in Tokens.Save() while saving the tokens: " + err.Message;
                return false;
            }

            // all done
            tw.Close();
            return true;
        }

        #endregion

    }
}
