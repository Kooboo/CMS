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
    /// ͼƬԲ�ǵĴ���
    /// <remarks>�������ַ���Ч����Ȼ���������ģ���������ɫ�仯ת���ͼƬ���ȽϽ��䣩ʱ�����к����Եķָ��� </remarks>
    /// �����˺ܶ��ַ�������Ч����������
    /// �������� http://forums.asp.net/p/942160/1130380.aspx �����Ļ���֮�Ͻ����޸�
    /// ����ʵ�����޾�ݵ�ͼƬԲ�Ǵ���
    /// ���Ϸ������ڣ�����ʹ���˰�ԭʼͼƬ��Ϊ��ˢ��Ȼ���������ˢ����Բ�Ǿ��Σ�Ŀǰ����Ӧ�����ܴﵽ����Ч����
    /// </summary>
    public class RoundedCornersFilter : BasicFilter
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundedCornersFilter" /> class.
        /// </summary>
        public RoundedCornersFilter()
        {
            Corner = Corner.All;
            this.BackGroundColor = Color.Transparent;
        } 
        #endregion

        #region Public Properties Tokens
        /// <summary>
        /// 
        /// </summary>
        public static string ROTATE_DEGREES_TOKEN = "radius";
        #endregion Public Properties Tokens

        #region Private Fields
        private float _cornerRadius = 50; //Default
        #endregion Private Fields


        #region Filter Properties
        /// <summary>
        /// Determins the corner's radius. in pixels
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        public float CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                if (value > 0)
                    _cornerRadius = value;
                else
                    _cornerRadius = 0;
            }
        }

        /// <summary>
        /// Gets or sets the corner.
        /// </summary>
        /// <value>
        /// The corner.
        /// </value>
        public Corner Corner
        {
            get;
            set;
        }
        #endregion Filter Properties

        #region Public Filter Methods
        /// <summary>
        /// Executes this curved corners
        /// filter on the input image and returns the result
        /// Make sure you set the BackGroundColor property before running this filter.
        /// </summary>
        /// <param name="inputImage">input image</param>
        /// <returns>
        /// Curved Corner Image
        /// </returns>
        /// <example>
        ///   <code>
        /// Image transformed;
        /// RoundedCornersFilter rounded = new RoundedCornersFilter();
        /// rounded.BackGroundColor = Color.FromArgb(255, 255, 255, 255);
        /// rounded.CornerRadius = 15;
        /// transformed = rounded.ExecuteFilter(myImg);
        ///   </code>
        ///   </example>
        public override Image ExecuteFilter(Image inputImage)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(inputImage.Width, inputImage.Height);
            bitmap.MakeTransparent();

            Graphics g = Graphics.FromImage(bitmap);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.Clear(BackGroundColor);

            Brush brush = new System.Drawing.TextureBrush(inputImage);

            //��ȸ߶ȼ�1��������ͼƬ�Ҳ���²಻������
            //�����ſ�������ߺ��ϲ�Գ�
            //��Ϊ���Ч����Ӱ�죬GDI+���ͼƬ������һ�����ؽ��а�͸���������˴�-1,-1��ʼ��ȥ���͸����
            g.FillRoundedRectangle(brush, -1, -1, inputImage.Width + 1, inputImage.Height + 1, CornerRadius, Corner);
            //FillRoundedRectangle(g, new Rectangle(0, 0, inputImage.Width, inputImage.Height), (int)CornerRadius, brush);

            return bitmap;
        }

        /// <summary>
        /// Demonostration Function. Could be left unimplimented.
        /// To be used for presentation purposes.
        /// </summary>
        /// <param name="inputImage"></param>
        /// <returns></returns>
        public override Image ExecuteFilterDemo(Image inputImage)
        {
            this.BackGroundColor = Color.FromArgb(255, 255, 255, 255);
            return this.ExecuteFilter(inputImage);
        }
        #endregion Public Filter Methods
    }
}
