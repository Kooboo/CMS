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
using System.Drawing.Imaging;


namespace Kooboo.Drawing.Filters
{
    /// <summary>
    /// Text Water Mark Filter .Can be used to add a watermark text to the image.
    /// The weater mark can be both horizontaly and verticaly aligned.
    /// Also provides simple captions functionality (Simple text on an image)
    /// </summary>
    public class ImageWatermarkFilter : WaterMarkFilter
    {
        #region Public Properties Tokens
        public const string WIDTH_TOKEN_NAME = "Width";
        public const string HEIGHT_TOKEN_NAME = "Height";
        public const string ALPHA_TOKEN_NAME = "ALPHA";
        #endregion Public Properties Tokens

        #region Private Fields
        private float _alpha = 0.3f; //default
        private Image _waterMarkImage;
        private Color _transparentColor = Color.FromArgb(255, 255, 255, 255);
        #endregion Private Fields

        #region Filter Properties
        //public HAlign Halign = HAlign.Bottom;
        //public VAlign Valign = VAlign.Center;




        /// <summary>
        /// A value between 0 to 1.0. Sets the opacity value
        /// </summary>
        public float Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                if ((value > 1) || (value < 0))
                    throw new Exception("Error setting the opacity value");
                else
                    _alpha = value;
            }
        }




        public Image WaterMarkImage
        {
            get { return _waterMarkImage; }
            set { _waterMarkImage = value; }
        }


        public Color TransparentColor
        {
            get { return _transparentColor; }
            set { _transparentColor = value; }
        }





        #endregion Filter Properties

        #region Public Functions
        /// <summary>
        /// Executes this filter on the input image and returns the image with the WaterMark
        /// </summary>
        /// <param name="rawImage">input image</param>
        /// <returns>transformed image</returns>
        /// <example>
        /// <code>
        /// Image transformed;
        /// ImageWatermarkFilter imageWaterMark = new ImageWatermarkFilter();
        /// imageWaterMark.Valign = ImageWatermarkFilter.VAlign.Right;
        /// imageWaterMark.Halign = ImageWatermarkFilter.HAlign.Bottom;
        /// imageWaterMark.WaterMarkImage = Image.FromFile("Images/pacman.gif");
        /// transformed = imageWaterMark.ExecuteFilter(myImg);
        /// </code>
        /// </example>
        public override Image ExecuteFilter(Image rawImage)
        {
            _height = rawImage.Height;
            _width = rawImage.Width;
            Bitmap bmWatermark = new Bitmap(rawImage);
            bmWatermark.SetResolution(rawImage.HorizontalResolution, rawImage.VerticalResolution);
            //Load this Bitmap into a new Graphic Object
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //To achieve a transulcent watermark we will apply (2) color 
            //manipulations by defineing a ImageAttributes object and 
            //seting (2) of its properties.
            ImageAttributes imageAttributes = new ImageAttributes();

            //The first step in manipulating the watermark image is to replace 
            //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
            //to do this we will use a Colormap and use this to define a RemapTable
            ColorMap colorMap = new ColorMap();

            //My watermark was defined with a background of 100% Green this will
            //be the color we search for and replace with transparency
            colorMap.OldColor = _transparentColor;
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            //The second color manipulation is used to change the opacity of the 
            //watermark.  This is done by applying a 5x5 matrix that contains the 
            //coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
            //to 0.3f we achive a level of opacity
            float[][] colorMatrixElements = { 
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},       
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  _alpha, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
              ColorAdjustType.Bitmap);

            //For this example we will place the watermark in the upper right
            //hand corner of the photograph. offset down 10 pixels and to the 
            //left 10 pixles


            float xPosOfWm;// = ((rawImage.Width - _waterMarkImage.Width) - 2);
            float yPosOfWm;
            CalcDrawPosition((int)_waterMarkImage.Width, (int)_waterMarkImage.Height, 0, out yPosOfWm, out xPosOfWm);

            grWatermark.DrawImage(_waterMarkImage,
              new Rectangle((int)xPosOfWm, (int)yPosOfWm, _waterMarkImage.Width, _waterMarkImage.Height),  //Set the detination Position
              0,                  // x-coordinate of the portion of the source image to draw. 
              0,                  // y-coordinate of the portion of the source image to draw. 
              _waterMarkImage.Width,            // Watermark Width
              _waterMarkImage.Height,		    // Watermark Height
              GraphicsUnit.Pixel, // Unit of measurment
              imageAttributes);   //ImageAttributes Object

            //Replace the original photgraphs bitmap with the new Bitmap
            //imgPhoto = bmWatermark;
            //grWatermark.Dispose();

            //save new image to file system.
            return bmWatermark; // bmPhoto;
        }

        public override Image ExecuteFilterDemo(Image rawImage)
        {
            //this.Caption = "Caption Demo";
            //this.TextSize = 18;
            //this.AutomaticTextSize = false;
            //this.Halign = HAlign.Bottom;
            //this.Valign = VAlign.Right;
            return null;//this.ExecuteFilter(rawImage);
        }



        #endregion Public Functions

        #region Private
        #endregion Private
    }
}
