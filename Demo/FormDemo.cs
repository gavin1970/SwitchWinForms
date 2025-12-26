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
            //pre-set Unicode characters.
            string close_Text_Example1 = SWFUnicodeChars.CLOSE;
            //custom Unicode characters can be passed here..
            string close_Text_Example2 = new SWFUnicodeChars(10062).ToString();
        */
        public FormDemo()
        {
            InitializeComponent();
            this.Icon = (Icon)SWFBoxing.ImageFromFile<Icon>(@"imgs\test.png");
            this.LoadSwitchWinForms();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
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
