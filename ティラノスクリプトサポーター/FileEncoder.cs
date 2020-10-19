using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;



namespace ティラノスクリプトサポーター
{
    class FileEncoder
    {
		//入出力先フォルダ
		const String DIRECTORY_INPUT   = "input";
		const String DIRECTORY_OUTPUT  = "output";

		//コマンドフォルダ
		const String DIRECTORY_SETTING = "setting";
		const String START_TEMPLATE    = "start";            //メッセージ開始
		const String FADE_TEMPLATE     = "fade";             //メッセージ終了
		const String NEXT_CHAPTER      = "nextChapter";      //チャプター切り替え
		const String NEXT_SCENE        = "nextScene";        //シーン切り替え
		const String CHARACTER         = "character";        //キャラメッセージ
		const String BACK_GROUND       = "bg";               //背景



		//読み込みクラス
		BackGroundReader bgReader    = new BackGroundReader();
		SoundReader      soundReader = new SoundReader(); 
		CharacterReader  charaReader = new CharacterReader();
		JumpScene        jumpScene   = new JumpScene();



		public void CreateFiles(ref object sender, ref EventArgs e)
		{
			//読み込みクラス初期化
			BackGroundReader.Initialized();
			SoundReader.Initialized();
			CharacterReader.Initialized();
			JumpScene.Initialized();



			//ファイルのパスを取得
			String path = "test.txt";
			String text = "テストです";
			String[] inputFiles = Directory.GetFiles(Path.GetFullPath(DIRECTORY_INPUT), "*");


			//コマンド格納
			String[] files        = Directory.GetFiles(Path.GetFullPath("setting"), "*");
			ReadFile setting      = new ReadFile();
			setting.startTemplate = File.ReadAllText(DIRECTORY_SETTING + "\\" + START_TEMPLATE             + ".txt");
			setting.fadeTemplate  = File.ReadAllText(DIRECTORY_SETTING + "\\" + FADE_TEMPLATE              + ".txt");
			setting.nextChapter   = File.ReadAllText(DIRECTORY_SETTING + "\\" + "initial\\" + NEXT_CHAPTER + ".txt");
			setting.nextScene     = File.ReadAllText(DIRECTORY_SETTING + "\\" + "initial\\" + NEXT_SCENE   + ".txt");
			setting.chara         = File.ReadAllText(DIRECTORY_SETTING + "\\" + "initial\\" + CHARACTER    + ".txt");
			setting.bg            = File.ReadAllText(DIRECTORY_SETTING + "\\" + "initial\\" + BACK_GROUND  + ".txt");



			foreach (String fileName in inputFiles)
			{
				string[] textlines = File.ReadAllLines(fileName);
				outputScenario(ref textlines, ref setting);                      //シーンファイル作成(全て)
				foreach (String test in textlines)
					Console.WriteLine(test);
			}

			Encoding enc = new UTF8Encoding(false);
			File.WriteAllText(path, text, enc);
			Console.WriteLine(Path.GetFullPath(path));


			MessageBox.Show("ファイル書き出し完了", "出力", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}





		//シーンファイルを作成・出力する
		private void outputScenario(ref string[] textlines, ref ReadFile setting)
		{
			int  chapterCnt = 0;
			int  sceneCnt   = 1;
			bool startFlag  = false;
			bool textFlag   = false;                      //テキスト中のフラグ
			var  textList   = new List<string>();
			String newLine  = "[p]";                      //改行



			foreach (String textline in textlines)
			{
				//未入力の場合
				if (textList.Count == 0)
				{
					textList.Add("*start");                   //startラベル挿入
				}

				//キャラクターのメッセージ( # )
				if (textline.Contains(setting.chara))
				{
					textFlag = false;

					charaReader.CulcTextureCommand(textline, ref textList);     //背景変更コマンド作成
					textFlag = true;

					newLine = "";
					continue;
				}

				//チャプター切り替え( *** )
				if (textline.Contains(setting.nextChapter))
				{
					textFlag = false;


					//次のチャプター名
					int next = chapterCnt + 1;
					string name = jumpScene.CreateJumpFileName(next, 1);



					//最初のみファイル生成を行わない(チャプターカウントを合わせるため)
					if (chapterCnt != 0)
						outputFile(chapterCnt, sceneCnt, textList, setting.fadeTemplate, name, true);         //ファイル出力
					chapterCnt++;                                                                             //次のチャプターへ
					sceneCnt  = 1;                                                                            //シーン初期化
					startFlag = false;
					textList  = new List<string>();                                                           //文字列リスト初期化
				}
				//シーン切り替え( ** )													          
				else if (textline.Contains(setting.nextScene))
				{
					textFlag = false;



					//次のシーン名
					int next    = sceneCnt + 1;
					string name = jumpScene.CreateJumpFileName(chapterCnt, next);



					outputFile(chapterCnt, sceneCnt, textList, setting.fadeTemplate, name, false);       //ファイル出力
					sceneCnt++;                                                                          //シーン初期化
					startFlag = false;
					textList  = new List<string>();                                                      //文字列リスト初期化
				}
				//背景切り替え( * )													             
				else if (textline.Contains(setting.bg))
				{
					textFlag = false;



					bgReader.CulcTextureCommand(textline, ref textList);     //背景変更コマンド作成
					soundReader.PlayBGM(textline, ref textList);

					if (!startFlag)
					{
						textList.Add(setting.startTemplate);      //開始コマンド挿入
						startFlag = true;
					}
				}
				//通常メッセージ														          
				else
				{
					//空白行以外で改行を入れる
					if (textline != "")
						newLine = "[p]";
					else
						newLine = "";

					textList.Add(textline + newLine);                                            //メッセージ挿入
				}
			}
		}




		//ファイル出力
		private void outputFile(int chapterCnt, int sceneCnt, List<string> textList, String endTemplate, String nextFile, bool charaDelete)
		{
			textList.Add(endTemplate);             //メッセージ終了

			//キャラ消去を入れるか
			if (charaDelete)
				charaReader.DeleteCharacter(textList);

			textList.Add(nextFile);                //シーン切り替え

			String text  = String.Join("\n", textList.ToArray());
			String path  = DIRECTORY_OUTPUT + "\\" + "Chapter" + chapterCnt + "Scene" + sceneCnt + ".ks";      //ファイル命名
			Encoding enc = new UTF8Encoding(false);
			File.WriteAllText(path, text, enc);
		}
	}
}