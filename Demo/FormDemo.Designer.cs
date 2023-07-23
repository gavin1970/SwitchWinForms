using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SwitchWinForms;               //Primary object for custom Titlebars, Body, Buttons, etc..

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
            this.Load += new System.EventHandler(this.Form1_Load);
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

