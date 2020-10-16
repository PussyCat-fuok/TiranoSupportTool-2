using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ティラノスクリプトサポーター
{
	public partial class Form1 : Form
	{
		


		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			//ファイル作成
			FileEncoder fileEncoder;
			fileEncoder = new FileEncoder();
			fileEncoder.CreateFiles(ref sender, ref e);
		}


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
