using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Games;
using Charlotte.Tests;

namespace Charlotte
{
	public static class TProgram
	{
		// 以下様式統一のため用途別に好きな方を使ってね -- ★要削除

#if false // 主にデバッガで実行するテスト用プログラム -- ★不要なら要削除
		public static void Run()
		{
			// -- choose one --

			new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
		}
#else // 主に実行ファイルにして使う/ゲームとして -- ★不要なら要削除
		public static void Run()
		{
			if (ProcMain.DEBUG)
			{
				RunOnDebug();
			}
			else
			{
				RunOnRelease();
			}
		}

		private static void RunOnDebug()
		{
			// テスト系 -- リリース版では使用しない。
#if DEBUG
			// -- choose one --

			Logo.Run();
			//TitleMenu.Run();
			//Game.Run();
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
#endif
		}

		private static void RunOnRelease()
		{
			Logo.Run();
		}
#endif
	}
}
