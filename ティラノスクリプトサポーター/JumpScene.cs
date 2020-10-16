using System.IO;
using System.Collections.Generic;



namespace ティラノスクリプトサポーター
{
    class JumpScene
    {
        const string JUMP_TEMPLATE = "jumpScene";		   //背景表示

        static string[] jump_template;


        static public void Initialized()
        {
            //テンプレートの文字を格納する
            jump_template = File.ReadAllLines("setting" + "\\" + JUMP_TEMPLATE + ".txt");
        }


        //背景変更コマンドにテクスチャ名を適用する
        public string CreateJumpFileName(int chapterCnt, int sceneCnt)
        {
            string name = "Chapter" + chapterCnt + "Scene" + sceneCnt + ".ks";

            string command = "";
            command += jump_template[0] + name;
            command += jump_template[1];
            command += jump_template[2] + "\n";
            command += jump_template[3];

            
            return command;
        }
    }
}