using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace ティラノスクリプトサポーター
{
    enum CHARACOMMAND
    {
        NAME,
        TIME,
        WAIT,
        STORAGE,
        WIDTH,
        HEIGHT,
        LEFT,
        TOP,
        REFLECT,
        BACK_BRACKET,
        TEXT_MODE,
        CHARACOMMAND_MAX
    }



    class CharacterReader
    {
        const string DIRECTORY_CHARA = "setting\\character\\";		   //キャラ関係コマンドフォルダ

        const string CHARA_ADD = "charaAdd";		               //キャラ登場コマンド
        const string CHARA_CHANGE = "charaChange";                  //キャラ変更コマンド
        const string CHARA_HIDE = "charaHide";                    //キャラ退場コマンド
        const string CHARA_LIST = "charaList";                    //キャラ割当て番号




        static string[] chara_Add_Command;
        static string[] chara_Change_Command;
        static string[] chara_Hide_Command;
        static string[] chara_list;
        static bool[] chara_exist;




        static public void Initialized()
        {
            //テンプレートの文字を格納する
            chara_Add_Command = File.ReadAllLines(DIRECTORY_CHARA + CHARA_ADD + ".txt");

            //テンプレートの文字を格納する
            chara_Change_Command = File.ReadAllLines(DIRECTORY_CHARA + CHARA_CHANGE + ".txt");

            //テンプレートの文字を格納する
            chara_Hide_Command = File.ReadAllLines(DIRECTORY_CHARA + CHARA_HIDE + ".txt");

            //キャラの割り当て番号を格納する
            chara_list = File.ReadAllLines(DIRECTORY_CHARA + CHARA_LIST + ".txt");


            chara_exist = new bool[chara_list.Length];
        }





        public void CulcTextureCommand(string chara_command, ref List<string> textList)
        {
            string chara_name = chara_command.Replace("#", "");   //キャラ名(表情番号付き)取得
            string name = RemoveName(chara_name);           //キャラ名(表情番号ナシ)取得

            int chara_num = GetCharaNumber(chara_name);        //キャラ番号取得 
            string face_num = GetFaceNumber(chara_name);         //表情番号取得



            //立ち絵のあるキャラのみ画像挿入
            if (chara_num > 0)
            {
                string directory = chara_num.ToString() + "\\" + face_num + ".png";

                int chara_index = chara_num - 1;

                if (!chara_exist[chara_index])
                    AddCharacter(chara_index, directory, ref textList);
                else
                    ChangeCharacter(name, directory, ref textList);

                //キャラ追加
                //AddCharacter(directory, textList);
            }


            textList.Add("[_tb_end_text]");
            textList.Add("[tb_start_text mode = 4]");
            textList.Add("#" + name);                      //キャラメッセージコマンド挿入
        }




        private void AddCharacter(int chara_index, string directory, ref List<string> textList)
        {
            int chara_number = chara_index + 1;

            string command;

            command = chara_Add_Command[(int)CHARACOMMAND.NAME] + chara_list[chara_index];
            command += chara_Add_Command[(int)CHARACOMMAND.TIME];
            command += chara_Add_Command[(int)CHARACOMMAND.WAIT];
            command += chara_Add_Command[(int)CHARACOMMAND.STORAGE] + "chara\\" + directory;
            command += chara_Add_Command[(int)CHARACOMMAND.WIDTH];
            command += chara_Add_Command[(int)CHARACOMMAND.HEIGHT];
            command += chara_Add_Command[(int)CHARACOMMAND.LEFT];
            command += chara_Add_Command[(int)CHARACOMMAND.TOP];
            command += chara_Add_Command[(int)CHARACOMMAND.REFLECT];


            command += chara_Add_Command[(int)CHARACOMMAND.BACK_BRACKET];


            chara_exist[chara_index] = true;


            //リストに追加
            textList.Add(command);                         //キャラ登場コマンド挿入
        }

        private void ChangeCharacter(string name, string directory, ref List<string> textList)
        {
            string command;
            command = chara_Change_Command[0] + "\n";

            command += chara_Change_Command[1] + name;
            command += chara_Change_Command[2];
            command += chara_Change_Command[3];
            command += chara_Change_Command[4] + "chara\\" + directory;

            //リストに追加
            textList.Add(command);                         //キャラ登場コマンド挿入
        }







        //名前だけ抜き出す
        private string RemoveName(string chara_name)
        {
            string name = Regex.Replace(chara_name, @"[0-9]", "");
            return name;
        }



        //リストから同じ名前のキャラを探す
        private int GetCharaNumber(string chara_name)
        {
            //キャラなし文
            if (chara_name == "")
                return -1;


            //同じ名前が見つかったら終了
            int number;
            for (number = 0; number < chara_list.Length; number++)
            {
                if (chara_list[number].Contains(chara_name))
                    break;
            }

            //立ち絵のないキャラ
            if (number >= chara_list.Length)
                return -1;


            //ビルダーとの数値のズレをなくしてパスを作成
            number++;
            return number;
        }


        private string GetFaceNumber(string chara_name)
        {
            //名前と表情番号の分離
            string face_num = Regex.Replace(chara_name, @"[^0-9]", "");

            //番号の書き忘れはデフォルト(000に)
            if (face_num == "")
                face_num = "000";


            return face_num;
        }



        public void DeleteCharacter(List<string> textList)
        {
            for (int i = 0; i < chara_exist.Length; i++)
            {
                if (chara_exist[i])
                {
                    string command = DeleteCharacter(chara_list[i]);
                    textList.Add(command);
                }
            }
        }

        public string DeleteCharacter(string charName)
        {
            int num = GetCharaNumber(charName);
            int index = num - 1;


            chara_exist[index] = false;
            string command;
            command = chara_Hide_Command[0] + charName;
            command += chara_Hide_Command[1];

            return command;
        }
    }
}