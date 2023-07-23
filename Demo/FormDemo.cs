using System;                       //EventArgs - required by Form Events
using System.Collections.Generic;   //List
using System.Drawing;               //Color, SystemColors, Font, FrontStyle, GraphicsUnit, StringAlignment, Point, Size
using System.Windows.Forms;         //Form
using SwitchWinForms;               //Primary object for custom Titlebars, Body, Buttons, etc..

namespace Demo
{
    public partial class FormDemo : Form
    {
        /*
            //pre-set unicode characters.
            string close_Text_Example1 = SWFUnicodeChars.CLOSE;
            //custom unicode characters can be passed here..
            string close_Text_Example2 = new SWFUnicodeChars(10062).ToString();
        */
        public FormDemo()
        {
            InitializeComponent();
            this.Icon = (Icon)SWFBoxing.ImageFromFile<Icon>(@"imgs\test.png");
            this.LoadSwitchWinForms();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //example of callback for form resize. Do this on or after load.
            //anything prior can cause errors, because the form isn't ready yet.
            _frmMain.Resize += Form1_Resize;

            SetupButtons();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            ResetSizes();
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

            ResetSizes();
        }
        private void ResetSizes()
        {
            Size reSize = new Size(this.ClientSize.Width, this.ClientSize.Height - _tbOptions.Height);

            int w = (int)((double)reSize.Width * .3);
            int h = (int)((double)reSize.Height * .3);
            _btn1TileImg.BoxingOptions.Size = new Size(w, h);

            w = (int)((double)reSize.Width);
            h = (int)((double)reSize.Height);

            int brd = _lblDisplay.BoxingOptions.BorderSize + 5;
            _lblDisplay.BoxingOptions.Location = new Point(brd, h - 40);
            _lblDisplay.BoxingOptions.Size = new Size(w - (brd * 2), 30);

            if (_lblDisplay.BoxingOptions.Location.Y - 40 > _tbOptions.Height)
            {
                _txtInput.BoxingOptions.Location = new Point(brd, _lblDisplay.BoxingOptions.Location.Y - 40);
                _txtInput.BoxingOptions.Size = _lblDisplay.BoxingOptions.Size;
            }
        }
        protected override void WndProc(ref Message m)
        {
            //this handles all form edges and button mouse overs for title.
            if (_titleBar == null || !_titleBar.WndProc(ref m))
                base.WndProc(ref m);
        }
    }
}
