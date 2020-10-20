using System;
using System.Collections.Generic;
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


    class ReadMarks
    {
        //コマンドフォルダ
        const string DIRECTORY_SETTING = "setting";

        //識別マーク
        const string NEXT_CHAPTER = "nextChapter";      //チャプター切り替え
        const string NEXT_SCENE   = "nextScene";        //シーン切り替え
        const string CHARACTER    = "character";        //キャラメッセージ
        const string BACK_GROUND  = "bg";               //背景



        public string[] defMarks { private set; get; }     //初期で設定された識別文字

        private string[] setMarks;                                            //自分で設定した識別文字



        public void Initialized()
        {
            defMarks = new string[(int)SETTING.SETTING_MAX];

            defMarks[(int)SETTING.CHARA]       = File.ReadAllText(DIRECTORY_SETTING + "\\" + "initial\\" + CHARACTER    + ".txt");
            defMarks[(int)SETTING.BACKGROUND]  = File.ReadAllText(DIRECTORY_SETTING + "\\" + "initial\\" + BACK_GROUND  + ".txt");
            defMarks[(int)SETTING.NEXTSCENE]   = File.ReadAllText(DIRECTORY_SETTING + "\\" + "initial\\" + NEXT_SCENE   + ".txt");
            defMarks[(int)SETTING.NEXTCHAPTER] = File.ReadAllText(DIRECTORY_SETTING + "\\" + "initial\\" + NEXT_CHAPTER + ".txt");


            //セーブファイルの読み込み
            setMarks = File.ReadAllLines("setting\\initial\\marks.txt");

            //設定されてなければデフォルトに
            for (int i = 0; i < setMarks.Length; i++)
            {
                if (setMarks[i] == "")
                    setMarks[i] = defMarks[i];
            }
        }




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
