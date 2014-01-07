#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace Kooboo.Drawing.Filters
{
    /// <summary>
    /// Text Water Mark Filter .Can be used to add a watermark text to the image.
    /// The weater mark can be both horizontaly and verticaly aligned.
    /// Also provides simple captions functionality (Simple text on an image)
    /// </summary>
    public class TextWatermarkFilter : WaterMarkFilter
    {
        #region Public Properties Tokens
        /// <summary>
        /// 
        /// </summary>
        public const string WIDTH_TOKEN_NAME = "Width";
        /// <summary>
        /// 
        /// </summary>
        public const string HEIGHT_TOKEN_NAME = "Height";
        /// <summary>
        /// 
        /// </summary>
        public const string ALPHA_TOKEN_NAME = "ALPHA";
        #endregion Public Properties Tokens

        #region Private Fields
        private int _alpha = 75;
        private Color _captionColor = Color.White;
        private string _caption = "Test";
        private int _textSize = 10; //default
        private bool _automaticTextSizing = false;
        #endregion Private Fields

        #region Filter Properties
        //public HAlign Halign = HAlign.Bottom;   
        //public VAlign Valign = VAlign.Center;

        /// <summary>
        /// Gets or sets the size of the text.
        /// </summary>
        /// <value>
        /// The size of the text.
        /// </value>
        public int TextSize
        {
            get { return _textSize; }
            set { _textSize = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic text size].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic text size]; otherwise, <c>false</c>.
        /// </value>
        public bool AutomaticTextSize
        {
            get { return _automaticTextSizing; }
            set { _automaticTextSizing = value; }
        }

        /// <summary>
        /// Gets or sets the caption alpha.
        /// </summary>
        /// <value>
        /// The caption alpha.
        /// </value>
        public int CaptionAlpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
            }
        }


        /// <summary>
        /// Gets or sets the color of the caption.
        /// </summary>
        /// <value>
        /// The color of the caption.
        /// </value>
        public Color CaptionColor
        {
            get { return _captionColor; }
            set { _captionColor = value; }
        }



        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }



        #endregion Filter Properties

        #region Public Functions
        /// <summary>
        /// Executes this filter on the input image and returns the image with the WaterMark
        /// </summary>
        /// <param name="rawImage">input image</param>
        /// <returns>
        /// transformed image
        /// </returns>
        /// <example>
        ///   <code>
        /// Image transformed;
        /// TextWatermarkFilter textWaterMark = new TextWatermarkFilter();
        /// textWaterMark.Caption = "Pitzi";
        /// textWaterMark.TextSize = 20;
        /// textWaterMark.AutomaticTextSize = false;
        /// textWaterMark.Valign = TextWatermarkFilter.VAlign.Right;
        /// textWaterMark.Halign = TextWatermarkFilter.HAlign.Bottom;
        /// textWaterMark.CaptionColor = Color.Red;
        /// transformed = textWaterMark.ExecuteFilter(myImg);
        ///   </code>
        ///   </example>
        public override Image ExecuteFilter(Image rawImage)
        {

            _width = rawImage.Width;
            _height = rawImage.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(rawImage.Width, rawImage.Height, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(rawImage.HorizontalResolution, rawImage.VerticalResolution);

            //load the Bitmap into a Graphics object 
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //Draws the photo Image object at original size to the graphics object.
            grPhoto.DrawImage(
              rawImage,                               // Photo Image object
              new Rectangle(0, 0, _width, _height), // Rectangle structure
              0,                                      // x-coordinate of the portion of the source image to draw. 
              0,                                      // y-coordinate of the portion of the source image to draw. 
              _width,                                // Width of the portion of the source image to draw. 
              _height,                               // Height of the portion of the source image to draw. 
              GraphicsUnit.Pixel);                    // Units of measure 


            //-------------------------------------------------------
            //Set up the automatic font settings
            //-------------------------------------------------------
            int[] sizes = new int[] { 128, 64, 32, 16, 14, 12, 10, 8, 6, 4 };

            Font crFont = null;
            SizeF crSize = new SizeF();


            if (_automaticTextSizing)
            {
                //If automatic sizing is turned on 
                //loop through the defined sizes checking the length of the caption string string
                for (int i = 0; i < sizes.Length; i++)
                {
                    //set a Font object to Arial (i)pt, Bold
                    crFont = new Font("arial", sizes[i], FontStyle.Bold);
                    //Measure the Copyright string in this Font
                    crSize = grPhoto.MeasureString(_caption, crFont);
                    if ((ushort)crSize.Width < (ushort)_width)
                        break;
                }
            }
            else
            {
                crFont = new Font("arial", _textSize, FontStyle.Bold);

            }
            crSize = grPhoto.MeasureString(_caption, crFont);

            //Since all photographs will have varying heights, determine a 
            //position 5% from the bottom of the image
            int yPixelsMargin = (int)(_height * .0002);

            float yPosFromBottom;
            float xPositionFromLeft;
            CalcDrawPosition((int)crSize.Width, (int)crSize.Height, yPixelsMargin, out yPosFromBottom, out xPositionFromLeft);


            //Define the text layout by setting the text alignment to centered
            StringFormat StrFormat = new StringFormat();
            //StrFormat.Alignment = StringAlignment.Near;

            //define a Brush which is semi trasparent black (Alpha set to 153)
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(_alpha, 0, 0, 0));

            //Draw the Copyright string
            grPhoto.DrawString(_caption,                 //string of text
              crFont,                                   //font
              semiTransBrush2,                           //Brush
              new PointF(xPositionFromLeft + 1, yPosFromBottom + 1),  //Position
              StrFormat);

            //define a Brush which is semi trasparent white (Alpha set to 153)

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(_alpha, _captionColor.R, _captionColor.G, _captionColor.B));

            //Draw the Copyright string a second time to create a shadow effect
            //Make sure to move this text 1 pixel to the right and down 1 pixel
            grPhoto.DrawString(_caption,                 //string of text
              crFont,                                   //font
              semiTransBrush,                           //Brush
              new PointF(xPositionFromLeft, yPosFromBottom),  //Position
              StrFormat);                               //Text alignment



            grPhoto.Dispose();

            return bmPhoto;
        }




        /// <summary>
        /// Executes the filter demo.
        /// </summary>
        /// <param name="rawImage">The raw image.</param>
        /// <returns></returns>
        public override Image ExecuteFilterDemo(Image rawImage)
        {
            this.Caption = "Caption Demo";
            this.TextSize = 18;
            this.AutomaticTextSize = false;
            this.Halign = HAlign.Bottom;
            this.Valign = VAlign.Right;
            return this.ExecuteFilter(rawImage);
        }




        #endregion Public Functions

        #region Private
        #endregion Private
    }
}
