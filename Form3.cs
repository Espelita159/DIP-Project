using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;


namespace DIP
{
    public partial class Form3 : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public Form3()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form3_FormClosing);
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        private void btnOpenWebcam_Click(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found.");
                return;
            }

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);

            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);

            videoSource.Start();
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap currentFrame = (Bitmap)eventArgs.Frame.Clone();

            ConvMatrix convMatrix = new ConvMatrix();
            convMatrix.SetAll(2);

            BitmapFilter.Conv3x3(currentFrame, convMatrix);

            pictureBox1.Image = currentFrame;
        }

        private void btnCloseWebcam_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        private void smoothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentFrame = (Bitmap)pictureBox1.Image.Clone();

                BitmapFilter.Smooth(currentFrame, 30);

                pictureBox2.Image = currentFrame;

                pictureBox2.Refresh();
            }
        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentFrame = (Bitmap)pictureBox1.Image.Clone();

                BitmapFilter.GaussianBlur(currentFrame);

                pictureBox2.Image = currentFrame;

                pictureBox2.Refresh();
            }
        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentFrame = (Bitmap)pictureBox1.Image.Clone();

                BitmapFilter.Sharpen(currentFrame);

                pictureBox2.Image = currentFrame;
                pictureBox2.Refresh();
            }
        }

        private void meanRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentFrame = (Bitmap)pictureBox1.Image.Clone();

                BitmapFilter.MeanRemoval(currentFrame);

                pictureBox2.Image = currentFrame;
                pictureBox2.Refresh();
            }
        }
    }
}
