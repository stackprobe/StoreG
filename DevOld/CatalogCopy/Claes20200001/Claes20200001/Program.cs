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

			Main4(new ArgsReader(new string[] { "/A", @"C:\temp\Output", @"C:\temp\Catalog.cata" }));
			//Main4(new ArgsReader(new string[] { "/B", @"C:\temp\Input", @"C:\temp\Catalog.cata", @"C:\temp\Difference.diff" }));
			//Main4(new ArgsReader(new string[] { "/C", @"C:\temp\Difference.diff", @"C:\temp\Output" }));
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

				//MessageBox.Show("" + ex, Path.GetFileNameWithoutExtension(ProcMain.SelfFile) + " / エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				//Console.WriteLine("Press ENTER key. (エラーによりプログラムを終了します)");
				//Console.ReadLine();
			}
		}

		private void Main5(ArgsReader ar)
		{
			if (ar.ArgIs("/A"))
			{
				string outputDir = ar.NextArg();
				string catalogFile = ar.NextArg();

				ar.End();

				MakeCatalogFile(outputDir, catalogFile);
			}
			else if (ar.ArgIs("/B"))
			{
				string inputDir = ar.NextArg();
				string catalogFile = ar.NextArg();
				string differenceDir = ar.NextArg();

				ar.End();

				MakeDifferenceDir(inputDir, catalogFile, differenceDir);
			}
			else if (ar.ArgIs("/C"))
			{
				string differenceDir = ar.NextArg();
				string outputDir = ar.NextArg();

				ar.End();

				PatchingDifferenceData(differenceDir, outputDir, false);
			}
			else if (ar.ArgIs("/D"))
			{
				string differenceDir = ar.NextArg();
				string outputDir = ar.NextArg();

				ar.End();

				PatchingDifferenceData(differenceDir, outputDir, true);
			}
			else
			{
				throw new Exception("不明なコマンド引数");
			}
		}

		private void MakeCatalogFile(string outputDir, string catalogFile)
		{
			outputDir = SCommon.MakeFullPath(outputDir);
			catalogFile = SCommon.MakeFullPath(catalogFile);

			if (!Directory.Exists(outputDir))
				throw new Exception("no outputDir");

			// catalogFile -- 出力先

			// 引数チェックここまで

			ProcMain.WriteLog("< " + outputDir);
			ProcMain.WriteLog("> " + catalogFile);

			CatalogData catalog = CatalogData.CreateByDir(outputDir);

			catalog.SaveToFile(catalogFile);

			ProcMain.WriteLog("done!");
		}

		private void MakeDifferenceDir(string inputDir, string catalogFile, string differenceDir)
		{
			inputDir = SCommon.MakeFullPath(inputDir);
			catalogFile = SCommon.MakeFullPath(catalogFile);
			differenceDir = SCommon.MakeFullPath(differenceDir);

			if (!Directory.Exists(inputDir))
				throw new Exception("no inputDir");

			if (!File.Exists(catalogFile))
				throw new Exception("no catalogFile");

			// differenceDir -- 出力先

			// 引数チェックここまで

			ProcMain.WriteLog("< " + inputDir);
			ProcMain.WriteLog("< " + catalogFile);
			ProcMain.WriteLog("> " + differenceDir);

			CatalogData rCatalog = CatalogData.CreateByDir(inputDir);
			CatalogData wCatalog = CatalogData.LoadFromFile(catalogFile);

			DifferenceData difference = DifferenceData.Create(rCatalog, wCatalog, inputDir);

			difference.SaveToDiffDir(differenceDir);

			ProcMain.WriteLog("done!");
		}

		private void PatchingDifferenceData(string differenceDir, string outputDir, bool moveFlag)
		{
			differenceDir = SCommon.MakeFullPath(differenceDir);
			outputDir = SCommon.MakeFullPath(outputDir);

			if (!Directory.Exists(differenceDir))
				throw new Exception("no differenceDir");

			if (!Directory.Exists(outputDir))
				throw new Exception("no outputDir");

			// 引数チェックここまで

			ProcMain.WriteLog("< " + differenceDir);
			ProcMain.WriteLog("> " + outputDir);
			ProcMain.WriteLog("M " + moveFlag);

			DifferenceData difference = DifferenceData.LoadFromDiffDir(differenceDir);

			difference.Patching(outputDir, moveFlag);

			ProcMain.WriteLog("done!");
		}
	}
}
