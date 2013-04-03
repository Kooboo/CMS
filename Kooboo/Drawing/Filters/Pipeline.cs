using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Kooboo.Drawing.Filters;


namespace Kooboo.Drawing.Filters
{
    /// <summary>
    /// A helper class for filters execution under the fluent methodology
    /// </summary>
    /// <example>
    /// <code>
    /// Image transformed;
    /// ZRLabs.Yael.Pipeline pipe = new ZRLabs.Yael.Pipeline(myImg);
    /// Image transformed = pipe.RoundCorners(10, Color.White).PolariodFrame("test").CurrentImage;
    /// </code>
    /// </example>
    public class Pipeline
    {
        private Image _image;

        public Image CurrentImage
        {
            get { return _image; }
            set { _image = value; }
        }

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

        public Pipeline(string inputFilename)
        {
            _image = Bitmap.FromFile(inputFilename);
        }

        public Pipeline Rotate(float Degrees)
        {
            RotateFilter filter = new RotateFilter();
            filter.RotateDegrees = Degrees;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        public Pipeline BlackAndWhite()
        {
            BlackAndWhiteFilter filter = new BlackAndWhiteFilter();
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        public Pipeline Watermark(string caption)
        {
            TextWatermarkFilter filter = new TextWatermarkFilter();
            filter.Caption = caption;
            filter.AutomaticTextSize = true;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

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
        public Pipeline RoundCorners(float cornerRadius, Color background, Corner roundCorner)
        {
            RoundedCornersFilter filter = new RoundedCornersFilter() { Corner = roundCorner };
            filter.CornerRadius = cornerRadius;
            filter.BackGroundColor = background;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        public Pipeline CutCorners(float cornerRadius, Color background, Corner roundCorner)
        {
            CutCornersFilter filter = new CutCornersFilter() { Corner = roundCorner };
            filter.CornerRadius = cornerRadius;
            filter.BackGroundColor = background;
            _image = filter.ExecuteFilter(_image);
            return this;
        }


        //public Pipeline FishEye(float curvature)
        //{
        //    FisheyeFilter filter = new FisheyeFilter();
        //    filter.Curvature = curvature;
        //    _image = filter.ExecuteFilter(_image);
        //    return this;
        //}

        public Pipeline FloorReflection(float alphaStartValue, float alphaDecreaseRate)
        {
            FloorReflectionFilter filter = new FloorReflectionFilter();
            filter.AlphaStartValue = alphaStartValue;
            filter.AlphaDecreaseRate = alphaDecreaseRate;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

        public Pipeline Skew(int rightShift, int upShift)
        {
            SkewFilter filter = new SkewFilter();
            filter.RightShift = rightShift;
            filter.UpShift = upShift;
            _image = filter.ExecuteFilter(_image);
            return this;
        }

    }
}