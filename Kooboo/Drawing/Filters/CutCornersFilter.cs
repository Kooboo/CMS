using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Kooboo.Drawing.Filters
{
    public class CutCornersFilter : RoundedCornersFilter
    {
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
    }
}
