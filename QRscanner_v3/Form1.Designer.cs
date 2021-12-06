using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using System.IO;
using System.Net.Http;
using System.Net;
using LattePanda.Firmata;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using QRscanner_v3;
using System.Management;
using System.Diagnostics;

namespace QRscanner_v3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        private string AssignSerialPort()
        {
            var instances = new ManagementClass("Win32_SerialPort").GetInstances();
            string PortName = string.Empty;
            foreach (ManagementObject port in instances)
            {
                PortName = port["deviceid"].ToString();
            }
            return PortName;
        }



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.outBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox3 = new QRscanner_v3.TextBox();
            this.textBox2 = new QRscanner_v3.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.textBox1 = new QRscanner_v3.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input Box";
            this.label1.Visible = false;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // outBox
            // 
            this.outBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.outBox.Location = new System.Drawing.Point(705, 435);
            this.outBox.Multiline = true;
            this.outBox.Name = "outBox";
            this.outBox.ReadOnly = true;
            this.outBox.Size = new System.Drawing.Size(239, 164);
            this.outBox.TabIndex = 2;
            this.outBox.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox3.Font = new System.Drawing.Font("Yu Gothic", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(206, 300);
            this.textBox3.Margin = new System.Windows.Forms.Padding(30);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox3.Size = new System.Drawing.Size(659, 77);
            this.textBox3.TabIndex = 5;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox3.TextChangedCompleteDelay = System.TimeSpan.Parse("00:00:01");
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox2.Font = new System.Drawing.Font("Yu Gothic", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(206, 218);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(659, 77);
            this.textBox2.TabIndex = 4;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.TextChangedCompleteDelay = System.TimeSpan.Parse("00:00:01");
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output Data";
            this.label2.Visible = false;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox1.Location = new System.Drawing.Point(70, 10);
            this.textBox1.MaxLength = 200000;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(922, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChangedCompleteDelay = System.TimeSpan.Parse("00:00:01");
            this.textBox1.Visible = false;
            this.textBox1.TextChangedComplete += new System.EventHandler<System.EventArgs>(this.textBox1_TextChangedComplete);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(826, 507);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(175, 68);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Yu Gothic", 36F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(424, 278);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 61);
            this.label3.TabIndex = 0;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.outBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1024, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.BackColorChanged += new System.EventHandler(this.Form1_BackColorChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private QRscanner_v3.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox outBox;
        private TextBox textBox2;
        private TextBox textBox3;
        private System.Windows.Forms.Timer timer1;
        private Label label2;
        private System.IO.Ports.SerialPort serialPort1;
        private PictureBox pictureBox1;
        private Label label3;
    }

    public class TextBox : System.Windows.Forms.TextBox
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        private System.Timers.Timer timer;


        public TextBox()
        {
            this.timer = new System.Timers.Timer(100);
            this.timer.Elapsed += timer_Elapsed;
            this.GotFocus += TextBoxGotFocus;
            this.Cursor = Cursors.Arrow; // mouse cursor like in other controls
        }

        private void TextBoxGotFocus(object sender, EventArgs args)
        {
            HideCaret(this.Handle);
        }

        public TimeSpan TextChangedCompleteDelay
        {
            get { return TimeSpan.FromMilliseconds(this.timer.Interval); }
            set { this.timer.Interval = value.TotalMilliseconds; }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs args)
        {
            this.timer.Stop();
            this.BeginInvoke(new MethodInvoker(this.OnTextChangedComplete));
        }

        public event EventHandler<EventArgs> TextChangedComplete;

        protected void OnTextChangedComplete()
        {
            if (this.TextChangedComplete != null)
                this.TextChangedComplete(this, new EventArgs());
        }

        protected override void OnTextChanged(EventArgs args)
        {
            if (!this.timer.Enabled)
                this.timer.Start();
            else
            {
                this.timer.Stop();
                this.timer.Start();
            }

            base.OnTextChanged(args);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.timer != null)
                    this.timer.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class GroupBoxQR : System.Windows.Forms.GroupBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (System.Drawing.Drawing2D.GraphicsPath gp = CreatePath(new Rectangle(System.Drawing.Point.Empty, base.Size), 45, false))
            {
                Pen redPen = new Pen(Color.Red, 1);
                e.Graphics.DrawPath(redPen, gp);
            }
            base.OnPaint(e);
        }

        public static System.Drawing.Drawing2D.GraphicsPath CreatePath(Rectangle rect, int nRadius, bool bOutline)
        {
            int nShift = bOutline ? 1 : 0;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X + nShift, rect.Y, nRadius, nRadius, 180f, 90f);
            path.AddArc((rect.Right - nRadius) - nShift, rect.Y, nRadius, nRadius, 270f, 90f);
            path.AddArc((rect.Right - nRadius) - nShift, (rect.Bottom - nRadius) - nShift, nRadius, nRadius, 0f, 90f);
            path.AddArc(rect.X + nShift, (rect.Bottom - nRadius) - nShift, nRadius, nRadius, 90f, 90f);
            path.CloseFigure();
            return path;
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }

}

