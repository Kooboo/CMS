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

using System.Collections.Specialized;

namespace Kooboo.Drawing.Filters
{
    /// <summary>
    /// Rotation filter class.
    /// References were taken from the following code example
    /// http://www.codeproject.com/csharp/rotateimage.asp
    /// </summary>
    public class RotateFilter : BasicFilter
    {
        #region Public Properties Tokens
        public static string ROTATE_DEGREES_TOKEN = "degress";
        #endregion Public Properties Tokens

        #region Private Fields
        private float _rotateDegrees = -25; //Default , is a little turn counter clockwise
        #endregion Private Fields

        #region Filter Properties
        /// <summary>
        /// Rotation degrees value. (x/360)
        /// </summary>
        public float RotateDegrees
        {
            get
            {
                return _rotateDegrees;
            }
            set
            {
                _rotateDegrees = value;
            }
        }
        #endregion Filter Properties

        #region Public Filter Methods
        /// <summary>
        /// Executes this filter on the input image and returns the result
        /// </summary>
        /// <param name="inputImage">input image</param>
        /// <returns>rotated image</returns>
        /// <example>
        /// <code>
        /// Image transformed;
        /// RotateFilter rotate = new RotateFilter();
        /// rotate.RotateDegrees = -30;
        /// transformed = rotate.ExecuteFilter(myImg);
        /// </code>
        /// </example>
        public override Image ExecuteFilter(Image inputImage)
        {
            return RotateImage(inputImage, (float)_rotateDegrees);
        }
        
        #endregion Public Filter Methods

        #region Private Filter Methods
        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// The following function code including its comments were taken from the following 
        /// article : http://www.codeproject.com/csharp/rotateimage.asp        
        /// </summary>
        /// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
        /// <returns>A new <see cref="System.Drawing.Bitmap"/> that is just large enough
        /// to contain the rotated image without cutting any corners off.</returns>    
        private Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                return null;

            const double pi2 = System.Math.PI / 2.0;

            double oldWidth = (double)image.Width;
            double oldHeight = (double)image.Height;

            // Convert degrees to radians
            double theta = ((double)angle) * System.Math.PI / 180.0;
            double locked_theta = theta;

            // Ensure theta is now [0, 2pi)
            while (locked_theta < 0.0)
                locked_theta += 2 * System.Math.PI;

            double newWidth, newHeight;
            int nWidth, nHeight; // The newWidth/newHeight expressed as ints

            #region Explaination of the calculations
            /*
       * The trig involved in calculating the new width and height
       * is fairly simple; the hard part was remembering that when 
       * PI/2 <= theta <= PI and 3PI/2 <= theta < 2PI the width and 
       * height are switched.
       * 
       * When you rotate a rectangle, r, the bounding box surrounding r
       * contains for right-triangles of empty space.  Each of the 
       * triangles hypotenuse's are a known length, either the width or
       * the height of r.  Because we know the length of the hypotenuse
       * and we have a known angle of rotation, we can use the trig
       * function identities to find the length of the other two sides.
       * 
       * sine = opposite/hypotenuse
       * cosine = adjacent/hypotenuse
       * 
       * solving for the unknown we get
       * 
       * opposite = sine * hypotenuse
       * adjacent = cosine * hypotenuse
       * 
       * Another interesting point about these triangles is that there
       * are only two different triangles. The proof for which is easy
       * to see, but its been too long since I've written a proof that
       * I can't explain it well enough to want to publish it.  
       * 
       * Just trust me when I say the triangles formed by the lengths 
       * width are always the same (for a given theta) and the same 
       * goes for the height of r.
       * 
       * Rather than associate the opposite/adjacent sides with the
       * width and height of the original bitmap, I'll associate them
       * based on their position.
       * 
       * adjacent/oppositeTop will refer to the triangles making up the 
       * upper right and lower left corners
       * 
       * adjacent/oppositeBottom will refer to the triangles making up 
       * the upper left and lower right corners
       * 
       * The names are based on the right side corners, because thats 
       * where I did my work on paper (the right side).
       * 
       * Now if you draw this out, you will see that the width of the 
       * bounding box is calculated by adding together adjacentTop and 
       * oppositeBottom while the height is calculate by adding 
       * together adjacentBottom and oppositeTop.
       */
            #endregion

            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;

            // We need to calculate the sides of the triangles based
            // on how much rotation is being done to the bitmap.
            //   Refer to the first paragraph in the explaination above for 
            //   reasons why.
            if ((locked_theta >= 0.0 && locked_theta < pi2) ||
              (locked_theta >= System.Math.PI && locked_theta < (System.Math.PI + pi2)))
            {
                adjacentTop = System.Math.Abs(System.Math.Cos(locked_theta)) * oldWidth;
                oppositeTop = System.Math.Abs(System.Math.Sin(locked_theta)) * oldWidth;

                adjacentBottom = System.Math.Abs(System.Math.Cos(locked_theta)) * oldHeight;
                oppositeBottom = System.Math.Abs(System.Math.Sin(locked_theta)) * oldHeight;
            }
            else
            {
                adjacentTop = System.Math.Abs(System.Math.Sin(locked_theta)) * oldHeight;
                oppositeTop = System.Math.Abs(System.Math.Cos(locked_theta)) * oldHeight;

                adjacentBottom = System.Math.Abs(System.Math.Sin(locked_theta)) * oldWidth;
                oppositeBottom = System.Math.Abs(System.Math.Cos(locked_theta)) * oldWidth;
            }

            newWidth = adjacentTop + oppositeBottom;
            newHeight = adjacentBottom + oppositeTop;

            nWidth = (int)System.Math.Ceiling(newWidth);
            nHeight = (int)System.Math.Ceiling(newHeight);

            Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);


            //rotatedBmp.MakeTransparent();
            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {



                // This array will be used to pass in the three points that 
                // make up the rotated image
                Point[] points;


                /*
                 * The values of opposite/adjacentTop/Bottom are referring to 
                 * fixed locations instead of in relation to the
                 * rotating image so I need to change which values are used
                 * based on the how much the image is rotating.
                 * 
                 * For each point, one of the coordinates will always be 0, 
                 * nWidth, or nHeight.  This because the Bitmap we are drawing on
                 * is the bounding box for the rotated bitmap.  If both of the 
                 * corrdinates for any of the given points wasn't in the set above
                 * then the bitmap we are drawing on WOULDN'T be the bounding box
                 * as required.
                 */
                if (locked_theta >= 0.0 && locked_theta < pi2)
                {
                    points = new Point[] { 
                                 new Point( (int) oppositeBottom, 0 ), 
                                 new Point( nWidth, (int) oppositeTop ),
                                 new Point( 0, (int) adjacentBottom )
                               };

                }
                else if (locked_theta >= pi2 && locked_theta < System.Math.PI)
                {
                    points = new Point[] { 
                                 new Point( nWidth, (int) oppositeTop ),
                                 new Point( (int) adjacentTop, nHeight ),
                                 new Point( (int) oppositeBottom, 0 )						 
                               };
                }
                else if (locked_theta >= System.Math.PI && locked_theta < (System.Math.PI + pi2))
                {
                    points = new Point[] { 
                                 new Point( (int) adjacentTop, nHeight ), 
                                 new Point( 0, (int) adjacentBottom ),
                                 new Point( nWidth, (int) oppositeTop )
                               };
                }
                else
                {
                    points = new Point[] { 
                                 new Point( 0, (int) adjacentBottom ), 
                                 new Point( (int) oppositeBottom, 0 ),
                                 new Point( (int) adjacentTop, nHeight )		
                               };
                }
                g.FillRectangle(new SolidBrush(BackGroundColor), 0, 0, rotatedBmp.Width, rotatedBmp.Height);
                //rotatedBmp.MakeTransparent(Color.FromArgb(0, 200, 0));
                g.DrawImage(image, points);
                //rotatedBmp.MakeTransparent(c);

            }

            return rotatedBmp;
        }
        #endregion Private Filter Methods
    }

}
