#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Kooboo.Drawing.Filters
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class FloorReflectionFilter : BasicFilter
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        public const string WIDTH_TOKEN_NAME = "WIDTH";
        /// <summary>
        /// 
        /// </summary>
        public const string HEIGHT_TOKEN_NAME = "HEIGHT";

        //Defualts
        private int _width = 200;
        private int _height = 200;
        private float _alphaStartValue = 110;
        private float _alphaDecreaseRate = 4;

        #endregion

        #region Properties

        /// <summary>
        /// Sets the initial transparecy value of the reflection.
        /// </summary>
        public float AlphaStartValue
        {
            get
            {
                return _alphaStartValue;
            }
            set
            {
                _alphaStartValue = value;
            }
        }


        /// <summary>
        /// Sets the decrease rate value of the transparency
        /// </summary>
        public float AlphaDecreaseRate
        {
            get
            {
                return _alphaDecreaseRate;
            }
            set
            {
                _alphaDecreaseRate = value;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Executes this filter on the input image and returns the result
        /// </summary>
        /// <param name="rawImage">The raw image.</param>
        /// <returns>
        /// transformed image
        /// </returns>
        /// <example>
        ///   <code>
        /// Image transformed;
        /// FloorReflectionFilter floorReflection = new FloorReflectionFilter();
        /// floorReflection.AlphaDecreaseRate = 3;
        /// transformed = floorReflection.ExecuteFilter(myImg);
        ///   </code>
        ///   </example>
        public override Image ExecuteFilter(Image rawImage)
        {
            _width = rawImage.Width;
            _height = rawImage.Height;

            int reflectionRows = (int)((_alphaStartValue) / _alphaDecreaseRate);
            Bitmap result = new Bitmap(_width, _height + reflectionRows);
            Graphics g = Graphics.FromImage(result);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(rawImage, 0, 0, (float)_width, (float)_height);
            Color pixelColor, newColor;
            Bitmap raw = (Bitmap)rawImage;
            int i, j;
            try
            {
                for (i = 0; i < reflectionRows - 1; i++)
                {
                    for (j = 0; j < _width - 1; j++)
                    {
                        pixelColor = raw.GetPixel(j, _height - i - 1);
                        newColor = Color.FromArgb((pixelColor.A) * ((int)_alphaStartValue - (i * (int)_alphaDecreaseRate)) / 255, pixelColor.R, pixelColor.G, pixelColor.B);
                        g.DrawRectangle(new Pen(newColor), j, i + _height - 1, 1, 1);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            //for (int i=1 ToolboxBitmapAttribute 
            Pen myTestPen = new Pen(Color.FromArgb(100, 5, 5, 5));
            //g.FillRectangle(new SolidBrush(Color.FromArgb(100, 5, 5, 5)), 0, 375, 150, 100);

            return result;
        }

        #endregion
    }
}
