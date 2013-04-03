using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using System.Collections.Specialized;
using System.Collections;

namespace Kooboo.Drawing.Filters
{
  /// <summary>
  /// Black And White filter class. Turns color images to black and white images.
  /// </summary>
  public class BlackAndWhiteFilter : BasicFilter
  {

    #region Private Fields
    private bool _isBright = true;
    #endregion Private Fields

    #region Public Properties Tokens

    /// <summary>
    /// Gets or sets the grayscale brightness. true for bright, dalse for dark.
    /// </summary>
    public bool Brighter
    {
      get { return _isBright; }
      set { _isBright = value; }
    }

    #endregion Public Properties Tokens

    #region Public Filter Methods
    /// <summary>
    /// Executes this filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>transformed image</returns>
    public override Image ExecuteFilter(Image inputImage)
    {
      ///Reference from http://www.bobpowell.net/grayscale.htm
      Bitmap bm = new Bitmap(inputImage.Width, inputImage.Height);
      Graphics g = Graphics.FromImage(bm);
      ColorMatrix cm;

      if (_isBright)
      {
        cm = new ColorMatrix(new float[][]{   new float[]{0.5f,0.5f,0.5f,0,0},
                                  new float[]{0.5f,0.5f,0.5f,0,0},
                                  new float[]{0.5f,0.5f,0.5f,0,0},
                                  new float[]{0,0,0,1,0,0},
                                  new float[]{0,0,0,0,1,0},
                                  new float[]{0,0,0,0,0,1}});
      }
      else
      {

        //Gilles Khouzams colour corrected grayscale shear
        cm = new ColorMatrix(new float[][]{   new float[]{0.3f,0.3f,0.3f,0,0},
                                new float[]{0.59f,0.59f,0.59f,0,0},
                                new float[]{0.11f,0.11f,0.11f,0,0},
                                new float[]{0,0,0,1,0,0},
                                new float[]{0,0,0,0,1,0},
                                new float[]{0,0,0,0,0,1}});
      }

      ImageAttributes ia = new ImageAttributes();
      ia.SetColorMatrix(cm);
      g.DrawImage(inputImage, new Rectangle(0, 0, inputImage.Width, inputImage.Height), 0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel, ia);
      g.Dispose();
      return bm;
    }

    /// <summary>
    /// Demonostration Function. Calls the filter with default properties
    /// To be used for presentation purposes.
    /// </summary>
    /// <param name="inputImage"></param>
    /// <returns>Transformed image</returns>
    /// <summary>
    public Image ExecuteFilterDemo(Image inputImage, NameValueCollection filterProperties)
    {
      return this.ExecuteFilter(inputImage);
    }
    #endregion Public Filter Methods

  }
}
