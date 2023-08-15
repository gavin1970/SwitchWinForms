using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SwitchWinForms
{
    internal class Global
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private const string DefaultTextFont = "Courier New";
        private static readonly object _thLock = new object();

        internal static bool IsDoubleBuffered { get; private set; } = false;

        internal static List<Keys> IgnoreKeys { get; } = new List<Keys>() { Keys.Right, Keys.Left, Keys.Home, Keys.End,
                                                                    Keys.Enter, Keys.Return, Keys.Back, Keys.Delete, 
                                                                    Keys.Escape, Keys.Shift, Keys.ShiftKey, Keys.Capital, 
                                                                    Keys.Alt, Keys.Control, Keys.ControlKey,  
                                                                    Keys.CapsLock, Keys.Snapshot, Keys.Print, Keys.PrintScreen, 
                                                                    Keys.Menu, Keys.Apps, Keys.Up, Keys.Down, Keys.PageDown, 
                                                                    Keys.PageUp, Keys.VolumeDown,
                                                                    Keys.VolumeUp, Keys.VolumeMute };
        internal static Dictionary<Keys, char> ConvertKey { get; } = new Dictionary<Keys, char>()
        {
            { Keys.Enter, '\n' },
            { Keys.Tab, '\t' },
            { Keys.OemPeriod, '.' },
            { Keys.Decimal, '.' },
            { Keys.NumPad1, '1' },
            { Keys.NumPad2, '2' },
            { Keys.NumPad3, '3' },
            { Keys.NumPad4, '4' },
            { Keys.NumPad5, '5' },
            { Keys.NumPad6, '6' },
            { Keys.NumPad7, '7' },
            { Keys.NumPad8, '8' },
            { Keys.NumPad9, '9' },
            { Keys.NumPad0, '0' },
            { Keys.D1, '1' },
            { Keys.D2, '2' },
            { Keys.D3, '3' },
            { Keys.D4, '4' },
            { Keys.D5, '5' },
            { Keys.D6, '6' },
            { Keys.D7, '7' },
            { Keys.D8, '8' },
            { Keys.D9, '9' },
            { Keys.D0, '0' },
        };
        internal static Font DefaultFont { get; } = new Font("Verdana", 12, FontStyle.Regular, GraphicsUnit.Pixel);
        internal static Font DefaultButtonFont { get; } = new Font("Verdana", 12, FontStyle.Regular, GraphicsUnit.Pixel);
        internal static Color DefaultFontColor { get; } = Color.Black;
        internal static bool IsInBounds(Form frm, Point pt, Rectangle rect, Cursor cursorType = null)
        {
            bool retVal = false;

            if (pt.X >= rect.Left &&
                pt.Y >= rect.Top &&
                pt.X <= rect.Left + rect.Width &&
                pt.Y <= rect.Top + rect.Height)
            {
                if (frm != null && cursorType != null)
                    frm.Cursor = cursorType;

                retVal = true;
            }

            return retVal;
        }
        internal static bool MouseIsClose(Point pt, Rectangle rect)
        {
            int count = 0;
            int closeBy = 2;
            bool[] hasBool = new bool[4];

            hasBool[0] = pt.X < rect.X && (rect.X - pt.X <= closeBy) ? true : false;
            hasBool[1] = pt.Y < rect.Y && (rect.Y - pt.Y <= closeBy) ? true : false;
            hasBool[2] = pt.X > (rect.X + rect.Width) && (pt.X - (rect.X + rect.Width) <= closeBy) ? true : false;
            hasBool[3] = pt.Y > (rect.Y + rect.Height) && (pt.Y - (rect.Y + rect.Height) <= closeBy) ? true : false;

            for(int i= 0; i < 4; i++)
            {
                if (hasBool[i])
                    count++;
            }

            return count >= 1;
        }
        internal static BoxInfo CreateBox(Graphics g, BoxInfo boxInfo)
        {
            int buffer = 1;
            Size maxSize = boxInfo.Rectangle.Size;
            Rectangle innerBoxRect = boxInfo.Rectangle;

            if (innerBoxRect.Width < 1 || innerBoxRect.Height < 1)
                return boxInfo;

            //create the shadow first.
            if (boxInfo.Shadowing != null && 
                boxInfo.Shadowing.Visibilty != SWF_TRANSPARENT_PERCENTAGE.Empty && 
                !boxInfo.ButtonClicked)
            {
                Color shaColor = Color.FromArgb(
                        boxInfo.Shadowing.getVisibilty,
                        boxInfo.Shadowing.Color.R,
                        boxInfo.Shadowing.Color.G,
                        boxInfo.Shadowing.Color.B);

                Brush shadowBrush = new SolidBrush(shaColor);

                Rectangle shadowBox = new Rectangle(
                                            boxInfo.Rectangle.X + boxInfo.Shadowing.Depth,
                                            boxInfo.Rectangle.Y + boxInfo.Shadowing.Depth,
                                            boxInfo.Rectangle.Width,
                                            boxInfo.Rectangle.Height);

                FillRoundedRectangle(g, shadowBrush, boxInfo, shadowBox, true);
            }
            
            //&& !boxInfo.BackgroundImageModified
            if (boxInfo.BackgroundImage != null )
            {
                Image newImg = boxInfo.BackgroundImage;
                if (boxInfo.ReplaceImageColor.Count > 0)
                {
                    //we dont want to call MakeTransparent each roll, this
                    //will cause jumping excuted mutliple times.
                    foreach (ReplaceColor rc in boxInfo.ReplaceImageColor)
                        newImg = MakeTransparent(newImg, rc, rc.ImageColorTolerance);
                    
                    boxInfo.ReplaceImageColor = new List<ReplaceColor>();
                    //This makes it perminate until next restart.
                    
                    boxInfo.BackgroundImage = newImg;
                    boxInfo.BackgroundImageModified = true;
                }

                if (boxInfo.BackgroundImageLayout != SWF_BG_IMG_LAYOUT.Tile &&
                    (boxInfo.BackgroundImage.Width > boxInfo.Rectangle.Width ||
                    boxInfo.BackgroundImage.Height > boxInfo.Rectangle.Height))
                {
                    newImg = ResizeBitmap(new Bitmap(newImg), boxInfo.Rectangle.Size);
                    boxInfo.BackgroundImage = newImg;
                    boxInfo.BackgroundImageModified = true;
                }
            }

            //create the background.
            SolidBrush bgColor = new SolidBrush(boxInfo.BackColor);
            FillRoundedRectangle(g, bgColor, boxInfo, boxInfo.Rectangle, false);

            //if border, then draw border ontop of background.
            if (boxInfo.BorderSize > 0)
            {
                for (int i = 0; i < boxInfo.BorderSize; i++)
                {
                    Rectangle borderRect = new Rectangle()
                    {
                        Location = new Point(boxInfo.Rectangle.Location.X + i, boxInfo.Rectangle.Location.Y + i),
                        Size = new Size((maxSize.Width - (i * 2)) - buffer, (maxSize.Height - (i * 2)) - buffer)
                    };

                    //create the border
                    Pen titlePen = new Pen(boxInfo.BorderColor, boxInfo.BorderSize);
                    DrawRoundedRectangle(g, titlePen, boxInfo, borderRect);

                    innerBoxRect = borderRect;
                }
            }

            if (!string.IsNullOrWhiteSpace(boxInfo.Text))
            {
                StringFormat stringFormat = new StringFormat()
                {
                    Alignment = boxInfo.HAlign,
                    LineAlignment = boxInfo.VAlign
                };

                innerBoxRect = new Rectangle(innerBoxRect.Left + boxInfo.Padding.Left,
                                            innerBoxRect.Top + boxInfo.Padding.Top,
                                            innerBoxRect.Width - boxInfo.Padding.Right,
                                            innerBoxRect.Height - boxInfo.Padding.Bottom);

                if (boxInfo.Type == SWF_BOX_TYPE.Titlebar && boxInfo.Icon != null)
                    innerBoxRect.Location = new Point((innerBoxRect.X * 2) + innerBoxRect.Height + (boxInfo.Padding.Left * 2), innerBoxRect.Y);
                else if (boxInfo.Type == SWF_BOX_TYPE.Titlebar)
                    innerBoxRect.Location = new Point((innerBoxRect.X * 2) + (boxInfo.Padding.Left * 2), innerBoxRect.Y);

                Font ft = boxInfo.Font;
                if (boxInfo.Type == SWF_BOX_TYPE.TextBox && boxInfo.InputStart)
                {
                    //We have to change the font to "Courier New", since I"m overlaying so I can add the * at the cursor spot. 
                    //An asterisk can be a differnt size than a letter for example if not using a squared font letter.
                    ft = new Font(DefaultTextFont, boxInfo.Font.Size, boxInfo.Font.Style, GraphicsUnit.Pixel);
                    int cursor = boxInfo.TextCursor;
                    string text;

                    if (cursor > 0)
                        text = boxInfo.Text.Substring(0, cursor) + "*";
                    else
                        text = "*";

                    g.DrawString(text, ft, new SolidBrush(Color.Gray), innerBoxRect, stringFormat);
                }

                g.DrawString(boxInfo.Text, ft, new SolidBrush(boxInfo.ForeColor), innerBoxRect, stringFormat);
            }

            return boxInfo;
        }
        internal static bool IsValidGraphic(Graphics g)
        {
            bool retVal = true;

            try
            {
                if (!g.IsClipEmpty)
                    retVal = false;
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }
        internal static Image GetImage(string path)
        {
            if (!File.Exists(path))
                throw new IOException($"File not found.\n${path}");

            Image retVal;
            FileInfo fi = new FileInfo(path);
            try
            {
                switch (fi.Extension.ToLower())
                {
                    case ".exe":
                    case ".ico":
                        retVal = (Image)GetEXEImage<Image>(path);
                        break;
                    default:
                        retVal = Image.FromFile(path);
                        break;
                }
            }
            catch
            {
                throw new IOException("Invalid filetype.");
            }

            return retVal;
        }
        internal static Icon GetIcon(string path)
        {
            if (!File.Exists(path))
                throw new IOException($"File not found.\n${path}");

            FileInfo fi = new FileInfo(path);
            Icon retVal;
            try
            {
                switch (fi.Extension.ToLower())
                {
                    case ".exe":
                    case ".ico":
                        retVal = (Icon)GetEXEImage<Icon>(path);
                        break;
                    default:
                        Bitmap bmp = (Bitmap)Image.FromFile(path);
                        if (bmp.Width > 128 || bmp.Height > 128)    //max icon size: 128x128 .ico
                            bmp = ResizeBitmap(bmp, new Size(128, 128));
                        IntPtr Hicon = bmp.GetHicon();
                        retVal = (Icon)Icon.FromHandle(Hicon).Clone();
                        //Required cleanup or creates memory leak.
                        DestroyIcon(Hicon);
                        break;
                }
            }
            catch
            {
                throw new IOException("Invalid filetype.");
            }

            return retVal;
        }
        private static Bitmap ResizeBitmap(Bitmap img, Size sz)
        {
            float width = sz.Width;
            float height = sz.Height;
            var brush = new SolidBrush(Color.Transparent);
            float scale = Math.Min(width / ((float)img.Width), height / ((float)img.Height));
            var bmp = new Bitmap(sz.Width, sz.Height);
            var graph = Graphics.FromImage(bmp);
            var scaleWidth = (int)(img.Width * scale);
            var scaleHeight = (int)(img.Height * scale);
            RectangleF rec = new RectangleF(((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight);
            graph.FillRectangle(brush, rec);
            graph.DrawImage(img, rec);

            return bmp;
        }
        internal static void DrawRoundedRectangle(Graphics graphics, Pen pen, BoxInfo boxInfo, Rectangle bounds)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            if (pen == null)
                throw new ArgumentNullException("pen");

            using (GraphicsPath path = RoundedRect(bounds, boxInfo))
            {
                graphics.DrawPath(pen, path);
            }
        }
        internal static void FillRoundedRectangle(Graphics graphics, Brush brush, BoxInfo boxInfo, Rectangle bounds, bool isShadow = false)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            if (brush == null)
                throw new ArgumentNullException("brush");

            using (GraphicsPath path = RoundedRect(bounds, boxInfo))
            {
                graphics.FillPath(brush, path);
                if (boxInfo.BackgroundImage != null && !isShadow)
                {
                    if (boxInfo.BackgroundImageLayout == SWF_BG_IMG_LAYOUT.Normal)
                        graphics.DrawImage(boxInfo.BackgroundImage, bounds.Location);
                    //if (boxInfo.BackgroundImageLayout == SWF_BG_IMG_LAYOUT.Tile)
                    else
                    {
                        Brush gbrush = new TextureBrush(boxInfo.BackgroundImage);
                        graphics.FillPath(gbrush, path);
                    }
                    /*else if (boxInfo.BackgroundImageLayout == SWF_BG_IMG_LAYOUT.Stretch)
                    {
                        graphics.DrawImage(boxInfo.BackgroundImage, bounds.Location);
                    }
                    else if (boxInfo.BackgroundImageLayout == SWF_BG_IMG_LAYOUT.Center)
                    {
                        graphics.DrawImage(boxInfo.BackgroundImage, bounds.Location);
                    }*/
                }
            }
        }
        internal static GraphicsPath PointsToGraphicsPath(List<PointF> points)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(points.ToArray());
            return gp;
        }
        internal static List<PointF> GraphicsPathToPoints(GraphicsPath gp)
        {
            return gp.PathPoints.ToList();
        }
        internal static bool DeleteFile(string path)
        {
            bool retVal = true;

            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }
        internal static bool SetDoubleBuffered(Form frm, out Exception exc)
        {
            //default
            exc = null;

            lock (_thLock)
            {
                //save processing
                if (IsDoubleBuffered)
                    return IsDoubleBuffered;

                if (frm != null)
                {
                    if (SystemInformation.TerminalServerSession)
                    {
                        exc = new Exception("## WARN ##, Flicking may occur, because your in a terminal session.");
                        return IsDoubleBuffered;
                    }

                    try
                    {
                        PropertyInfo aProp =
                            typeof(Control).GetProperty("DoubleBuffered",
                                BindingFlags.NonPublic | BindingFlags.Instance);

                        aProp.SetValue(frm, true, null);
                        IsDoubleBuffered = true;
                    }
                    catch (Exception ex)
                    {
                        exc = ex;
                    }
                }
                else
                    exc = new Exception("## WARN ##, Missing Form object.");
            }

            return IsDoubleBuffered;
        }
        private static GraphicsPath RoundedRect(Rectangle bounds, BoxInfo boxInfo)
        {
            int radiusTopLeft = 0;
            int radiusTopRight = 0;
            int radiusBottomRight = 0;
            int radiusBottomLeft = 0;

            if ((boxInfo.RoundedCorners & SWF_ARC_CORNERS.TopLeft) > 0)
                radiusTopLeft = boxInfo.RoundedRadius;
            if ((boxInfo.RoundedCorners & SWF_ARC_CORNERS.TopRight) > 0)
                radiusTopRight = boxInfo.RoundedRadius;
            if ((boxInfo.RoundedCorners & SWF_ARC_CORNERS.BottomRight) > 0)
                radiusBottomRight = boxInfo.RoundedRadius;
            if ((boxInfo.RoundedCorners & SWF_ARC_CORNERS.BottomLeft) > 0)
                radiusBottomLeft = boxInfo.RoundedRadius;

            var size = new Size(radiusTopLeft << 1, radiusTopLeft << 1);
            var arc = new Rectangle(bounds.Location, size);
            var path = new GraphicsPath();

            // top left arc
            if (radiusTopLeft == 0)
                path.AddLine(arc.Location, arc.Location);
            else
                path.AddArc(arc, 180, 90);

            // top right arc
            if (radiusTopRight != radiusTopLeft)
            {
                size = new Size(radiusTopRight << 1, radiusTopRight << 1);
                arc.Size = size;
            }

            arc.X = bounds.Right - size.Width;
            if (radiusTopRight == 0)
                path.AddLine(arc.Location, arc.Location);
            else
                path.AddArc(arc, 270, 90);

            // bottom right arc
            if (radiusTopRight != radiusBottomRight)
            {
                size = new Size(radiusBottomRight << 1, radiusBottomRight << 1);
                arc.X = bounds.Right - size.Width;
                arc.Size = size;
            }

            arc.Y = bounds.Bottom - size.Height;
            if (radiusBottomRight == 0)
                path.AddLine(arc.Location, arc.Location);
            else
                path.AddArc(arc, 0, 90);

            // bottom left arc
            if (radiusBottomRight != radiusBottomLeft)
            {
                arc.Size = new Size(radiusBottomLeft << 1, radiusBottomLeft << 1);
                arc.Y = bounds.Bottom - arc.Height;
            }

            arc.X = bounds.Left;
            if (radiusBottomLeft == 0)
                path.AddLine(arc.Location, arc.Location);
            else
                path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
        private static object GetEXEImage<T>(string path)
        {
            object retVal;
            string typeofName = typeof(T).Name.ToLower();
            try
            {
                Icon IEIcon = Icon.ExtractAssociatedIcon(path);
                if (typeofName == "icon")
                    retVal = (object)IEIcon;
                else
                    retVal = IEIcon.ToBitmap();
            }
            catch
            {
                retVal = null;
            }

            return retVal;
        }
        private static Image MakeTransparent(Image image, ReplaceColor color, int tolerance)
        {
            return MakeTransparent(new Bitmap(image), color, tolerance);
        }
        private static Bitmap MakeTransparent(Bitmap bitmap, ReplaceColor color, int tolerance)
        {
            Bitmap transparentImage = new Bitmap(bitmap);

            for (int i = transparentImage.Size.Width - 1; i >= 0; i--)
            {
                for (int j = transparentImage.Size.Height - 1; j >= 0; j--)
                {
                    var currentColor = transparentImage.GetPixel(i, j);
                    if (Math.Abs(color.Delete.R - currentColor.R) < tolerance &&
                         Math.Abs(color.Delete.G - currentColor.G) < tolerance &&
                         Math.Abs(color.Delete.B - currentColor.B) < tolerance)
                        transparentImage.SetPixel(i, j, color.Add);
                }
            }

            //transparentImage.MakeTransparent(color.Delete);
            return transparentImage;
        }
    }
}
