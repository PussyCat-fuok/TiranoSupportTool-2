using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ティラノスクリプトサポーター
{
    class SoundReader
    {
        const string SOUND_DIRECTORY = "sound";		   //背景表示
        const string PLAY_BGM        = "playBGM";
        const string STOP_BGM        = "stopBGM";

        static string[] playBGM;
        static string[] stopBGM;



        static public void Initialized()
        {
            //テンプレートの文字を格納する
            playBGM = File.ReadAllLines("setting" + "\\" + SOUND_DIRECTORY + "\\" + PLAY_BGM + ".txt");
            stopBGM = File.ReadAllLines("setting" + "\\" + SOUND_DIRECTORY + "\\" + STOP_BGM + ".txt");
        }

        

        //背景変更コマンドにテクスチャ名を適用する
        public void PlayBGM(string bgName , ref List<string> textList)
        {
            string name = bgName.Replace("*", "");

            string command;
            command  = playBGM[0] + name + ".ogg";
            command += playBGM[1];

            textList.Add("[_tb_end_text]");
            textList.Add(command);
        }



        public void StopBGM(ref List<string> textList)
        {
            textList.Add("[_tb_end_text]");
            textList.Add(stopBGM[0]);
        }
    }
}