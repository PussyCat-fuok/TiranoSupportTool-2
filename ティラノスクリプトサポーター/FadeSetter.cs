using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ティラノスクリプトサポーター
{
    class FadeSetter
    {
        const string FADE_DIRECTORY  = "fade";		   //フェードコマンドのディレクトリ
        const string FADEIN_COMMAND  = "fadeIn";       //フェードインコマンド
        const string FADEOUT_COMMAND = "fadeOut";      //フェードアウトコマンド

        static string fadeIn;
        static string fadeOut;



        static public void Initialized()
        {
            //テンプレートの文字を格納する
            fadeIn  = File.ReadAllText("setting" + "\\" + FADE_DIRECTORY + "\\" + FADEIN_COMMAND  + ".txt");
            fadeOut = File.ReadAllText("setting" + "\\" + FADE_DIRECTORY + "\\" + FADEOUT_COMMAND + ".txt");
        }



        //背景変更コマンドにテクスチャ名を適用する
        public void SetFadeIn(ref List<string> textList)
        {
            string command = fadeIn;

            textList.Add(command);
        }


        public void SetFadeOut(ref List<string> textList)
        {
            string command = fadeOut;

            textList.Add(command);
        }
    }
}