using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
	public static class PluralizationServiceUtil
	{
		public static bool DoesWordContainSuffix(string word, IEnumerable<string> suffixes, CultureInfo culture)
		{
			return suffixes.Any((string s) => word.EndsWith(s, true, culture));
		}
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
	}
}
