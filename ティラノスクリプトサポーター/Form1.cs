using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Text;
using System.Windows.Forms;
using static System.Console;

namespace ティラノスクリプトサポーター
{
	public partial class Form1 : Form
	{
        //private string[] inputFiles;
        //private object path;

        public Form1()
		{
			InitializeComponent();

        }

		private void Form1_Load(object sender, EventArgs e)
		{
            
            //panel1.Dock = DockStyle.Fill;

            // わかりやすいようにPanelの背景色を設定
            panel1.BackColor = Color.DarkOrange;
        }

		private void button1_Click(object sender, EventArgs e)
		{
			//ファイル作成
			FileEncoder fileEncoder;
			fileEncoder = new FileEncoder();
			fileEncoder.CreateFiles(ref sender, ref e);
            //加筆修正だけ
            /*MessageBox.Show("ファイル書き出し完了\nティラノスクリプトサポーター\\bin\\Debug\\output", "出力",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Owner = this;//form1の手前にform2を出す
            form2.Show();//form2の表示
            //DialogResult dr = form2.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
               e.Effect = DragDropEffects.All;//所定位置でカーソル変更
               
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            // ファイルがなければ、何もしない
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            foreach (var TextPath in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                string[] dragFilePathArr = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                textBox1.Text = dragFilePathArr[0];
                //
                string stFilePath = System.IO.Path.GetFullPath(@"..\..\test.txt");
                //
                FileEncoder fileEncoder;//クラス呼ぶ
                fileEncoder = new FileEncoder();//インスタンス生成
                fileEncoder.SetTextPath(TextPath);//パス呼ぶ
                Debug.WriteLine("TextPath={0}",TextPath);//値検証
            }
        }
    }
}
