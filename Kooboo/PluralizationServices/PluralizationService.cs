using System;
using System.Globalization;
namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
	public abstract class PluralizationService
	{
		public CultureInfo Culture
		{
			get;
			protected set;
		}
		public abstract bool IsPlural(string word);
		public abstract bool IsSingular(string word);
		public abstract string Pluralize(string word);
		public abstract string Singularize(string word);
		public static PluralizationService CreateService(CultureInfo culture)
		{
			if (culture.TwoLetterISOLanguageName == "en")
			{
				return new EnglishPluralizationService();
			}
			throw new NotImplementedException("We don't support locales other than english yet");
		}
	}
}
