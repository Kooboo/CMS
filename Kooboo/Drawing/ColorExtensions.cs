using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Kooboo.Drawing
{
    public static class ColorExtensions
    {
        /// <summary>        
        /// 让颜色变淡一些
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Color Light(this Color color)
        {
            return ControlPaint.Light(color);
        }
        /// <summary>
        /// Darks the specified color.
        /// 让颜色变深一些
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Color Dark(this Color color)
        {
            return ControlPaint.Dark(color);
        }
    }
}
