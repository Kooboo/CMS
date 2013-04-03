using System;
using System.Collections.Generic;
namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
	public class StringBidirectionalDictionary : BidirectionalDictionary<string, string>
	{
		public StringBidirectionalDictionary()
		{
		}
		public StringBidirectionalDictionary(Dictionary<string, string> firstToSecondDictionary) : base(firstToSecondDictionary)
		{
		}
		public override bool ExistsInFirst(string value)
		{
			return base.ExistsInFirst(value.ToLowerInvariant());
		}
		public override bool ExistsInSecond(string value)
		{
			return base.ExistsInSecond(value.ToLowerInvariant());
		}
		public override string GetFirstValue(string value)
		{
			return base.GetFirstValue(value.ToLowerInvariant());
		}
		public override string GetSecondValue(string value)
		{
			return base.GetSecondValue(value.ToLowerInvariant());
		}
	}
}
