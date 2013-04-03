using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Collections.Specialized;

namespace Kooboo.Drawing.Filters
{
  /// <summary>
  /// Summary description for Class1.
  /// </summary>
  public class SkewFilter : BasicFilter
  {
    #region Basic Members
    public const string RIGHT_SHIFT_TOKEN_NAME = "RightShift";
    public const string UP_SHIFT_TOKEN_NAME = "UpShift";

    private int _rightShift = -20; //Defualts
    private int _upShift = 0;


    public int RightShift
    {
      get
      {
        return _rightShift;
      }
      set
      {
        _rightShift = value;
      }
    }

    public int UpShift
    {
      get
      {
        return _upShift;
      }
      set
      {
        _upShift = value;
      }
    }

    /// <summary>
    /// Executes this filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>transformed image</returns>
    /// <example>
    /// <code>
    /// SkewFilter skewFilter = new SkewFilter();
    /// skewFilter.UpShift = 0;
    /// skewFilter.RightShift = 5;
    /// transformed = skewFilter.ExecuteFilter(myImg);
    /// </code>
    /// </example>
    public override Image ExecuteFilter(Image rawImage)
    {

      Bitmap result = new Bitmap(rawImage.Width + System.Math.Abs(_rightShift), rawImage.Height + System.Math.Abs(_upShift));
      Graphics g = Graphics.FromImage(result);
      g.InterpolationMode = InterpolationMode.HighQualityBicubic;
      Point[] points = new Point[3];
      int horShiftCorrections = 0;
      int verShiftCorrections = 0;
      if (_rightShift < 0)
      {
        horShiftCorrections = _rightShift * (-1);
      }
      if (_upShift < 0)
      {
        verShiftCorrections = _upShift * (-1);
      }
      points[0] = new Point(horShiftCorrections + _rightShift, verShiftCorrections);
      points[1] = new Point(horShiftCorrections + _rightShift + rawImage.Width, verShiftCorrections + _upShift);
      points[2] = new Point(horShiftCorrections, verShiftCorrections+rawImage.Height );

      try
      {
        g.DrawImage(rawImage,points);
      }
      catch
      {

      }
      return result;
    }


    #endregion

  }
}
