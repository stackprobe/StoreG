using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.WebServices;
using System.IO;

namespace Charlotte.Actions
{
	public static class Action_Download
	{
		public static void Perform(HTTPServerChannel channel)
		{
			string host = GetHeaderValue(channel, "host");
			string downloadPath = GetDownloadPath(channel);
			string downloadUrl = "http://" + host + downloadPath;

			string resText;

			if (Common.PageDownloadFile == null)
			{
				resText = string.Format(@"<html>
<head>
<title>ダウンロード</title>
</head>
<body>
<center>
<h1>ファイルをダウンロードするには下記リンクをクリックして下さい。</h1>
<hr/>
<a href=""{0}"" download>{1}</a>
</center>
</body>
</html>
"
					, downloadUrl
					, downloadUrl
					);
			}
			else
			{
				resText = File.ReadAllText(Common.PageDownloadFile, Encoding.UTF8)
					.Replace("${download-url}", downloadUrl);
			}

			channel.ResStatus = 200;
			channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/html; charset=UTF-8" });
			channel.ResBody = new byte[][] { Encoding.UTF8.GetBytes(resText) };
			channel.ResBodyLength = -1L;
		}

		private static string GetHeaderValue(HTTPServerChannel channel, string name)
		{
			foreach (string[] pair in channel.HeaderPairs)
				if (SCommon.EqualsIgnoreCase(pair[0], name))
					return pair[1];

			throw new Exception("no header name: " + name);
		}

		private static string GetDownloadPath(HTTPServerChannel channel)
		{
			string downloadPath = channel.PathQuery;

			if (downloadPath[0] != '/')
				throw new Exception("Bad donwloadPath 1");

			int p = downloadPath.IndexOf('/', 1); // 2番目の'/'

			if (p == -1)
				throw new Exception("Bad downloadPath 2");

			downloadPath = downloadPath.Substring(p); // 2番目の'/'より前を除去
			return downloadPath;
		}
	}
}
