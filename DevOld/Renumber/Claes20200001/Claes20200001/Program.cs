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

			Main4(new ArgsReader(new string[] { @"C:\temp" }));
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
			string targetDir = SCommon.MakeFullPath(ar.NextArg());

			ProcMain.WriteLog("* " + targetDir);

			if (!Directory.Exists(targetDir))
				throw new Exception("no targetDir");

			// チェック用
			string[] dirNames = Directory.GetDirectories(targetDir)
				.Select(xdir => Path.GetFileName(xdir))
				.ToArray();

			string[] fileNames = Directory.GetFiles(targetDir)
				.Select(file => Path.GetFileName(file))
				.OrderBy(SCommon.CompIgnoreCase)
				.ToArray();

			if (dirNames.Any(dirName => SCommon.StartsWithIgnoreCase(dirName, Consts.MID_FILE_NAME_PREFIX)))
				throw new Exception("Bad dirName");

			if (fileNames.Any(fileName => SCommon.StartsWithIgnoreCase(fileName, Consts.MID_FILE_NAME_PREFIX)))
				throw new Exception("Bad fileName");

			string[] midFileNames = fileNames
				.Select(fileName => Consts.MID_FILE_NAME_PREFIX + fileName)
				.ToArray();

			string[] destFileNames = new string[fileNames.Length];

			for (int index = 0; index < fileNames.Length; index++)
			{
				string fileName = fileNames[index];

				if (Regex.IsMatch(fileName, "^[0-9]{4}_.+$"))
					fileName = fileName.Substring(5);

				destFileNames[index] = ((index + 1) * 10).ToString("D4") + "_" + fileName;
			}

			RenameTestAllFile(targetDir, fileNames, midFileNames);
			RenameAllFile(targetDir, fileNames, midFileNames);
			RenameAllFile(targetDir, midFileNames, destFileNames);

			ProcMain.WriteLog("done!");
		}

		private void RenameTestAllFile(string dir, string[] fileNames, string[] destFileNames)
		{
			for (int index = 0; index < fileNames.Length; index++)
			{
				RenameFile(dir, fileNames[index], destFileNames[index]);
				RenameFile(dir, destFileNames[index], fileNames[index]);
			}
		}

		private void RenameAllFile(string dir, string[] fileNames, string[] destFileNames)
		{
			for (int index = 0; index < fileNames.Length; index++)
			{
				RenameFile(dir, fileNames[index], destFileNames[index]);
			}
		}

		private void RenameFile(string dir, string fileName, string destFileName)
		{
			ProcMain.WriteLog("D " + dir);
			ProcMain.WriteLog("< " + fileName);
			ProcMain.WriteLog("> " + destFileName);

			File.Move(
				Path.Combine(dir, fileName),
				Path.Combine(dir, destFileName)
				);

			ProcMain.WriteLog("done");
		}
	}
}
