using SwitchWinForms;               //Primary object for custom Titlebars, Body, Buttons, etc..
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    partial class FormDemo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
        private readonly Font DefaultButtonFont = new Font("Verdana", 12, FontStyle.Regular, GraphicsUnit.Pixel);
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 488);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
        }
        #endregion

        #region Added User code
        /// <summary>
        /// In use of SwitchWinForms.dll
        /// </summary>
        private void LoadSwitchWinForms()
        {
            _tbOptions = new SWFTitlebarOptions()
            {
                RoundedCorners = SWF_ARC_CORNERS.TopLeft | SWF_ARC_CORNERS.TopRight,
                Icon = this.Icon.ToBitmap(),
                RoundedRadius = 15,
                Height = 30,
                Padding = new Padding(2, 2, 2, 2),
                BackColor = Color.FromArgb(255, 30, 30, 30),// SystemColors.ActiveCaption,
                BorderColor = Color.Black,
                BorderSize = 2,
                ButtonsEnabled = SWF_TITLE_BUTTON.All,
                Close = new SWFTitlebarOptions.ButtonsState() //customizing only the close and defaults for Min and Max/Norm
                {
                    RoundedCorners = SWF_ARC_CORNERS.TopRight,
                    RoundedRadius = 5,
                    BorderColor = Color.Transparent,
                    BorderSize = 1,
                    BackColor = Color.Wheat,
                    TextSetup = new TextUtil.TextOptions()
                    {
                        Color = Color.Black,
                        Text = SWFUnicodeChars.CLOSE.ToString(),
                        Font = DefaultButtonFont,
                        HorizonalAlign = StringAlignment.Center,
                        VerticalAlign = StringAlignment.Center,
                    }
                },
                Minimized = new SWFTitlebarOptions.ButtonsState() //customizing only the close and defaults for Min and Max/Norm
                {
                    RoundedCorners = SWF_ARC_CORNERS.None,
                    RoundedRadius = 15,
                    BorderColor = Color.Transparent,
                    BorderSize = 1,
                    BackColor = Color.Wheat,
                    TextSetup = new TextUtil.TextOptions()
                    {
                        Color = Color.Black,
                        Text = SWFUnicodeChars.MIN.ToString(),
                        Font = DefaultButtonFont,
                        HorizonalAlign = StringAlignment.Center,
                        VerticalAlign = StringAlignment.Center,
                    }
                },
                Maximize = new SWFTitlebarOptions.ButtonsState() //customizing only the close and defaults for Min and Max/Norm
                {
                    RoundedCorners = SWF_ARC_CORNERS.None,
                    RoundedRadius = 15,
                    BorderColor = Color.Transparent,
                    BorderSize = 1,
                    BackColor = Color.Wheat,
                    TextSetup = new TextUtil.TextOptions()
                    {
                        Color = Color.Black,
                        Text = SWFUnicodeChars.MAX.ToString(),
                        Font = DefaultButtonFont,
                        HorizonalAlign = StringAlignment.Center,
                        VerticalAlign = StringAlignment.Center,
                    }
                },
                Normal = new SWFTitlebarOptions.ButtonsState() //customizing only the close and defaults for Min and Max/Norm
                {
                    RoundedCorners = SWF_ARC_CORNERS.None,
                    RoundedRadius = 15,
                    BorderColor = Color.Transparent,
                    BorderSize = 1,
                    BackColor = Color.Wheat,
                    TextSetup = new TextUtil.TextOptions()
                    {
                        Color = Color.Black,
                        Text = SWFUnicodeChars.NORM.ToString(),
                        Font = DefaultButtonFont,
                        HorizonalAlign = StringAlignment.Center,
                        VerticalAlign = StringAlignment.Center,
                    }
                },
                TextSetup = new TextUtil.TextOptions()
                {
                    //BackgroundImage = (Image)SWFBoxing.ImageFromFile<Image>(@"imgs\test.png"),
                    //BackgroundImageLayout = SWF_BG_IMG_LAYOUT.Tile,
                    //ReplaceImageColor = new List<ReplaceColor>() { new ReplaceColor(Color.FromArgb(241, 81, 27), Color.Transparent, 100) },
                    Text = $"Demo v1",
                    Color = Color.Wheat,
                    Font = new Font("Verdana", 10, FontStyle.Bold),
                    HorizonalAlign = StringAlignment.Near,
                    VerticalAlign = StringAlignment.Center
                },
            };
            _frmOptions = new SWFBoxingOptions()
            {
                Name = "FrmBody",
                Type = SWF_BOX_TYPE.FormBody,
                TitleBarHeight = _tbOptions.Height, //default is 30, if not set for SWF_BOX_TYPE.FormBody
                RoundedCorners = SWF_ARC_CORNERS.BottomLeft | SWF_ARC_CORNERS.BottomRight,
                RoundedRadius = 15,
                BackColor = Color.FromArgb(255, 62, 62, 66),
                BorderColor = Color.Black,
                BorderSize = 2,
                Location = new Point(0, 0),
                Padding = new Padding(2),
                Size = new Size(this.Width, this.Height),
                TextSetup = new TextUtil.TextOptions()
                {
                    Color = Color.Wheat,
                    Text = "Gavin",
                    Font = new Font("Verdana", 24, FontStyle.Bold, GraphicsUnit.Pixel),
                    HorizonalAlign = StringAlignment.Center,
                    VerticalAlign = StringAlignment.Center
                },
            };

            _frmMain = new SWFBoxing(this, _frmOptions);
            _titleBar = new SWFTitlebar(this, _tbOptions);

            SetupButtons();

            _frmMain.Resize += Form1_Resize;
        }
        private void SetupButtons()
        {
            _btn2StretchImg = new SWFBoxing(this, new SWFBoxingOptions()
            {
                Name = "BtnStretchedButton",
                RoundedCorners = SWF_ARC_CORNERS.TopLeft | SWF_ARC_CORNERS.BottomRight,
                RoundedRadius = 80,
                BackColor = Color.DarkGray,
                BorderColor = Color.Black,
                BorderSize = 1,
                Location = new Point(100, 100),
                Padding = new Padding(2),
                Size = new Size(200, 200),
                IsAnimatedButton = true,
                Type = SWF_BOX_TYPE.Button,
                Cursors = new SWFCursors(Cursors.Hand, Cursors.Default),
                Shadowing = new ShadowingSetup()
                {
                    Visibilty = SWF_TRANSPARENT_PERCENTAGE.Forty,
                    Color = Color.Black,
                    Depth = 20
                },
                TextSetup = new TextUtil.TextOptions()
                {
                    BackgroundImage = (Image)SWFBoxing.ImageFromFile<Image>(@"imgs\test.png"),
                    BackgroundImageLayout = SWF_BG_IMG_LAYOUT.Stretch,
                    ReplaceImageColor = new List<ReplaceColor>() {
                        new ReplaceColor(Color.FromArgb(27, 157, 44), Color.Transparent, 100),
                        new ReplaceColor(Color.FromArgb(159, 215, 90), Color.Transparent, 50),
                        new ReplaceColor(Color.White, Color.Transparent, 90)
                    },
                    Color = Color.Black,
                    Text = "Click Stretched Button",
                    Font = new Font("Verdana", 10, FontStyle.Bold, GraphicsUnit.Pixel),
                    HorizonalAlign = StringAlignment.Center,
                    VerticalAlign = StringAlignment.Center
                },
            });

            _btn2StretchImg.MouseUp += (sender, e) =>
            {
                lock (_lblDisplay.BoxingOptions.TextSetup)
                {
                    SWFBoxing box = (SWFBoxing)sender;
                    _lblDisplay.BoxingOptions.TextSetup.Text = $"Name: \"{box.BoxingOptions.Name}\" - Text:\"{box.BoxingOptions.TextSetup.Text}\"";
                }
            };

            _btn1TileImg = new SWFBoxing(this, new SWFBoxingOptions()
            {
                Name = "BtnTiledButton",
                RoundedCorners = SWF_ARC_CORNERS.TopRight,
                RoundedRadius = 40,
                //BackColor = Color.Red,
                BackColor = Color.Wheat,
                BorderColor = Color.Black,
                BorderSize = 1,
                Location = new Point(_btn2StretchImg.BoxingOptions.Location.X + _btn2StretchImg.BoxingOptions.Size.Width + 30
                                    , _btn2StretchImg.BoxingOptions.Location.Y),
                Padding = new Padding(2),
                Size = new Size(200, 200),
                IsAnimatedButton = true,
                Cursors = new SWFCursors(Cursors.Hand, Cursors.Default),
                Type = SWF_BOX_TYPE.Button,
                Shadowing = new ShadowingSetup()
                {
                    Visibilty = SWF_TRANSPARENT_PERCENTAGE.Twenty,
                    Color = Color.Black,
                    Depth = 7
                },
                TextSetup = new TextUtil.TextOptions()
                {
                    BackgroundImage = (Image)SWFBoxing.ImageFromFile<Image>(@"imgs\test.png"),
                    BackgroundImageLayout = SWF_BG_IMG_LAYOUT.Tile,
                    ReplaceImageColor = new List<ReplaceColor>() {
                                            new ReplaceColor(Color.FromArgb(247, 172, 8), Color.Transparent, 100),      //solid color
                                            new ReplaceColor(Color.FromArgb(255, 255, 111), Color.Transparent, 50) },   //border around color
                    Color = Color.Black,
                    Text = "Click Tiled Button",
                    Font = new Font("Verdana", 10, FontStyle.Bold, GraphicsUnit.Pixel),
                    HorizonalAlign = StringAlignment.Center,
                    VerticalAlign = StringAlignment.Center
                },
            });

            _btn1TileImg.MouseUp += (sender, e) =>
            {
                lock (_lblDisplay.BoxingOptions.TextSetup)
                {
                    SWFBoxing box = (SWFBoxing)sender;
                    _lblDisplay.BoxingOptions.TextSetup.Text = $"Name: \"{box.BoxingOptions.Name}\" - Text:\"{box.BoxingOptions.TextSetup.Text}\"";
                }
            };

            _lblDisplay = new SWFBoxing(this, new SWFBoxingOptions()
            {
                Name = "LblDisplay",
                BackColor = Color.FromArgb(60, 255, 255, 255),
                BorderColor = Color.Black,
                BorderSize = 1,
                Padding = new Padding(2),
                //Location = new Point(0, _frmMain.BoxingOptions.Size.Height),          //handled at resize
                //Size = new Size(_frmMain.BoxingOptions.Size.Width, 30),               //handled at resize
                Type = SWF_BOX_TYPE.Label,
                TextSetup = new TextUtil.TextOptions()
                {
                    Color = Color.Black,
                    Text = "Waiting for click",
                    Font = new Font("Verdana", 10, FontStyle.Bold, GraphicsUnit.Pixel),
                    HorizonalAlign = StringAlignment.Near,
                    VerticalAlign = StringAlignment.Center
                },
            });

            _txtInput = new SWFBoxing(this, new SWFBoxingOptions()
            {
                Name = "TxtInput",
                BackColor = Color.White,
                BorderColor = Color.Black,
                BorderSize = 1,
                Padding = new Padding(2),
                Cursors = new SWFCursors(Cursors.IBeam, Cursors.Default),
                //Location = new Point(30, _lblDisplay.BoxingOptions.Location.Y - 40),  //handled at resize
                //Size = new Size(_frmMain.BoxingOptions.Size.Width - 60, 30),          //handled at resize
                Type = SWF_BOX_TYPE.TextBox,
                TextSetup = new TextUtil.TextOptions()
                {
                    Color = Color.Black,
                    Text = "[Email Here]",
                    ValidInputExpression = @"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$",
                    Font = new Font("Courier New", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                    HorizonalAlign = StringAlignment.Near,
                    VerticalAlign = StringAlignment.Center
                },
            });

            //show any exceptions throw by input here.
            _txtInput.Exception += (sender, e) =>
            {
                lock (_txtInput.BoxingOptions.TextSetup)
                {
                    SWFBoxing box = (SWFBoxing)sender;

                    string msg = $"{box.BoxingOptions.Name}: {e.Exception.Message}";
                    _lblDisplay.BoxingOptions.TextSetup.Text = msg;
                    MessageBox.Show(msg, "Exception");
                    this.Invalidate();
                }
            };

            //show success messages here.
            _txtInput.Success += (sender, e) =>
            {
                lock (_txtInput.BoxingOptions.TextSetup)
                {
                    SWFBoxing box = (SWFBoxing)sender;

                    string msg = $"{box.BoxingOptions.Name}: {e.Message}";
                    _lblDisplay.BoxingOptions.TextSetup.Text = msg;
                    this.Invalidate();
                }
            };
        }

        private SWFTitlebarOptions _tbOptions;
        private SWFBoxingOptions _frmOptions;
        private SWFTitlebar _titleBar;
        private SWFBoxing _frmMain;
        private SWFBoxing _btn1TileImg;
        private SWFBoxing _btn2StretchImg;
        private SWFBoxing _lblDisplay;
        private SWFBoxing _txtInput;
        #endregion
    }
}

