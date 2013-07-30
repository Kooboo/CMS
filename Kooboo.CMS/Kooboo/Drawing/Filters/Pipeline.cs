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
    /// A helper class for filters execution under the fluent methodology
    /// </summary>
    /// <example>
    ///   <code>
    /// Image transformed;
    /// ZRLabs.Yael.Pipeline pipe = new ZRLabs.Yael.Pipeline(myImg);
    /// Image transformed = pipe.RoundCorners(10, Color.White).PolariodFrame("test").CurrentImage;
    ///   </code>
    ///   </example>
    public class Pipeline
    {
        #region Properties
        private Image _image;

        public Image CurrentImage
        {
            get { return _image; }
            set { _image = value; }
        }
        #endregion

        #region .ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputImage"></param>
        /// <example>
        /// <code>
        /// Image transformed;
        /// ZRLabs.Yael.Pipeline pipe = new ZRLabs.Yael.Pipeline(myImg);
        /// Image transformed = pipe.RoundCorners(10, Color.White).PolariodFrame("test").CurrentImage;
        /// </code>
        /// </example>
        public Pipeline(Image inputImage)
        {
            _image = inputImage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline" /> class.
        /// </summary>
        /// <param name="inputFilename">The input filename.</param>
        public Pipeline(string inputFilename)
        {
            _image = Bitmap.FromFile(inputFilename);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Rotates the specified degrees.
        /// </summary>
        /// <param name="Degrees">The degrees.</param>
        /// <returns></returns>
        public Pipeline Rotate(float Degrees)
        {
            RotateFilter filter = new RotateFilter();
            filter.RotateDegrees = Degrees;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        /// <summary>
        /// Blacks the and white.
        /// </summary>
        /// <returns></returns>
        public Pipeline BlackAndWhite()
        {
            BlackAndWhiteFilter filter = new BlackAndWhiteFilter();
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        /// <summary>
        /// Watermarks the specified caption.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        public Pipeline Watermark(string caption)
        {
            TextWatermarkFilter filter = new TextWatermarkFilter();
            filter.Caption = caption;
            filter.AutomaticTextSize = true;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        /// <summary>
        /// Polariods the frame.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        public Pipeline PolariodFrame(string caption)
        {
            PolaroidFrameFilter filter = new PolaroidFrameFilter();
            filter.Caption = caption;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        //public Pipeline RoundCorners(float cornerRadius, Color background, Corner roundCorner)
        //{
        //    return this.RoundCorners(cornerRadius, background, roundCorner, 0, Color.Transparent);
        //}
        /// <summary>
        /// Rounds the corners.
        /// </summary>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="background">The background.</param>
        /// <param name="roundCorner">The round corner.</param>
        /// <returns></returns>
        public Pipeline RoundCorners(float cornerRadius, Color background, Corner roundCorner)
        {
            RoundedCornersFilter filter = new RoundedCornersFilter() { Corner = roundCorner };
            filter.CornerRadius = cornerRadius;
            filter.BackGroundColor = background;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        /// <summary>
        /// Cuts the corners.
        /// </summary>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="background">The background.</param>
        /// <param name="roundCorner">The round corner.</param>
        /// <returns></returns>
        public Pipeline CutCorners(float cornerRadius, Color background, Corner roundCorner)
        {
            CutCornersFilter filter = new CutCornersFilter() { Corner = roundCorner };
            filter.CornerRadius = cornerRadius;
            filter.BackGroundColor = background;
            _image = filter.ExecuteFilter(_image);
            return this;
        }


        /// <summary>
        /// Floors the reflection.
        /// </summary>
        /// <param name="alphaStartValue">The alpha start value.</param>
        /// <param name="alphaDecreaseRate">The alpha decrease rate.</param>
        /// <returns></returns>
        public Pipeline FloorReflection(float alphaStartValue, float alphaDecreaseRate)
        {
            FloorReflectionFilter filter = new FloorReflectionFilter();
            filter.AlphaStartValue = alphaStartValue;
            filter.AlphaDecreaseRate = alphaDecreaseRate;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        /// <summary>
        /// Skews the specified right shift.
        /// </summary>
        /// <param name="rightShift">The right shift.</param>
        /// <param name="upShift">Up shift.</param>
        /// <returns></returns>
        public Pipeline Skew(int rightShift, int upShift)
        {
            SkewFilter filter = new SkewFilter();
            filter.RightShift = rightShift;
            filter.UpShift = upShift;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        #endregion
    }
}