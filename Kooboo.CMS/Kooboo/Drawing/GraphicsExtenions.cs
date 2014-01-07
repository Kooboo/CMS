#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Kooboo.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    abstract class CornersItem
    {
        private bool visible;
        public CornersItem()
        {
            visible = true;
        }
        public abstract void addToPath(GraphicsPath path);
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CornersLine : CornersItem
    {
        private double x1;
        private double y1;
        private double x2;
        private double y2;
        private double newX1;
        private double newY1;
        private double newX2;
        private double newY2;

        public double X1
        {
            get { return x1; }
            set { x1 = value; newX1 = value; }
        }
        public double Y1
        {
            get { return y1; }
            set { y1 = value; newY1 = value; }
        }
        public double X2
        {
            get { return x2; }
            set { x2 = value; newX2 = value; }
        }
        public double Y2
        {
            get { return y2; }
            set { y2 = value; newY2 = value; }
        }
        public double NewX1
        {
            get { return newX1; }
            set { newX1 = value; }
        }
        public double NewY1
        {
            get { return newY1; }
            set { newY1 = value; }
        }
        public double NewX2
        {
            get { return newX2; }
            set { newX2 = value; }
        }
        public double NewY2
        {
            get { return newY2; }
            set { newY2 = value; }
        }
        public override void addToPath(GraphicsPath path)
        {
            if (Visible)
            {
                if (Math.Sqrt(Math.Pow(newX1 - newX2, 2) + Math.Pow(newY1 - newY2, 2)) > 0.01)
                {
                    path.AddLine((float)newX1, (float)newY1, (float)newX2, (float)newY2);
                }
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    class CornersArc : CornersItem
    {
        private double x;
        private double y;
        private double width;
        private double height;
        private double startAngle;
        private double sweepAngle;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Width
        {
            get { return width; }
            set { width = value; }
        }
        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public double StartAngle
        {
            get { return startAngle; }
            set { startAngle = value; }
        }

        public double SweepAngle
        {
            get { return sweepAngle; }
            set { sweepAngle = value; }
        }

        public override void addToPath(GraphicsPath path)
        {
            if (Visible)
            {
                path.AddArc((float)x, (float)y, (float)width, (float)height, (float)startAngle, (float)(sweepAngle));
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Corners
    {
        #region Fields
        float x, y, width, height;
        private double radius;
        private List<CornersItem> list = new List<CornersItem>();

        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="Corners" /> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corner">The corner.</param>
        public Corners(float x, float y, float width, float height, double radius, Corner corner)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.radius = radius;
            FillList(corner);
        }
        #endregion

        #region Methods

        private void FillList(Corner corner)
        {
            //roundedRectangle[0].X = basePoint.X;
            //roundedRectangle[0].Y = basePoint.Y;
            //roundedRectangle[1].X = basePoint.X + width;
            //roundedRectangle[1].Y = basePoint.Y;
            //roundedRectangle[2].X = basePoint.X + width;
            //roundedRectangle[2].Y = basePoint.Y + height;
            //roundedRectangle[3].X = basePoint.X;
            //roundedRectangle[3].Y = basePoint.Y + height;
            //roundedRectangle[4].X = basePoint.X;
            //roundedRectangle[4].Y = basePoint.Y;

            list.Add(new CornersLine() { X1 = x, Y1 = y, X2 = x + width, Y2 = y });
            if ((corner & Corner.TopRight) == Corner.TopRight)
            {
                list.Add(new CornersArc());
            }
            list.Add(new CornersLine() { X1 = x + width, Y1 = y, X2 = x + width, Y2 = y + height });
            if ((corner & Corner.BottomRight) == Corner.BottomRight)
            {
                list.Add(new CornersArc());
            }
            list.Add(new CornersLine() { X1 = x + width, Y1 = y + height, X2 = x, Y2 = y + height });
            if ((corner & Corner.BottomLeft) == Corner.BottomLeft)
            {
                list.Add(new CornersArc());
            }
            list.Add(new CornersLine() { X1 = x, Y1 = y + height, X2 = x, Y2 = y });
            if ((corner & Corner.TopLeft) == Corner.TopLeft)
            {
                list.Add(new CornersArc());
            }
        }
        /// <summary>
        /// Executes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Execute(GraphicsPath path)
        {
            CornersLine line1, line2;
            CornersArc arc;

            int i = 0;
            while (true)
            {
                if (i == list.Count) break;
                line1 = list[i] as CornersLine;
                i++;
                if (i == list.Count) break;
                if (list[i] is CornersArc)
                {
                    arc = list[i] as CornersArc;
                    i++;
                    if (i == list.Count)
                        line2 = list[0] as CornersLine;
                    else
                        line2 = list[i] as CornersLine;
                    CalculateRoundLines(line1, line2, arc);
                }
            }

            for (int j = 0; j < list.Count; j++)
            {
                (list[j] as CornersItem).addToPath(path);
            }
        }

        private void CalculateRoundLines(CornersLine line1, CornersLine line2,
            CornersArc arc)
        {
            double f1 = Math.Atan2(line1.Y1 - line1.Y2, line1.X1 - line1.X2);
            double f2 = Math.Atan2(line2.Y2 - line2.Y1, line2.X2 - line2.X1);
            double alfa = f2 - f1;
            if ((alfa == 0) || (Math.Abs(alfa) == Math.PI))
                addWithoutArc(arc);
            else
                addWithArc(line1, line2, arc, f1, f2, alfa);
        }

        private static void addWithoutArc(CornersArc arc)
        {
            arc.Visible = false;
        }

        private void addWithArc(CornersLine line1, CornersLine line2, CornersArc arc, double f1, double f2, double alfa)
        {
            double s = radius / Math.Tan(alfa / 2);
            double line1Length = Math.Sqrt(Math.Pow(line1.X1 - line1.X2, 2) + Math.Pow(line1.Y1 - line1.Y2, 2));
            double line2Length = Math.Sqrt(Math.Pow(line2.X1 - line2.X2, 2) + Math.Pow(line2.Y1 - line2.Y2, 2));
            double newRadius = radius;

            if ((Math.Abs(s) > line1Length / 2) || (Math.Abs(s) > line2Length / 2))
            {
                if (s < 0)
                    s = -Math.Min(line1Length / 2, line2Length / 2);
                else
                    s = Math.Min(line1Length / 2, line2Length / 2);
                newRadius = s * Math.Tan(alfa / 2);
            }

            line1.NewX2 = line1.X2 + Math.Abs(s) * Math.Cos(f1);
            line1.NewY2 = line1.Y2 + Math.Abs(s) * Math.Sin(f1);
            line2.NewX1 = line2.X1 + Math.Abs(s) * Math.Cos(f2);
            line2.NewY1 = line2.Y1 + Math.Abs(s) * Math.Sin(f2);

            double circleCenterAngle = f1 + alfa / 2;
            double cs = newRadius / Math.Sin(alfa / 2);
            PointF circleCenter = new PointF();
            if (s > 0)
            {
                circleCenter.X = (float)(line1.X2 + cs * Math.Cos(circleCenterAngle));
                circleCenter.Y = (float)(line1.Y2 + cs * Math.Sin(circleCenterAngle));
            }
            else
            {
                circleCenter.X = (float)(line1.X2 - cs * Math.Cos(circleCenterAngle));
                circleCenter.Y = (float)(line1.Y2 - cs * Math.Sin(circleCenterAngle));
            }

            double firstAngle = Math.Atan2(line1.NewY2 - circleCenter.Y, line1.NewX2 - circleCenter.X);
            double secondAngle = Math.Atan2(line2.NewY1 - circleCenter.Y, line2.NewX1 - circleCenter.X);
            double startAngle = firstAngle;
            double sweepAngle = secondAngle - firstAngle;
            if (sweepAngle > Math.PI)
                sweepAngle = -(2 * Math.PI - sweepAngle);
            else
            {
                if (sweepAngle < -Math.PI)
                    sweepAngle = (2 * Math.PI + sweepAngle);
            }
            arc.X = circleCenter.X - newRadius;
            arc.Y = circleCenter.Y - newRadius;
            arc.Width = newRadius * 2;
            arc.Height = newRadius * 2;
            arc.StartAngle = startAngle * (180 / Math.PI);
            arc.SweepAngle = sweepAngle * (180 / Math.PI);
        }

        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public static class GraphicsExtenions
    {
        #region Circle
        /// <summary>
        /// Draws the circle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="radius">The radius.</param>
        public static void DrawCircle(this Graphics gp, Pen pen, int x, int y, int radius)
        {
            gp.DrawCircle(pen, new Rectangle(x, y, radius, radius));
        }
        /// <summary>
        /// Draws the circle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="rect">The rect.</param>
        public static void DrawCircle(this Graphics gp, Pen pen, Rectangle rect)
        {
            gp.DrawEllipse(pen, rect);
        }
        /// <summary>
        /// Fills the circle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="radius">The radius.</param>
        public static void FillCircle(this Graphics gp, Brush brush, int radius)
        {
            gp.FillCircle(brush, new Rectangle(0, 0, radius, radius));
        }
        /// <summary>
        /// Fills the circle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="rect">The rect.</param>
        public static void FillCircle(this Graphics gp, Brush brush, Rectangle rect)
        {
            gp.FillPie(brush, rect, 0, 360);
        }
        #endregion

        #region Round corner Rectangle
        /// <summary>
        /// Fills the rounded rectangle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="border">The border.</param>
        /// <param name="borderColor">Color of the border.</param>
        /// <param name="corner">The corner.</param>
        public static void FillRoundedRectangle(this Graphics gp, System.Drawing.Brush brush,
          float x, float y,
          float width, float height, float radius, float border, Color borderColor, Corner corner = Corner.All)
        {
            PointF basePoint = new PointF(x, y);

            //PointF[] roundedRectangle = new PointF[5];
            //roundedRectangle[0].X = basePoint.X;
            //roundedRectangle[0].Y = basePoint.Y;
            //roundedRectangle[1].X = basePoint.X + width;
            //roundedRectangle[1].Y = basePoint.Y;
            //roundedRectangle[2].X = basePoint.X + width;
            //roundedRectangle[2].Y = basePoint.Y + height;
            //roundedRectangle[3].X = basePoint.X;
            //roundedRectangle[3].Y = basePoint.Y + height;
            //roundedRectangle[4].X = basePoint.X;
            //roundedRectangle[4].Y = basePoint.Y;
            var border1 = 0f;
            var border2 = border;
            if (border2 > 1)
            {
                border1 = border2 / 2.0f - 1;
                border2 = border2 - 1;
            }

            var path = GetRoundedPath(x + border1, y + border1, width - border2, height - border2, radius, corner);

            gp.FillPath(brush, path);
            if (border > 0)
            {
                Pen pen = new Pen(borderColor, border);
                gp.DrawPath(pen, path);
                pen.Dispose();
            }

            brush.Dispose();
            //Pen pen = new Pen(System.Drawing.Color.Black,2);
            //gp.DrawPath(pen, path);
        }
        /// <summary>
        /// Gets the rounded path.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corner">The corner.</param>
        /// <returns></returns>
        private static GraphicsPath GetRoundedPath(float x, float y, float width, float height, float radius, Corner corner)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius > 0)
            {
                Corners r = new Corners(x, y, width, height, radius, corner);
                r.Execute(path);
            }
            else
            {
                path.AddRectangle(new RectangleF(x, y, width, height));
            }
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Fills the rounded rectangle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corner">The corner.</param>
        public static void FillRoundedRectangle(this Graphics gp, System.Drawing.Brush brush,
          float x, float y,
          float width, float height, float radius, Corner corner = Corner.All)
        {

            gp.FillRoundedRectangle(brush, x, y, width, height, radius, 0, Color.Transparent, corner);
            //RectangleF rectangle = new RectangleF(x, y, width, height);
            //GraphicsPath path = GetRoundedRect(rectangle, radius, corner);
            //gp.FillPath(brush, path);

            //Pen pen = new Pen(System.Drawing.Color.Black,2);
            //gp.DrawPath(pen, path);
        }

        /// <summary>
        /// Draws the rounded rectangle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corner">The corner.</param>
        public static void DrawRoundedRectangle(this Graphics gp, System.Drawing.Pen pen,
          float x, float y,
          float width, float height, float radius, Corner corner = Corner.All)
        {
            RectangleF rectangle = new RectangleF(x, y, width - pen.Width, height - pen.Width);
            GraphicsPath path = GetRoundedRect(rectangle, radius, corner);
            gp.DrawPath(pen, path);
        }

        #region Get the desired Rounded Rectangle path.
        /// <summary>
        /// Gets the rounded rect.
        /// </summary>
        /// <param name="baseRect">The base rect.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corner">The corner.</param>
        /// <returns></returns>
        private static GraphicsPath GetRoundedRect(RectangleF baseRect,
           float radius, Corner corner)
        {
            // if corner radius is less than or equal to zero, 

            // return the original rectangle 

            if (radius <= 0.0F)
            {
                GraphicsPath mPath = new GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }

            // if the corner radius is greater than or equal to 

            // half the width, or height (whichever is shorter) 

            // then return a capsule instead of a lozenge 

            //if (radius >= (System.Math.Min(baseRect.Width, baseRect.Height)) / 2.0)
            //    return GetCapsule(baseRect);

            // create the arc for the rectangle sides and declare 

            // a graphics path object for the drawing 

            float diameter = radius * 2.0F;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            if ((corner & Corner.TopLeft) == Corner.TopLeft)
            {
                path.AddArc(arc, 180, 90);
            }
            else
            {
                path.AddLine(new PointF(baseRect.X, baseRect.Y), new PointF(radius, baseRect.Y));
            }

            //画多边形边
            path.AddLine(
                new PointF(radius, baseRect.Y),
                new PointF(baseRect.Right - radius, baseRect.Y)
            );

            // top right arc 

            arc.X = baseRect.Right - diameter;
            if ((corner & Corner.TopRight) == Corner.TopRight)
            {
                path.AddArc(arc, 270, 90);
                path.AddLine(
                new PointF(baseRect.Right, radius),
                new PointF(baseRect.Right, baseRect.Bottom - radius));
            }
            else
            {
                path.AddLine(new PointF(baseRect.Right - radius, baseRect.Y), new PointF(baseRect.Right, baseRect.Y));
                path.AddLine(new PointF(baseRect.Right, baseRect.Y), new PointF(baseRect.Right, radius));
            }

            arc.Y = baseRect.Bottom - diameter;
            // bottom right arc 
            if ((corner & Corner.BottomRight) == Corner.BottomRight)
            {
                path.AddArc(arc, 0, 90);
            }
            else
            {
                path.AddLine(new PointF(baseRect.Right, baseRect.Bottom - radius), new PointF(baseRect.Right, baseRect.Bottom));
                path.AddLine(new PointF(baseRect.Right, baseRect.Bottom), new PointF(baseRect.Right - radius, baseRect.Bottom));
            }


            path.AddLine(
            new PointF(baseRect.Right - radius, baseRect.Bottom),
            new PointF(radius, baseRect.Bottom));

            arc.X = baseRect.Left;
            if ((corner & Corner.BottomLeft) == Corner.BottomLeft)
            {
                path.AddArc(arc, 90, 90);
            }
            else
            {
                path.AddLine(new PointF(baseRect.Right - radius, baseRect.Bottom), new PointF(baseRect.X, baseRect.Bottom));
                path.AddLine(new PointF(baseRect.X, baseRect.Bottom), new PointF(baseRect.X, baseRect.Bottom - radius));
            }
            // bottom left arc      

            path.AddLine(
            new PointF(baseRect.X, baseRect.Bottom - radius),
            new PointF(baseRect.Y, radius));

            if ((corner & Corner.TopLeft) != Corner.TopLeft)
            {
                path.AddLine(new PointF(baseRect.X, baseRect.Y), new PointF(baseRect.X, radius));
            }

            path.CloseFigure();
            return path;
        }

        private static GraphicsPath GetCapsule(RectangleF baseRect)
        {
            float diameter;
            RectangleF arc;
            GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            try
            {
                if (baseRect.Width > baseRect.Height)
                {
                    // return horizontal capsule 

                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    // return vertical capsule 

                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    // return circle 

                    path.AddEllipse(baseRect);
                }
            }
            catch
            {
                path.AddEllipse(baseRect);
            }
            finally
            {
                path.CloseFigure();
            }
            return path;
        }
        #endregion

        #endregion

        #region Cut corner Rectangle
        /// <summary>
        /// Fills the cut rectangle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corner">The corner.</param>
        public static void FillCutRectangle(this Graphics gp, System.Drawing.Brush brush,
          float x, float y,
          float width, float height, float radius, Corner corner = Corner.All)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = GetCutRect(rectangle, radius, corner);
            gp.FillPath(brush, path);
        }

        /// <summary>
        /// Draws the cut rectangle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corner">The corner.</param>
        public static void DrawCutRectangle(this Graphics gp, System.Drawing.Pen pen, int x, int y,
         int width, int height, int radius, Corner corner)
        {
            float fx = Convert.ToSingle(x);
            float fy = Convert.ToSingle(y);
            float fwidth = Convert.ToSingle(width);
            float fheight = Convert.ToSingle(height);
            float fradius = Convert.ToSingle(radius);
            gp.DrawCutRectangle(pen, fx, fy, fwidth, fheight, fradius, corner);
        }
        /// <summary>
        /// Draws the cut rectangle.
        /// </summary>
        /// <param name="gp">The gp.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corner">The corner.</param>
        public static void DrawCutRectangle(this Graphics gp, System.Drawing.Pen pen,
          float x, float y,
          float width, float height, float radius, Corner corner = Corner.All)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = GetCutRect(rectangle, radius, corner);
            gp.DrawPath(pen, path);
        }

        #region Gets the cut Rectangle path.
        /// <summary>
        /// Gets the cut rect.
        /// </summary>
        /// <param name="baseRect">The base rect.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corner">The corner.</param>
        /// <returns></returns>
        private static GraphicsPath GetCutRect(RectangleF baseRect,
      float radius, Corner corner)
        {
            // if corner radius is less than or equal to zero, 

            // return the original rectangle 

            if (radius <= 0.0F)
            {
                GraphicsPath mPath = new GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }

            // if the corner radius is greater than or equal to 

            // half the width, or height (whichever is shorter) 

            // then return a capsule instead of a lozenge 

            //if (radius >= (System.Math.Min(baseRect.Width, baseRect.Height)) / 2.0)
            //    return GetCapsule(baseRect);

            // create the arc for the rectangle sides and declare 

            // a graphics path object for the drawing 

            //float diameter = radius * 2.0F;
            //SizeF sizeF = new SizeF(diameter, diameter);
            //RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            if ((corner & Corner.TopLeft) == Corner.TopLeft)
            {
                path.AddLine(new PointF(radius, baseRect.Y), new PointF(0, radius));
            }
            else
            {
                path.AddLine(new PointF(baseRect.X, baseRect.Y), new PointF(radius, baseRect.Y));
                path.AddLine(new PointF(baseRect.X, baseRect.Y), new PointF(baseRect.X, radius));
            }

            //画多边形边
            path.AddLine(
                new PointF(radius, baseRect.Y),
                new PointF(baseRect.Right - radius, baseRect.Y)
            );

            // top right corner

            if ((corner & Corner.TopRight) == Corner.TopRight)
            {
                path.AddLine(
                new PointF(baseRect.Right - radius, baseRect.Y),
                new PointF(baseRect.Right, radius));
            }
            else
            {
                path.AddLine(new PointF(baseRect.Right - radius, baseRect.Y), new PointF(baseRect.Right, baseRect.Y));
                path.AddLine(new PointF(baseRect.Right, baseRect.Y), new PointF(baseRect.Right, radius));
            }


            // bottom right arc 
            if ((corner & Corner.BottomRight) == Corner.BottomRight)
            {
                path.AddLine(
                 new PointF(baseRect.Right, baseRect.Bottom - radius),
                 new PointF(baseRect.Right - radius, baseRect.Bottom));
            }
            else
            {
                path.AddLine(new PointF(baseRect.Right, baseRect.Bottom - radius), new PointF(baseRect.Right, baseRect.Bottom));
                path.AddLine(new PointF(baseRect.Right, baseRect.Bottom), new PointF(baseRect.Right - radius, baseRect.Bottom));
            }


            path.AddLine(
            new PointF(baseRect.Right - radius, baseRect.Bottom),
            new PointF(radius, baseRect.Bottom));

            if ((corner & Corner.BottomLeft) == Corner.BottomLeft)
            {
                path.AddLine(
                  new PointF(radius, baseRect.Bottom),
                  new PointF(0, baseRect.Bottom - radius));
            }
            else
            {
                path.AddLine(new PointF(baseRect.Right - radius, baseRect.Bottom), new PointF(baseRect.X, baseRect.Bottom));
                path.AddLine(new PointF(baseRect.X, baseRect.Bottom), new PointF(baseRect.X, baseRect.Bottom - radius));
            }
            // bottom left arc      

            path.AddLine(
            new PointF(baseRect.X, baseRect.Bottom - radius),
            new PointF(baseRect.X, radius));

            if ((corner & Corner.TopLeft) != Corner.TopLeft)
            {
                path.AddLine(new PointF(baseRect.X, baseRect.Y), new PointF(baseRect.X, radius));
            }

            path.CloseFigure();
            return path;
        }
        #endregion
        #endregion


    }

}
