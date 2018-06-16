namespace WindowsFormsApplication2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO.Ports;
    using System.Threading;
    using System.Windows.Forms;

    public class Form1 : Form
    {
        private Button button1;
        private Button button2;
        private IContainer components = null;
        private ListBox listBox1;
        private ListBox listBox2;
        private ListBox listBox3;
        public List<PointF> points = new List<PointF>();
        private PointF[] pointsf = new PointF[0xc80];
        private bool readport = false;
        private static int sample;
        private int sampleorder = 0;
        private static int samplerate = 100;
        private static SerialPort serialPort = new SerialPort();
        private System.Windows.Forms.Timer timer1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private System.Windows.Forms.Timer timer2;

        public Form1()
        {
            this.InitializeComponent();
            this.listBox2.Items.Add("4800");
            this.listBox2.Items.Add("9600");
            this.listBox2.Items.Add("19200");
            this.listBox2.Items.Add("38400");
            this.listBox2.Items.Add("57600");
            this.listBox2.Items.Add("115200");
            this.listBox2.Items.Add("230400");
            this.listBox3.Items.Add("50");
            this.listBox3.Items.Add("100");
            this.listBox3.Items.Add("200");
            this.listBox3.Items.Add("400");
            this.listBox3.Items.Add("800");
            this.listBox3.Items.Add("1600");
            this.listBox3.Items.Add("3200");
            for (int a = 0; a < samplerate; a++)
            {
                this.pointsf[a] = new PointF(150f + (((1f / ((float) samplerate)) * a) * 1000f), 150f);
                this.points.Insert(a, this.pointsf[a]);
            }
            base.Paint += new PaintEventHandler(this.Form1_Paint);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.readport)
            {
                samplerate = Convert.ToInt32(this.listBox3.SelectedItem);
                this.points.Clear();
                StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
                Thread readThread = new Thread(new ThreadStart(this.Read));
                serialPort.BaudRate = Convert.ToInt32(this.listBox2.SelectedItem);
                serialPort.Parity = Parity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
                serialPort.Handshake = Handshake.None;
                serialPort.PortName = this.listBox1.SelectedItem.ToString();
                serialPort.ReadTimeout = 500;
                serialPort.Open();
                readThread.Start();
            }
            else
            {
                serialPort.Close();
            }
            this.readport = !this.readport;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> ports = this.GetAllPorts();
            this.listBox1.Items.Clear();
            foreach (string portname in ports)
            {
                this.listBox1.Items.Insert(0, portname);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawLines(Pens.Black, this.points.ToArray());
        }

        public List<string> GetAllPorts()
        {
            List<string> allPorts = new List<string>();
            foreach (string portName in SerialPort.GetPortNames())
            {
                allPorts.Add(portName);
            }
            return allPorts;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(176, 515);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Read Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(84, 433);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(126, 43);
            this.listBox1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(80, 515);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "List Ports";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 30;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(249, 433);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(126, 43);
            this.listBox2.TabIndex = 4;
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(418, 433);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(126, 43);
            this.listBox3.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 407);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Ports";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 407);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Ports";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(279, 524);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Hüseyin Göktaş | 2018";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(415, 407);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Sample";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1312, 552);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Usb Com Port Plotter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public void Read()
        {
            while (true)
            {
                try
                {
                    sample = Convert.ToInt32(serialPort.ReadLine());
                }
                catch (Exception)
                {
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.readport)
            {
                this.pointsf[this.sampleorder].Y = 150 - (sample / 10);
                this.points.RemoveAt(this.sampleorder);
                this.points.Insert(this.sampleorder, this.pointsf[this.sampleorder]);
                this.sampleorder++;
                if (this.sampleorder >= samplerate)
                {
                    this.sampleorder -= samplerate;
                }
                this.timer1.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            base.Invalidate();
        }
    }
}

