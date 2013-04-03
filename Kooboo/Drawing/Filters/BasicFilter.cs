using System;
using System.Drawing;
using System.Collections.Specialized;
using System.Collections;



namespace Kooboo.Drawing.Filters
{
	/// <summary>
	/// Summary description for BasicFilter.
	/// </summary>
	public abstract class BasicFilter :  IFilter
  {
    #region Privates
    private Color _backColor = Color.FromArgb(0,0,0,0); // Default is a transparent background
    #endregion Privates

    public Color BackGroundColor
    {
      get
      {
        return _backColor;
      }
      set
      {
        _backColor = value;
      }

    }

    /// <summary>
    /// Executes this filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>transformed image</returns>
    public abstract Image ExecuteFilter(Image inputImage);
    
    /// <summary>
    /// Demonostration Function. Could be left unimplimented.
    /// To be used for presentation purposes.
    /// </summary>
    /// <param name="inputImage"></param>
    /// <returns></returns>
    public virtual Image ExecuteFilterDemo(Image inputImage)
    {
      return this.ExecuteFilter(inputImage);
      //throw new MissingMethodException();
    }
	}
}
