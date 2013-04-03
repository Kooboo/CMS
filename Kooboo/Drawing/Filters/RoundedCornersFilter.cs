using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Collections.Specialized;

namespace Kooboo.Drawing.Filters
{

    /// <summary>
    /// 图片圆角的处理
    /// 
    /// <remarks>下面这种方案效果仍然不是完美的，当处理颜色变化转大的图片（比较渐变）时，会有很明显的分隔线 </remarks>
    /// 尝试了很多种方案，但效果都不理想
    /// 最后在这个 http://forums.asp.net/p/942160/1130380.aspx 方案的基础之上进行修改
    /// 终于实现了无锯齿的图片圆角处理
    /// 以上方案过期，最终使用了把原始图片作为画刷，然后用这个画刷来画圆角矩形，目前来看应该是能达到完美效果。   
    /// </summary>
    public class RoundedCornersFilter : BasicFilter
    {
        public RoundedCornersFilter()
        {
            Corner = Corner.All;
            this.BackGroundColor = Color.Transparent;
        }
        #region Public Properties Tokens
        public static string ROTATE_DEGREES_TOKEN = "radius";
        #endregion Public Properties Tokens

        #region Private Fields
        private float _cornerRadius = 50; //Default
        #endregion Private Fields


        #region Filter Properties
        /// <summary>
        /// Determins the corner's radius. in pixels
        /// </summary>
        public float CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                if (value > 0)
                    _cornerRadius = value;
                else
                    _cornerRadius = 0;
            }
        }

        public Corner Corner
        {
            get;
            set;
        }
        #endregion Filter Properties

        #region Public Filter Methods
        /// <summary>
        /// Executes this curved corners 
        /// filter on the input image and returns the result
        /// Make sure you set the BackGroundColor property before running this filter.
        /// </summary>
        /// <param name="inputImage">input image</param>
        /// <returns>Curved Corner Image</returns>
        /// <example>
        /// <code>
        /// Image transformed;
        /// RoundedCornersFilter rounded = new RoundedCornersFilter();
        /// rounded.BackGroundColor = Color.FromArgb(255, 255, 255, 255);
        /// rounded.CornerRadius = 15;
        /// transformed = rounded.ExecuteFilter(myImg);
        /// </code>
        /// </example>
        public override Image ExecuteFilter(Image inputImage)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(inputImage.Width, inputImage.Height);
            bitmap.MakeTransparent();

            Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.Clear(BackGroundColor);

            Brush brush = new System.Drawing.TextureBrush(inputImage);

            //宽度高度减1，可以让图片右侧和下侧不会贴边
            //这样才可以与左边和上侧对称
            //因为锯齿效果的影响，GDI+会对图片的最外一个像素进行半透明处理，因此从-1,-1开始，去掉半透明边
            g.FillRoundedRectangle(brush, -1, -1, inputImage.Width+1, inputImage.Height+1, CornerRadius, Corner);
            //FillRoundedRectangle(g, new Rectangle(0, 0, inputImage.Width, inputImage.Height), (int)CornerRadius, brush);

            return bitmap;
        }



        //private void FillRoundedRectangle(Graphics g, Rectangle r, int d, Brush b)
        //{

        //    // anti alias distorts fill so remove it.

        //    //System.Drawing.Drawing2D.SmoothingMode mode = g.SmoothingMode;

        //    var remainder = d % 2;


        //    if ((Corner & Filters.Corner.TopLeft) == Filters.Corner.TopLeft)
        //    {
        //        g.FillPie(b, r.X, r.Y, d, d, 180, 90);
        //    }
        //    else
        //    {
        //        var rectWidth = d / 2 + 1;
        //        g.FillRectangle(b, r.X, r.Y, rectWidth, rectWidth);
        //    }
        //    if ((Corner & Filters.Corner.TopRight) == Filters.Corner.TopRight)
        //    {
        //        g.FillPie(b, r.X + r.Width - d - 2, r.Y, d + 1, d + 1, 270, 90);
        //    }
        //    else
        //    {
        //        var rectWidth = d / 2 + 1;
        //        g.FillRectangle(b, r.X + r.Width - rectWidth, r.Y, rectWidth - 1, rectWidth);
        //    }
        //    if ((Corner & Filters.Corner.ButtomLeft) == Filters.Corner.ButtomLeft)
        //    {
        //        g.FillPie(b, r.X, r.Y + r.Height - d - 2, d + 1, d + 1, 90, 90);
        //    }
        //    else
        //    {
        //        var rectWidth = d / 2 + 1;
        //        g.FillRectangle(b, r.X, r.Y + r.Height - rectWidth, rectWidth + 1, rectWidth - 1);
        //    }
        //    if ((Corner & Filters.Corner.ButtomRight) == Filters.Corner.ButtomRight)
        //    {
        //        g.FillPie(b, r.X + r.Width - d - 2, r.Y + r.Height - d - 2, d + 1, d + 1, 0, 90);
        //    }
        //    else
        //    {
        //        var rectWidth = d / 2 + 1;
        //        g.FillRectangle(b, r.X + r.Width - rectWidth, r.Y + r.Height - rectWidth, rectWidth - 1, rectWidth - 1);
        //    }


        //    g.FillRectangle(b, r.X + d / 2, r.Y, r.Width - d, d / 2 + 1);

        //    g.FillRectangle(b, r.X, r.Y + d / 2, r.Width - 1, r.Height - d);

        //    g.FillRectangle(b, r.X + d / 2, r.Y + r.Height - d / 2 - 2, r.Width - d, d / 2 + 1);

        //    //g.SmoothingMode = mode;


        //}


        public override Image ExecuteFilterDemo(Image inputImage)
        {
            this.BackGroundColor = Color.FromArgb(255, 255, 255, 255);
            return this.ExecuteFilter(inputImage);
        }
        #endregion Public Filter Methods
    }
}
