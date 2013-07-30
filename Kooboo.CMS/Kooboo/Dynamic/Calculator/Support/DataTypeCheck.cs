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

namespace Kooboo.Dynamic.Calculator.Support
{
    /// <summary>
    /// 
    /// </summary>
    public class DataTypeCheck
    {

        #region Fields
        public static string[] ReservedWords = { "int", "long", "float", "decimal", "currency", "date", "value", "while", "for", "do", "break", "continue", "foreach", "next", "else", "end", "endif", "string", "text", "char", "list", "rule", "expression", "function", "macro", "express", "int", "integer", "list", "sub", "set", ":=" };
        public static string[] OperandFunctions = { "avg", "abs", "iif", "lcase", "left", "len", "mid", "right", "round", "sqrt", "ucase", "isnullorempty", "istrueornull", "isfalseornull", "trim", "rtrim", "ltrim", "dateadd", "concat", "date", "rpad", "lpad", "join", "searchstring", "day", "month", "year", "substring", "numericmax", "numericmin", "datemax", "datemin", "stringmax", "stringmin", "contains", "between", "indexof", "now", "replace", "eval", "remove", "quote", "pcase", "sin", "cos", "not", "isalldigits" };
        public static string[] ArithOperators = { "^", "*", "/", "%", "+", "-" };
        public static string[] LogicalOperators = { "and", "or" };
        public static string[] ComparisonOperators = { "<", "<=", ">", ">=", "<>", "=" };
        public static string[] AssignmentOperators = { ":=" }; 
        #endregion
        
        #region Methods
        /// <summary>
        /// All digits....no decimals, commas, or -
        /// </summary>
        /// <param name="CheckString"></param>
        /// <returns></returns>
        public static bool IsAllDigits(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim();

            bool allDigits = true;
            foreach (char c in CheckString)
            {
                if (Char.IsDigit(c) == false)
                {
                    allDigits = false;
                    break;
                }
            }

            return allDigits;
        }

        /// <summary>
        /// The number of '.' in a string
        /// </summary>
        /// <param name="CheckString"></param>
        /// <returns></returns>
        public static int DecimalCount(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return 0;

            CheckString = CheckString.Trim();

            int decimalCount = 0;
            foreach (char c in CheckString)
            {
                if (c == '.') decimalCount++;
            }

            return decimalCount;
        }


        /// <summary>
        /// Text items are surronded by strings.
        /// </summary>
        /// <param name="CheckString"></param>
        /// <returns></returns>
        public static bool IsText(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim();

            if (CheckString.Length == 1) return false;
            if (CheckString.StartsWith("\"") == false) return false;
            if (CheckString.EndsWith("\"") == false) return false;

            return true;
        }


        /// <summary>
        /// integers have all digits and no decimals.  they can also start with -
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static bool IsInteger(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim();

            bool isInteger = true;  // assume the value is an integer
            int intPosition = 0; // the current position we are checking

            foreach (char c in CheckString)
            {
                if (Char.IsNumber(c) == false)
                {
                    if (c != '-')
                    {
                        isInteger = false;
                        break;
                    }
                    else if (intPosition != 0)
                    {
                        isInteger = false;
                        break;
                    }
                }

                intPosition++;
            }

            return isInteger;
        }

        /// <summary>
        /// Determines whether the specified check string is date.
        /// </summary>
        /// <param name="CheckString">The check string.</param>
        /// <returns>
        ///   <c>true</c> if the specified check string is date; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDate(string CheckString)
        {

            if (String.IsNullOrEmpty(CheckString) == true) return false;
            CheckString = CheckString.Trim();

            DateTime d = DateTime.MinValue;
            return DateTime.TryParse(CheckString, out d);
        }

        /// <summary>
        /// Determines whether the specified check string is double.
        /// </summary>
        /// <param name="CheckString">The check string.</param>
        /// <returns>
        ///   <c>true</c> if the specified check string is double; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDouble(string CheckString)
        {

            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim();


            bool isDouble = true; // assume the item is a double
            int decimalCount = 0; // the number of decimals

            int intPosition = 0;

            foreach (char c in CheckString)
            {
                if (Char.IsNumber(c) == false)
                {
                    if (c == '.')
                    {
                        decimalCount++;
                    }
                    else if ((c == '-') && (intPosition == 0))
                    {
                        // this is valid, keep going
                    }
                    else
                    {
                        isDouble = false;
                        break;
                    }
                }

                intPosition++;
            }

            if (isDouble == true)
            {
                // make sure there is only 1 decimal

                if (decimalCount <= 1)
                    return true;
                else
                    return false;

            }
            else
                return false;
        }

        /// <summary>
        /// Determines whether [is reserved word] [the specified operand text].
        /// </summary>
        /// <param name="OperandText">The operand text.</param>
        /// <returns>
        ///   <c>true</c> if [is reserved word] [the specified operand text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsReservedWord(string OperandText)
        {
            bool isReservedWord = false;
            if (String.IsNullOrEmpty(OperandText) == true) return false;
            OperandText = OperandText.Trim().ToLower();
            if (String.IsNullOrEmpty(OperandText) == true) return false;

            for (int i = 0; i < ReservedWords.Length; i++)
            {
                if (OperandText == ReservedWords[i])
                {
                    isReservedWord = true;
                    break;
                }
            }

            return isReservedWord;
        }

        /// <summary>
        /// Anies the puncuation.
        /// </summary>
        /// <param name="Text">The text.</param>
        /// <param name="PuncMark">The punc mark.</param>
        /// <returns></returns>
        public static bool AnyPuncuation(string Text, out Char PuncMark)
        {
            PuncMark = ' ';
            foreach (char c in Text)
            {
                if (Char.IsPunctuation(c) == true)
                {
                    PuncMark = c;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is operand function] [the specified operand text].
        /// </summary>
        /// <param name="OperandText">The operand text.</param>
        /// <returns>
        ///   <c>true</c> if [is operand function] [the specified operand text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOperandFunction(string OperandText)
        {
            if (String.IsNullOrEmpty(OperandText) == true) return false;

            OperandText = OperandText.Trim().ToLower();

            bool isOperandFunnction = false;

            for (int i = 0; i < OperandFunctions.Length; i++)
            {
                if (OperandText == OperandFunctions[i])
                {
                    isOperandFunnction = true;
                    break;
                }
            }

            return isOperandFunnction;
        }

        /// <summary>
        /// Determines whether the specified operator text is operator.
        /// </summary>
        /// <param name="OperatorText">The operator text.</param>
        /// <returns>
        ///   <c>true</c> if the specified operator text is operator; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOperator(string OperatorText)
        {

            if (String.IsNullOrEmpty(OperatorText) == true) return false;

            OperatorText = OperatorText.Trim().ToLower();
            bool isOperator = false;

            for (int i = 0; i < ArithOperators.Length; i++)
            {
                if (OperatorText == ArithOperators[i])
                {
                    isOperator = true;
                    break;
                }
            }
            if (isOperator == true) return isOperator;



            for (int i = 0; i < LogicalOperators.Length; i++)
            {
                if (OperatorText == LogicalOperators[i])
                {
                    isOperator = true;
                    break;
                }
            }
            if (isOperator == true) return isOperator;



            for (int i = 0; i < ComparisonOperators.Length; i++)
            {
                if (OperatorText == ComparisonOperators[i])
                {
                    isOperator = true;
                    break;
                }
            }
            if (isOperator == true) return isOperator;


            for (int i = 0; i < AssignmentOperators.Length; i++)
            {
                if (OperatorText == AssignmentOperators[i])
                {
                    isOperator = true;
                    break;
                }
            }
            if (isOperator == true) return isOperator;


            return false;
        }

        /// <summary>
        /// Determines whether the specified check string is boolean.
        /// </summary>
        /// <param name="CheckString">The check string.</param>
        /// <returns>
        ///   <c>true</c> if the specified check string is boolean; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBoolean(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;

            CheckString = CheckString.Trim().ToLower();

            if (CheckString == "true") return true;
            if (CheckString == "false") return true;

            return false;

        }

        /// <summary>
        /// Removes the text quotes.
        /// </summary>
        /// <param name="CheckString">The check string.</param>
        /// <returns></returns>
        public static string RemoveTextQuotes(string CheckString)
        {
            if (IsText(CheckString) == false) return CheckString;

            return CheckString.Substring(1, CheckString.Length - 2);


        }

        /// <summary>
        /// Functions the description.
        /// </summary>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="Syntax">The syntax.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Example">The example.</param>
        public static void FunctionDescription(string FunctionName, out string Syntax, out string Description, out string Example)
        {
            Syntax = "Unknown function";
            Description = "";
            Example = "";

            switch (FunctionName.Trim().ToLower())
            {
                case "cos":
                    Description = "Calculates the cosine of a number.";
                    Syntax = "cos[p1] where p1 can be converted to doubles.";
                    Example = "cos[90] < 0";
                    break;

                case "avg":
                    Description = "Calculates the average of a list of numbers.  The list items must be able to convert to doubles.";
                    Syntax = "avg[p1, ..., pn] where p1,...,pn can be converted to doubles.";
                    Example = "avg[1, 2, 3] = 2";
                    break;

                case "abs":
                    Description = "Calculates the absolute value of a numeric parameter.";
                    Syntax = "abs[p1] where p1 can be converted to a double.";
                    Example = "abs[-10] = 10";
                    break;

                case "iif":
                    Description = "Performs an if-else-end";
                    Syntax = "iif[c, a, b] where c is the condition and must evaluate to a boolean.  The value a is returned if c is true, otherwise, the value b is returned.";
                    Example = "iif[year[now[]] = 2008, \"2008\", \"Not 2008\"]";
                    break;

                case "lcase":
                    Description = "Converts a string to lower case";
                    Syntax = "LCase[a]";
                    Example = "LCase[\"TEST\"] = \"test\"";
                    break;

                case "left":
                    Description = "Returns the left number of characters from a string parameter.";
                    Syntax = "left[s, n] where s is the string and n is the number of characters";
                    Example = "left[\"abcd\", 2] = \"ab\"";
                    break;

                case "len":
                    Description = "Returns te length of a string";
                    Syntax = "len[a] where a is a string variable";
                    Example = "len[\"test\"] = 4";
                    break;

                case "mid":
                    Description = "Calculates the median for a list of numbers";
                    Syntax = "mid[p1, ..., pn] where p1, ..., pn are numberic values";
                    Example = "mid[1, 4, 100] = 4";
                    break;

                case "right":
                    Description = "Returns the right number of characters from a string parameter";
                    Syntax = "right[s, n] where s is the string and n is the number of characters.";
                    Example = "right[\"abcd\", 2] = \"cd\"";
                    break;

                case "round":
                    Description = "Rounds a numeric value to the number of decimal places";
                    Syntax = "round[n, d] where n is the numberic value to be rounded, and d is the number of decimal places.";
                    Example = "round[123.45, 0] = 123";
                    break;

                case "sqrt":
                    Description = "Calculates the square root of a number.";
                    Syntax = "sqrt[a] where a is a numberic parameter";
                    Example = "sqrt[25] = 5";
                    break;

                case "ucase":
                    Description = "Converts a string to upper case";
                    Syntax = "ucase[a]";
                    Example = "ucase[\"test\"] = \"TEST\"";
                    break;

                case "isnullorempty":
                    Description = "Indicates is the parameter is null or empty.";
                    Syntax = "IsNullOrEmpty[a]";
                    Example = "";
                    break;

                case "istrueornull":
                    Description = "Indicates if the parameter has the value true or is null;";
                    Syntax = "isTrueorNull[a]";
                    Example = "";
                    break;

                case "isfalseornull":
                    Description = "Indicates if the parameter has the value false or is null";
                    Syntax = "IsFalseOrNull[a]";
                    Example = "IIF[IsFalseOrNull[a], \"false\", \"not false\"]";
                    break;

                case "trim":
                    Description = "Trims the spaces from the entire string";
                    Syntax = "trim[a]";
                    Example = "";
                    break;

                case "rtrim":
                    Description = "Trims the spaces from the right of a string";
                    Syntax = "rtrim[a]";
                    Example = "";
                    break;

                case "ltrim":
                    Description = "Trims the spaces from the left of a string";
                    Syntax = "ltrim[a]";
                    Example = "";
                    break;

                case "dateadd":
                    Description = "Adds an amount to a date.  Please note that the amount may be negative.";
                    Syntax = "dateadd[date, \"type\", amount] where date is a valid date, and type is \"y\", \"m\", \"d\" or \"b\" (representing year, month, day, or business days) and amount is an integer";
                    Example = "dateadd[now[], \"b\", 5]";
                    break;

                case "concat":
                    Description = "This operand function concatenates the parameters together to make a string.";
                    Syntax = "concat[p1, ..., pn]";
                    Example = "concat[\"This\", \" \", \"is\", \" \", \"a\", \" \", \"test\"] = \"This is a test\"";
                    break;

                case "date":
                    Description = "Create a new date data type";
                    Syntax = "date[m, d, y] where m is an integer and is the month, d is an integer and is the day, and y is an integer and is the year";
                    Example = "date[3, 20, 2007]";
                    break;

                case "rpad":
                    Description = "Pads a string on the right with new values";
                    Syntax = "rpad[a, b, n]  where a and b are string values and n is numeric.  The parameter p will be appended to the right of parameter a, n times.";
                    Example = "rpad[\"test\", \".\", 10] = \"test..........\"";
                    break;

                case "lpad":
                    Description = "Pads a string on the left with new values";
                    Syntax = "lpad[a, b, n]  where a and b are string values and n is numeric.  The parameter p will be appended to the left of parameter a, n times.";
                    Example = "lpad[\"test\", \".\", 10] = \"..........test\"";
                    break;

                case "join":
                    Description = "Joins a list of items together using a delimiter";
                    Syntax = "join[a, b1, ..., bn] where a is the delimiter and b1, ..., bn are the items to be joined.";
                    Example = "join[\" \", \"This\", \"is\", \"a\", \"test\"] = \"This is a test\"";
                    break;

                case "searchstring":
                    Description = "Searches for a string within another string at a specified starting position";
                    Syntax = "SearchString[a, n, b] where a is the string that is being searched, b is the string that is being sought, and n is the start position in a";
                    Example = "SearchString[\"abcdefghijk\", 0, \"efg\"] = 4";
                    break;

                case "day":
                    Description = "Returns the day of a date";
                    Syntax = "day[d1] where d1 is a date value";
                    Example = "day[3.20.1999] = 20";
                    break;

                case "month":
                    Description = "Returns the month of a date";
                    Syntax = "month[d1] where d1 is a date value";
                    Example = "month[now[]] returns the current month";
                    break;

                case "year":
                    Description = "Returns the year of a date";
                    Syntax = "year[d1] where d1 is a date";
                    Example = "year[now[]] returns the current year";
                    break;

                case "substring":
                    Description = "Extracts a substring from a string";
                    Syntax = "SubString[s, a, b] where s is the string, a is the starting point, and b is the number of characters extracted.";
                    Example = "substring[\"abcdefghijk\", 3, 5] = \"defgh\"";
                    break;

                case "numericmax":
                    Description = "Finds the maximum numeric value in a list";
                    Syntax = "NumericMax[p1, ..., pn]";
                    Example = "";
                    break;

                case "numericmin":
                    Description = "Finds the numeric minimum value in a list";
                    Syntax = "NumericMin[p1, ..., pn]";
                    Example = "";
                    break;

                case "datemax":
                    Description = "Returns the maximum date in the list";
                    Syntax = "datemax[d1, ..., dn] where d1, ..., dn are dates";
                    Example = "datemax[ 3.20.1999, 3.20.2005, 3.20.2008] = 3.20.2008";
                    break;

                case "datemin":
                    Description = "Returns the minimum date in the list.";
                    Syntax = "datemin[d1, ..., dn] where d1, ..., dn are dates.";
                    Example = "datemax[ 3.20.1999, 3.20.2005, 3.20.2008] = 3.20.1999";
                    break;

                case "stringmax":
                    Description = "Finds the maximum string in the list";
                    Syntax = "StringMax[p1, ..., pn]";
                    Example = "StringMax[\"Apple\", \"Zebra\"] = \"Zebra\"";
                    break;

                case "stringmin":
                    Description = "Finds the minimum string in the list";
                    Syntax = "StringMax[p1, ..., pn]";
                    Example = "StringMax[\"Apple\", \"Zebra\"] = \"Apple\"";
                    break;

                case "contains":
                    Description = "Indicates if the item is contained in the list.";
                    Syntax = "contains[p1, p2, ...., pn]   If p1 in in the list p2, ..., pn, this function returns \"true\" otherwise, this function returns \"false\".";
                    Example = "contains[state, \"NY\", \"WA\", \"CA\"] = true";
                    break;

                case "between":
                    Description = "Indicates if a value is between the other values.  Please note that the comparison is inclusive.";
                    Syntax = "between[var, val1, val2] where var, val1, and val2 are integers.  if var >= val1 and var <= val2 then the function returns \"true\", otherwise, the function return \"false\".";
                    Example = "between[fico, 400, 700]   Note that fico is a variable.";
                    break;

                case "indexof":
                    Description = "Returns the index of a list item.";
                    Syntax = "indexof[a, b1, ..., bn]  If the list b1, ..., bn contains the value a, the index of the value is returned, otherwise, -1 is returned.  Pleaes note that this is zero based indexing";
                    Example = "iif[indexof[a, \"CA\", \"NY\", \"WA\"] >= 0, \"found state\", \"not found\"]";
                    break;

                case "now":
                    Description = "Returns the current date";
                    Syntax = "now[]  This operand function takes no parameters";
                    Example = "year[now[]] = 2008";
                    break;

                case "replace":
                    Description = "Replaces one string with another string";
                    Syntax = "Replace[a, b, c] where a is the search string, b is the value being replaced, and c is the value that is being inserted";
                    Example = "replace[\"3.20.2008\", \".\", \"-\"] = \"3-20-2008\"";
                    break;

                case "eval":
                    Description = "Evaluates a string rule";
                    Syntax = "eval[r] where r is any valid rule";
                    Example = "eval[concat[\"1\", \"+\", \"2\"]] = 3";
                    break;

                case "remove":
                    Description = "Removes the specified characters from the string";
                    Syntax = "remove[a, b] where a and b are string";
                    Example = "InsertOnSubmit[\"....this...is..a...test...\", \".a\"] = \"thisistest\"";
                    break;

                case "quote":
                    Description = "Returns a double quote";
                    Syntax = "quote[]";
                    Example = "quote[] = \"";
                    break;

                case "pcase":
                    Description = "Converts a string to Proper Case";
                    Syntax = "pcase[a] where a is a string";
                    Example = "join[\" \", pcase[\"dave\"], pcase[\"SMITH\"] ] = \"Dave Smith\"";
                    break;

                case "sin":
                    Description = "Calcuates the sin of a number";
                    Syntax = "sin[a]";
                    Example = "sin[45] = 0.85";
                    break;

                case "isalldigits":
                    Description = "Determine if the parameter contains all digits.";
                    Syntax = "isalldigits[p1] where p1 is a string parameter";
                    Example = "isalldigits[\"12345\"] = true";
                    break;

                case "not":
                    Description = "Performs a NOT on a boolean parameter.";
                    Syntax = "not[p1] where p1 is a boolean parameter";
                    Example = "not[5<10]=false";
                    break;

            }

        }


        /// <summary>
        /// Determines whether the specified check string is NULL.
        /// </summary>
        /// <param name="CheckString">The check string.</param>
        /// <returns>
        ///   <c>true</c> if the specified check string is NULL; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNULL(string CheckString)
        {
            if (String.IsNullOrEmpty(CheckString) == true) return false;
            CheckString = CheckString.Trim().ToLower();

            return (CheckString == "null");

        }

        /// <summary>
        /// Determines whether the specified check string contains operator.
        /// </summary>
        /// <param name="CheckString">The check string.</param>
        /// <param name="sOperand">The s operand.</param>
        /// <param name="sOperator">The s operator.</param>
        /// <returns>
        ///   <c>true</c> if the specified check string contains operator; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsOperator(string CheckString, out string sOperand, out string sOperator)
        {
            // initialize the outgoing variables
            sOperand = "";
            sOperator = "";

            // clean the token
            if (String.IsNullOrEmpty(CheckString) == true) return false;
            CheckString = CheckString.Trim();

            // loop through the arith. operators
            bool containsOperator = false; // assume an operator is not in the opperand
            for (int i = 0; i < Support.DataTypeCheck.ArithOperators.Length; i++)
            {
                if (CheckString.EndsWith(Support.DataTypeCheck.ArithOperators[i]) == true)
                {
                    containsOperator = true;
                    sOperator = Support.DataTypeCheck.ArithOperators[i];
                    sOperand = CheckString.Substring(0, CheckString.Length - sOperator.Length);
                    break;
                }
            }
            if (containsOperator == true) return true;


            // loop through the comparison operators
            for (int i = 0; i < Support.DataTypeCheck.ComparisonOperators.Length; i++)
            {
                if (CheckString.EndsWith(Support.DataTypeCheck.ComparisonOperators[i]) == true)
                {
                    containsOperator = true;
                    sOperator = Support.DataTypeCheck.ComparisonOperators[i];
                    sOperand = CheckString.Substring(0, CheckString.Length - sOperator.Length);
                    break;
                }
            }
            if (containsOperator == true) return true;

            return false;

        } 
        #endregion

    }
}
