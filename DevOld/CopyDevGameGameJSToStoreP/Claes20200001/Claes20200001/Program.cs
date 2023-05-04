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

			Main4(new ArgsReader(new string[] { "E" }));
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

		private string WRootDir;

		private void Main5(ArgsReader ar)
		{
			string alpha = ar.NextArg();

			ar.End();

			if (alpha.Length != 1)
				throw new Exception("Bad alpha (Length)");

			if (!SCommon.ALPHA_UPPER.Contains(alpha[0]))
				throw new Exception("Bad alpha (not A-Z)");

			WRootDir = string.Format(Consts.W_ROOT_DIR_FORMAT, alpha);

			ProcMain.WriteLog("< " + Consts.R_ROOT_DIR);
			ProcMain.WriteLog("> " + WRootDir);

			if (!Directory.Exists(Consts.R_ROOT_DIR))
				throw new Exception("no R_ROOT_DIR");

			if (!Directory.Exists(WRootDir))
				throw new Exception("no WRootDir");

			ProcMain.WriteLog("start!");

			// 出力先(全)クリア
			SCommon.DeletePath(WRootDir);
			SCommon.CreateDir(WRootDir);

			Queue<string[]> q = new Queue<string[]>();

			q.Enqueue(Directory.GetDirectories(Consts.R_ROOT_DIR));

			while (1 <= q.Count)
			{
				foreach (string dir in q.Dequeue())
				{
					if (IsProjectDir(dir))
					{
						CopySourceDir(dir);
					}
					else
					{
						SCommon.CreateDir(SCommon.ChangeRoot(dir, Consts.R_ROOT_DIR, WRootDir));
						q.Enqueue(Directory.GetDirectories(dir));
					}
				}
			}
			ProcMain.WriteLog("done!");
		}

		private bool IsProjectDir(string dir)
		{
			return Consts.SRC_LOCAL_DIRS.Any(v => Directory.Exists(Path.Combine(dir, v)));
		}

		private void CopySourceDir(string projectDir)
		{
			foreach (string srcLocalDir in Consts.SRC_LOCAL_DIRS)
			{
				string rDir = Path.Combine(projectDir, srcLocalDir);

				if (Directory.Exists(rDir))
				{
					string wDir = SCommon.ChangeRoot(rDir, Consts.R_ROOT_DIR, WRootDir);

					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("> " + wDir);

					SCommon.CopyDir(rDir, wDir);

					ProcMain.WriteLog("done");

					// ----

					CopyBatchFile(projectDir, "Clean.bat");
					CopyBatchFile(projectDir, "Debug.bat");
					CopyBatchFile(projectDir, "Release.bat");

					CopyResourceDir(projectDir, "doc", true); // ドキュメント etc.

					if (srcLocalDir[0] == 'E') // Game
					{
						CopyResourceDir(projectDir, @"dat\dat", true); // 画像・音楽 etc.
						CopyResourceDir(projectDir, @"dat\res", false); // シナリオ・マップデータ etc.
					}
					else if (srcLocalDir[0] == 'G') // GameJS
					{
						CopyResourceDir(projectDir, "res", true); // 画像・音楽 etc.
					}

					CopyOtherResourceFiles(projectDir);
					CopyOtherResourceDirs(projectDir);
				}
			}
		}

		private void CopyBatchFile(string projectDir, string batchLocalName)
		{
			string rFile = Path.Combine(projectDir, batchLocalName);
			string wFile = SCommon.ChangeRoot(rFile, Consts.R_ROOT_DIR, WRootDir);

			if (File.Exists(rFile))
			{
				ProcMain.WriteLog("< " + rFile);
				ProcMain.WriteLog("> " + wFile);

				File.Copy(rFile, wFile);
			}
		}

		private void CopyOtherResourceFiles(string projectDir)
		{
			foreach (string rFile in Directory.GetFiles(projectDir))
			{
				string wFile = SCommon.ChangeRoot(rFile, Consts.R_ROOT_DIR, WRootDir);

				if (!Common.ExistsPath(wFile))
				{
					ProcMain.WriteLog("OF");
					ProcMain.WriteLog("< " + rFile);
					ProcMain.WriteLog("> " + wFile);

					File.Copy(rFile, wFile);
				}
			}
		}

		private void CopyResourceDir(string projectDir, string resourceRelDir, bool outputFileListMode)
		{
			string rDir = Path.Combine(projectDir, resourceRelDir);

			if (Directory.Exists(rDir))
			{
				string wDir = SCommon.ChangeRoot(rDir, Consts.R_ROOT_DIR, WRootDir);

				if (outputFileListMode)
				{
					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("T " + wDir);

					string treeFile = Path.Combine(wDir, "_Tree.txt");
					string[] treeFileData = MakeTreeFileData(rDir);

					SCommon.CreateDir(wDir);

					File.WriteAllLines(treeFile, treeFileData, Encoding.UTF8);
				}
				else
				{
					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("> " + wDir);

					SCommon.CopyDir(rDir, wDir);
				}
			}
		}

		private void CopyOtherResourceDirs(string projectDir)
		{
			foreach (string rDir in Directory.GetDirectories(projectDir))
			{
				string wDir = SCommon.ChangeRoot(rDir, Consts.R_ROOT_DIR, WRootDir);

				if (!Common.ExistsPath(wDir))
				{
					ProcMain.WriteLog("OD");
					ProcMain.WriteLog("< " + rDir);
					ProcMain.WriteLog("T " + wDir);

					string treeFile = Path.Combine(wDir, "_Tree.txt");
					string[] treeFileData = MakeTreeFileData(rDir);

					SCommon.CreateDir(wDir);

					File.WriteAllLines(treeFile, treeFileData, Encoding.UTF8);
				}
			}
		}

		private string[] MakeTreeFileData(string targDir)
		{
			string[] paths = Directory.GetDirectories(targDir, "*", SearchOption.AllDirectories)
				.Concat(Directory.GetFiles(targDir, "*", SearchOption.AllDirectories))
				.OrderBy(SCommon.Comp)
				.ToArray();

			List<string> dest = new List<string>();

			foreach (string path in paths)
			{
				dest.Add(SCommon.ChangeRoot(path, targDir));

				if (Directory.Exists(path))
				{
					dest.Add("\t-> Directory");
				}
				else
				{
					FileInfo info = new FileInfo(path);

					dest.Add(string.Format(
						"\t-> File {0} / {1} / {2:#,0}"
						, new SCommon.SimpleDateTime(info.CreationTime)
						, new SCommon.SimpleDateTime(info.LastWriteTime)
						, info.Length
						));
				}
			}

			if (dest.Count == 0)
				dest.Add("Nothing");

			return dest.ToArray();
		}
	}
}
