using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetectionWithHaarCascade
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FilterInfoCollection filter;
        VideoCaptureDevice device;
        void findCamera()
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "";
            filter = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (filter.Count != 0)
            {
                foreach (FilterInfo device in filter)
                {
                    comboBox1.Items.Add(device.Name);
                    comboBox1.SelectedIndex = 0;

                    comboBox1.SelectedIndex = 0;
                }
                device = new VideoCaptureDevice();
            }
            else
            {
                MessageBox.Show("Hata : Bilgisayarýnýza baðlý herhangi bir web kamerasý bulunamadý !");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            findCamera();
            SelectedGlass = glassesList[0];
            selImage = Image.FromFile(SelectedGlass);
            radioButton1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            findCamera();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            device = new VideoCaptureDevice(filter[comboBox1.SelectedIndex].MonikerString);
            device.NewFrame += Device_NewFrame;
            device.Start();
        }
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
        Rectangle r;
        private void Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr, byte> grayImage = bitmap.ToImage<Bgr, byte>();
            Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(grayImage, 1.2, 1);
            foreach (Rectangle rectangle in rectangles)
            {
                //Graphics g = Graphics.FromImage(bitmap);
                //Image ballImage = Image.FromFile("bball.png");
                //g.DrawImage(ballImage, rectangle);
                r = rectangle;


            }
            pictureBox1.Image = bitmap;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (device.IsRunning) device.SignalToStop();
        }
        string[] glassesList = { "1.png", "2.png" };
        string SelectedGlass;
        Image selImage;
        int correction = -20;
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(selImage, r.X, r.Y + correction + vScrollBar1.Value, r.Width, r.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Name == "radioButton1")
            {
                SelectedGlass = glassesList[0];
            }
            else
            {
                SelectedGlass = glassesList[1];
            }
            selImage = Image.FromFile(SelectedGlass);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}
