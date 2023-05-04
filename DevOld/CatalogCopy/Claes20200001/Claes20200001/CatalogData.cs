using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	/// <summary>
	/// カタログ情報
	/// </summary>
	public class CatalogData
	{
		public class FileData
		{
			public string StrPath;
			public long Size;
			public long LastWriteTimeStamp;

			public static FileData CreateByFile(string file, string rootDir)
			{
				FileInfo info = new FileInfo(file);

				return new FileData()
				{
					StrPath = SCommon.ChangeRoot(file, rootDir),
					Size = info.Length,
					LastWriteTimeStamp = SCommon.TimeStampToSec.ToTimeStamp(info.LastWriteTime),
				};
			}
		}

		public List<string> Dirs;
		public List<FileData> Files;

		/// <summary>
		/// 指定ディレクトリのカタログ情報を生成する。
		/// </summary>
		/// <param name="rootDir">指定ディレクトリ</param>
		/// <returns>カタログ情報</returns>
		public static CatalogData CreateByDir(string rootDir)
		{
			rootDir = SCommon.MakeFullPath(rootDir);

			CatalogData catalog = new CatalogData();

			catalog.Dirs = Directory.GetDirectories(rootDir, "*", SearchOption.AllDirectories)
				.Select(v => SCommon.ChangeRoot(v, rootDir))
				.OrderBy(SCommon.Comp)
				.ToList();

			catalog.Files = Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories)
				.Select(v => FileData.CreateByFile(v, rootDir))
				.OrderBy((a, b) => SCommon.Comp(a.StrPath, b.StrPath))
				.ToList();

			return catalog;
		}

		/// <summary>
		/// 指定ファイルにカタログ情報を書き出す。
		/// </summary>
		/// <param name="catalogFile">出力先カタログ情報ファイル</param>
		public void SaveToFile(string catalogFile)
		{
			catalogFile = SCommon.MakeFullPath(catalogFile);

			List<string> dest = new List<string>();

			dest.Add(Consts.CATALOG_FILE_SIGNATURE);

			foreach (string dir in this.Dirs)
			{
				dest.Add("D");
				dest.Add(dir);
			}
			foreach (FileData file in this.Files)
			{
				dest.Add("F");
				dest.Add(file.StrPath);
				dest.Add("" + file.Size);
				dest.Add("" + file.LastWriteTimeStamp);
			}
			dest.Add("E");

			File.WriteAllLines(catalogFile, dest, Encoding.UTF8);
		}

		/// <summary>
		/// 指定ファイルからカタログ情報を読み込む。
		/// </summary>
		/// <param name="catalogFile">入力元カタログ情報ファイル</param>
		/// <returns>カタログ情報</returns>
		public static CatalogData LoadFromFile(string catalogFile)
		{
			catalogFile = SCommon.MakeFullPath(catalogFile);

			string[] lines = File.ReadAllLines(catalogFile, Encoding.UTF8);
			int r = 0;

			CatalogData catalog = new CatalogData()
			{
				Dirs = new List<string>(),
				Files = new List<FileData>(),
			};

			if (lines[r++] != Consts.CATALOG_FILE_SIGNATURE)
				throw new Exception("Bad CATALOG_FILE_SIGNATURE");

			for (; ; )
			{
				switch (lines[r++])
				{
					case "D":
						catalog.Dirs.Add(lines[r++]);
						break;

					case "F":
						{
							FileData file = new FileData();

							file.StrPath = lines[r++];
							file.Size = long.Parse(lines[r++]);
							file.LastWriteTimeStamp = long.Parse(lines[r++]);

							catalog.Files.Add(file);
						}
						break;

					case "E":
						goto endLoop;

					default:
						throw new Exception("Bad command");
				}
			}
		endLoop:

			// 簡単なチェック
			{
				foreach (string dir in catalog.Dirs)
				{
					if (string.IsNullOrEmpty(dir))
						throw new Exception("Bad dir");
				}

				foreach (FileData file in catalog.Files)
				{
					if (string.IsNullOrEmpty(file.StrPath))
						throw new Exception("Bad file.StrPath");

					if (file.Size < 0L)
						throw new Exception("Bad file.Size");

					if (!Common.IsFairTimeStamp(file.LastWriteTimeStamp))
						throw new Exception("Bad file.LastWriteTimeStamp");
				}
			}

			// 補正
			{
				catalog.Dirs.Sort(SCommon.Comp);
				catalog.Files.Sort((a, b) => SCommon.Comp(a.StrPath, b.StrPath));
			}

			return catalog;
		}
	}
}
