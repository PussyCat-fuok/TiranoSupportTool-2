using System.IO;
using System.Collections.Generic;



namespace ティラノスクリプトサポーター
{
    enum BGCOMMAND
    {
        TIME,
        STORAGE,
        WIDTH,
        HEIGHT,
        X,
        Y,
        CLICKABLE_IMG,
        NAME,
        BACK_BRACKET,
        TEXT_MODE,
        BGCOMMAND_MAX
    }



    class BackGroundReader
    {
        const string BG_TEMPLATE = "bgTemplate";		   //背景表示

        static string[] bg_template;


        static public void Initialized()
        {
            //テンプレートの文字を格納する
            bg_template = File.ReadAllLines("setting" + "\\" + BG_TEMPLATE + ".txt");
        }


        //背景変更コマンドにテクスチャ名を適用する
        public void CulcTextureCommand(string bg_command, ref List<string> textList)
        {
            string bg_name = bg_command.Replace("*", "");     //* を除いた
            string command;

            
            command  = bg_template[(int)BGCOMMAND.TIME];
            command += bg_template[(int)BGCOMMAND.STORAGE] + bg_name + ".jpg";
            command += bg_template[(int)BGCOMMAND.WIDTH];
            command += bg_template[(int)BGCOMMAND.HEIGHT];
            command += bg_template[(int)BGCOMMAND.X];
            command += bg_template[(int)BGCOMMAND.Y];
            command += bg_template[(int)BGCOMMAND.CLICKABLE_IMG];
            command += bg_template[(int)BGCOMMAND.NAME];

            command += bg_template[(int)BGCOMMAND.BACK_BRACKET];


            //リストに追加
            textList.Add(command);                             //背景変更コマンド挿入
            textList.Add(";" + bg_command);                    //背景
        }
    }
}