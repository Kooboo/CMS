//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
//
//using System.Collections.Specialized;


//namespace Kooboo.Drawing.Filters
//{
//  /// <summary>
//  /// Summary description for Class1.
//  /// </summary>
//  public class MatrixTransformFilter : BasicFilter
//  {
//    #region IFilter Members
//    public const string RIGHT_SHIFT_TOKEN_NAME = "RightShift";
//    public const string UP_SHIFT_TOKEN_NAME = "UpShift";

//    private int _rightShift = 200; //Defualts
//    private int _upShift = 20;

//    /// <summary>
//    /// Needs some more work
//    /// </summary>
//    /// <param name="rawImage"></param>
//    /// <returns></returns>
//    public override Image ExecuteFilter(Image rawImage)
//    {
//      Bitmap raw = (Bitmap)rawImage;




//      double xRotation, yRotation, zRotation;
//      xRotation = 0.6;
//      yRotation = -0.7;
//      zRotation = 0;
//      double a11, a12, a13, a21, a22, a23, a31, a32, a33;
//      a11 = a12 = a13 = a21 = a22 = a23 = a31 = a32 = a33 = 0;
//      a33 = a11 = a22 = 1;
//      a33 = 1;
//      a21 = 0;
//      //a11 = 0.95; // The same as a22
//      //a12 = shear X (-0.2)
//      //a13 = x transform
//      //a21 = Shear Y (-0.2)
//      //a22 = -1; //??
//      //a23 = y transform
//      //a31 = Y axis rotation 0.005
//      //a32 = X axis rotation 0.005
//      //a33 = z back and forth

//      //a21 = -0.2;
//      //a13 = 50;
//      //a23 = 50;
//      //a21 = System.Math.PI/6;
//      a31 = 0.005;

//      //a21 = -0.2; //shear
//      //a12 = 0.2;
//      //a32 = 0.0005;

//      double x, y, transX, transY, maxX, maxY;
//      Pen pen = new Pen(Color.Black);

//      //Get the maximum width and height values
//      //Top Right
//      maxX = (a11 * (rawImage.Width) + a12 * (0) + a13) / (a31 * (rawImage.Width) + a32 * (0) + a33);
//      //Lower Left corner
//      maxY = (a21 * (0) + a22 * (rawImage.Height) + a23) / (a31 * (0) + a32 * (rawImage.Height) + a33);

//      Bitmap result = new Bitmap((int)maxX + 1, (int)maxY + 2);


//      Graphics g = Graphics.FromImage(result);
//      //Graphics g2 = Graphics.FromImage(rawImage);
//      g.InterpolationMode = InterpolationMode.HighQualityBicubic;

//      for (x = 0; x < rawImage.Width; x++)
//      {
//        for (y = 0; y < rawImage.Height; y++)
//        {
//          //transX = GetTranslatedX(x, y, 0, xRotation, yRotation, zRotation);
//          //transY = GetTranslatedY(x, y, 0, xRotation, yRotation, zRotation);

//          //transX = (a11 * (x - 100) + a12 * (y - 50) + a13) / (a31 * (x - 100) + a32 * (y - 50) + a33);
//          //transY = (a21 * (x - 100) + a22 * (y - 50) + a23) / (a31 * (x - 100) + a32 * (y - 50) + a33);
//          transX = (a11 * (x) + a12 * (y) + a13) / (a31 * (x) + a32 * (y) + a33);
//          transY = (a21 * (x) + a22 * (y) + a23) / (a31 * (x) + a32 * (y) + a33);
//          //transZ = GetTranslatedZ(x, y, 0, xRotation, yRotation, zRotation);

//          if (transX < maxX && transY < maxY)
//          {
//            Color srcColor = raw.GetPixel((int)x, (int)y);
//            Color exsitingColor = result.GetPixel((int)transX, (int)transY);
//            if ((srcColor.A > 0 && srcColor.A < 255))
//            {
//              pen.Color = Color.FromArgb((int)((y * 255) / raw.Height), srcColor.R, srcColor.G, srcColor.B);
//            }
//            else
//              pen.Color = Color.FromArgb(srcColor.A, srcColor.R, srcColor.G, srcColor.B);
//            g.DrawRectangle(pen, (float)transX, (float)transY + 1, 1, 1);
//          }
//        }
//      }
//      return result;
//    }

//    private double GetTranslatedX(double x, double y, double z, double xRotation, double yRotation, double zRotation)
//    {
//      double calc = 0;
//      //x
//      calc += (System.Math.Cos(zRotation) * System.Math.Cos(yRotation)) * x;
//      //y
//      calc += (System.Math.Sin(zRotation) * System.Math.Cos(yRotation)) * (-1) * y;
//      //y
//      calc += (System.Math.Sin(yRotation)) * z;

//      return calc;
//    }
//    private double GetTranslatedY(double x, double y, double z, double xRotation, double yRotation, double zRotation)
//    {
//      double calc = 0;
//      //x
//      calc += ((System.Math.Cos(zRotation) * System.Math.Sin(yRotation) * System.Math.Sin(xRotation)) + (System.Math.Sin(zRotation) * System.Math.Cos(xRotation))) * x;
//      //y
//      calc += ((System.Math.Cos(zRotation) * System.Math.Cos(xRotation)) - (System.Math.Sin(xRotation) * System.Math.Sin(yRotation) * System.Math.Sin(zRotation))) * y;
//      //y
//      calc += (System.Math.Cos(yRotation) * System.Math.Sin(xRotation)) * (-1) * z;

//      return calc;

//    }
//    private double GetTranslatedZ(double x, double y, double z, double xRotation, double yRotation, double zRotation)
//    {
//      double calc = 0;
//      //x
//      calc += ((System.Math.Sin(zRotation) * System.Math.Sin(xRotation)) - (System.Math.Cos(zRotation) * System.Math.Sin(yRotation) * System.Math.Cos(xRotation))) * x;
//      //y
//      calc += ((System.Math.Sin(zRotation) * System.Math.Sin(yRotation) * System.Math.Cos(xRotation)) + (System.Math.Sin(xRotation) * System.Math.Cos(yRotation))) * y;
//      //y
//      calc += (System.Math.Cos(yRotation) * System.Math.Cos(xRotation)) * z;
//      return calc;
//    }


//    #endregion

//  }
//}
