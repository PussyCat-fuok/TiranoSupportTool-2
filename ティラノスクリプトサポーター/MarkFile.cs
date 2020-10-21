using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Windows.Forms;



namespace ティラノスクリプトサポーター
{
    //このクラスは設定画面で弄るものを入れていきます

    enum SETTING
    {
        CHARA,
        BACKGROUND,
        NEXTSCENE,
        NEXTCHAPTER,
        SETTING_MAX
    }


    class MarkFile
    {
        //コマンドフォルダ
        const string DIRECTORY_SETTING = "setting";


        private string[] defMarks;                            //初期で設定された識別文字

        public string[] setMarks { private set; get; }        //自分で設定した識別文字



        public void Initialized()
        {
            defMarks = File.ReadAllLines("setting\\initial\\defmarks.txt");


            //セーブファイルの読み込み
            setMarks = File.ReadAllLines("setting\\initial\\setmarks.txt");


            //デフォルト設定ファイルに抜けがあった場合
            for (int i = 0; i < defMarks.Length; i++)
            { 
                if(defMarks[i]=="")
                    MessageBox.Show("識別文字の数が足りません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
                

            //個人設定ファイルに抜けがあればデフォルトに
            for (int i = 0; i < setMarks.Length; i++)
            {
                if (setMarks[i] == "")
                    setMarks[i] = defMarks[i];
            }
        }


        //=======================================
        //
        //  ファイル設定用関数
        //
        //=======================================

        public void SetMark(SETTING num, string mark)
        {
            //未入力はスルー
            if (mark == "")
                return;

            setMarks[(int)num] = mark;
        }



        public void SaveFile()
        {
            string text = string.Join("\n", setMarks);
            string path = "setting\\initial\\marks.txt";      //ファイル命名
            Encoding enc = new UTF8Encoding(false);
            File.WriteAllText(path, text, enc);
        }



        public void UndoToDefault()
        {
            for (int i = 0; i < (int)SETTING.SETTING_MAX; i++)
            {
                setMarks[i] = defMarks[i];
            }
        }
    }
}
