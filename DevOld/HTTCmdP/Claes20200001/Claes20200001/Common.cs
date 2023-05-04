using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Common
	{
		/// <summary>
		/// BatchService-の実行者の信用証明用文字列
		/// コマンド引数(/B)から設定可能
		/// URLに使用できないコントロール文字(水平タブ)で初期化しておく。
		/// </summary>
		public static string BatchServiceCredentials = "\t";

		/// <summary>
		/// ダウンロードページのテンプレートファイル
		/// null == 未指定
		/// </summary>
		public static string PageDownloadFile = null;
	}
}
