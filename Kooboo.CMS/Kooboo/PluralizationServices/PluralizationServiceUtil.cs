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
using System.Globalization;
using System.Linq;
namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
    /// <summary>
    /// 
    /// </summary>
	public static class PluralizationServiceUtil
	{
        #region Method
        /// <summary>
        /// Doeses the word contain suffix.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="suffixes">The suffixes.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static bool DoesWordContainSuffix(string word, IEnumerable<string> suffixes, CultureInfo culture)
        {
            return suffixes.Any((string s) => word.EndsWith(s, true, culture));
        }
        /// <summary>
        /// Tries the get matched suffix for word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="suffixes">The suffixes.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="matchedSuffix">The matched suffix.</param>
        /// <returns></returns>
        public static bool TryGetMatchedSuffixForWord(string word, IEnumerable<string> suffixes, CultureInfo culture, out string matchedSuffix)
        {
            matchedSuffix = null;
            if (PluralizationServiceUtil.DoesWordContainSuffix(word, suffixes, culture))
            {
                matchedSuffix = suffixes.First((string s) => word.EndsWith(s, true, culture));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Tries the inflect on suffix in word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="suffixes">The suffixes.</param>
        /// <param name="operationOnWord">The operation on word.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="newWord">The new word.</param>
        /// <returns></returns>
        public static bool TryInflectOnSuffixInWord(string word, IEnumerable<string> suffixes, Func<string, string> operationOnWord, CultureInfo culture, out string newWord)
        {
            newWord = null;
            string text;
            if (PluralizationServiceUtil.TryGetMatchedSuffixForWord(word, suffixes, culture, out text))
            {
                newWord = operationOnWord(word);
                return true;
            }
            return false;
        } 
        #endregion
	}
}
