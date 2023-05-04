using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2);
		}

		private void Main2(ArgsReader ar)
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4(ar);
			}
			SCommon.OpenOutputDirIfCreated();
		}

		private void Main3()
		{
			// テスト系 -- リリース版では使用しない。
#if DEBUG
			// -- choose one --

			Main4(new ArgsReader(new string[] { }));
			//new Test0001().Test01();
			//new Test0002().Test01();
			//new Test0003().Test01();

			// --
#endif
			SCommon.Pause();
		}

		private void Main4(ArgsReader ar)
		{
			try
			{
				Main5(ar);
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}
		}

		private void Main5(ArgsReader ar)
		{
			if (ar.ArgIs("/D")) // ? 展開
			{
				Main6(ar, false);
			}
			else // ? 圧縮
			{
				Main6(ar, true);
			}
		}

		private void Main6(ArgsReader ar, bool encryptMode)
		{
			string rFile = SCommon.MakeFullPath(ar.NextArg());
			string wFile = SCommon.MakeFullPath(ar.NextArg());

			ar.End();

			Console.WriteLine("< " + rFile);
			Console.WriteLine("> " + wFile);

			if (!File.Exists(rFile))
				throw new Exception("入力ファイルが見つかりません。");

			File.WriteAllBytes(wFile, SCommon.EMPTY_BYTES); // 書き出しテスト
			SCommon.DeletePath(wFile);

			try
			{
				if (encryptMode)
					SCommon.Compress(rFile, wFile);
				else
					SCommon.Decompress(rFile, wFile);
			}
			catch
			{
				SCommon.DeletePath(wFile);
				throw;
			}

			Console.WriteLine("Done");
		}
	}
}
