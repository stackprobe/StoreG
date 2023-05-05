using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	public static class Consts
	{
		/// <summary>
		/// コピー元フォルダパス
		/// </summary>
		public const string SRC_DIR = @"C:\";

		/// <summary>
		/// コピー先フォルダパス
		/// </summary>
		public const string DEST_DIR = @"P:\";

		/// <summary>
		/// コピー元で無視するフォルダのローカル名
		/// </summary>
		public static string[] SRC_IGNORE_NAMES = new string[]
		{
			"$Recycle.Bin",
			"$SysReset",
			"$WinREAgent",
			"Config.Msi",
			"Documents and Settings",
			"MSOCache",
			"PerfLogs",
			"Program Files",
			"Program Files (x86)",
			"ProgramData",
			"Programs", // 常駐系プログラムの格納場所(個人的に作成)
			"Recovery",
			"RECYCLER",
			"System Volume Information",
			"Users",
			"Windows",
			"Windows.old",
			"WINNT",
		};

		/// <summary>
		/// コピー先で無視するフォルダのローカル名
		/// </summary>
		public static string[] DEST_IGNORE_NAMES = new string[]
		{
			"$Recycle.Bin",
			"System Volume Information",
		};

		private static string _logFile2 = null;
		private static string _logFile3 = null;

		/// <summary>
		/// ログファイル出力先(1)
		/// </summary>
		public static readonly string LOG_FILE_1 = Path.Combine(DEST_DIR, "Backup.log");

		/// <summary>
		/// ログファイル出力先(2)
		/// </summary>
		public static string LOG_FILE_2
		{
			get
			{
				if (_logFile2 == null)
				{
					string dir = Environment.GetEnvironmentVariable("TMP");

					if (string.IsNullOrEmpty(dir))
						throw new Exception("Bad TMP");

					if (!Directory.Exists(dir))
						throw new Exception("no TMP");

					_logFile2 = Path.Combine(dir, "Backup_{929b33b1-1b3d-4115-98cd-81658694cd43}.log");
				}
				return _logFile2;
			}
		}

		/// <summary>
		/// ログファイル出力先(3)
		/// </summary>
		public static string LOG_FILE_3
		{
			get
			{
				if (_logFile3 == null)
				{
					string dir = @"C:\temp";

					if (!Directory.Exists(dir))
						throw new Exception("no " + dir);

					_logFile3 = Path.Combine(dir, "Backup.log");
				}
				return _logFile3;
			}
		}
	}
}
