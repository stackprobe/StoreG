using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Consts
	{
		/// <summary>
		/// 入力ルートDIR
		/// </summary>
		public const string R_ROOT_DIR = @"C:\Dev";

		/// <summary>
		/// 出力ルートDIR の フォーマット
		/// </summary>
		public const string W_ROOT_DIR_FORMAT = @"C:\home\GitHub\Store{0}\Dev";

		/// <summary>
		/// ソースDIR の ローカル名
		/// </summary>
		public static readonly string[] SRC_LOCAL_DIRS = new string[]
		{
			"Elsa20200001", // Game
			"Claes20200001", // CUI
			"Silvia20200001", // GUI
			"Gattonero20200001", // GameJS
			"Petra20200001", // C-Lang
		};
	}
}
