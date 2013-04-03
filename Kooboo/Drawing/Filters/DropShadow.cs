using System;
using System.Drawing;
using System.Reflection;
using System.Resources;

using System.Collections.Specialized;

namespace Kooboo.Drawing.Filters
{
  #region TODOs
  //TODO: 
  //1.For non rectengular images, add an edge detection algo, the drops 
  //the shadow according to the object in the image.
  //2.Can be done with gradint brushes
  //3.Add shadow direction options.
  #endregion TODOs

  /// <summary>
  /// DropShadowFilter. adds the picture a drop shadow as if the image is hovering 
  /// above the floor or infront of the wall.
  /// </summary>
  public class DropShadowFilter : BasicFilter
  {
    /// <summary>
    /// Executes this drop shadow 
    /// filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>Shadow Dropped Image</returns>
    /// <example>
    /// <code>
    /// Image transformed;
    /// DropShadowFilter dropShadow = new DropShadowFilter();
    /// transformed = dropShadow.ExecuteFilter(myImg);
    /// </code>
    /// </example>
    public override Image ExecuteFilter(Image inputImage)
    {
      int rightMargin = 4;
      int bottomMargin = 4;
      
      //Get the shadow image
      Assembly asm = Assembly.GetExecutingAssembly();
      ResourceManager rm = new ResourceManager("Kooboo.Drawing.Filters.Images", asm);
      Bitmap shadow = (Bitmap)rm.GetObject("img");


      
      Bitmap fullImage = new Bitmap(inputImage.Width + 6, inputImage.Height + 6);
      Graphics g = Graphics.FromImage(fullImage);
      g.DrawImage(inputImage, 0, 0, inputImage.Width, inputImage.Height);
      GraphicsUnit units = GraphicsUnit.Pixel;


      //Draw the shadow's right lower corner
      Point ulCorner = new Point(fullImage.Width - 6, fullImage.Height - 6);
      Point urCorner = new Point(fullImage.Width, fullImage.Height - 6);
      Point llCorner = new Point(fullImage.Width - 6, fullImage.Height);
      Point[] destPara = { ulCorner, urCorner, llCorner };
      Rectangle srcRect = new Rectangle(shadow.Width - 6, shadow.Height - 6, 6, 6);
      g.DrawImage(shadow, destPara, srcRect, units);

      //Draw the shadow's right hand side
      ulCorner = new Point(fullImage.Width - 6, bottomMargin);
      urCorner = new Point(fullImage.Width, bottomMargin);
      llCorner = new Point(fullImage.Width - 6, fullImage.Height - 6);
      destPara = new Point[] { ulCorner, urCorner, llCorner };
      srcRect = new Rectangle(shadow.Width - 6, 0, 6, shadow.Height - 6);
      g.DrawImage(shadow, destPara, srcRect, units);

      //Draw the shadow's bottom hand side
      ulCorner = new Point(rightMargin, fullImage.Height - 6);
      urCorner = new Point(fullImage.Width - 6, fullImage.Height - 6);
      llCorner = new Point(rightMargin, fullImage.Height);
      destPara = new Point[] { ulCorner, urCorner, llCorner };
      srcRect = new Rectangle(0, shadow.Height - 6, shadow.Width - 6, 6);
      g.DrawImage(shadow, destPara, srcRect, units);

      return fullImage;
    }
    
  }
}
