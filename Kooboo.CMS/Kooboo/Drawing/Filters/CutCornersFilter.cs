#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Drawing;

namespace Kooboo.Drawing.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class CutCornersFilter : RoundedCornersFilter
    {
        #region Methods
        /// <summary>
        /// Executes this curved corners
        /// filter on the input image and returns the result
        /// Make sure you set the BackGroundColor property before running this filter.
        /// </summary>
        /// <param name="inputImage">input image</param>
        /// <returns>
        /// Curved Corner Image
        /// </returns>
        /// <example>
        ///   <code>
        /// Image transformed;
        /// RoundedCornersFilter rounded = new RoundedCornersFilter();
        /// rounded.BackGroundColor = Color.FromArgb(255, 255, 255, 255);
        /// rounded.CornerRadius = 15;
        /// transformed = rounded.ExecuteFilter(myImg);
        ///   </code>
        ///   </example>
        public override Image ExecuteFilter(Image inputImage)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(inputImage.Width, inputImage.Height);
            bitmap.MakeTransparent();

            Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.Clear(BackGroundColor);

            Brush brush = new System.Drawing.TextureBrush(inputImage);

            g.FillCutRectangle(brush, 0, 0, inputImage.Width, inputImage.Height, CornerRadius, Corner);
            //FillRoundedRectangle(g, new Rectangle(0, 0, inputImage.Width, inputImage.Height), (int)CornerRadius, brush);

            return bitmap;
        }
        #endregion
    }
}
