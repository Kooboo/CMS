#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;


namespace Kooboo.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class Quantizer
    {
        #region .ctor
        /// <summary>
        /// Construct the quantizer
        /// </summary>
        /// <param name="singlePass">If true, the quantization only needs to loop through the source pixels once</param>
        /// <remarks>
        /// If you construct this class with a true value for singlePass, then the code will, when quantizing your image,
        /// only call the 'QuantizeImage' function. If two passes are required, the code will call 'InitialQuantizeImage'
        /// and then 'QuantizeImage'.
        /// </remarks>
        public Quantizer(bool singlePass)
        {
            _singlePass = singlePass;
            _pixelSize = Marshal.SizeOf(typeof(Color32));
        }

        #endregion

        #region Methods
        /// <summary>
        /// Quantize an image and return the resulting output bitmap
        /// </summary>
        /// <param name="source">The image to quantize</param>
        /// <returns>A quantized version of the image</returns>
        public Bitmap Quantize(Image source)
        {
            // Get the size of the source image
            int height = source.Height;
            int width = source.Width;

            // And construct a rectangle from these dimensions
            Rectangle bounds = new Rectangle(0, 0, width, height);

            // First off take a 32bpp copy of the image
            Bitmap copy = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            // And construct an 8bpp version
            Bitmap output = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            // Now lock the bitmap into memory
            using (Graphics g = Graphics.FromImage(copy))
            {
                g.PageUnit = GraphicsUnit.Pixel;

                // Draw the source image onto the copy bitmap,
                // which will effect a widening as appropriate.
                g.DrawImage(source, bounds);
            }

            // Define a pointer to the bitmap data
            BitmapData sourceData = null;

            try
            {
                // Get the source image bits and lock into memory
                sourceData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                // Call the FirstPass function if not a single pass algorithm.
                // For something like an octree quantizer, this will run through
                // all image pixels, build a data structure, and create a palette.
                if (!_singlePass)
                    FirstPass(sourceData, width, height);

                // Then set the color palette on the output bitmap. I'm passing in the current palette 
                // as there's no way to construct a new, empty palette.
                output.Palette = this.GetPalette(output.Palette);

                // Then call the second pass which actually does the conversion
                SecondPass(sourceData, output, width, height, bounds);
            }
            finally
            {
                // Ensure that the bits are unlocked
                copy.UnlockBits(sourceData);
            }

            // Last but not least, return the output bitmap
            return output;
        }

        /// <summary>
        /// Execute the first pass through the pixels in the image
        /// </summary>
        /// <param name="sourceData">The source data</param>
        /// <param name="width">The width in pixels of the image</param>
        /// <param name="height">The height in pixels of the image</param>
        protected virtual void FirstPass(BitmapData sourceData, int width, int height)
        {
            // Define the source data pointers. The source row is a byte to
            // keep addition of the stride value easier (as this is in bytes)
            IntPtr pSourceRow = sourceData.Scan0;

            // Loop through each row
            for (int row = 0; row < height; row++)
            {
                // Set the source pixel to the first pixel in this row
                IntPtr pSourcePixel = pSourceRow;

                // And loop through each column
                for (int col = 0; col < width; col++)
                {
                    // Now I have the pixel, call the FirstPassQuantize function...
                    InitialQuantizePixel(new Color32(pSourcePixel));
                    pSourcePixel = (IntPtr)(pSourcePixel.ToInt64() + _pixelSize);
                }

                // Add the stride to the source row
                pSourceRow = (IntPtr)(pSourceRow.ToInt64() + sourceData.Stride);
            }
        }

        /// <summary>
        /// Execute a second pass through the bitmap
        /// </summary>
        /// <param name="sourceData">The source bitmap, locked into memory</param>
        /// <param name="output">The output bitmap</param>
        /// <param name="width">The width in pixels of the image</param>
        /// <param name="height">The height in pixels of the image</param>
        /// <param name="bounds">The bounding rectangle</param>
        protected virtual void SecondPass(BitmapData sourceData, Bitmap output, int width, int height, Rectangle bounds)
        {
            BitmapData outputData = null;

            try
            {
                // Lock the output bitmap into memory
                outputData = output.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

                // Define the source data pointers. The source row is a byte to
                // keep addition of the stride value easier (as this is in bytes)
                IntPtr pSourceRow = sourceData.Scan0;
                IntPtr pSourcePixel = pSourceRow;
                IntPtr pPreviousPixel = pSourcePixel;

                // Now define the destination data pointers
                IntPtr pDestinationRow = outputData.Scan0;
                IntPtr pDestinationPixel = pDestinationRow;

                // And convert the first pixel, so that I have values going into the loop
                byte pixelValue = QuantizePixel(new Color32(pSourcePixel));
                // Assign the value of the first pixel
                Marshal.WriteByte(pDestinationPixel, pixelValue);

                // Loop through each row
                for (int row = 0; row < height; row++)
                {
                    // Set the source pixel to the first pixel in this row
                    pSourcePixel = pSourceRow;

                    // And set the destination pixel pointer to the first pixel in the row
                    pDestinationPixel = pDestinationRow;

                    // Loop through each pixel on this scan line
                    for (int col = 0; col < width; col++)
                    {
                        // Check if this is the same as the last pixel. If so use that value
                        // rather than calculating it again. This is an inexpensive optimisation.
                        if (Marshal.ReadInt32(pPreviousPixel) != Marshal.ReadInt32(pSourcePixel))
                        {
                            // Quantize the pixel
                            pixelValue = QuantizePixel(new Color32(pSourcePixel));

                            // And setup the previous pointer
                            pPreviousPixel = pSourcePixel;
                        }

                        // And set the pixel in the output
                        Marshal.WriteByte(pDestinationPixel, pixelValue);

                        pSourcePixel = (IntPtr)(pSourcePixel.ToInt64() + _pixelSize);
                        pDestinationPixel = (IntPtr)(pDestinationPixel.ToInt64() + 1);

                    }

                    // Add the stride to the source row
                    pSourceRow = (IntPtr)(pSourceRow.ToInt64() + sourceData.Stride);

                    // And to the destination row
                    pDestinationRow = (IntPtr)(pDestinationRow.ToInt64() + outputData.Stride);
                }
            }
            finally
            {
                // Ensure that I unlock the output bits
                output.UnlockBits(outputData);
            }
        }

        /// <summary>
        /// Override this to process the pixel in the first pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <remarks>
        /// This function need only be overridden if your quantize algorithm needs two passes,
        /// such as an Octree quantizer.
        /// </remarks>
        protected virtual void InitialQuantizePixel(Color32 pixel)
        {
        }

        /// <summary>
        /// Override this to process the pixel in the second pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <returns>The quantized value</returns>
        protected abstract byte QuantizePixel(Color32 pixel);

        /// <summary>
        /// Retrieve the palette for the quantized image
        /// </summary>
        /// <param name="original">Any old palette, this is overrwritten</param>
        /// <returns>The new color palette</returns>
        protected abstract ColorPalette GetPalette(ColorPalette original);
        #endregion

        #region Properties
        /// <summary>
        /// Flag used to indicate whether a single pass or two passes are needed for quantization.
        /// </summary>
        private bool _singlePass;
        private int _pixelSize;

        /// <summary>
        /// Struct that defines a 32 bpp colour
        /// </summary>
        /// <remarks>
        /// This struct is used to read data from a 32 bits per pixel image
        /// in memory, and is ordered in this manner as this is the way that
        /// the data is layed out in memory
        /// </remarks>
        [StructLayout(LayoutKind.Explicit)]
        public struct Color32
        {

            public Color32(IntPtr pSourcePixel)
            {
                this = (Color32)Marshal.PtrToStructure(pSourcePixel, typeof(Color32));
            }

            /// <summary>
            /// Holds the blue component of the colour
            /// </summary>
            [FieldOffset(0)]
            public byte Blue;
            /// <summary>
            /// Holds the green component of the colour
            /// </summary>
            [FieldOffset(1)]
            public byte Green;
            /// <summary>
            /// Holds the red component of the colour
            /// </summary>
            [FieldOffset(2)]
            public byte Red;
            /// <summary>
            /// Holds the alpha component of the colour
            /// </summary>
            [FieldOffset(3)]
            public byte Alpha;

            /// <summary>
            /// Permits the color32 to be treated as an int32
            /// </summary>
            [FieldOffset(0)]
            public int ARGB;

            /// <summary>
            /// Return the color for this Color32 object
            /// </summary>
            public Color Color
            {
                get { return Color.FromArgb(Alpha, Red, Green, Blue); }
            }
        }
        #endregion
    }

    /// <summary>
    /// Quantize using an Octree
    /// </summary>
    internal class OctreeQuantizer : Quantizer
    {
        #region .ctor
        /// <summary>
        /// Construct the octree quantizer
        /// </summary>
        /// <remarks>
        /// The Octree quantizer is a two pass algorithm. The initial pass sets up the octree,
        /// the second pass quantizes a color based on the nodes in the tree
        /// </remarks>
        /// <param name="maxColors">The maximum number of colors to return</param>
        /// <param name="maxColorBits">The number of significant bits</param>
        public OctreeQuantizer(int maxColors, int maxColorBits)
            : base(false)
        {
            if (maxColors > 255)
                throw new ArgumentOutOfRangeException("maxColors", maxColors, "The number of colors should be less than 256");

            if ((maxColorBits < 1) | (maxColorBits > 8))
                throw new ArgumentOutOfRangeException("maxColorBits", maxColorBits, "This should be between 1 and 8");

            // Construct the octree
            _octree = new Octree(maxColorBits);

            _maxColors = maxColors;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process the pixel in the first pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <remarks>
        /// This function need only be overridden if your quantize algorithm needs two passes,
        /// such as an Octree quantizer.
        /// </remarks>
        protected override void InitialQuantizePixel(Color32 pixel)
        {
            // Add the color to the octree
            _octree.AddColor(pixel);
        }

        /// <summary>
        /// Override this to process the pixel in the second pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <returns>The quantized value</returns>
        protected override byte QuantizePixel(Color32 pixel)
        {
            byte paletteIndex = (byte)_maxColors;	// The color at [_maxColors] is set to transparent

            // Get the palette index if this non-transparent
            if (pixel.Alpha > 0)
                paletteIndex = (byte)_octree.GetPaletteIndex(pixel);

            return paletteIndex;
        }

        /// <summary>
        /// Retrieve the palette for the quantized image
        /// </summary>
        /// <param name="original">Any old palette, this is overrwritten</param>
        /// <returns>The new color palette</returns>
        protected override ColorPalette GetPalette(ColorPalette original)
        {
            // First off convert the octree to _maxColors colors
            ArrayList palette = _octree.Palletize(_maxColors - 1);

            // Then convert the palette based on those colors
            for (int index = 0; index < palette.Count; index++)
                original.Entries[index] = (Color)palette[index];

            // Add the transparent color
            original.Entries[_maxColors] = Color.FromArgb(0, 0, 0, 0);

            return original;
        }

        #endregion

        #region Fields
        /// <summary>
        /// Stores the tree
        /// </summary>
        private Octree _octree;

        /// <summary>
        /// Maximum allowed color depth
        /// </summary>
        private int _maxColors;

        /// <summary>
        /// Class which does the actual quantization
        /// </summary>
        private class Octree
        {
            /// <summary>
            /// Construct the octree
            /// </summary>
            /// <param name="maxColorBits">The maximum number of significant bits in the image</param>
            public Octree(int maxColorBits)
            {
                _maxColorBits = maxColorBits;
                _leafCount = 0;
                _reducibleNodes = new OctreeNode[9];
                _root = new OctreeNode(0, _maxColorBits, this);
                _previousColor = 0;
                _previousNode = null;
            }

            /// <summary>
            /// Add a given color value to the octree
            /// </summary>
            /// <param name="pixel"></param>
            public void AddColor(Color32 pixel)
            {
                // Check if this request is for the same color as the last
                if (_previousColor == pixel.ARGB)
                {
                    // If so, check if I have a previous node setup. This will only ocurr if the first color in the image
                    // happens to be black, with an alpha component of zero.
                    if (null == _previousNode)
                    {
                        _previousColor = pixel.ARGB;
                        _root.AddColor(pixel, _maxColorBits, 0, this);
                    }
                    else
                        // Just update the previous node
                        _previousNode.Increment(pixel);
                }
                else
                {
                    _previousColor = pixel.ARGB;
                    _root.AddColor(pixel, _maxColorBits, 0, this);
                }
            }

            /// <summary>
            /// Reduce the depth of the tree
            /// </summary>
            public void Reduce()
            {
                int index;

                // Find the deepest level containing at least one reducible node
                for (index = _maxColorBits - 1; (index > 0) && (null == _reducibleNodes[index]); index--) ;

                // Reduce the node most recently added to the list at level 'index'
                OctreeNode node = _reducibleNodes[index];
                _reducibleNodes[index] = node.NextReducible;

                // Decrement the leaf count after reducing the node
                _leafCount -= node.Reduce();

                // And just in case I've reduced the last color to be added, and the next color to
                // be added is the same, invalidate the previousNode...
                _previousNode = null;
            }

            /// <summary>
            /// Get/Set the number of leaves in the tree
            /// </summary>
            public int Leaves
            {
                get { return _leafCount; }
                set { _leafCount = value; }
            }

            /// <summary>
            /// Return the array of reducible nodes
            /// </summary>
            protected OctreeNode[] ReducibleNodes
            {
                get { return _reducibleNodes; }
            }

            /// <summary>
            /// Keep track of the previous node that was quantized
            /// </summary>
            /// <param name="node">The node last quantized</param>
            protected void TrackPrevious(OctreeNode node)
            {
                _previousNode = node;
            }

            /// <summary>
            /// Convert the nodes in the octree to a palette with a maximum of colorCount colors
            /// </summary>
            /// <param name="colorCount">The maximum number of colors</param>
            /// <returns>An arraylist with the palettized colors</returns>
            public ArrayList Palletize(int colorCount)
            {
                while (Leaves > colorCount)
                    Reduce();

                // Now palettize the nodes
                ArrayList palette = new ArrayList(Leaves);
                int paletteIndex = 0;
                _root.ConstructPalette(palette, ref paletteIndex);

                // And return the palette
                return palette;
            }

            /// <summary>
            /// Get the palette index for the passed color
            /// </summary>
            /// <param name="pixel"></param>
            /// <returns></returns>
            public int GetPaletteIndex(Color32 pixel)
            {
                return _root.GetPaletteIndex(pixel, 0);
            }

            /// <summary>
            /// Mask used when getting the appropriate pixels for a given node
            /// </summary>
            private static int[] mask = new int[8] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

            /// <summary>
            /// The root of the octree
            /// </summary>
            private OctreeNode _root;

            /// <summary>
            /// Number of leaves in the tree
            /// </summary>
            private int _leafCount;

            /// <summary>
            /// Array of reducible nodes
            /// </summary>
            private OctreeNode[] _reducibleNodes;

            /// <summary>
            /// Maximum number of significant bits in the image
            /// </summary>
            private int _maxColorBits;

            /// <summary>
            /// Store the last node quantized
            /// </summary>
            private OctreeNode _previousNode;

            /// <summary>
            /// Cache the previous color quantized
            /// </summary>
            private int _previousColor;

            /// <summary>
            /// Class which encapsulates each node in the tree
            /// </summary>
            protected class OctreeNode
            {
                /// <summary>
                /// Construct the node
                /// </summary>
                /// <param name="level">The level in the tree = 0 - 7</param>
                /// <param name="colorBits">The number of significant color bits in the image</param>
                /// <param name="octree">The tree to which this node belongs</param>
                public OctreeNode(int level, int colorBits, Octree octree)
                {
                    // Construct the new node
                    _leaf = (level == colorBits);

                    _red = _green = _blue = 0;
                    _pixelCount = 0;

                    // If a leaf, increment the leaf count
                    if (_leaf)
                    {
                        octree.Leaves++;
                        _nextReducible = null;
                        _children = null;
                    }
                    else
                    {
                        // Otherwise add this to the reducible nodes
                        _nextReducible = octree.ReducibleNodes[level];
                        octree.ReducibleNodes[level] = this;
                        _children = new OctreeNode[8];
                    }
                }

                /// <summary>
                /// Add a color into the tree
                /// </summary>
                /// <param name="pixel">The color</param>
                /// <param name="colorBits">The number of significant color bits</param>
                /// <param name="level">The level in the tree</param>
                /// <param name="octree">The tree to which this node belongs</param>
                public void AddColor(Color32 pixel, int colorBits, int level, Octree octree)
                {
                    // Update the color information if this is a leaf
                    if (_leaf)
                    {
                        Increment(pixel);
                        // Setup the previous node
                        octree.TrackPrevious(this);
                    }
                    else
                    {
                        // Go to the next level down in the tree
                        int shift = 7 - level;
                        int index = ((pixel.Red & mask[level]) >> (shift - 2)) |
                                    ((pixel.Green & mask[level]) >> (shift - 1)) |
                                    ((pixel.Blue & mask[level]) >> (shift));

                        OctreeNode child = _children[index];

                        if (null == child)
                        {
                            // Create a new child node & store in the array
                            child = new OctreeNode(level + 1, colorBits, octree);
                            _children[index] = child;
                        }

                        // Add the color to the child node
                        child.AddColor(pixel, colorBits, level + 1, octree);
                    }

                }

                /// <summary>
                /// Get/Set the next reducible node
                /// </summary>
                public OctreeNode NextReducible
                {
                    get { return _nextReducible; }
                    set { _nextReducible = value; }
                }

                /// <summary>
                /// Return the child nodes
                /// </summary>
                public OctreeNode[] Children
                {
                    get { return _children; }
                }

                /// <summary>
                /// Reduce this node by removing all of its children
                /// </summary>
                /// <returns>The number of leaves removed</returns>
                public int Reduce()
                {
                    _red = _green = _blue = 0;
                    int children = 0;

                    // Loop through all children and add their information to this node
                    for (int index = 0; index < 8; index++)
                    {
                        if (null != _children[index])
                        {
                            _red += _children[index]._red;
                            _green += _children[index]._green;
                            _blue += _children[index]._blue;
                            _pixelCount += _children[index]._pixelCount;
                            ++children;
                            _children[index] = null;
                        }
                    }

                    // Now change this to a leaf node
                    _leaf = true;

                    // Return the number of nodes to decrement the leaf count by
                    return (children - 1);
                }

                /// <summary>
                /// Traverse the tree, building up the color palette
                /// </summary>
                /// <param name="palette">The palette</param>
                /// <param name="paletteIndex">The current palette index</param>
                public void ConstructPalette(ArrayList palette, ref int paletteIndex)
                {
                    if (_leaf)
                    {
                        // Consume the next palette index
                        _paletteIndex = paletteIndex++;

                        // And set the color of the palette entry
                        palette.Add(Color.FromArgb(_red / _pixelCount, _green / _pixelCount, _blue / _pixelCount));
                    }
                    else
                    {
                        // Loop through children looking for leaves
                        for (int index = 0; index < 8; index++)
                        {
                            if (null != _children[index])
                                _children[index].ConstructPalette(palette, ref paletteIndex);
                        }
                    }
                }

                /// <summary>
                /// Return the palette index for the passed color
                /// </summary>
                public int GetPaletteIndex(Color32 pixel, int level)
                {
                    int paletteIndex = _paletteIndex;

                    if (!_leaf)
                    {
                        int shift = 7 - level;
                        int index = ((pixel.Red & mask[level]) >> (shift - 2)) |
                                    ((pixel.Green & mask[level]) >> (shift - 1)) |
                                    ((pixel.Blue & mask[level]) >> (shift));

                        if (null != _children[index])
                            paletteIndex = _children[index].GetPaletteIndex(pixel, level + 1);
                        else
                            throw new Exception("Didn't expect this!");
                    }

                    return paletteIndex;
                }

                /// <summary>
                /// Increment the pixel count and add to the color information
                /// </summary>
                public void Increment(Color32 pixel)
                {
                    _pixelCount++;
                    _red += pixel.Red;
                    _green += pixel.Green;
                    _blue += pixel.Blue;
                }

                /// <summary>
                /// Flag indicating that this is a leaf node
                /// </summary>
                private bool _leaf;

                /// <summary>
                /// Number of pixels in this node
                /// </summary>
                private int _pixelCount;

                /// <summary>
                /// Red component
                /// </summary>
                private int _red;

                /// <summary>
                /// Green Component
                /// </summary>
                private int _green;

                /// <summary>
                /// Blue component
                /// </summary>
                private int _blue;

                /// <summary>
                /// Pointers to any child nodes
                /// </summary>
                private OctreeNode[] _children;

                /// <summary>
                /// Pointer to next reducible node
                /// </summary>
                private OctreeNode _nextReducible;

                /// <summary>
                /// The index of this node in the palette
                /// </summary>
                private int _paletteIndex;

            }
        }

        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ImageTools
    {
        #region Methods
        /// <summary>
        /// Determines whether [is image extension] [the specified extension].
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///   <c>true</c> if [is image extension] [the specified extension]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsImageExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                case ".bmp":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Converts to image format.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public static ImageFormat ConvertToImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".gif":
                    return ImageFormat.Gif;
                case ".png":
                    return ImageFormat.Png;
                case ".bmp":
                default:
                    return ImageFormat.Bmp;
            }
        }
        /// <summary>
        /// Validates the image.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static bool ValidateImage(string filePath)
        {
            System.Drawing.Image sourceImage;

            try
            {
                sourceImage = System.Drawing.Image.FromFile(filePath);
                sourceImage.Dispose();
                return true;
            }
            catch
            {
                // This is not a valid image. Do nothing.
                return false;
            }
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="preserverAspectRatio">if set to <c>true</c> [preserver aspect ratio].</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static bool ResizeImage(string sourceFile, string targetFile, int maxWidth, int maxHeight, bool preserverAspectRatio, int quality)
        {
            System.Drawing.Image sourceImage;

            try
            {
                sourceImage = System.Drawing.Image.FromFile(sourceFile);
            }
            catch (OutOfMemoryException)
            {
                // This is not a valid image. Do nothing.
                return false;
            }

            // If 0 is passed in any of the max sizes it means that that size must be ignored,
            // so the original image size is used.
            maxWidth = maxWidth == 0 ? sourceImage.Width : maxWidth;
            maxHeight = maxHeight == 0 ? sourceImage.Height : maxHeight;

            if (sourceImage.Width <= maxWidth && sourceImage.Height <= maxHeight)
            {
                sourceImage.Dispose();

                if (sourceFile != targetFile)
                    System.IO.File.Copy(sourceFile, targetFile);

                return true;
            }

            Size oSize;
            if (preserverAspectRatio)
            {
                // Gets the best size for aspect ratio resampling
                oSize = GetAspectRatioSize(maxWidth, maxHeight, sourceImage.Width, sourceImage.Height);
            }
            else
                oSize = new Size(maxWidth, maxHeight);

            System.Drawing.Image oResampled = null;
            Graphics oGraphics = null;

            try
            {
                oResampled = new Bitmap(oSize.Width, oSize.Height, PixelFormat.Format24bppRgb);

                // Creates a Graphics for the oResampled image
                oGraphics = Graphics.FromImage(oResampled);

                // The Rectangle that holds the Resampled image size
                Rectangle oRectangle;

                // High quality resizing
                if (quality > 80)
                {
                    oGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    // If HighQualityBicubic is used, bigger Rectangle is required to remove the white border
                    oRectangle = new Rectangle(-1, -1, oSize.Width + 1, oSize.Height + 1);
                }
                else
                    oRectangle = new Rectangle(0, 0, oSize.Width, oSize.Height);

                // Place a white background (for transparent images).
                oGraphics.FillRectangle(new SolidBrush(Color.White), oRectangle);

                // Draws over the oResampled image the resampled Image
                oGraphics.DrawImage(sourceImage, oRectangle);

                sourceImage.Dispose();

                String extension = System.IO.Path.GetExtension(targetFile).ToLower();

                if (extension == ".jpg" || extension == ".jpeg")
                {
                    ImageCodecInfo oCodec = GetJpgCodec();

                    if (oCodec != null)
                    {
                        EncoderParameters aCodecParams = new EncoderParameters(1);
                        aCodecParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                        oResampled.Save(targetFile, oCodec, aCodecParams);
                    }
                    else
                        oResampled.Save(targetFile);
                }
                else
                {
                    switch (extension)
                    {
                        case ".gif":
                            try
                            {
                                // Use a proper palette
                                OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);
                                using (Bitmap quantized = quantizer.Quantize(oResampled))
                                {
                                    quantized.Save(targetFile, System.Drawing.Imaging.ImageFormat.Gif);
                                }
                            }
                            catch (System.Security.SecurityException)
                            {
                                // The calls to Marshal might fail in Medium trust, save the image using the default palette
                                oResampled.Save(targetFile, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            break;

                        case ".png":
                            oResampled.Save(targetFile, System.Drawing.Imaging.ImageFormat.Png);
                            break;

                        case ".bmp":
                            oResampled.Save(targetFile, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                    }
                }
            }
            finally
            {
                if (oGraphics != null)
                    oGraphics.Dispose();
                if (oResampled != null)
                    oResampled.Dispose();
            }


            return true;
        }

        public static bool ResizeImage(Stream sourceStream, Stream targetStream, ImageFormat imageFormat, int maxWidth, int maxHeight, bool preserverAspectRatio, int quality)
        {
            System.Drawing.Image sourceImage;
            try
            {
                sourceImage = System.Drawing.Image.FromStream(sourceStream);
                sourceStream.Position = 0;
            }
            catch (OutOfMemoryException)
            {
                // This is not a valid image. Do nothing.
                return false;
            }

            // If 0 is passed in any of the max sizes it means that that size must be ignored,
            // so the original image size is used.
            maxWidth = maxWidth == 0 ? sourceImage.Width : maxWidth;
            maxHeight = maxHeight == 0 ? sourceImage.Height : maxHeight;

            if (sourceImage.Width <= maxWidth && sourceImage.Height <= maxHeight)
            {
                sourceImage.Dispose();

                sourceStream.CopyTo(targetStream);

                return true;
            }

            Size oSize;
            if (preserverAspectRatio)
            {
                // Gets the best size for aspect ratio resampling
                oSize = GetAspectRatioSize(maxWidth, maxHeight, sourceImage.Width, sourceImage.Height);
            }
            else
                oSize = new Size(maxWidth, maxHeight);

            System.Drawing.Image oResampled = null;
            Graphics oGraphics = null;
            try
            {
                oResampled = new Bitmap(oSize.Width, oSize.Height, PixelFormat.Format24bppRgb);

                // Creates a Graphics for the oResampled image
                oGraphics = Graphics.FromImage(oResampled);

                // The Rectangle that holds the Resampled image size
                Rectangle oRectangle;

                // High quality resizing
                if (quality > 80)
                {
                    oGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    // If HighQualityBicubic is used, bigger Rectangle is required to remove the white border
                    oRectangle = new Rectangle(-1, -1, oSize.Width + 1, oSize.Height + 1);
                }
                else
                    oRectangle = new Rectangle(0, 0, oSize.Width, oSize.Height);

                // Place a white background (for transparent images).
                oGraphics.FillRectangle(new SolidBrush(Color.White), oRectangle);

                // Draws over the oResampled image the resampled Image
                oGraphics.DrawImage(sourceImage, oRectangle);

                sourceImage.Dispose();

                if (imageFormat == ImageFormat.Jpeg)
                {
                    ImageCodecInfo oCodec = GetJpgCodec();

                    if (oCodec != null)
                    {
                        EncoderParameters aCodecParams = new EncoderParameters(1);
                        aCodecParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                        oResampled.Save(targetStream, oCodec, aCodecParams);
                    }
                    else
                        oResampled.Save(targetStream, imageFormat);
                }
                else if (imageFormat == ImageFormat.Gif)
                {

                    try
                    {
                        // Use a proper palette
                        OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);
                        using (Bitmap quantized = quantizer.Quantize(oResampled))
                        {
                            quantized.Save(targetStream, System.Drawing.Imaging.ImageFormat.Gif);
                        }
                    }
                    catch (System.Security.SecurityException)
                    {
                        // The calls to Marshal might fail in Medium trust, save the image using the default palette
                        oResampled.Save(targetStream, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                else
                {
                    oResampled.Save(targetStream, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            finally
            {
                if (oGraphics != null)
                    oGraphics.Dispose();
                if (oResampled != null)
                    oResampled.Dispose();
            }

            return true;
        }
        /// <summary>
        /// Crops the image.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="targetFilePath">The target file path.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static bool CropImage(string sourceFilePath, string targetFilePath, int x, int y, int width, int height)
        {
            var extension = Path.GetExtension(sourceFilePath);
            var imageFormat = ConvertToImageFormat(extension);
            using (FileStream fs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (FileStream target = new FileStream(targetFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    CropImage(fs, target, imageFormat, x, y, width, height);
                }
            }

            return true;
        }

        public static bool CropImage(Stream sourceStream, Stream targetStream, ImageFormat imageFormat, int x, int y, int width, int height)
        {
            Rectangle r = new Rectangle(x, y, width, height);

            using (Bitmap src = Image.FromStream(sourceStream) as Bitmap)
            {
                Bitmap target = new Bitmap(width, height);


                using (Graphics g = Graphics.FromImage(target))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), r, GraphicsUnit.Pixel);
                }

                target.Save(targetStream, imageFormat);

            }

            return true;
        }

        /// <summary>
        /// Gets the JPG codec.
        /// </summary>
        /// <returns></returns>
        private static ImageCodecInfo GetJpgCodec()
        {
            ImageCodecInfo[] aCodecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo oCodec = null;

            for (int i = 0; i < aCodecs.Length; i++)
            {
                if (aCodecs[i].MimeType.Equals("image/jpeg"))
                {
                    oCodec = aCodecs[i];
                    break;
                }
            }

            return oCodec;
        }

        /// <summary>
        /// Gets the size of the aspect ratio.
        /// </summary>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="actualWidth">The actual width.</param>
        /// <param name="actualHeight">The actual height.</param>
        /// <returns></returns>
        public static Size GetAspectRatioSize(int maxWidth, int maxHeight, int actualWidth, int actualHeight)
        {
            // Creates the Size object to be returned
            Size oSize = new System.Drawing.Size(maxWidth, maxHeight);

            // Calculates the X and Y resize factors
            float iFactorX = (float)maxWidth / (float)actualWidth;
            float iFactorY = (float)maxHeight / (float)actualHeight;

            // If some dimension have to be scaled
            if (iFactorX != 1 || iFactorY != 1)
            {
                // Uses the lower Factor to scale the opposite size
                if (iFactorX < iFactorY) { oSize.Height = (int)Math.Round((float)actualHeight * iFactorX); }
                else if (iFactorX > iFactorY) { oSize.Width = (int)Math.Round((float)actualWidth * iFactorY); }
            }

            if (oSize.Height <= 0) oSize.Height = 1;
            if (oSize.Width <= 0) oSize.Width = 1;

            // Returns the Size
            return oSize;
        }
        #endregion
    }
}
