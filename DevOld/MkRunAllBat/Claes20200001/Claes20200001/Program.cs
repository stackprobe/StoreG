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

			Main4(new ArgsReader(new string[] { @"C:\temp\bat", @"C:\temp\run.bat" }));
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
			string rootDir = SCommon.MakeFullPath(ar.NextArg());
			string destFile = SCommon.MakeFullPath(ar.NextArg());

			if (!Directory.Exists(rootDir))
				throw new Exception("no rootDir");

			if (Directory.Exists(destFile))
				throw new Exception("Bad destFile");

			ProcMain.WriteLog("rootDir: " + rootDir);
			ProcMain.WriteLog("destFile: " + destFile);

			List<string> destLines = new List<string>();

			foreach (string file in Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories).OrderBy(SCommon.Comp))
			{
				string ext = Path.GetExtension(file);

				if (SCommon.EqualsIgnoreCase(ext, ".bat"))
				{
					destLines.Add(string.Format("START \"\" /D\"{0}\" /B /WAIT cmd /c \"{1}\"", Path.GetDirectoryName(file), Path.GetFileName(file)));
				}
				else if (SCommon.EqualsIgnoreCase(ext, ".exe"))
				{
					destLines.Add(string.Format("START \"\" /D\"{0}\" /B /WAIT \"{1}\"", Path.GetDirectoryName(file), Path.GetFileName(file)));
				}
			}
			File.WriteAllLines(destFile, destLines, SCommon.ENCODING_SJIS);

			ProcMain.WriteLog("done!");
		}
	}
}
