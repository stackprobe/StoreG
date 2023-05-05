using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.Games;

namespace Charlotte
{
	public static class TProgram
	{
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

			// --
#endif
		}

		private static void RunOnRelease()
		{
			Logo.Run();
		}
	}
}
