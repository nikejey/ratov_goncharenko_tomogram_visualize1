using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ratov_goncharenko_tomogram_visualize1
{
    public partial class Form1 : Form
    {
        private Bin bin;
        private View view;
        private int currentLayer;
        private bool loaded;
        private bool needReload = true;
        private DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        private int FrameCount;
        private int currentmin = 0;
        private int currentwidthTF = 2000;
        private int mode;
        public Form1()
        {
            InitializeComponent();
            bin = new Bin();
            view = new View();
            currentLayer = 0;
            loaded = false;
            mode = 1;
        }





            private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;
                bin.readBIN(str);
                view.SetupView(glControl1.Width, glControl1.Height);
                loaded = true;
                glControl1.Invalidate
            }
        }

        private void glControl4_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                if (needReload)
                {
                    if (mode == 1)
                    {
                        view.DrawQuads(currentLayer, currentmin, currentwidthTF);
                        needReload = false;
                        glControl1.SwapBuffers();
                    }
                    else if (mode == 2)
                    {
                        view.generateTextureImage(currentLayer, currentmin, currentwidthTF);
                        view.Load2DTexture();
                        view.DrawTexture();
                        glControl1.SwapBuffers();
                        needReload = false;

                    }

                    else if (mode == 3)
                    {
                        view.DrawQuadsStrip(currentLayer, currentmin, currentwidthTF);
                        needReload = false;
                        glControl1.SwapBuffers();
                    }
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
        }

        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUpdate)
            {
                this.Text = String.Format("CT Visualizer" +
                                          " (fps={0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }



        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }
        
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
        }
    }
}
