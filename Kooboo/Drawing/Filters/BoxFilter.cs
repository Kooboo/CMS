using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using System.Collections.Specialized;
namespace Kooboo.Drawing.Filters
{

  /// <summary>
  /// Box filter class. Creates an isometric box and renders the image on that specific view . gives a 3d view.
  /// </summary>
  public class BoxFilter : BasicFilter
  {
    #region TODOs
    //TODO: Add the option to paint images instead
    //of colors on the side of the box
    #endregion TODOs

    #region Private Fields
    private int _boxDepth = 10; //Defualt
    private Color _boxStartColor = Color.DarkBlue; //Defualt
    private Color _boxEndColor =  Color.LightBlue; //Defualt
    private Image _topPanelImage = null;
    private Image _sidePanelImage = null;
    
    #endregion Private Fields

    #region Filter Properties
    /// <summary>
    /// Defines the "3d" depth of the box
    /// </summary>
    public int BoxDepth
    {
      get
      {
        return _boxDepth;
      }
      set
      {
        _boxDepth = value;
      }
    }
    /// <summary>
    /// Sets the starting color of the box gradient brush
    /// </summary>
    public Color BoxStartColor
    {
      get
      {
        return _boxStartColor;
      }
      set
      {
        _boxStartColor = value;
      }
    }
    /// <summary>
    /// Sets the ending color of the box gradient brush
    /// </summary>
    public Color BoxEndColor
    {
      get
      {
        return _boxEndColor;
      }
      set
      {
        _boxEndColor = value;
      }
    }
    /// <summary>
    /// A side panel image, reset to null to turn off and use the regular gradient
    /// </summary>
    public Image SidePanelImage
    {
      get { return _sidePanelImage; }
      set { _sidePanelImage = value; }
    }
    /// <summary>
    /// A top panel image, reset to null to turn off and use the regular gradient
    /// </summary>
    public Image TopPanelImage
    {
      get { return _topPanelImage; }
      set { _topPanelImage = value; }
    }

    #endregion Filter Properties

    #region Public Filter Methods
    /// <summary>
    /// Executes this filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>transformes image into a boxed layout</returns>
    /// <example>
    /// <code>
    /// Image transformed;
    /// BoxFilter box = new BoxFilter();
    /// box.BoxDepth = 20;
    /// box.BoxStartColor = Color.Tan;
    /// box.BoxEndColor = Color.Wheat;
    /// transformed = box.ExecuteFilter(myImg);
    /// </code>
    /// </example>
    public override Image ExecuteFilter(Image inputImage)
    {

      Bitmap raw = (Bitmap)inputImage;
      double alpha = System.Math.PI / 6; //30deg
      //Setting up the box 3d depth values.
      int sideWidth = _boxDepth;
      int sideHeight = raw.Height;
      int topWidth = raw.Width;
      int topHeight = sideWidth;
      int totalWidth = (int)(sideWidth * System.Math.Cos(alpha) + raw.Width * System.Math.Cos(alpha));
      int totalHeight = (int)(raw.Height + raw.Width * System.Math.Sin(alpha) + sideWidth * System.Math.Sin(alpha));

      //Set up the new canvas
      Bitmap result = new Bitmap(totalWidth,totalHeight);
      Graphics g = Graphics.FromImage(result);
      g.InterpolationMode = InterpolationMode.HighQualityBicubic;
      //Set background
      g.FillRectangle(new SolidBrush(BackGroundColor), 0, 0, result.Width, result.Height);
      
      //FrontSide
      //Point rightBottom = new Point((int)(raw.Width * System.Math.Cos(alpha)) , raw.Height - (int)(raw.Width * System.Math.Sin(alpha)) + yAlign);
      Point leftTop = new Point((int)(sideWidth*System.Math.Cos(alpha)),(int)((sideWidth+raw.Width)*System.Math.Sin(alpha)));
      Point leftBottom = new Point((int)(sideWidth * System.Math.Cos(alpha)), raw.Height + (int)((sideWidth + raw.Width) * System.Math.Sin(alpha)));
      Point rightTop = new Point((int)((raw.Width+sideWidth) * System.Math.Cos(alpha)) , (int)(sideWidth*System.Math.Sin(alpha)));
      g.DrawImage(raw, new Point[] { leftTop, rightTop, leftBottom });

      
      //TopSide
      Point topUpperRight = new Point(rightTop.X - (int)(topHeight * System.Math.Cos(alpha)), rightTop.Y - (int)(topHeight * System.Math.Sin(alpha)));
      Point topLowerRight = new Point(rightTop.X, rightTop.Y);
      Point topLowerLeft = new Point(leftTop.X, leftTop.Y);
      Point topUpperLeft = new Point(leftTop.X - (int)(sideWidth * System.Math.Cos(alpha)), leftTop.Y - (int)(sideWidth * System.Math.Sin(alpha)));
      Point[] top = new Point[4];
      top[0] = topUpperLeft;
      top[1] = topUpperRight;
      top[2] = topLowerRight;
      top[3] = topLowerLeft;
      LinearGradientBrush topBrush = new LinearGradientBrush(topLowerRight, topUpperLeft, _boxStartColor, _boxEndColor);
      if (_topPanelImage != null)
        g.DrawImage(_topPanelImage, new Point[] { topUpperLeft, topUpperRight, topLowerLeft });
      else
        g.FillPolygon(topBrush, top);

      //LeftSide
      Point sideUpperRight = new Point(leftTop.X, leftTop.Y);
      Point sideLowerRight = new Point(leftBottom.X, leftBottom.Y);
      Point sideLowerLeft = new Point(leftBottom.X - (int)(sideWidth * System.Math.Cos(alpha)), leftBottom.Y - (int)(sideWidth * System.Math.Sin(alpha)));
      Point sideUpperLeft = new Point(leftTop.X - (int)(sideWidth * System.Math.Cos(alpha)), leftTop.Y - (int)(sideWidth * System.Math.Sin(alpha)));
      Point[] side = new Point[4];
      side[0] = sideUpperLeft;
      side[1] = sideUpperRight;
      side[2] = sideLowerRight;
      side[3] = sideLowerLeft;
      LinearGradientBrush sideBrush = new LinearGradientBrush(sideUpperLeft, sideLowerRight, _boxStartColor, _boxEndColor);
      if (_sidePanelImage != null)
        g.DrawImage(_sidePanelImage, new Point[] { sideUpperLeft, sideUpperRight, sideLowerLeft });
      else
        g.FillPolygon(sideBrush, side);

      return result;
    }
    #endregion Public Filter Methods
  }
}
