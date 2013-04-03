using System;
namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
	public interface ICustomPluralizationMapping
	{
		void AddWord(string singular, string plural);
	}
}
