using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.Tests;
using Charlotte.WebServices;
using Charlotte.Utilities;

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

			//Main4(new ArgsReader(new string[] { }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/K" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/T", @"C:\temp\1.tsv" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/T", @"C:\temp\1.tsv", "/K" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/K", "/T", @"C:\temp\1.tsv" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/K", "/T", @"C:\temp\1.tsv" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/K", "/T", @"C:\temp\1.tsv", "/H", @"C:\temp\2.tsv" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/K", "/H", @"C:\temp\2.tsv" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "8080", "/K", "/T", @"C:\temp\1.tsv", "/H", @"C:\temp\2.tsv" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/K", "/N", @"C:\temp\1.html" }));
			//Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/K", "/P", @"C:\temp\ABCDEF" }));
			Main4(new ArgsReader(new string[] { @"..\..\..\..\TestData\DocRoot", "80", "/K", "/B", "ABCDEF" }));
			//new Test0001().Test01();
			//new Test0001().Test02();
			//new Test0001().Test03();

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
			// 複数のサーバーを起動していた場合、全て停止できるようにマニュアル・リセットとする。
			using (EventWaitHandle evStop = new EventWaitHandle(false, EventResetMode.ManualReset, Consts.SERVER_STOP_EVENT_NAME))
			{
				HTTPServer hs = new HTTPServer()
				{
					PortNo = 80,
					Backlog = 300,
					ConnectMax = 100,
					Interlude = () => !evStop.WaitOne(0),
					HTTPConnected = P_Connected,
				};

				SockChannel.ThreadTimeoutMillis = 100;

				HTTPServer.KeepAliveTimeoutMillis = 5000;

				HTTPServerChannel.RequestTimeoutMillis = 10000; // 10 sec
				HTTPServerChannel.ResponseTimeoutMillis = -1;
				HTTPServerChannel.FirstLineTimeoutMillis = 2000;
				HTTPServerChannel.IdleTimeoutMillis = 600000; // 10 min
				HTTPServerChannel.BodySizeMax = 300000000; // 300 MB

				SockCommon.TimeWaitMonitor.CTR_ROT_SEC = 60;
				SockCommon.TimeWaitMonitor.COUNTER_NUM = 5;
				SockCommon.TimeWaitMonitor.COUNT_LIMIT = 10000;

				// サーバーの設定ここまで

				if (ar.ArgIs("/S"))
				{
					evStop.Set();
					return;
				}
				if (ar.HasArgs())
				{
					this.DocRoot = SCommon.ToFullPath(ar.NextArg());

					if (!Directory.Exists(this.DocRoot))
						throw new Exception("ドキュメントルートが見つかりません");

					if (ar.HasArgs())
					{
						hs.PortNo = int.Parse(ar.NextArg());

						if (hs.PortNo < 1 || 65535 < hs.PortNo)
							throw new Exception("不正なポート番号");

						for (; ; ) // 拡張オプション
						{
							if (ar.ArgIs("/K"))
							{
								Func<bool> baseEvent = hs.Interlude;
								hs.Interlude = () => baseEvent() && !Console.KeyAvailable;
								ProcMain.WriteLog("キー入力を検出するとサーバーは停止します。");
								continue;
							}
							if (ar.ArgIs("/T"))
							{
								ContentTypeCollection.I.AddContentTypesByTsvFile(ar.NextArg());
								continue;
							}
							if (ar.ArgIs("/H"))
							{
								LoadHost2DocRoot(ar.NextArg());
								continue;
							}
							if (ar.ArgIs("/N"))
							{
								this.Page404File = SCommon.ToFullPath(ar.NextArg());
								ProcMain.WriteLog("Page404File: " + this.Page404File);

								if (!File.Exists(this.Page404File))
									throw new Exception("Page404File is not found");

								continue;
							}
							if (ar.ArgIs("/B"))
							{
								Common.BatchServiceCredentials = ar.NextArg();
								ProcMain.WriteLog("BatchServiceCredentials: " + Common.BatchServiceCredentials);
								continue;
							}
							if (ar.ArgIs("/P"))
							{
								ProgramDataFolder.RootDir = SCommon.MakeFullPath(ar.NextArg());
								ProcMain.WriteLog("ProgramDataFolder.RootDir: " + ProgramDataFolder.RootDir);
								continue;
							}
							if (ar.ArgIs("/D"))
							{
								Common.PageDownloadFile = SCommon.MakeFullPath(ar.NextArg());
								ProcMain.WriteLog("PageDownloadFile: " + Common.PageDownloadFile);

								if (!File.Exists(Common.PageDownloadFile))
									throw new Exception("PageDownloadFile is not found");

								continue;
							}
							break;
						}
					}
				}
				else
				{
					this.DocRoot = Directory.GetCurrentDirectory();
				}

				ProgramDataFolder.I = new ProgramDataFolder();

				ProcMain.WriteLog("HTTCmd-P-Start");
				ProcMain.WriteLog("DocRoot: " + this.DocRoot);
				ProcMain.WriteLog("PortNo: " + hs.PortNo);

				hs.Run();

				ProcMain.WriteLog("HTTCmd-P-End");
			}
		}

		private void LoadHost2DocRoot(string tsvFile)
		{
			this.Host2DocRoot = SCommon.CreateDictionaryIgnoreCase<string>();

			using (CsvFileReader reader = new CsvFileReader(tsvFile, Encoding.UTF8, CsvFileReader.DELIMITER_TAB))
			{
				foreach (string[] row in reader.ReadToEnd())
				{
					if (row.Length != 2)
						continue;

					string host = row[0];
					string docRoot = SCommon.ToFullPath(row[1]);

					SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, string.Format("Add Host-DocRoot Pair: {0} = {1}", host, docRoot));

					if (!Directory.Exists(docRoot))
						throw new Exception("ドキュメントルートが見つかりません(Host2DocRoot)");

					this.Host2DocRoot.Add(host, docRoot);
				}
			}
		}

		private string DocRoot;
		private Dictionary<string, string> Host2DocRoot = null;
		private string Page404File = null;

		private void P_Connected(HTTPServerChannel channel)
		{
			SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "クライアント：" + channel.Channel.Handler.RemoteEndPoint);

			if (10 < channel.Method.Length) // rough limit
				throw new Exception("Received method is too long");

			SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "要求メソッド：" + channel.Method);

			bool head = false;
			if (channel.Method == "HEAD")
			{
				head = true;
			}
			else if (channel.Method == "GET")
			{
				// noop
			}
			else if (channel.Method == "POST")
			{
				// noop
			}
			else
			{
				throw new Exception("Unsupported method: " + channel.Method);
			}

			string docRoot = this.DocRoot;
			string host = GetHeaderValue(channel, "Host");
			if (host != null)
			{
				string hostName;
				int hostPortNo;

				// host -> hostName, hostPortNo
				{
					int colon = host.IndexOf(':');

					if (colon != -1)
					{
						hostName = host.Substring(0, colon);
						hostPortNo = int.Parse(host.Substring(colon + 1));
					}
					else
					{
						hostName = host;
						hostPortNo = 80;
					}
				}

				if (hostName != "localhost" && hostPortNo != 80) // localhostではないとき、ポート番号80以外は80へ飛ばす。★自宅サーバー特有の処理
				{
					ProcMain.WriteLog("ポート80へ転送(自宅サーバー特有の処理)");

					channel.ResStatus = 301;
					channel.ResHeaderPairs.Add(new string[] { "Location", "http://" + hostName + channel.PathQuery });
					channel.ResBody = null;
					channel.ResBodyLength = -1L;

					goto endOfSetResponse;
				}

				if (
					this.Host2DocRoot != null &&
					this.Host2DocRoot.ContainsKey(hostName)
					)
					docRoot = this.Host2DocRoot[hostName];
			}

			string urlPath = channel.PathQuery;

			// クエリ除去
			{
				int ques = urlPath.IndexOf('?');

				if (ques != -1)
					urlPath = urlPath.Substring(0, ques);
			}

			if (1000 < urlPath.Length) // rough limit
				throw new Exception("Received path is too long");

			SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "要求パス：" + urlPath);

			string relPath;
			string path;
			if (urlPath == "/")
			{
				relPath = "_ROOT"; // ダミーで何か入れておく
				path = docRoot;
			}
			else
			{
				relPath = SCommon.ToFairRelPath(urlPath, docRoot.Length);
				path = Path.Combine(docRoot, relPath);
			}

			bool targetToFile = false;

			if (urlPath.EndsWith("/"))
			{
				path = Path.Combine(path, "index.htm");

				if (!File.Exists(path))
					path += "l";

				targetToFile = true;
			}

			SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "目的パス：" + path);

			if (ActionServer.TryPerform(head, urlPath, channel))
			{
				// noop
			}
			else if (!targetToFile && Directory.Exists(path))
			{
				if (host == null)
					throw new Exception("No HOST header value");

				channel.ResStatus = 301;
				channel.ResHeaderPairs.Add(new string[] { "Location", "http://" + host + "/" + string.Join("", SCommon.Tokenize(relPath, "\\").Select(v => EncodeUrl(v) + "/")) });
				channel.ResBody = null;
				channel.ResBodyLength = -1L;
			}
			else if (File.Exists(path))
			{
				string file = path;
				FileInfo fileInfo = new FileInfo(path);

				channel.ResStatus = 200;
				channel.ResHeaderPairs.Add(new string[] { "Content-Type", ContentTypeCollection.I.GetContentType(Path.GetExtension(path)) });
				channel.ResBody = E_ReadFile(file, fileInfo.Length);
				channel.ResBodyLength = fileInfo.Length;

				if (head && channel.ResBody != null)
				{
					channel.ResHeaderPairs.Add(new string[] { "Content-Length", fileInfo.Length.ToString() });
					channel.ResHeaderPairs.Add(new string[] { "X-Last-Modified-Time", new SCommon.SimpleDateTime(fileInfo.LastWriteTime).ToString("{0}/{1:D2}/{2:D2} {4:D2}:{5:D2}:{6:D2}") });
					channel.ResBody = null;
				}
			}
			else
			{
				channel.ResStatus = 404;
				//channel.ResHeaderPairs.Add();
				channel.ResBody = null;
				channel.ResBodyLength = -1L;

				if (!head && this.Page404File != null)
				{
					string file = this.Page404File;
					long fileSize = new FileInfo(file).Length;

					channel.ResHeaderPairs.Add(new string[] { "Content-Type", "text/html" });
					channel.ResBody = E_ReadFile(file, fileSize);
					channel.ResBodyLength = fileSize;
				}
			}

		endOfSetResponse:
			channel.ResHeaderPairs.Add(new string[] { "Server", "HTTCmd-P" });

			SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "RES-STATUS " + channel.ResStatus);

			foreach (string[] pair in channel.ResHeaderPairs)
				SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "RES-HEADER " + pair[0] + " = " + pair[1]);

			SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "RES-BODY " + (channel.ResBody != null));
			SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "RES-BODY-LENGTH " + channel.ResBodyLength);
		}

		private static string GetHeaderValue(HTTPServerChannel channel, string name)
		{
			foreach (string[] pair in channel.HeaderPairs)
				if (SCommon.EqualsIgnoreCase(pair[0], name))
					return pair[1];

			return null;
		}

		private static string EncodeUrl(string str)
		{
			StringBuilder buff = new StringBuilder();

			foreach (byte chr in Encoding.UTF8.GetBytes(str))
			{
				buff.Append('%');
				buff.Append(chr.ToString("x2"));
			}
			return buff.ToString();
		}

		private static IEnumerable<byte[]> E_ReadFile(string file, long fileSize)
		{
			for (long offset = 0L; offset < fileSize; )
			{
				int readSize = (int)Math.Min(fileSize - offset, (long)(512 * 1024));
				byte[] buff = new byte[readSize];

				//SockCommon.WriteLog(SockCommon.ErrorLevel_e.INFO, "READ " + offset + " " + readSize + " " + fileSize + " " + (offset * 100.0 / fileSize).ToString("F2") + " " + ((offset + readSize) * 100.0 / fileSize).ToString("F2")); // 頻出するので抑止

				using (FileStream reader = new FileStream(file, FileMode.Open, FileAccess.Read))
				{
					reader.Seek(offset, SeekOrigin.Begin);
					reader.Read(buff, 0, readSize);
				}
				yield return buff;

				offset += (long)readSize;
			}
		}
	}
}
