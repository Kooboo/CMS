#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

/*
 *http://www.codeproject.com/KB/recipes/aspx
 **/
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Dynamic.Calculator.Evaluate
{

    /// <summary>
    /// 
    /// </summary>
    public class Evaluator
    {
        #region Local Variables

        private Parser.Token token;

        private double tokenEvalTime = 0;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluator" /> class.
        /// </summary>
        /// <param name="Tokens">The tokens.</param>
        public Evaluator(Parser.Token Tokens)
        {
            token = Tokens;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the token eval time.
        /// </summary>
        /// <value>
        /// The token eval time.
        /// </value>
        public double TokenEvalTime
        {
            get
            {
                return tokenEvalTime;
            }
        }

        #endregion

        #region Public Methods

        #region Old Versions

        /*
        public bool Evaluate_Old_Version1(out string sValue, out string ErrorMsg)
        {
            // initialize the outgoing variable
            ErrorMsg = "";
            sValue = "";

            // reset the results in the token
            token.LastEvaluationResult = "";

            // create a stop watch to time the evaluation
            System.Diagnostics.Stopwatch evalTime = System.Diagnostics.Stopwatch.StartNew();        

            // make sure the otkens are valid
            if (token.AnyErrors == true)
            {
                // the token already has an error, return the token error as the evaluator error message
                ErrorMsg = token.LastErrorMessage;
                return false;
            }


            // create the evaluation stack
            Support.ExStack<Parser.TokenItem> eval = new Support.ExStack<Parser.TokenItem>(token.TokenItems.Count);

            // start looping through the tokens
            int count = token.RPNQueue.Count;            

            for (int i=0; i<count; i++)
            {
                // get the next token item
                Parser.TokenItem item = token.RPNQueue[i];

                System.Diagnostics.Debug.WriteLine(item.TokenName);

                if (item.TokenDataType == Parser.TokenDataType.Token_DataType_Variable)
                {
                    #region Token_DataType_Variable

                    // lookup the value of the variable and push it onto the evaluation stack
                    if (token.Variables.VariableExists(item.TokenName) == true)
                    {
                        // the variable exists, push it on the stack
                        eval.Push(new Parser.TokenItem(token.Variables[item.TokenName].VariableValue, Parser.TokenType.Token_Operand, item.InOperandFunction));
                    }
                    else
                    {
                        // the variable does not exist...push an empty string on the stack
                        eval.Push(new Parser.TokenItem("", Parser.TokenType.Token_Operand, item.InOperandFunction));
                    }

                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Operator)
                {
                    #region Token_Operator

                    // pop 2 items off the stack and perform the operation
                    // push the result back onto the evaluation stack

                    Parser.TokenItem rightOperand = null;
                    Parser.TokenItem leftOperand = null;
                    try
                    {
                        if (eval.Count > 0) rightOperand = eval.Pop();
                        if (eval.Count > 0) leftOperand = eval.Pop();
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Error in Evaluator.Evaluate() while popping 2 tokens for an operator: " + err.Message;
                        return false;
                    }

                    // double check that we got the tokens before we evaluate
                    if (rightOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The right operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }
                    if (leftOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The left operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }

                    // process the operator
                    try
                    {
                        Parser.TokenItem result = null;
                        if (EvaluateTokens(leftOperand, rightOperand, item, out result, out ErrorMsg) == false)
                            return false;
                        else
                        {
                            // double check that we got a result
                            if (result == null)
                            {
                                ErrorMsg = "Failed to evaluate the rule expression: The result of an operator is null: There may be an issue with the rule syntax.";
                                return false;
                            }
                            else
                            {
                                eval.Push(result);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The result of an operator threw an error: " + err.Message;
                        return false;
                    }


                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Operand_Function_Stop)
                {
                    #region Token_Operand_Function_Stop

                    // find the start of the function by popping off items
                    // evaluate the function and push the result back onto the evaluation stack

                    // start popping items from the evaluation stack
                    // until we get the start of the of the operand function
                    int evalCount = eval.Count;
                    Parser.TokenItems parameters = new Parser.TokenItems();

                    try
                    {
                        for (int j = 0; j < evalCount; j++)
                        {
                            Parser.TokenItem opItem = eval.Pop();

                            if (opItem.TokenType == Parser.TokenType.Token_Operand_Function_Start)
                            {
                                // we found the start of the operand function; let's evaluate it

                                Parser.TokenItem result = null;
                                if (EvaluateOperandFunction(opItem, parameters, out result, out ErrorMsg) == false)
                                    return false;
                                else
                                {
                                    // make sure we got a result
                                    if (result == null)
                                    {
                                        ErrorMsg = "Failed to evaluate the rule expression: The result of an operand function is null: There may be an issue with the rule syntax.";
                                        return false;
                                    }
                                    else
                                        eval.Push(result);
                                }
                                break;
                            }
                            else if (opItem.TokenType != Parser.TokenType.Token_Operand_Function_Delimiter)
                            {
                                // we have a parameter to the operand function
                                parameters.AddToFront(opItem);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The evaluation of an operand function threw an error: " + err.Message;
                        return false;
                    }

                    #endregion
                }
                else
                {
                    // push the item on the evaluation stack
                    eval.Push(item);
                }
            }


            if (eval.Count == 1)
            {
                // just 1 item on the stack; should be our answer
                try
                {
                    Parser.TokenItem final = eval.Pop();
                    sValue = final.TokenName;

                    // set the results in the token
                    token.LastEvaluationResult = sValue;
                }
                catch (Exception err)
                {
                    ErrorMsg = "Failed to evaluate the rule expression after all the tokens have been considered: " + err.Message;
                    return false;
                }
            }
            else
            {
                ErrorMsg = "Invalid Rule Syntax";
                return false;
            }

            // stop the timer
            evalTime.Stop();
            
            tokenEvalTime = evalTime.Elapsed.TotalMilliseconds;
            token.LastEvaluationTime = tokenEvalTime; // set this evaluation time in the token object.
            return true;

        }
        */

        #endregion


        /// <summary>
        /// This new evaluate function includes support to assignment and short circuit of the IIF[] operand function
        /// </summary>
        /// <param name="RPNQueue"></param>
        /// <param name="sValue"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool Evaluate(Support.ExQueue<Parser.TokenItem> RPNQueue, out string sValue, out string ErrorMsg)
        {
            // initialize the outgoing variable
            ErrorMsg = "";
            sValue = "";

            // reset the results in the token
            token.LastEvaluationResult = "";

            // create a stop watch to time the evaluation
            System.Diagnostics.Stopwatch evalTime = System.Diagnostics.Stopwatch.StartNew();

            // make sure the otkens are valid
            if (token.AnyErrors == true)
            {
                // the token already has an error, return the token error as the evaluator error message
                ErrorMsg = token.LastErrorMessage;
                return false;
            }


            // create the evaluation stack
            Support.ExStack<Parser.TokenItem> eval = new Support.ExStack<Parser.TokenItem>(token.TokenItems.Count);

            // start looping through the tokens
            int count = RPNQueue.Count;
            int index = 0;  // the index of the curent token item in the rpn queue


            while (index < count) 
            {
                // get the next token item
                Parser.TokenItem item = RPNQueue[index];
                index++;

                System.Diagnostics.Debug.WriteLine(item.TokenName);

                if (item.TokenDataType == Parser.TokenDataType.Token_DataType_Variable)
                {
                    #region Token_DataType_Variable

                    // determine if we need to assign the variable represented by the token
                    // or the rule syntax is doing the assignment
                    if (item.WillBeAssigned == false)
                    {
                        // The rule syntax is not doing the assignment, we are doing it.
                        // lookup the value of the variable and push it onto the evaluation stack
                        if (token.Variables.VariableExists(item.TokenName) == true)
                        {
                            // the variable exists, push it on the stack
                            eval.Push(new Parser.TokenItem(token.Variables[item.TokenName].VariableValue, Parser.TokenType.Token_Operand, item.InOperandFunction));
                        }
                        else
                        {
                            // the variable does not exist...push an empty string on the stack
                            eval.Push(new Parser.TokenItem("", Parser.TokenType.Token_Operand, item.InOperandFunction));
                        }
                    }
                    else
                    {
                        // the rule syntax is doing the assignment, add the token item to the evaluation stack
                        eval.Push(item);
                    }

                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Operator)
                {
                    #region Token_Operator

                    // pop 2 items off the stack and perform the operation
                    // push the result back onto the evaluation stack

                    Parser.TokenItem rightOperand = null;
                    Parser.TokenItem leftOperand = null;
                    try
                    {
                        if (eval.Count > 0) rightOperand = eval.Pop();
                        if (eval.Count > 0) leftOperand = eval.Pop();
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Error in Evaluator.Evaluate() while popping 2 tokens for an operator: " + err.Message;
                        return false;
                    }

                    // double check that we got the tokens before we evaluate
                    if (rightOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The right operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }
                    if (leftOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The left operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }

                    // process the operator
                    try
                    {
                        Parser.TokenItem result = null;
                        if (EvaluateTokens(leftOperand, rightOperand, item, out result, out ErrorMsg) == false)
                            return false;
                        else
                        {
                            // double check that we got a result
                            if (result == null)
                            {
                                ErrorMsg = "Failed to evaluate the rule expression: The result of an operator is null: There may be an issue with the rule syntax.";
                                return false;
                            }
                            else
                            {
                                eval.Push(result);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The result of an operator threw an error: " + err.Message;
                        return false;
                    }


                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Operand_Function_Stop)
                {
                    #region Token_Operand_Function_Stop

                    // find the start of the function by popping off items
                    // evaluate the function and push the result back onto the evaluation stack

                    // start popping items from the evaluation stack
                    // until we get the start of the of the operand function
                    int evalCount = eval.Count;
                    Parser.TokenItems parameters = new Parser.TokenItems(token);

                    try
                    {
                        for (int j = 0; j < evalCount; j++)
                        {
                            Parser.TokenItem opItem = eval.Pop();

                            if (opItem.TokenType == Parser.TokenType.Token_Operand_Function_Start)
                            {
                                // we found the start of the operand function; let's evaluate it

                                Parser.TokenItem result = null;
                                if (EvaluateOperandFunction(opItem, parameters, out result, out ErrorMsg) == false)
                                    return false;
                                else
                                {
                                    // make sure we got a result
                                    if (result == null)
                                    {
                                        ErrorMsg = "Failed to evaluate the rule expression: The result of an operand function is null: There may be an issue with the rule syntax.";
                                        return false;
                                    }
                                    else
                                        eval.Push(result);
                                }
                                break;
                            }
                            else if (opItem.TokenType != Parser.TokenType.Token_Operand_Function_Delimiter)
                            {
                                // we have a parameter to the operand function
                                parameters.AddToFront(opItem);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The evaluation of an operand function threw an error: " + err.Message;
                        return false;
                    }

                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Assignemt_Start)
                {
                    #region Token_Assignment_Start

                    // assign the value to the variable

                    // pop 2 items off the stack - save the value into the variable

                    Parser.TokenItem rightOperand = null;
                    Parser.TokenItem leftOperand = null;
                    try
                    {
                        if (eval.Count > 0) rightOperand = eval.Pop();
                        if (eval.Count > 0) leftOperand = eval.Pop();
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Error in Evaluator.Evaluate() while popping 2 tokens for an operator: " + err.Message;
                        return false;
                    }

                    // double check that we got the tokens before we evaluate
                    if (rightOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The right operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }
                    if (leftOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The left operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }

                    // look for the variable and assign the value to it
                    if (token.Variables.VariableExists(leftOperand.TokenName) == true)
                    {
                        // the variable exists, push it on the stack
                        token.Variables[leftOperand.TokenName].VariableValue = rightOperand.TokenName;
                    }
                    else
                    {
                        // failed to find the variable....this is an error
                        ErrorMsg = "Failed to evaluate the rule expression: Failed to find the variable '" + leftOperand.TokenName + "' for the assignment.";
                        return false;
                    }




                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Operand_Function_Start)
                {
                    #region New Short Circuit Code
                    
                    // we are only short circuiting the IIF[] operand function
                    if (item.TokenName.Trim().ToLower() != "iif[")
                    {
                        // add the token to the evaluation stack
                        eval.Push(item);
                    }
                    else
                    {
                        // we found the iif statement.

                        // see if the iff[] operand function allows for short circuiting
                        if (item.CanShortCircuit == false)
                        {
                            // no short circuiting, add it to the evaluation stack
                            eval.Push(item);
                        }
                        else
                        {
                            ////////////////////////////////////////////////
                            // We can short circuit this iif[] statement  //
                            ////////////////////////////////////////////////

                            Parser.TokenItem result = item.ShortCircuit.Evaluate(out ErrorMsg);

                            if (result == null)
                            {
                                // there was an error doing the short circuit
                                return false;
                            }
                            else
                            {
                                // we successfully did the short circuit
                                eval.Push(result);

                                // increment the index so we skip the ] which should be the next token
                                index++;
                            }

                        }
                    }

                    #endregion
                }
                else
                {
                    // push the item on the evaluation stack
                    eval.Push(item);
                }
            }

            if (eval.Count == 1)
            {
                // just 1 item on the stack; should be our answer
                try
                {
                    Parser.TokenItem final = eval.Pop();
                    sValue = final.TokenName;

                    // set the results in the token
                    token.LastEvaluationResult = sValue;
                }
                catch (Exception err)
                {
                    ErrorMsg = "Failed to evaluate the rule expression after all the tokens have been considered: " + err.Message;
                    return false;
                }
            }
            else if (eval.Count == 0)
            {
                // there is no result in the evaluation stack because it my have been assigned
                // do nothing here
            }
            else
            {
                ErrorMsg = "Invalid Rule Syntax";
                return false;
            }


            // stop the timer
            evalTime.Stop();

            tokenEvalTime = evalTime.Elapsed.TotalMilliseconds;
            token.LastEvaluationTime = tokenEvalTime; // set this evaluation time in the token object.
            return true;

        }

        /// <summary>
        /// This new evaluate function includes support to assignment and short circuit of the IIF[] operand function
        /// </summary>
        /// <param name="sValue"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool Evaluate(out string sValue, out string ErrorMsg)
        {
            return Evaluate(token.RPNQueue, out sValue, out ErrorMsg);

            #region Old Code
            /*
            // initialize the outgoing variable
            ErrorMsg = "";
            sValue = "";

            // reset the results in the token
            token.LastEvaluationResult = "";

            // create a stop watch to time the evaluation
            System.Diagnostics.Stopwatch evalTime = System.Diagnostics.Stopwatch.StartNew();

            // make sure the otkens are valid
            if (token.AnyErrors == true)
            {
                // the token already has an error, return the token error as the evaluator error message
                ErrorMsg = token.LastErrorMessage;
                return false;
            }

            
            // create the evaluation stack
            Support.ExStack<Parser.TokenItem> eval = new Support.ExStack<Parser.TokenItem>(token.TokenItems.Count);

            // start looping through the tokens
            int count = token.RPNQueue.Count;

            
            for (int i = 0; i < count; i++)
            {                             
                // get the next token item
                Parser.TokenItem item = token.RPNQueue[i];

                System.Diagnostics.Debug.WriteLine(item.TokenName);

                if (item.TokenDataType == Parser.TokenDataType.Token_DataType_Variable)
                {
                    #region Token_DataType_Variable

                    // determine if we need to assign the variable represented by the token
                    // or the rule syntax is doing the assignment
                    if (item.WillBeAssigned == false)
                    {
                        // The rule syntax is not doing the assignment, we are doing it.
                        // lookup the value of the variable and push it onto the evaluation stack
                        if (token.Variables.VariableExists(item.TokenName) == true)
                        {
                            // the variable exists, push it on the stack
                            eval.Push(new Parser.TokenItem(token.Variables[item.TokenName].VariableValue, Parser.TokenType.Token_Operand, item.InOperandFunction));
                        }
                        else
                        {
                            // the variable does not exist...push an empty string on the stack
                            eval.Push(new Parser.TokenItem("", Parser.TokenType.Token_Operand, item.InOperandFunction));
                        }
                    }
                    else
                    {
                        // the rule syntax is doing the assignment, add the token item to the evaluation stack
                        eval.Push(item);
                    }

                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Operator)
                {
                    #region Token_Operator

                    // pop 2 items off the stack and perform the operation
                    // push the result back onto the evaluation stack

                    Parser.TokenItem rightOperand = null;
                    Parser.TokenItem leftOperand = null;
                    try
                    {
                        if (eval.Count > 0) rightOperand = eval.Pop();
                        if (eval.Count > 0) leftOperand = eval.Pop();
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Error in Evaluator.Evaluate() while popping 2 tokens for an operator: " + err.Message;
                        return false;
                    }

                    // double check that we got the tokens before we evaluate
                    if (rightOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The right operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }
                    if (leftOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The left operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }

                    // process the operator
                    try
                    {
                        Parser.TokenItem result = null;
                        if (EvaluateTokens(leftOperand, rightOperand, item, out result, out ErrorMsg) == false)
                            return false;
                        else
                        {
                            // double check that we got a result
                            if (result == null)
                            {
                                ErrorMsg = "Failed to evaluate the rule expression: The result of an operator is null: There may be an issue with the rule syntax.";
                                return false;
                            }
                            else
                            {
                                eval.Push(result);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The result of an operator threw an error: " + err.Message;
                        return false;
                    }


                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Operand_Function_Stop)
                {
                    #region Token_Operand_Function_Stop

                    // find the start of the function by popping off items
                    // evaluate the function and push the result back onto the evaluation stack

                    // start popping items from the evaluation stack
                    // until we get the start of the of the operand function
                    int evalCount = eval.Count;
                    Parser.TokenItems parameters = new Parser.TokenItems();

                    try
                    {
                        for (int j = 0; j < evalCount; j++)
                        {
                            Parser.TokenItem opItem = eval.Pop();

                            if (opItem.TokenType == Parser.TokenType.Token_Operand_Function_Start)
                            {
                                // we found the start of the operand function; let's evaluate it

                                Parser.TokenItem result = null;
                                if (EvaluateOperandFunction(opItem, parameters, out result, out ErrorMsg) == false)
                                    return false;
                                else
                                {
                                    // make sure we got a result
                                    if (result == null)
                                    {
                                        ErrorMsg = "Failed to evaluate the rule expression: The result of an operand function is null: There may be an issue with the rule syntax.";
                                        return false;
                                    }
                                    else
                                        eval.Push(result);
                                }
                                break;
                            }
                            else if (opItem.TokenType != Parser.TokenType.Token_Operand_Function_Delimiter)
                            {
                                // we have a parameter to the operand function
                                parameters.AddToFront(opItem);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The evaluation of an operand function threw an error: " + err.Message;
                        return false;
                    }

                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Assignemt_Start)
                {
                    #region Token_Assignment_Start
                    
                    // assign the value to the variable

                    // pop 2 items off the stack - save the value into the variable

                    Parser.TokenItem rightOperand = null;
                    Parser.TokenItem leftOperand = null;
                    try
                    {
                        if (eval.Count > 0) rightOperand = eval.Pop();
                        if (eval.Count > 0) leftOperand = eval.Pop();
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Error in Evaluator.Evaluate() while popping 2 tokens for an operator: " + err.Message;
                        return false;
                    }

                    // double check that we got the tokens before we evaluate
                    if (rightOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The right operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }
                    if (leftOperand == null)
                    {
                        ErrorMsg = "Failed to evaluate the rule expression: The left operand token is null: There may be an issue with the rule syntax.";
                        return false;
                    }

                    // look for the variable and assign the value to it
                    if (token.Variables.VariableExists(leftOperand.TokenName) == true)
                    {
                        // the variable exists, push it on the stack
                        token.Variables[leftOperand.TokenName].VariableValue = rightOperand.TokenName;
                    }
                    else
                    {
                        // failed to find the variable....this is an error
                        ErrorMsg = "Failed to evaluate the rule expression: Failed to find the variable '" + leftOperand.TokenName + "' for the assignment.";
                        return false;
                    }




                    #endregion
                }
                else if (item.TokenType == Parser.TokenType.Token_Operand_Function_Start)
                {
                    
                    #region New Short Circuit Code

                    // we are only short circuiting the IIF[] operand function
                    if (item.TokenName.Trim().ToLower() != "iif[")
                    {
                        // add the token to the evaluation stack
                        eval.Push(item);
                    }
                    else
                    {
                        // we found the iif statement.

                        // see if the iff[] operand function allows for short circuiting
                        if (item.CanShortCircuit == false)
                        {
                            // no short circuiting, add it to the evaluation stack
                            eval.Push(item);
                        }
                        else
                        {
                            ////////////////////////////////////////////////
                            // We can short circuit this iif[] statement  //
                            ////////////////////////////////////////////////

                            eval.Push(item);

                        }
                    }

                    #endregion
                                        
                    
                }
                else
                {
                    // push the item on the evaluation stack
                    eval.Push(item);
                }
            }

            if (eval.Count == 1)
            {
                // just 1 item on the stack; should be our answer
                try
                {
                    Parser.TokenItem final = eval.Pop();
                    sValue = final.TokenName;

                    // set the results in the token
                    token.LastEvaluationResult = sValue;
                }
                catch (Exception err)
                {
                    ErrorMsg = "Failed to evaluate the rule expression after all the tokens have been considered: " + err.Message;
                    return false;
                }
            }
            else if (eval.Count == 0)
            {
                // there is no result in the evaluation stack because it my have been assigned
                // do nothing here
            }
            else
            {
                ErrorMsg = "Invalid Rule Syntax";
                return false;
            }
            

            // stop the timer
            evalTime.Stop();

            tokenEvalTime = evalTime.Elapsed.TotalMilliseconds;
            token.LastEvaluationTime = tokenEvalTime; // set this evaluation time in the token object.
            return true;
            */
            #endregion
        }

        #endregion

        #region Private Methods

        private bool EvaluateTokens(Parser.TokenItem LeftOperand, Parser.TokenItem RightOperand, Parser.TokenItem Operator, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // intitialize the outgoing variables
            Result = null;
            ErrorMsg = "";

            // local variables
            double dResult = 0;
            bool boolResult = false;

            // validate the parameters
            if (LeftOperand == null)
            {
                ErrorMsg = "Failed to evaluate the operator: The left token is null.";
                return false;
            }
            
            if (RightOperand == null)
            {
                ErrorMsg = "Failed to evaluate the operator: The right token is null.";
                return false;
            }
            
            if (Operator == null)
            {
                ErrorMsg = "Failed to evaluate the operator: The operator token is null.";
                return false;
            }


            switch (Operator.TokenName.Trim().ToLower())
            {
                case "^":
                    #region Exponents

                    // Exponents require that both operands can be converted to doubles
                    try
                    {
                        if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            dResult = System.Math.Pow(LeftOperand.TokenName_Double, RightOperand.TokenName_Double);
                            Result = new Parser.TokenItem(dResult.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values for exponents.";
                            return false;
                        }                        
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Exponent operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "*":
                    #region Multiplication

                    //  multiplication expects that the operands can be converted to doubles
                    try
                    {
                        if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            dResult = LeftOperand.TokenName_Double * RightOperand.TokenName_Double;
                            Result = new Parser.TokenItem(dResult.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to multiply.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Multiplication operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "/":
                    #region Division

                    // divison requires that both operators can be converted to doubles and the denominator is not 0

                    try
                    {
                        if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            double denominator = RightOperand.TokenName_Double;

                            if (denominator != 0)
                            {
                                dResult = LeftOperand.TokenName_Double / denominator;
                                Result = new Parser.TokenItem(dResult.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                            }
                            else
                            {
                                ErrorMsg = "Syntax Error: Division by zero.";
                                return false;
                            }
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to divide.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Division operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "%":
                    #region Modulus
                    try
                    {
                        // modulus expects that both operators are numeric and the right operand is not zero
                        if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            double denominator = RightOperand.TokenName_Double;

                            if (denominator != 0)
                            {

                                dResult = LeftOperand.TokenName_Double % RightOperand.TokenName_Double;
                                Result = new Parser.TokenItem(dResult.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                            }
                            else
                            {
                                ErrorMsg = "Syntax Error: Modulus by zero.";
                                return false;
                            }
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to modulus.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Modulus operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "+":
                    #region Addition

                    try
                    {
                        // addition only works on numeric operands
                        if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            dResult = LeftOperand.TokenName_Double + RightOperand.TokenName_Double;
                            Result = new Parser.TokenItem(dResult.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to add.";
                            return false;
                        }
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Addition operator: " + err.Message;
                        return false;
                    }

                    break;
                    #endregion

                case "-":
                    #region Subtraction
                    try
                    {
                        // subtraction only works on numeric operands
                        if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                        {
                            dResult = LeftOperand.TokenName_Double - RightOperand.TokenName_Double;
                            Result = new Parser.TokenItem(dResult.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, LeftOperand.InOperandFunction);
                        }
                        else
                        {
                            ErrorMsg = "Syntax Error: Expecting numeric values to subtract.";
                            return false;
                        }                        
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the Subtraction operator: " + err.Message;
                        return false;
                    }
                    break;
                    #endregion

                case "<":
                    #region Less Than
                    if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a numeric comparison
                            boolResult = (LeftOperand.TokenName_Double < RightOperand.TokenName_Double);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Less Than operator on double operands: " + err.Message;
                            return false;
                        }
                    }
                    else if ((Support.DataTypeCheck.IsDate(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDate(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a date comparison
                            TimeSpan ts = LeftOperand.TokenName_DateTime.Subtract(RightOperand.TokenName_DateTime);
                            boolResult = (ts.TotalDays < 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Less Than operator on date operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            // do a string comparison
                            string lText = Support.DataTypeCheck.RemoveTextQuotes(LeftOperand.TokenName);
                            string rText = Support.DataTypeCheck.RemoveTextQuotes(RightOperand.TokenName);

                            boolResult = (lText.CompareTo(rText) < 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Less Than operator on string operands: " + err.Message;
                            return false;
                        }
                    }
                   
                    break;
                    #endregion

                case "<=":
                    #region Less Than Equal To

                    if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a numeric comparison
                            boolResult = (LeftOperand.TokenName_Double <= RightOperand.TokenName_Double);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Less Than or Equal to operator on double operands: " + err.Message;
                            return false;
                        }
                    }
                    else if ((Support.DataTypeCheck.IsDate(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDate(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a date comparison
                            TimeSpan ts = LeftOperand.TokenName_DateTime.Subtract(RightOperand.TokenName_DateTime);
                            boolResult = (ts.TotalDays <= 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Less Than or Equal to operator on date operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            // do a string comparison
                            string lText = Support.DataTypeCheck.RemoveTextQuotes(LeftOperand.TokenName);
                            string rText = Support.DataTypeCheck.RemoveTextQuotes(RightOperand.TokenName);

                            boolResult = (lText.CompareTo(rText) <= 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Less Than or Equal to operator on string operands: " + err.Message;
                            return false;
                        }
                    }                 
                    break;
                    #endregion

                case ">":
                    #region Greater than

                    if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a numeric comparison
                            boolResult = (LeftOperand.TokenName_Double > RightOperand.TokenName_Double);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Greater Than to operator on double operands: " + err.Message;
                            return false;
                        }
                    }
                    else if ((Support.DataTypeCheck.IsDate(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDate(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a date comparison
                            TimeSpan ts = LeftOperand.TokenName_DateTime.Subtract(RightOperand.TokenName_DateTime);
                            boolResult = (ts.TotalDays > 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Greater Than to operator on date operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            // do a string comparison
                            string lText = Support.DataTypeCheck.RemoveTextQuotes(LeftOperand.TokenName);
                            string rText = Support.DataTypeCheck.RemoveTextQuotes(RightOperand.TokenName);

                            boolResult = (lText.CompareTo(rText) > 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Greater Than to operator on string operands: " + err.Message;
                            return false;
                        }
                    }
                    break;
                    #endregion

                case ">=":
                    #region Greater than equal to
                    if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a numeric comparison
                            boolResult = (LeftOperand.TokenName_Double >= RightOperand.TokenName_Double);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Greater Than or Equal to operator on double operands: " + err.Message;
                            return false;
                        }
                    }
                    else if ((Support.DataTypeCheck.IsDate(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDate(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a date comparison
                            TimeSpan ts = LeftOperand.TokenName_DateTime.Subtract(RightOperand.TokenName_DateTime);
                            boolResult = (ts.TotalDays >= 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Greater Than or Equal to operator on date operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            // do a string comparison
                            string lText = Support.DataTypeCheck.RemoveTextQuotes(LeftOperand.TokenName);
                            string rText = Support.DataTypeCheck.RemoveTextQuotes(RightOperand.TokenName);

                            boolResult = (lText.CompareTo(rText) >= 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Greater Than or Equal to operator on string operands: " + err.Message;
                            return false;
                        }
                    }
                    break;
                    #endregion

                case "<>":
                    #region Not equal to
                    if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a numeric comparison
                            boolResult = (LeftOperand.TokenName_Double != RightOperand.TokenName_Double);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Not Equal To operator on double operands: " + err.Message;
                            return false;
                        }
                    }
                    else if ((Support.DataTypeCheck.IsDate(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDate(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a date comparison
                            TimeSpan ts = LeftOperand.TokenName_DateTime.Subtract(RightOperand.TokenName_DateTime);
                            boolResult = (ts.TotalDays != 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Not Equal To operator on date operands: " + err.Message;
                            return false;
                        }
                    }
                    else if ((Support.DataTypeCheck.IsBoolean(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsBoolean(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            boolResult = (LeftOperand.TokenName_Boolean != RightOperand.TokenName_Boolean);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Not Equal To operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            // do a string comparison
                            string lText = Support.DataTypeCheck.RemoveTextQuotes(LeftOperand.TokenName);
                            string rText = Support.DataTypeCheck.RemoveTextQuotes(RightOperand.TokenName);

                            boolResult = (lText.Equals(rText) == false);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Not Equal To operator on string operands: " + err.Message;
                            return false;
                        }
                    }
                    break;
                    #endregion

                case "=":
                    #region Equal to

                    if ((Support.DataTypeCheck.IsDouble(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDouble(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a numeric comparison
                            boolResult = (LeftOperand.TokenName_Double == RightOperand.TokenName_Double);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Equal To operator on double operands: " + err.Message;
                            return false;
                        }
                    }
                    else if ((Support.DataTypeCheck.IsDate(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsDate(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            // do a date comparison
                            TimeSpan ts = LeftOperand.TokenName_DateTime.Subtract(RightOperand.TokenName_DateTime);
                            boolResult = (ts.TotalDays == 0);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Equal To operator on date operands: " + err.Message;
                            return false;
                        }
                    }
                    else if ((Support.DataTypeCheck.IsBoolean(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsBoolean(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            boolResult = (LeftOperand.TokenName_Boolean == RightOperand.TokenName_Boolean);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Equal To operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            // do a string comparison
                            string lText = Support.DataTypeCheck.RemoveTextQuotes(LeftOperand.TokenName);
                            string rText = Support.DataTypeCheck.RemoveTextQuotes(RightOperand.TokenName);

                            boolResult = lText.Equals(rText);
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the Equal To operator on stirng operands: " + err.Message;
                            return false;
                        }
                    }
                    break;
                    #endregion

                case "and":
                    #region and

                    // the and operator must be performed on boolean operators
                    if ((Support.DataTypeCheck.IsBoolean(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsBoolean(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            boolResult = LeftOperand.TokenName_Boolean && RightOperand.TokenName_Boolean;
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the AND operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        ErrorMsg = "Syntax Error: Expecting boolean operands to AND.";
                        return false;
                    }

                    break;
                    
                    #endregion

                case "or":
                    #region or

                    if ((Support.DataTypeCheck.IsBoolean(LeftOperand.TokenName) == true) && (Support.DataTypeCheck.IsBoolean(RightOperand.TokenName) == true))
                    {
                        try
                        {
                            boolResult = LeftOperand.TokenName_Boolean || RightOperand.TokenName_Boolean;
                            Result = new Parser.TokenItem(boolResult.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, LeftOperand.InOperandFunction);
                        }
                        catch (Exception err)
                        {
                            ErrorMsg = "Failed to evaluate the OR operator on boolean operands: " + err.Message;
                            return false;
                        }
                    }
                    else
                    {
                        ErrorMsg = "Syntax Error: Expecting boolean operands to OR.";
                        return false;
                    }

                    break;

                    #endregion
            
                default:
                    #region Unknown Operator

                    ErrorMsg = "Failed to evaluate the operator: The operator token is null.";
                    return false;

                    #endregion
                    //break;
            }

            if (Result == null)
            {
                ErrorMsg = "Syntax Error: Failed to evaluate the expression.";
                return false;
            }
            else
                return true;
        }

        private bool EvaluateOperandFunction(Parser.TokenItem OperandFunction, Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // intitialize the outgoing variables
            Result = null;  // assume a failure by setting the result to null
            ErrorMsg = "";

            // local variables
            bool success = true;

            // validate the parameters
            if (OperandFunction == null)
            {
                ErrorMsg = "Failed to evaluate the operand function: The operand function token is null.";
                return false;
            }

            if (Parameters == null)
            {
                ErrorMsg = "Failed to evaluate the operand function: The parameters collection is null.";
                return false;
            }

            // launch the correct operand function
            switch (OperandFunction.TokenName.Trim().ToLower())
            {
                case "cos[":
                    try
                    {
                        success = Cos(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "avg[":
                    try
                    {
                        success = Avg(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "abs[":
                    try
                    {
                        success = Abs(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "not[":
                    try
                    {
                        success = Not(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "isalldigits[":
                    try
                    {
                        success = IsAllDigits(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "concat[":
                    try
                    {
                        success = ConCat(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "date[":
                    try
                    {
                        success = NewDate(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "day[":
                    try
                    {
                        success = Day(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;
                    
                case "month[":
                    try
                    {
                        success = Month(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;
                    
                case "year[":
                    try
                    {
                        success = Year(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "iif[":
                    try
                    {
                        success = IIF(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "lcase[":
                    try
                    {
                        success = LCase(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "pcase[":
                    try
                    {
                        success = PCase(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "left[":
                    try
                    {
                        success = Left(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "len[":
                    try
                    {
                        success = Length(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "numericmax[":
                    try
                    {
                        success = NumericMax(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "mid[":
                    try
                    {
                        success = Mid(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "numericmin[":
                    try
                    {
                        success = NumericMin(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "datemax[":
                    try
                    {
                        success = DateMax(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;
                    
                case "datemin[":
                    try
                    {
                        success = DateMin(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "stringmax[":
                    try
                    {
                        success = StringMax(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;
                    
                case "stringmin[":
                    try
                    {
                        success = StringMin(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "right[":
                    try
                    {
                        success = Right(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "round[":
                    try
                    {
                        success = Round(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "sqrt[":
                    try
                    {
                        success = Sqrt(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "ucase[":
                    try
                    {
                        success = UCase(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "contains[":
                    try
                    {
                        success = Contains(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "between[":
                    try
                    {
                        success = Between(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "indexof[":
                    try
                    {
                        success = IndexOf(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "isnullorempty[":
                    try
                    {
                        success = IsNullOrEmpty(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "istrueornull[":
                    try
                    {
                        success = IsTrueOrNull(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "isfalseornull[":
                    try
                    {
                        success = IsFalseOrNull(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "trim[":
                    try
                    {
                        success = Trim(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "rtrim[":
                    try
                    {
                        success = RTrim(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "ltrim[":
                    try
                    {
                        success = LTrim(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "now[":
                    try
                    {
                        success = Now(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "dateadd[":
                    try
                    {
                        success = DateAdd(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "replace[":
                    try
                    {
                        success = Replace(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "remove[":
                    try
                    {
                        success = Remove(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "eval[":
                    try
                    {
                        success = Eval(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "quote[":
                    try
                    {
                        success = true;
                        Result = new Parser.TokenItem("\"", Parser.TokenType.Token_Operand,  Parser.TokenDataType.Token_DataType_String, false);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "rpad[":
                    try
                    {
                        success = RPad(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;
                    
                case "lpad[":
                    try
                    {
                        success = LPad(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;
                    
                case "join[":
                    try
                    {
                        success = Join(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "searchstring[":
                    try
                    {
                        success = SearchString(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "substring[":
                    try
                    {
                        success = SubString(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                case "sin[":
                    try
                    {
                        success = Sin(Parameters, out Result, out ErrorMsg);
                    }
                    catch (Exception err)
                    {
                        ErrorMsg = "Failed to evaluate the operand function " + OperandFunction.TokenName.Trim().ToLower() + ": " + err.Message;
                        success = false;
                    }
                    break;

                default:
                    ErrorMsg = "Unknown operand function";
                    return false;
                    
            }

            return success;

        }

        #endregion

        #region Operand Functions

        private bool Abs(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function Abs[]: Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (Support.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double abs_temp = System.Math.Abs(temp);

                Result = new Parser.TokenItem(abs_temp.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Error in operand function Abs[]: Operand Function can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Not(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function Not[]: Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a Not of a boolean token
            if (Support.DataTypeCheck.IsBoolean(Parameters[0].TokenName) == true)
            {
                bool temp = Parameters[0].TokenName_Boolean;
                temp = !temp;

                Result = new Parser.TokenItem(temp.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, false);
            }
            else
            {
                ErrorMsg = "Error in operand function Not[]: Operand Function can only evaluate parameters that are boolean.";
                return false;
            }

            return true;
        }

        private bool IsAllDigits(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function IsAllDigits[]: Operand Function requires 1 parameter.";
                return false;
            }

            // check the first parameter
            string checkString = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);
            bool allDigits = true; // assume all digits
            foreach (char c in checkString)
            {
                if (Char.IsDigit(c) == false)
                {
                    allDigits = false;
                    break;
                }
            }

            Result = new Parser.TokenItem(allDigits.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, false);

            return true;
        }

        private bool Cos(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function Cos[]: Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a Cos of an item that can be converted to a double
            if (Support.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double cos_temp = System.Math.Cos(temp);

                Result = new Parser.TokenItem(cos_temp.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Error in operand function Cos[]: Operand Function can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool Avg(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "Error in operand function Avg[]: Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            double total = 0;
            try
            {
                foreach (Parser.TokenItem tItem in Parameters)
                {
                    if (Support.DataTypeCheck.IsDouble(tItem.TokenName) == true)
                    {
                        total += tItem.TokenName_Double;
                    }
                    else
                    {
                        ErrorMsg = "Error in operand function Avg[]: Operand Function can only calculate the average of parameters that can be converted to double.";
                        return false;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorMsg = "Error in operand function Avg[]: " + err.Message;
                return false;
            }

            double dAvg = 0;
            try
            {
                dAvg = total / Convert.ToDouble(Parameters.Count);
            }
            catch (Exception err)
            {
                ErrorMsg = "Error in operand function Avg[] while calcuating the average: " + err.Message;
                return false;
            }

            Result = new Parser.TokenItem(dAvg.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);

            return true;
        }

        private bool ConCat(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "Error in operand function ConCat[]: Operand Function requires at least 1 parameter.";
                return false;
            }

            string conCatString = "\"";
            try
            {
                foreach (Parser.TokenItem tItem in Parameters)
                {
                    conCatString += Support.DataTypeCheck.RemoveTextQuotes(tItem.TokenName);
                }
            }
            catch (Exception err)
            {
                ErrorMsg = "Error in operand function ConCat[]: " + err.Message;
                return false;
            }

            conCatString += "\"";

            Result = new Parser.TokenItem(conCatString, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool NewDate(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 3 parameters
            if (Parameters.Count != 3)
            {
                ErrorMsg = "Error in operand function NewDate[]: Operand Function requires 3 parameter.";
                return false;
            }

            // the 3 parameters must be integers
            for (int i = 0; i < 3; i++)
            {
                if (Support.DataTypeCheck.IsInteger(Parameters[i].TokenName) == false)
                {
                    ErrorMsg = "Error in operand function NewDate[]: Operand Function requires all 3 paraemters to be integers.";                    
                    return false;
                }
            }

            DateTime dt = DateTime.MinValue;
            try
            {
                dt = new DateTime(Parameters[2].TokenName_Int, Parameters[0].TokenName_Int, Parameters[1].TokenName_Int);
            }
            catch
            {
                ErrorMsg = "Error in operand function NewDate[]: Invalid Date";
                return false;
            }

            Result = new Parser.TokenItem(dt.ToString("M.d.yyyy"), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Date, false);

            return true;
        }

        private bool Day(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 1 
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function Day[]: Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a Day of an item that can be converted to a date
            if (Support.DataTypeCheck.IsDate(Parameters[0].TokenName) == true)
            {
                int day = Parameters[0].TokenName_DateTime.Day;
                Result = new Parser.TokenItem(day.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Int, false);
            }
            else
            {
                ErrorMsg = "Error in operand function Day[]: Operand Function requires the parameter of type date time.";
                return false;
            }

            return true;
        }

        private bool Month(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 1 
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function Month[]: Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a Day of an item that can be converted to a date
            if (Support.DataTypeCheck.IsDate(Parameters[0].TokenName) == true)
            {
                int month = Parameters[0].TokenName_DateTime.Month;
                Result = new Parser.TokenItem(month.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Int, false);
            }
            else
            {
                ErrorMsg = "Error in operand function Avg[]: Operand Function requires 1 parameter that can be converted to a date time.";
                return false;
            }

            return true;
        }

        private bool Year(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 1 
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function Year[]: Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a Day of an item that can be converted to a date
            if (Support.DataTypeCheck.IsDate(Parameters[0].TokenName) == true)
            {
                int year = Parameters[0].TokenName_DateTime.Year;
                Result = new Parser.TokenItem(year.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Int, false);
            }
            else
            {
                ErrorMsg = "Error in operand function Year[]: Operand Function requires 1 parameter that can be converted to date time.";
                return false;
            }

            return true;
        }

        private bool IIF(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 3 parameters
            if (Parameters.Count != 3)
            {
                ErrorMsg = "Error in operand function IIF[]: Operand Function requires 3 parameter.";
                return false;
            }

            // the first parameter must be a boolean
            if (Support.DataTypeCheck.IsBoolean(Parameters[0].TokenName) == false)
            {
                ErrorMsg = "Error in operand function IIF[]: Operand Function requires the first paraemter to be a boolean.";
                return false;
            }

            if (Parameters[0].TokenName_Boolean == true)
            {
                // return the first parameter
                Result = Parameters[1];
            }
            else
            {
                // return the second parameter
                Result = Parameters[2];
            }

            return true;
        }

        private bool LCase(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function LCase[]: Operand Function requires 1 parameter.";
                return false;
            }

            Result = new Parser.TokenItem(Parameters[0].TokenName.ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }
        
        private bool PCase(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function PCase[]: Operand Function requires 1 parameter.";
                return false;
            }


            string final = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName.ToLower().Trim());
            string finished = final.Substring(0, 1).ToUpper() + final.Substring(1, final.Length - 1);

            Result = new Parser.TokenItem("\"" + finished + "\"", Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool Left(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 2 parameter
            if (Parameters.Count != 2)
            {
                ErrorMsg = "Error in operand function Left[]: Operand Function requires 2 parameter.";
                return false;
            }

            // get the token from the first parameter         
            bool isText = Support.DataTypeCheck.IsText(Parameters[0].TokenName);
            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);            

            // the second parameter must be an integer
            if (Support.DataTypeCheck.IsInteger(Parameters[1].TokenName) == false)
            {
                ErrorMsg = "Error in operand function Left[]: Operand Function requires an integer as the second parameter.";
                return false;
            }

            int leftAmount = Parameters[1].TokenName_Int;

            // the left amount must be less than or equal to the length of the string
            if (leftAmount > tempToken.Length)
            {
                ErrorMsg = "Error in operand function Left[]: Operand Function requires an integer as the second parameter.";
                return false;
            }

            string newValue = tempToken.Substring(0, leftAmount);
            if (isText == true) newValue = "\"" + newValue + "\"";

            Result = new Parser.TokenItem(newValue, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool Length(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Error in operand function Length[]: Operand Function requires 1 parameter.";
                return false;
            }

            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);

            Result = new Parser.TokenItem(tempToken.Length.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Int, false);

            return true;
        }

        private bool NumericMax(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {

            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "Error in operand function NumericMax[]: Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            double dblMax = 0;
            bool firstItem = true;
            foreach (Parser.TokenItem tItem in Parameters)
            {
                if (Support.DataTypeCheck.IsDouble(tItem.TokenName) == true)
                {
                    if (firstItem == true)
                    {
                        dblMax = tItem.TokenName_Double;
                        firstItem = false;
                    }
                    else
                    {
                        if (tItem.TokenName_Double > dblMax) dblMax = tItem.TokenName_Double;
                    }
                }
                else
                {
                    ErrorMsg = "Error in operand function NumericMax[]: Operand Function expects that all parameters can be converted to double.";
                    return false;
                }
            }

            Result = new Parser.TokenItem(dblMax.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);

            return true;


        }

        private bool NumericMin(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {

            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "Error in operand function NumericMin[]: Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            double dblMin = 0;
            bool firstItem = true;
            foreach (Parser.TokenItem tItem in Parameters)
            {
                if (Support.DataTypeCheck.IsDouble(tItem.TokenName) == true)
                {
                    if (firstItem == true)
                    {
                        dblMin = tItem.TokenName_Double;
                        firstItem = false;
                    }
                    else
                    {
                        if (tItem.TokenName_Double < dblMin) dblMin = tItem.TokenName_Double;
                    }
                }
                else
                {
                    ErrorMsg = "Error in operand function NumericMin[]: Operand Function expects that all parameters can be converted to double.";
                    return false;
                }
            }

            Result = new Parser.TokenItem(dblMin.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);

            return true;

        }

        private bool DateMax(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {

            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "DateMax[] Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            DateTime maxDate = DateTime.MinValue;
            bool firstItem = true;
            foreach (Parser.TokenItem tItem in Parameters)
            {
                if (Support.DataTypeCheck.IsDate(tItem.TokenName) == true)
                {
                    if (firstItem == true)
                    {
                        maxDate = tItem.TokenName_DateTime;
                        firstItem = false;
                    }
                    else
                    {
                        TimeSpan ts = maxDate.Subtract(tItem.TokenName_DateTime);
                        if (ts.TotalDays < 0) maxDate = tItem.TokenName_DateTime;
                    }
                }
                else
                {
                    ErrorMsg = "DateMax[] Operand Function expects that all parameters can be converted to date time.";
                    return false;
                }
            }

            Result = new Parser.TokenItem(maxDate.ToString("M.d.yyyy"), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Date, false);

            return true;


        }

        private bool DateMin(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {

            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "DateMin[] Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            DateTime minDate = DateTime.MinValue;
            bool firstItem = true;
            foreach (Parser.TokenItem tItem in Parameters)
            {
                if (Support.DataTypeCheck.IsDate(tItem.TokenName) == true)
                {
                    if (firstItem == true)
                    {
                        minDate = tItem.TokenName_DateTime;
                        firstItem = false;
                    }
                    else
                    {
                        TimeSpan ts = minDate.Subtract(tItem.TokenName_DateTime);
                        if (ts.TotalDays > 0) minDate = tItem.TokenName_DateTime;
                    }
                }
                else
                {
                    ErrorMsg = "DateMin[] Operand Function expects that all parameters can be converted to date time.";
                    return false;
                }
            }

            Result = new Parser.TokenItem(minDate.ToString("M.d.yyyy"), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Date, false);

            return true;

        }

        private bool StringMax(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {

            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "StringMax[] Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            string maxString = "";
            bool firstItem = true;
            foreach (Parser.TokenItem tItem in Parameters)
            {
                if (firstItem == true)
                {
                    maxString = Support.DataTypeCheck.RemoveTextQuotes(tItem.TokenName);
                    firstItem = false;
                }
                else
                {
                    string currString = Support.DataTypeCheck.RemoveTextQuotes(tItem.TokenName);
                    if (maxString.CompareTo(currString) < 0) maxString = currString;
                }
            }

            Result = new Parser.TokenItem("\"" + maxString + "\"", Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;

        }

        private bool StringMin(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {

            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "StringMax[] Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the average of items that can be convert to double
            string minString = "";
            bool firstItem = true;
            foreach (Parser.TokenItem tItem in Parameters)
            {
                if (firstItem == true)
                {
                    minString = Support.DataTypeCheck.RemoveTextQuotes(tItem.TokenName);
                    firstItem = false;
                }
                else
                {
                    string currString = Support.DataTypeCheck.RemoveTextQuotes(tItem.TokenName);
                    if (minString.CompareTo(currString) > 0) minString = currString;
                }
            }
            
            Result = new Parser.TokenItem("\"" + minString + "\"", Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;


        }

        private bool Right(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 2 parameter
            if (Parameters.Count != 2)
            {
                ErrorMsg = "Right[] Operand Function requires 2 parameter.";
                return false;
            }

            // get the token from the first parameter         
            bool isText = Support.DataTypeCheck.IsText(Parameters[0].TokenName);
            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);

            // the second parameter must be an integer
            if (Support.DataTypeCheck.IsInteger(Parameters[1].TokenName) == false)
            {
                ErrorMsg = "Right[] Operand Function requires an integer as the second parameter.";
                return false;
            }

            int rightAmount = Parameters[1].TokenName_Int;

            // the left amount must be less than or equal to the length of the string
            if (rightAmount > tempToken.Length)
            {
                ErrorMsg = "Right[] Operand Function requires an integer as the second parameter.";
                return false;
            }

            string newValue = tempToken.Substring(tempToken.Length - rightAmount, rightAmount);
            if (isText == true) newValue = "\"" + newValue + "\"";

            Result = new Parser.TokenItem(newValue, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool Mid(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count == 0)
            {
                ErrorMsg = "Mid[] Operand Function requires at least 1 parameter.";
                return false;
            }

            // we can only take the mid of items that can be convert to double
            double[] arrData = new double[Parameters.Count];
            int index = 0;
            foreach (Parser.TokenItem tItem in Parameters)
            {
                if (Support.DataTypeCheck.IsDouble(tItem.TokenName) == true)
                {
                    arrData[index] = tItem.TokenName_Double;
                    index++;
                }
                else
                {
                    ErrorMsg = "Mid[] Operand Function can only calculate the middle of parameters that can be converted to double.";
                    return false;
                }
            }

            // sort the array of doubles
            Array.Sort<double>(arrData);

            double mid = 0;
            double midDBLItem = ((arrData.Length + 1) / 2) - 1;
            int midItem = Convert.ToInt32(System.Math.Floor(midDBLItem));

            if ((arrData.Length % 2) == 0)
            {
                // there is an even number of items in the array
                double item1 = arrData[midItem];
                double item2 = arrData[midItem + 1];

                mid = (item1 + item2) / 2;
            }
            else
            {
                // there is an odd number of items in the array.
                mid = arrData[midItem];
            }


            Result = new Parser.TokenItem(mid.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);

            return true;
        }

        private bool Round(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 2 parameters
            if (Parameters.Count != 2)
            {
                ErrorMsg = "Round[] Operand Function requires 2 parameter.";
                return false;
            }

            // the first parameters must be a double
            if (Support.DataTypeCheck.IsDouble(Parameters[0].TokenName) == false)
            {
                ErrorMsg = "Round[] Operand Function requires the first parameter to be a double.";
                return false;
            }

            // the second parameter must be a integer
            if (Support.DataTypeCheck.IsInteger(Parameters[1].TokenName) == false)
            {
                ErrorMsg = "Round[] Operand Function requires the second parameter to be a integer.";
                return false;
            }

            double roundItem = Parameters[0].TokenName_Double;
            int roundAmt = Parameters[1].TokenName_Int;
            if (roundAmt < 0)
            {
                ErrorMsg = "Round[] Operand Function requires the second parameter to be a positive integer.";
                return false;
            }

            double final = System.Math.Round(roundItem, roundAmt);

            string format = "#";
            if (roundAmt > 0)
            {
                format += ".";
                for (int i = 0; i < roundAmt; i++) format += "#";
            }

            Result = new Parser.TokenItem(final.ToString(format), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);

            return true;
        }

        private bool Sqrt(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Sqrt[] Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (Support.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double sqrt_temp = System.Math.Sqrt(temp);

                Result = new Parser.TokenItem(sqrt_temp.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Sqrt[] can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        private bool UCase(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "UCase[] Operand Function requires 1 parameter.";
                return false;
            }

            Result = new Parser.TokenItem(Parameters[0].TokenName.ToUpper(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool Contains(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 2 parameter
            if (Parameters.Count <= 1)
            {
                ErrorMsg = "Contains[] Operand Function requires at least 2 parameters.";
                return false;
            }

            // get the search item
            string searchString = Parameters[0].TokenName;

            // loop through the items
            bool foundItem = false;
            for (int i = 1; i < Parameters.Count; i++)
            {
                if (Support.DataTypeCheck.RemoveTextQuotes(Parameters[i].TokenName) == searchString)
                {
                    foundItem = true;
                    break;
                }
            }

            Result = new Parser.TokenItem(foundItem.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, false);

            return true;
        }

        private bool IndexOf(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 2 parameter
            if (Parameters.Count <= 1)
            {
                ErrorMsg = "Contains[] Operand Function requires at least 2 parameters.";
                return false;
            }

            // get the search item
            string searchString = Parameters[0].TokenName;

            // loop through the items
            int index = -1;
            for (int i = 1; i < Parameters.Count; i++)
            {
                if (Support.DataTypeCheck.RemoveTextQuotes(Parameters[i].TokenName) == searchString)
                {
                    index = i - 1;
                    break;
                }
            }

            Result = new Parser.TokenItem(index.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Int, false);

            return true;
        }

        private bool IsNullOrEmpty(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "IsNullOrEmpty[] Operand Function requires 1 parameter.";
                return false;
            }

            // check if the parameter is null;
            if (Support.DataTypeCheck.IsNULL(Parameters[0].TokenName) == true)
                Result = new Parser.TokenItem("true", Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, false);
            else
            {

                string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);
                tempToken = tempToken.Trim().ToLower();

                if (tempToken == "null")
                    Result = new Parser.TokenItem("true", Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, false);
                else
                {
                    bool final = String.IsNullOrEmpty(tempToken);

                    Result = new Parser.TokenItem(final.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, false);
                }
            }

            return true;
        }

        private bool IsTrueOrNull(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "IsTrueOrNull[] Operand Function requires 1 parameter.";
                return false;
            }

            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);
            tempToken = tempToken.Trim();


            bool isTrueOrNull = true;
            if (String.IsNullOrEmpty(tempToken) == false)
            {
                isTrueOrNull = (tempToken.ToLower() == "true");
            }

            Result = new Parser.TokenItem(isTrueOrNull.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, false);

            return true;
        }

        private bool IsFalseOrNull(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "IsFalseOrNull[] Operand Function requires 1 parameter.";
                return false;
            }

            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);
            tempToken = tempToken.Trim();


            bool isfalseOrNull = true;
            if (String.IsNullOrEmpty(tempToken) == false)
            {
                isfalseOrNull = (tempToken.ToLower() == "false");
            }

            Result = new Parser.TokenItem(isfalseOrNull.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, false);

            return true;
        }

        private bool Trim(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Trim[] Operand Function requires 1 parameter.";
                return false;
            }

            // get the token from the first parameter         
            bool isText = Support.DataTypeCheck.IsText(Parameters[0].TokenName);
            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);

            string newValue = tempToken.Trim();
            if (isText == true) newValue = "\"" + newValue + "\"";

            Result = new Parser.TokenItem(newValue, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool LTrim(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "LTrim[] Operand Function requires 1 parameter.";
                return false;
            }

            // get the token from the first parameter         
            bool isText = Support.DataTypeCheck.IsText(Parameters[0].TokenName);
            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);

            string newValue = tempToken.TrimStart();
            if (isText == true) newValue = "\"" + newValue + "\"";

            Result = new Parser.TokenItem(newValue, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool RTrim(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "RTrim[] Operand Function requires 1 parameter.";
                return false;
            }

            // get the token from the first parameter         
            bool isText = Support.DataTypeCheck.IsText(Parameters[0].TokenName);
            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);

            string newValue = tempToken.TrimEnd();
            if (isText == true) newValue = "\"" + newValue + "\"";

            Result = new Parser.TokenItem(newValue, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool Now(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            Result = new Parser.TokenItem(DateTime.Now.ToString("M.d.yyyy"), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Date, false);

            return true;
        }

        private bool DateAdd(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 3 parameters
            if (Parameters.Count != 3)
            {
                ErrorMsg = "DateAdd[] Operand Function requires 3 parameter.";
                return false;
            }

            // the first parameter must be a date
            if (Support.DataTypeCheck.IsDate(Parameters[0].TokenName) == false)
            {
                ErrorMsg = "DateAdd[] Operand Function requires a date in the first parameter.";
                return false;
            }

            // the second parameter must be a "d", "m", or "y";
            string dateAddType = Support.DataTypeCheck.RemoveTextQuotes(Parameters[1].TokenName);
            dateAddType = dateAddType.Trim().ToLower();

            if (dateAddType != "d") 
            {
                if (dateAddType != "m")
                {
                    if (dateAddType != "y")
                    {
                        if (dateAddType != "b")
                        {
                            ErrorMsg = "DateAdd[] Operand Function requires that the second parameter is a d, m, y, b.";
                            return false;
                        }
                    }
                }
            }

            // the last parameter must be an integer
            if (Support.DataTypeCheck.IsInteger(Parameters[2].TokenName) == false)
            {
                ErrorMsg = "DateAdd[] Operand Function requires an integer in the third parameter.";
                return false;
            }
            int dateAmt = Parameters[2].TokenName_Int;

            // get the data value
            DateTime dateValue = Parameters[0].TokenName_DateTime;

            if (dateAddType == "d")
            {
                dateValue = dateValue.AddDays(dateAmt);
            }
            else if (dateAddType == "m")
            {
                dateValue = dateValue.AddMonths(dateAmt);
            }
            else if (dateAddType == "y")
            {
                dateValue = dateValue.AddYears(dateAmt);
            }
            else if (dateAddType == "b")
            {
                int numAdded = 0;

                DateTime tempDate  = dateValue;
                while (numAdded < dateAmt)
                {
                    tempDate = tempDate.AddDays(1);
                    if (tempDate.DayOfWeek != DayOfWeek.Saturday)
                    {
                        if (tempDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            numAdded++;
                            dateValue = tempDate;
                        }
                    }
                }
            }

            Result = new Parser.TokenItem(dateValue.ToString("M.d.yyyy"), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Date, false);

            return true;
        }

        private bool Replace(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 3 parameters
            if (Parameters.Count != 3)
            {
                ErrorMsg = "Replace[] Operand Function requires 3 text parameter.";
                return false;
            }

            string newValue = Parameters[0].TokenName;
            string find = Support.DataTypeCheck.RemoveTextQuotes(Parameters[1].TokenName.ToString());
            string replace = Support.DataTypeCheck.RemoveTextQuotes(Parameters[2].TokenName.ToString());

            string final = newValue.Replace(find, replace);

            Result = new Parser.TokenItem(final, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool Remove(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 3 parameters
            if (Parameters.Count != 2)
            {
                ErrorMsg = "InsertOnSubmit[] Operand Function requires 2 text parameter.";
                return false;
            }

            string newValue = Parameters[0].TokenName;
            string replace = Support.DataTypeCheck.RemoveTextQuotes(Parameters[1].TokenName.ToString());

            string[] arrItems = newValue.Split(replace.ToCharArray(),  StringSplitOptions.RemoveEmptyEntries);

            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arrItems.Length; i++) sb.Append(arrItems[i]);

            Result = new Parser.TokenItem(sb.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool Eval(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Eval[] Operand Function requires 1 parameter.";
                return false;
            }

            // get the expression to be evaluated
            string expression = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);

            // create the tokens
            Parser.Token tokens = new Parser.Token(expression);
            if (tokens.AnyErrors == true)
            {
                ErrorMsg = tokens.LastErrorMessage;
                return false;
            }

            // create the evaluator object
            Evaluator e = new Evaluator(tokens);

            string value = "";

            if (e.Evaluate(out value, out ErrorMsg) == false) return false;

            if (Support.DataTypeCheck.IsInteger(value) == true)
                Result = new Parser.TokenItem(value, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Int, false);
            else if (Support.DataTypeCheck.IsDouble(value) == true)
                Result = new Parser.TokenItem(value, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);
            else if (Support.DataTypeCheck.IsDate(value) == true)
                Result = new Parser.TokenItem(value, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Date, false);
            else
                Result = new Parser.TokenItem(value, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool RPad(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 3 parameter
            if (Parameters.Count != 3)
            {
                ErrorMsg = "RPad[] Operand Function requires 3 parameter.";
                return false;
            }

            string padText = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);
            string padString = Support.DataTypeCheck.RemoveTextQuotes(Parameters[1].TokenName);

            // the last parameter must be an integer
            if (Support.DataTypeCheck.IsInteger(Parameters[2].TokenName) == false)
            {
                ErrorMsg = "RPad[] Operand Function requires the 3rd parameter to be an integer.";
                return false;
            }

            int padCount = Parameters[2].TokenName_Int;

            string finalPad = padText;
            for (int i = 0; i < padCount; i++) finalPad += padString;

            Result = new Parser.TokenItem("\"" + finalPad + "\"", Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool LPad(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 3 parameter
            if (Parameters.Count != 3)
            {
                ErrorMsg = "RPad[] Operand Function requires 3 parameter.";
                return false;
            }

            string padText = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);
            string padString = Support.DataTypeCheck.RemoveTextQuotes(Parameters[1].TokenName);

            // the last parameter must be an integer
            if (Support.DataTypeCheck.IsInteger(Parameters[2].TokenName) == false)
            {
                ErrorMsg = "RPad[] Operand Function requires the 3rd parameter to be an integer.";
                return false;
            }

            int padCount = Parameters[2].TokenName_Int;

            string finalPad = "";
            for (int i = 0; i < padCount; i++) finalPad += padString;

            finalPad += padText;
            Result = new Parser.TokenItem("\"" + finalPad + "\"", Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool Join(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 2 parameter
            if (Parameters.Count <= 1)
            {
                ErrorMsg = "Join[] Operand Function requires at least 2 parameters.";
                return false;
            }

            // get the join delimiter
            string joinString = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);

            // loop through the items
            string finalJoin = "";
            for (int i = 1; i < Parameters.Count; i++)
            {
                finalJoin += Support.DataTypeCheck.RemoveTextQuotes(Parameters[i].TokenName);
                if (i != Parameters.Count - 1) finalJoin += joinString;
            }

            Result = new Parser.TokenItem("\"" + finalJoin + "\"", Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }

        private bool SubString(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 3 parameter
            if (Parameters.Count != 3)
            {
                ErrorMsg = "SubString[] Operand Function requires 3 parameter.";
                return false;
            }

            // get the token from the first parameter         
            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);

            // the second parameter must be an integer
            if (Support.DataTypeCheck.IsInteger(Parameters[1].TokenName) == false)
            {
                ErrorMsg = "SubString[] Operand Function requires an integer as the second parameter.";
                return false;
            }
            int startPosition = Parameters[1].TokenName_Int;

            // the last parameter must be an integer
            if (Support.DataTypeCheck.IsInteger(Parameters[2].TokenName) == false)
            {
                ErrorMsg = "SubString[] Operand Function requires an integer as the third parameter.";
                return false;
            }
            int length = Parameters[2].TokenName_Int;

            // the start position and length canno be longer than the string
            if ((startPosition + length) > tempToken.Length)
            {
                ErrorMsg = "SubString[] Operand Function: The start position and length cannot be longer than the string.";
                return false;
            }

            string newValue = tempToken.Substring(startPosition, length);
            newValue = "\"" + newValue + "\"";

            Result = new Parser.TokenItem(newValue, Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_String, false);

            return true;
        }


        private bool Between(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 3 parameter
            if (Parameters.Count != 3)
            {
                ErrorMsg = "Between[] Operand Function requires 3 parameter.";
                return false;
            }

            // all 3 parameters must be able to convert to double
            for (int i = 0; i < 3; i++)
            {
                if (Support.DataTypeCheck.IsDouble(Parameters[0].TokenName) == false)
                {
                    ErrorMsg = "Between[] Operand Function requires 3 parameter that can be converted to double.";
                    return false;
                }
            }

            // get the 3 doubles
            double d1 = Parameters[0].TokenName_Double;
            double d2 = Parameters[1].TokenName_Double;
            double d3 = Parameters[2].TokenName_Double;

            // double result
            bool result = false;
            if (d1 >= d2)
            {
                if (d1 <= d3)
                {
                    result = true;
                }
            }

            Result = new Parser.TokenItem(result.ToString().ToLower(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Boolean, false);

            return true;
        }

        private bool SearchString(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have 2 parameter
            if (Parameters.Count != 3)
            {
                ErrorMsg = "SearchString[] Operand Function requires 3 parameter.";
                return false;
            }

            // get the token from the first parameter         
            string tempToken = Support.DataTypeCheck.RemoveTextQuotes(Parameters[0].TokenName);

            // the second parameter must be an integer
            if (Support.DataTypeCheck.IsInteger(Parameters[1].TokenName) == false)
            {
                ErrorMsg = "SearchString[] Operand Function requires an integer as the second parameter.";
                return false;
            }
            int startPosition = Parameters[1].TokenName_Int;

            // the last parameter must be an string
            string searchFor = Support.DataTypeCheck.RemoveTextQuotes(Parameters[2].TokenName);


            int location = tempToken.IndexOf(searchFor, startPosition);

            Result = new Parser.TokenItem(location.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Int, false);

            return true;
        }


        private bool Sin(Parser.TokenItems Parameters, out Parser.TokenItem Result, out string ErrorMsg)
        {
            // initialize the outgoing variables
            ErrorMsg = "";
            Result = null;

            // make sure we have at least 1 parameter
            if (Parameters.Count != 1)
            {
                ErrorMsg = "Sin[] Operand Function requires 1 parameter.";
                return false;
            }

            // we can only take a abs of an item that can be converted to a double
            if (Support.DataTypeCheck.IsDouble(Parameters[0].TokenName) == true)
            {
                double temp = Parameters[0].TokenName_Double;
                double sin_temp = System.Math.Sin(temp);

                Result = new Parser.TokenItem(sin_temp.ToString(), Parser.TokenType.Token_Operand, Parser.TokenDataType.Token_DataType_Double, false);
            }
            else
            {
                ErrorMsg = "Sin[] can only evaluate parameters that can be converted to a double.";
                return false;
            }

            return true;
        }

        #endregion

    }
   

}
