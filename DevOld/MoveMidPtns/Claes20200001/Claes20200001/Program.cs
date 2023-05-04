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

			Main4(new ArgsReader(new string[] { @"C:\temp\from", @"C:\temp\to", "ABC", "DEF", "GHI" }));
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
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);

				MessageBox.Show("" + ex, Path.GetFileNameWithoutExtension(ProcMain.SelfFile) + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			string srcDir = SCommon.MakeFullPath(ar.NextArg());
			string destDir = SCommon.MakeFullPath(ar.NextArg());
			string[] midPtns = ar.TrailArgs().ToArray();

			ProcMain.WriteLog("< " + srcDir);
			ProcMain.WriteLog("> " + destDir);
			ProcMain.WriteLog("P " + string.Join(" OR ", midPtns));

			if (!Directory.Exists(srcDir))
				throw new Exception("no srcDir");

			if (!Directory.Exists(destDir))
				throw new Exception("no destDir");

			if (midPtns.Length == 0)
				throw new Exception("no midPtns");

			if (midPtns.Any(midPtn => string.IsNullOrWhiteSpace(midPtn)))
				throw new Exception("Bad midPtns");

			foreach (string file in Directory.GetFiles(srcDir))
			{
				if (midPtns.Any(midPtn => SCommon.ContainsIgnoreCase(Path.GetFileName(file), midPtn)))
				{
					string destFile = Path.Combine(destDir, Path.GetFileName(file));
					destFile = SCommon.ToCreatablePath(destFile);

					ProcMain.WriteLog("< " + file);
					ProcMain.WriteLog("> " + destFile);

					File.Move(file, destFile);

					ProcMain.WriteLog("done");
				}
			}
			ProcMain.WriteLog("done!");
		}
	}
}
