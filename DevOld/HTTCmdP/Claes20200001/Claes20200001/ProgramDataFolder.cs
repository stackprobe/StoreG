using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte
{
	/// <summary>
	/// 共有データパス管理
	/// </summary>
	public class ProgramDataFolder
	{
		public static ProgramDataFolder I;

		public ProgramDataFolder()
		{
			SCommon.CreateDir(RootDir);
		}

		/// <summary>
		/// 共有データのルートフォルダ
		/// 変更する場合は、初期化前に設定すること。
		/// </summary>
		public static string RootDir = @"C:\temp\HTTCmdP_ProgramData";

		/// <summary>
		/// 利用者：Action_AccessCounter
		/// </summary>
		/// <returns>パス</returns>
		public string GetAccessCounterFile()
		{
			string file = Path.Combine(RootDir, "AccessCounter.txt");

			if (!File.Exists(file))
				File.WriteAllBytes(file, SCommon.EMPTY_BYTES);

			return file;
		}
	}
}
