/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;

using bedrock.util;

namespace muzzle
{
    /// <summary>
    /// How should the chart be rendered?
    /// </summary>
    public enum ChartStyle
    {
        /// <summary>
        /// Bar char
        /// </summary>
        Bar,
        /// <summary>
        /// Lines
        /// </summary>
        Line,
        /// <summary>
        /// Points
        /// </summary>
        Point
    }

    // TODO: Add vertical scrolling as an option.
    /// <summary>
    /// A StripChart is a scrolling window showing a set number of data points.
    /// As new points are added, old points get shifted along.
    /// </summary>
    [SVN(@"$Id$")]
    public class StripChart : System.Windows.Forms.UserControl
    {
        private bool       m_first     = true;
        private float      m_min       = 0F;
        private float      m_max       = 100F;
        private float      m_last      = 0F;
        private double     m_mean      = 0F;
        private double     m_var_s     = 0F;
        private long       m_count     = 0;

        private int        m_hist      = 100;
        private int        m_pointSize = 5;
        private bool       m_auto      = true;
        private bool       m_label     = true;
        private bool       m_zero      = true;
        private bool       m_showLast  = false;
        private bool       m_showStats = false;
        private string     m_title     = null;
        private Queue      m_list      = new Queue(100);
        private ChartStyle m_style     = ChartStyle.Bar;
        private Color      m_textColor = Color.Red;
        private Color      m_zeroColor = Color.Black;
        private Color      m_statsColor = Color.Wheat;
        private System.Windows.Forms.PictureBox pictureBox1;

        private static float[] s_sampleData = new float[] {
            .9800F,
            .7572F,
            .8259F,
            .3314F,
            .6175F,
            .9606F,
            .7810F,
            .7958F,
            .4636F,
            .0264F };

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Create a stripchart.
        /// </summary>
        public StripChart()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if( components != null )
                    components.Dispose();
            }
            base.Dispose( disposing );
        }

        /// <summary>
        /// Display the last value sent to the chart, in the upper right.
        /// </summary>
        [Description("Display the last value sent to the chart, in the upper right.")]
        [DefaultValue(false)]
        [Category("Chart")]
        public bool ShowLastValue
        {
            get { return m_showLast; }
            set
            {
                if (m_showLast != value)
                {
                    m_showLast = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Display the mean and standard deviation, graphically.
        /// </summary>
        [Description("Display the mean and standard deviation, graphically.")]
        [DefaultValue(false)]
        [Category("Chart")]
        public bool ShowStatistics
        {
            get { return m_showStats; }
            set
            {
                if (m_showStats != value)
                {
                    m_showStats = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Title to display in the chart.  Null for none.
        /// </summary>
        [Description("Title to display in the chart.  Null for none.")]
        [DefaultValue(null)]
        [Category("Chart")]
        public string Title
        {
            get { return m_title; }
            set
            {
                if (m_title != value)
                {
                    m_title = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// The size of points, when in point style.  Ignored otherwise.
        /// </summary>
        [Description("The size of points, when in point style.  Ignored otherwise.")]
        [DefaultValue(5)]
        [Category("Chart")]
        public int PointSize
        {
            get
            {
                return m_pointSize;
            }
            set
            {
                if (m_pointSize != value)
                {
                    m_pointSize = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Chart drawing style.
        /// </summary>
        [Description("Chart drawing style")]
        [DefaultValue(ChartStyle.Bar)]
        [Category("Chart")]
        public ChartStyle Style
        {
            get
            {
                return m_style;
            }
            set
            {
                if (m_style != value)
                {
                    m_style = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Initial minimum value shown
        /// </summary>
        [Description("Initial minimum value shown")]
        [DefaultValue(0F)]
        [Category("Chart")]
        public float Min
        {
            get
            {
                return m_min;
            }
            set
            {
                if (m_min != value)
                {
                    m_min = value;
                    DesignReDraw();
                }
            }
        }
        /// <summary>
        /// Initial maximum value shown
        /// </summary>
        [Description("Initial maximum value shown")]
        [DefaultValue(100F)]
        [Category("Chart")]
        public float Max
        {
            get
            {
                return m_max;
            }
            set
            {
                if (m_max != value)
                {
                    m_max = value;
                    DesignReDraw();
                }
            }
        }
        /// <summary>
        /// Reset min and max as necessary to show all points.
        /// This must be set before adding any points.
        /// </summary>
        [Description("Reset min and max as necessary to show all points.  " +
             "This must be set before adding any points.")]
        [DefaultValue(true)]
        [Category("Chart")]
        public bool AutoScale
        {
            get
            {
                return m_auto;
            }
            set
            {
                m_auto = value;
            }
        }

        /// <summary>
        /// Draw labels with min and max of chart.  Useful with AutoSize set to true.
        /// </summary>
        [Description("Draw labels with min and max of chart.  Useful with AutoSize set to true.")]
        [DefaultValue(true)]
        [Category("Chart")]
        public bool Labels
        {
            get
            {
                return m_label;
            }
            set
            {
                if (m_label != value)
                {
                    m_label = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Draw a line at zero?
        /// </summary>
        [Description("Draw a line at zero?")]
        [DefaultValue(true)]
        [Category("Chart")]
        public bool ZeroLine
        {
            get
            {
                return m_zero;
            }
            set
            {
                if (m_zero != value)
                {
                    m_zero = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Number of points to show
        /// </summary>
        [Description("Number of points to show")]
        [DefaultValue(100)]
        [Category("Chart")]
        public int History
        {
            get
            {
                return m_hist;
            }
            set
            {
                if (m_hist != value)
                {
                    m_hist = value;
                    lock (m_list)
                    {
                        while (m_list.Count > m_hist)
                            m_list.Dequeue();
                    }
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Color to draw the min/max value in, if Labels is true
        /// </summary>
        [Description("Color to draw the min/max value in, if Labels is true")]
        [DefaultValue("Red")]
        [Category("Appearance")]
        public Color TextColor
        {
            get
            {
                return m_textColor;
            }
            set
            {
                if (m_textColor != value)
                {
                    m_textColor = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Color to draw zero line in, if ZeroLine is true
        /// </summary>
        [Description("Color to draw zero line in, if ZeroLine is true")]
        [DefaultValue("Black")]
        [Category("Appearance")]
        public Color ZeroColor
        {
            get
            {
                return m_zeroColor;
            }
            set
            {
                if (m_zeroColor != value)
                {
                    m_zeroColor = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Color to draw the min/max value in, if Labels is true
        /// </summary>
        [Description("Color to draw the standard deviation range in, if ShowStats is true")]
        [DefaultValue("Wheat")]
        [Category("Appearance")]
        public Color StatsColor
        {
            get
            {
                return m_statsColor;
            }
            set
            {
                if (m_statsColor != value)
                {
                    m_statsColor = value;
                    DesignReDraw();
                }
            }
        }


        /// <summary>
        /// Foreground color
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (base.ForeColor != value)
                {
                    base.ForeColor = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Background color
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (base.BackColor != value)
                {
                    base.BackColor = value;
                    pictureBox1.BackColor = value;
                    DesignReDraw();
                }
            }
        }

        /// <summary>
        /// Add a point to the strip chart.  If more than the history size are already
        /// in the chart, extras are dropped.
        /// </summary>
        /// <param name="val">The value to add</param>
        public void AddPoint(float val)
        {
            lock (m_list)
            {
                while (m_list.Count >= m_hist)
                    m_list.Dequeue();
                m_list.Enqueue(val);
            }
            m_last = val;
            if (m_auto)
            {
                if (val > m_max)
                    m_max = val;
                if (val < m_min)
                    m_min = val;
            }

            if (m_showStats)
            {
                // See:  http://en.wikipedia.org/wiki/Algorithms_for_calculating_variance#Algorithm_III
                m_count++;
                double delta = val - m_mean;
                m_mean += delta / (double)m_count;
                m_var_s += delta * (val - m_mean);
            }
            if (!DesignMode)
                ReDraw();
        }

        /// <summary>
        /// The last value inserted into the chart.
        /// </summary>
        [Browsable(false)]
        public float Last
        {
            get { return m_last; }
        }

        /// <summary>
        /// Clear all of the points from the chart
        /// </summary>
        public void Clear()
        {
            lock(m_list)
                m_list.Clear();
            ReDraw();
        }

        /// <summary>
        /// Save the current image to the specified filename.
        /// </summary>
        /// <param name="filename"></param>
        public void SaveTo(string filename)
        {
            pictureBox1.Image = ReDrawNoInvoke();
            pictureBox1.Image.Save(filename);
        }

        private void DesignReDraw()
        {
            if (!DesignMode)
                return;
            lock (m_list)
            {
                bool cleanup = false;
                double mean = m_mean;
                double var_s = m_var_s;
                if (m_list.Count == 0)
                {
                    cleanup = true;
                    foreach (float x in s_sampleData)
                        AddPoint(m_min + (x * (m_max - m_min)));
                }
                ExecReDraw();
                if (cleanup)
                {
                    m_list.Clear();
                    m_mean = mean;
                    m_var_s = var_s;
                }
            }
        }

        private void ReDraw()
        {
            if (DesignMode)
                ExecReDraw();
            else
            {
                Thread t = new Thread(new ThreadStart(ExecReDraw));
                t.IsBackground = true;
                t.Start();
            }
        }

        private delegate void BMCB(Bitmap bm);

        private void BitBlt(Bitmap bm)
        {
            pictureBox1.Image = bm;
        }

        private Bitmap ReDrawNoInvoke()
        {
            Font font = this.Font;
            int fh = font.Height + 2;

            if ((this.Width == 0) || (this.Height == 0)) return null;

            float    h  = this.Height - (2*fh);
            float    w  = this.Width;
            float    s  = m_max - m_min;
            if (s <= float.Epsilon)
                return null;

            if ((this.Height <= 0) || (this.Width <= 0))
                return null;

            Bitmap   bm = new Bitmap(this.Width, this.Height);
            Graphics g  = Graphics.FromImage(bm);
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackColor);
            lock (m_list)
            {
                if (m_list.Count > 0)
                {
                    float stripw = w / ((float)m_hist - 1F);
                    Debug.Assert(s != 0);
                    float y   = 0F;
                    int count = 0;

                    // this is kinda ugly, because of the repeated code.
                    // But it's better, perf-wise, than repeating the switch statement
                    // every time through the loop.
                    switch (m_style)
                    {
                    case ChartStyle.Bar:
                        RectangleF[] rects = new RectangleF[m_list.Count];
                        foreach (float val in m_list)
                        {
                            y = h * (1 - (val - m_min) / s) + fh;
                            rects[count] = new RectangleF(count * stripw, y, stripw, h - y);
                            count++;
                        }
                        g.FillRectangles(new SolidBrush(ForeColor), rects);
                        break;
                    case ChartStyle.Point:
                        Brush brush = new SolidBrush(ForeColor);
                        float p2 = fh - (m_pointSize / 2F);
                        foreach (float val in m_list)
                        {
                            y = h * (1 - (val - m_min) / s) + p2;
                            g.FillEllipse(brush, count * stripw, y, m_pointSize, m_pointSize);
                            count++;
                        }
                        break;
                    case ChartStyle.Line:
                        if (m_list.Count > 1)
                        {
                            PointF[] points = new PointF[m_list.Count];
                            foreach (float val in m_list)
                            {
                                y = h * (1 - (val - m_min) / s) + fh;
                                points[count] = new PointF(count * stripw, y);
                                count++;
                            }
                            g.DrawLines(new Pen(ForeColor), points);
                        }
                        break;
                    }
                }
            }
            Brush textBrush = new SolidBrush(m_textColor);

            if (m_zero)
            {
                float y = h * (1 + m_min / s) + fh;
                g.DrawLine(new Pen(m_zeroColor, 1F), 0, y, w, y);
            }
            if (m_showStats)
            {
                float y = (float)(h * (1 - (m_mean - m_min) / s) + fh);
                Color stats_color = Color.FromArgb(120, m_zeroColor);
                Pen stats_pen = new Pen(stats_color, 1.0F);
                stats_pen.DashStyle = DashStyle.Dash;
                g.DrawLine(stats_pen, 0, y, w, y);
                if (m_count > 1)
                {
                    stats_pen.DashStyle = DashStyle.Dot;
                    double stddev = Math.Sqrt(m_var_s / (m_count - 1));
                    y = (float)(h * (1 - (m_mean + stddev - m_min) / s) + fh);
                    g.DrawLine(stats_pen, 0, y, w, y);
                    y = (float)(h * (1 - (m_mean - stddev - m_min) / s) + fh);
                    g.DrawLine(stats_pen, 0, y, w, y);

                    Brush b = new SolidBrush(Color.FromArgb(120, m_statsColor));
                    float std = (float)(2.0F * h * (stddev / s));
                    g.FillRectangle(b, 0, y - std, w, std);
                }
            }
            if (m_label)
            {
                g.DrawString(m_min.ToString(), font, textBrush, 2, h + fh);
                g.DrawString(m_max.ToString(), font, textBrush, 2, 2);
            }
            if (m_showLast)
            {
                string last = m_last.ToString();
                float fw = g.MeasureString(last, font).Width + 2;
                g.DrawString(last, font, textBrush, (w - fw), 2);
            }
            if (m_title != null)
            {
                float fw = g.MeasureString(m_title, font).Width;
                g.DrawString(m_title, font, textBrush, (w - fw)/2F, 2);
            }
            return bm;
        }

        private void ExecReDraw()
        {
            Bitmap bm = ReDrawNoInvoke();
            if (bm == null)
                return;

            if (this.IsHandleCreated)
            {
                if (this.InvokeRequired)
                    Invoke(new BMCB(BitBlt), new object[] { bm });
                else
                    BitBlt(bm);
            }
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            //
            // pictureBox1
            //
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(150, 150);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            //
            // StripChart
            //
            this.Controls.Add(this.pictureBox1);
            this.Name = "StripChart";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The control has been resized.  Redraw.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            ReDraw();
        }
        /// <summary>
        /// Control has been loaded.  Redraw.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(System.EventArgs e)
        {
            ReDraw();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!m_first)
                return;
            m_first = false;
            ReDraw();
        }
    }
}
