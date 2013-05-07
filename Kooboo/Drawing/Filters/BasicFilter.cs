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
    /// Summary description for BasicFilter.
    /// </summary>
    public abstract class BasicFilter : IFilter
    {
        #region Privates
        private Color _backColor = Color.FromArgb(0, 0, 0, 0); // Default is a transparent background
        #endregion Privates

        #region Properties
        /// <summary>
        /// Gets or sets the color of the back ground.
        /// </summary>
        /// <value>
        /// The color of the back ground.
        /// </value>
        public Color BackGroundColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
            }

        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes this filter on the input image and returns the result
        /// </summary>
        /// <param name="inputImage">input image</param>
        /// <returns>transformed image</returns>
        public abstract Image ExecuteFilter(Image inputImage);

        /// <summary>
        /// Demonostration Function. Could be left unimplimented.
        /// To be used for presentation purposes.
        /// </summary>
        /// <param name="inputImage"></param>
        /// <returns></returns>
        public virtual Image ExecuteFilterDemo(Image inputImage)
        {
            return this.ExecuteFilter(inputImage);
            //throw new MissingMethodException();
        }
        #endregion
    }
}
