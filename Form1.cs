using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xbox_Controller_As_Mouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Visible == false)
            {
                var inputMonitor = new XBoxControllerAsMouse();
                inputMonitor.Start();
                button1.Visible = false;
                textBox1.Text = ("Started.\nTo stop this program, simply exit.");
                textBox1.Visible = true;
            }

        }
    }
}
