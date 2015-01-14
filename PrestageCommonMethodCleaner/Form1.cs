using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrestageCommonMethodCleanerHelpers;
using System.Threading;

namespace PrestageCommonMethodCleaner
{
    public partial class Form1 : Form
    {
        private bool _isThreadRunning = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(this.printStatus));
            t1.Start();
            PrestageClassInfo classInfo = new PrestageClassInfo(richTextBox1.Text);
            _isThreadRunning = false;
        }

        private void printStatus()
        {
            Logger log = Logger.Instance;
            while (_isThreadRunning)
            {
                Thread.Sleep(100);
                Console.WriteLine(log.Progress);
            }
        }
    }
}
