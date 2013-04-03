using System;
using System.Drawing;
using System.Collections.Specialized;

namespace Kooboo.Drawing.Filters
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public interface IFilter
	{
	  Image ExecuteFilter(Image inputImage);
	}
}
